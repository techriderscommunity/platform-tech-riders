"""Transforma los posts crudos de WordPress (fetch.py) al formato que espera
POST /api/knowledge-articles/import.

Para cada post:
  - content.rendered (HTML) -> Markdown (markdownify).
  - Cada <img src> se descarga a assets/<slug>/<filename> y se reescribe en el
    Markdown final con la URL definitiva y deterministica del blob
    (https://<storage>.blob.core.windows.net/<container>/knowledge/<slug>/<filename>).
  - Categorias/tags de WordPress se pasan como nombres (CategoryNames/SkillNames);
    el backend los resuelve contra Category/Skill existentes con fallback a
    "Sin categoria" (ver KnowledgeArticleService.ResolveCategoryIdsAsync).

Sin fallback de contenido: si un post no tiene contenido o falla la descarga de
una imagen, se registra el error en import-summary.json y NO se genera un .md
a medias; ese post debe revisarse a mano antes de subirlo.

Uso:
    python transform.py --raw-posts repo-intake/generated/wp-import/2026-09-09/raw/posts.json \
        --output-dir repo-intake/generated/wp-import/2026-09-09 \
        --storage-account-url https://storagetetxito.blob.core.windows.net \
        --container knowledge
"""
import argparse
import json
import re
import urllib.request
import urllib.error
from pathlib import Path
from datetime import datetime, timezone
from urllib.parse import urlparse

from bs4 import BeautifulSoup
from markdownify import markdownify as html_to_markdown


def utc_now() -> str:
    return datetime.now(timezone.utc).isoformat()


def strip_html(value: str) -> str:
    return BeautifulSoup(value, "html.parser").get_text().strip()


def embedded_terms(post: dict, taxonomy: str) -> list[str]:
    groups = post.get("_embedded", {}).get("wp:term", [])
    names = []
    for group in groups:
        for term in group:
            if term.get("taxonomy") == taxonomy:
                names.append(term.get("name", "").strip())
    return [n for n in names if n]


def image_filename(src: str, seen: set[str]) -> str:
    raw_name = Path(urlparse(src).path).name or "image"
    name = re.sub(r"[^A-Za-z0-9_.-]+", "-", raw_name).strip("-") or "image"
    candidate = name
    counter = 1
    while candidate in seen:
        stem, dot, ext = name.partition(".")
        candidate = f"{stem}-{counter}{dot}{ext}"
        counter += 1
    seen.add(candidate)
    return candidate


def normalize_image_src(src: str) -> str | None:
    if not src or not src.strip():
        return None
    candidate = src.strip()
    if candidate.startswith("//"):
        candidate = "https:" + candidate
    parsed = urlparse(candidate)
    if parsed.scheme not in {"http", "https"}:
        return None
    return candidate


def download_image(src: str, dest_path: Path) -> None:
    request = urllib.request.Request(src, headers={"User-Agent": "techriders-wp-import/1.0"})
    with urllib.request.urlopen(request, timeout=30) as response:
        dest_path.write_bytes(response.read())


def process_post(post: dict, assets_root: Path, blob_base_url: str) -> dict:
    slug = post["slug"]
    title = strip_html(post.get("title", {}).get("rendered", ""))
    raw_html = post.get("content", {}).get("rendered", "")
    if not raw_html.strip():
        raise ValueError("El post no tiene contenido (content.rendered vacio)")

    soup = BeautifulSoup(raw_html, "html.parser")
    article_assets_dir = assets_root / slug
    seen_filenames: set[str] = set()
    image_manifest: list[dict] = []

    for img in list(soup.find_all("img")):
        src = img.get("src")
        normalized = normalize_image_src(src)
        if not normalized:
            img.decompose()
            continue

        try:
            filename = image_filename(normalized, seen_filenames)
            article_assets_dir.mkdir(parents=True, exist_ok=True)
            download_image(normalized, article_assets_dir / filename)
        except Exception:
            img.decompose()
            continue

        blob_path = f"knowledge/{slug}/{filename}"
        img["src"] = f"{blob_base_url.rstrip('/')}/{blob_path}"
        image_manifest.append({"originalUrl": normalized, "blobPath": blob_path})

    content_md = html_to_markdown(str(soup), heading_style="ATX")

    return {
        "title": title,
        "slug": slug,
        "contentBlobPath": f"knowledge/{slug}.md",
        "contentMd": content_md,
        "publishedAt": post.get("date_gmt") and f"{post['date_gmt']}Z",
        "categoryNames": embedded_terms(post, "category"),
        "skillNames": embedded_terms(post, "post_tag"),
        "sourceWpId": post.get("id"),
        "sourceUrl": post.get("link"),
        "images": image_manifest,
    }


def main() -> int:
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--raw-posts", required=True, help="Ruta al posts.json generado por fetch.py")
    parser.add_argument("--output-dir", required=True, help="Carpeta de salida (misma usada en fetch.py)")
    parser.add_argument("--storage-account-url", required=True, help="https://<cuenta>.blob.core.windows.net")
    parser.add_argument("--container", default="knowledge")
    args = parser.parse_args()

    output_dir = Path(args.output_dir)
    articles_dir = output_dir / "articles"
    assets_dir = output_dir / "assets"
    articles_dir.mkdir(parents=True, exist_ok=True)
    assets_dir.mkdir(parents=True, exist_ok=True)

    blob_base_url = f"{args.storage_account_url.rstrip('/')}/{args.container}"

    posts = json.loads(Path(args.raw_posts).read_text(encoding="utf-8"))

    manifest_items = []
    failures = []
    seen_slugs: dict[str, int] = {}

    for post in posts:
        slug = post.get("slug", "")
        try:
            if slug in seen_slugs:
                raise ValueError(f"Slug duplicado dentro del propio export de WordPress (post id {post.get('id')})")
            seen_slugs[slug] = post.get("id")

            result = process_post(post, assets_dir, blob_base_url)
            (articles_dir / f"{slug}.md").write_text(result.pop("contentMd"), encoding="utf-8")
            manifest_items.append(result)
        except Exception as ex:  # noqa: BLE001 - queremos capturar y reportar, no abortar el batch
            failures.append({"slug": slug, "sourceWpId": post.get("id"), "error": str(ex)})

    manifest = {"items": manifest_items}
    (output_dir / "manifest.json").write_text(json.dumps(manifest, ensure_ascii=False, indent=2), encoding="utf-8")

    summary = {
        "generatedAt": utc_now(),
        "totalPosts": len(posts),
        "transformed": len(manifest_items),
        "failed": len(failures),
        "failures": failures,
    }
    (output_dir / "import-summary.json").write_text(json.dumps(summary, ensure_ascii=False, indent=2), encoding="utf-8")

    print(f"Transformados {len(manifest_items)}/{len(posts)} posts. Fallos: {len(failures)}.")
    print(f"Siguiente paso manual: subir {articles_dir} y {assets_dir} al container '{args.container}' "
          f"respetando las rutas indicadas en manifest.json (knowledge/<slug>.md, knowledge/<slug>/<archivo>).")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
