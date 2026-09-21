"""Descarga (one-shot) los posts publicados de WordPress via wp-json.

No requiere dependencias externas (solo stdlib). Pagina wp/v2/posts?_embed
siguiendo el header X-WP-TotalPages y vuelca el JSON crudo a disco para que
transform.py lo procese por separado (permite reintentar la transformacion
sin volver a golpear el sitio de WordPress).

Uso:
    python fetch.py --wp-url https://techriders.tajamar.es --output-dir repo-intake/generated/wp-import/2026-09-09
"""
import argparse
import json
import urllib.request
import urllib.error
from pathlib import Path
from datetime import datetime, timezone

PER_PAGE = 100


def utc_now() -> str:
    return datetime.now(timezone.utc).isoformat()


def fetch_page(wp_url: str, page: int) -> tuple[list[dict], int]:
    url = f"{wp_url.rstrip('/')}/wp-json/wp/v2/posts?per_page={PER_PAGE}&page={page}&_embed=1&status=publish"
    request = urllib.request.Request(url, headers={"Accept": "application/json"})
    with urllib.request.urlopen(request, timeout=30) as response:
        total_pages = int(response.headers.get("X-WP-TotalPages", "1"))
        body = json.loads(response.read().decode("utf-8"))
        return body, total_pages


def fetch_all_posts(wp_url: str) -> list[dict]:
    posts: list[dict] = []
    page = 1
    while True:
        try:
            batch, total_pages = fetch_page(wp_url, page)
        except urllib.error.HTTPError as ex:
            if ex.code == 400 and page > 1:
                # WordPress devuelve 400 "rest_post_invalid_page_number" al pasarse de la ultima pagina.
                break
            raise
        if not batch:
            break
        posts.extend(batch)
        if page >= total_pages:
            break
        page += 1
    return posts


def main() -> int:
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--wp-url", required=True, help="Base URL de WordPress, ej. https://techriders.tajamar.es")
    parser.add_argument("--output-dir", required=True, help="Carpeta de salida (se crea si no existe)")
    args = parser.parse_args()

    output_dir = Path(args.output_dir)
    (output_dir / "raw").mkdir(parents=True, exist_ok=True)

    posts = fetch_all_posts(args.wp_url)

    raw_path = output_dir / "raw" / "posts.json"
    raw_path.write_text(json.dumps(posts, ensure_ascii=False, indent=2), encoding="utf-8")

    print(f"[{utc_now()}] Descargados {len(posts)} posts publicados -> {raw_path}")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
