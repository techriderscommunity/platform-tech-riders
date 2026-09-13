import { renderMarkdownArticle, resolveAssetUrl, rewriteHtmlAssetUrls } from './markdown-renderer';

describe('markdown-renderer', () => {
  const blobBaseUrl = 'https://storagetetxito.blob.core.windows.net/knowledge';
  const assetApiBaseUrl = '/api/knowledge-articles/assets';
  const slug = 'ecotaskapp-angular';

  describe('resolveAssetUrl', () => {
    it('should return absolute non-blob HTTP/HTTPS URLs unchanged', () => {
      const url = 'https://example.com/image.png';
      expect(resolveAssetUrl(url, slug, blobBaseUrl, assetApiBaseUrl)).toBe(url);
    });

    it('should proxy absolute blob storage URLs through the backend asset endpoint', () => {
      const url = 'https://storagetetxito.blob.core.windows.net/knowledge/knowledge/ecotaskapp-angular/diagram.png';
      expect(resolveAssetUrl(url, slug, blobBaseUrl, assetApiBaseUrl)).toBe(
        '/api/knowledge-articles/assets/knowledge/ecotaskapp-angular/diagram.png'
      );
    });

    it('should resolve relative images with slug prefix through the backend asset endpoint', () => {
      expect(resolveAssetUrl('image.png', slug, blobBaseUrl, assetApiBaseUrl)).toBe(
        '/api/knowledge-articles/assets/knowledge/ecotaskapp-angular/image.png'
      );
      expect(resolveAssetUrl('./assets/diagram.png', slug, blobBaseUrl, assetApiBaseUrl)).toBe(
        '/api/knowledge-articles/assets/knowledge/ecotaskapp-angular/assets/diagram.png'
      );
    });

    it('should handle knowledge/ and articles/ prefixes', () => {
      expect(resolveAssetUrl('knowledge/ecotaskapp-angular/arch.png', slug, blobBaseUrl, assetApiBaseUrl)).toBe(
        '/api/knowledge-articles/assets/knowledge/ecotaskapp-angular/arch.png'
      );
    });
  });

  describe('rewriteHtmlAssetUrls', () => {
    it('should rewrite <img> src attributes', () => {
      const html = '<p>Ejemplo <img src="screen.png" alt="Demo"></p>';
      const rewritten = rewriteHtmlAssetUrls(html, slug, blobBaseUrl, assetApiBaseUrl);
      expect(rewritten).toContain('src="/api/knowledge-articles/assets/knowledge/ecotaskapp-angular/screen.png"');
    });
  });

  describe('renderMarkdownArticle', () => {
    it('should render GFM markdown and resolve image assets', () => {
      const markdown = '# Titulo\n\nTexto con ![Captura](diagram.png)\n\n- Punto 1\n- Punto 2';
      const rendered = renderMarkdownArticle(markdown, slug, blobBaseUrl, assetApiBaseUrl, false);
      expect(rendered).toContain('Titulo</h1>');
      expect(rendered).toContain('src="/api/knowledge-articles/assets/knowledge/ecotaskapp-angular/diagram.png"');
      expect(rendered).toContain('<li>Punto 1</li>');
    });
  });
});
