import { remark } from 'remark';
import remarkGfm from 'remark-gfm';
import remarkRehype from 'remark-rehype';
import rehypeSlug from 'rehype-slug';
import rehypeExternalLinks from 'rehype-external-links';
import rehypeStringify from 'rehype-stringify';
import DOMPurify from 'dompurify';

export function resolveAssetUrl(src: string, slug: string, blobBaseUrl: string, assetApiBaseUrl: string): string {
  if (!src) return '';
  const trimmed = src.trim();

  const cleanBlobBase = blobBaseUrl.replace(/\/+$/, '');
  const cleanApiBase = assetApiBaseUrl.replace(/\/+$/, '');

  // El storage de blobs no permite acceso publico (ver PublicAccessNotPermitted): las URLs absolutas
  // ya horneadas en el contenido migrado de WordPress deben reescribirse al proxy autenticado del backend.
  if (trimmed.startsWith(`${cleanBlobBase}/`)) {
    const blobPath = trimmed.slice(cleanBlobBase.length + 1);
    return `${cleanApiBase}/${blobPath}`;
  }

  if (/^(https?:|data:|\/\/)/i.test(trimmed)) {
    return trimmed;
  }

  const cleanPath = trimmed.replace(/^\.\//, '').replace(/^\//, '');

  if (cleanPath.startsWith('knowledge/') || cleanPath.startsWith('articles/')) {
    return `${cleanApiBase}/${cleanPath}`;
  }

  if (slug && cleanPath.startsWith(`${slug}/`)) {
    return `${cleanApiBase}/knowledge/${cleanPath}`;
  }

  return `${cleanApiBase}/knowledge/${slug}/${cleanPath}`;
}

export function rewriteHtmlAssetUrls(html: string, slug: string, blobBaseUrl: string, assetApiBaseUrl: string): string {
  if (!html) return '';

  let processed = html.replace(/<img\s+([^>]*?)src=["']([^"']+)["']([^>]*?)>/gi, (match, prefix, src, suffix) => {
    const resolved = resolveAssetUrl(src, slug, blobBaseUrl, assetApiBaseUrl);
    return `<img ${prefix}src="${resolved}" ${suffix}>`;
  });

  processed = processed.replace(/<a\s+([^>]*?)href=["']([^"']+)["']([^>]*?)>/gi, (match, prefix, href, suffix) => {
    if (!href || /^(https?:|mailto:|tel:|#|\/\/)/i.test(href)) {
      return match;
    }
    const resolved = resolveAssetUrl(href, slug, blobBaseUrl, assetApiBaseUrl);
    return `<a ${prefix}href="${resolved}" target="_blank" rel="noopener noreferrer" ${suffix}>`;
  });

  return processed;
}

export function renderMarkdownArticle(
  markdown: string,
  slug: string,
  blobBaseUrl: string,
  assetApiBaseUrl: string,
  isBrowser: boolean = true
): string {
  if (!markdown || !markdown.trim()) {
    return '<p>Este artículo todavía no tiene contenido disponible.</p>';
  }

  let rawHtml = '';
  try {
    const file = remark()
      .use(remarkGfm)
      .use(remarkRehype, { allowDangerousHtml: true })
      .use(rehypeSlug)
      .use(rehypeExternalLinks, { target: '_blank', rel: ['noopener', 'noreferrer'] })
      .use(rehypeStringify, { allowDangerousHtml: true })
      .processSync(markdown);

    rawHtml = String(file.value);
  } catch {
    rawHtml = `<p>${markdown.replace(/\n\n/g, '</p><p>').replace(/\n/g, '<br>')}</p>`;
  }

  const htmlWithAssets = rewriteHtmlAssetUrls(rawHtml, slug, blobBaseUrl, assetApiBaseUrl);

  if (isBrowser && typeof window !== 'undefined') {
    return DOMPurify.sanitize(htmlWithAssets, {
      ADD_ATTR: ['target', 'rel'],
      ADD_TAGS: ['iframe', 'img']
    });
  }

  return htmlWithAssets;
}
