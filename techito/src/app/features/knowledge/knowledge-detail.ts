import { ChangeDetectionStrategy, Component, DestroyRef, PLATFORM_ID, computed, inject, signal } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { catchError, of, switchMap, tap } from 'rxjs';
import { DomSanitizer } from '@angular/platform-browser';
import { KnowledgeService } from './services/knowledge.service';
import { KnowledgeArticleDetailApi } from './models/knowledge.models';
import { renderMarkdownArticle } from './utils/markdown-renderer';
import { environment } from '@env/environment';

@Component({
  selector: 'app-knowledge-detail',
  standalone: true,
  imports: [RouterLink],
  template: `
    <main class="knowledge-detail-page">
      <nav class="knowledge-detail-breadcrumb">
        <a routerLink="/knowledge">← Volver a conocimiento</a>
      </nav>

      @if (loading()) {
        <p class="knowledge-detail-loading">Cargando artículo...</p>
      } @else if (error()) {
        <section class="knowledge-detail-error">
          <h1>Artículo no disponible</h1>
          <p>{{ error() }}</p>
          <a routerLink="/knowledge">Ver todos los artículos</a>
        </section>
      } @else if (article(); as currentArticle) {
        <article class="knowledge-detail-article">
          <header class="knowledge-detail-header">
            <p class="knowledge-detail-kicker">{{ currentArticle.categories.join(' · ') || 'Knowledge' }}</p>
            <h1>{{ currentArticle.title }}</h1>
            <div class="knowledge-detail-meta">
              <span>{{ currentArticle.authorName || 'TechRiders' }}</span>
              <span>·</span>
              <time>{{ formatDate(currentArticle.publishedAt) }}</time>
            </div>
          </header>

          <div class="knowledge-detail-content" [innerHTML]="renderedContent()"></div>
        </article>
      }

      <a routerLink="/knowledge" class="knowledge-detail-fab" aria-label="Volver al buscador de conocimiento" title="Volver al buscador">
        ↑ Buscador
      </a>
    </main>
  `,
  styles: [
    `
      :host { display: block; }
      .knowledge-detail-page { max-width: 880px; margin: 0 auto; padding: 2rem 1.25rem 4rem; color: #e7edf7; }
      .knowledge-detail-breadcrumb { margin-bottom: 1.5rem; }
      .knowledge-detail-breadcrumb a { color: #80c7ff; text-decoration: none; font-weight: 500; }
      .knowledge-detail-breadcrumb a:hover { text-decoration: underline; }
      .knowledge-detail-header { margin-bottom: 2rem; border-bottom: 1px solid rgba(146, 168, 215, 0.15); padding-bottom: 1.5rem; }
      .knowledge-detail-kicker { text-transform: uppercase; letter-spacing: 0.12em; color: #8ab4ff; font-size: 0.75rem; font-weight: 700; margin: 0 0 0.75rem; }
      .knowledge-detail-header h1 { margin: 0; font-size: clamp(2rem, 5vw, 2.75rem); line-height: 1.2; font-weight: 800; color: #ffffff; }
      .knowledge-detail-meta { display: flex; gap: 0.5rem; color: #9fb5d9; margin-top: 1rem; font-size: 0.9rem; flex-wrap: wrap; }
      .knowledge-detail-loading { font-size: 1.1rem; color: #9fb5d9; }
      .knowledge-detail-content { background: rgba(14, 19, 30, 0.65); border: 1px solid rgba(146, 168, 215, 0.15); border-radius: 18px; padding: 2rem; line-height: 1.8; font-size: 1.05rem; }
      .knowledge-detail-content :is(h1, h2, h3, h4, h5, h6) { color: #f4f7ff; margin-top: 2rem; margin-bottom: 0.75rem; line-height: 1.3; }
      .knowledge-detail-content h1 { font-size: 1.8rem; }
      .knowledge-detail-content h2 { font-size: 1.5rem; border-bottom: 1px solid rgba(146, 168, 215, 0.1); padding-bottom: 0.5rem; }
      .knowledge-detail-content h3 { font-size: 1.25rem; }
      .knowledge-detail-content p, .knowledge-detail-content li { color: #dfe9fb; margin-bottom: 1rem; }
      .knowledge-detail-content ul, .knowledge-detail-content ol { padding-left: 1.5rem; margin-bottom: 1.25rem; }
      .knowledge-detail-content a { color: #8ab4ff; text-decoration: underline; text-underline-offset: 2px; }
      .knowledge-detail-content img { max-width: 100%; height: auto; border-radius: 12px; margin: 1.5rem 0; display: block; border: 1px solid rgba(146, 168, 215, 0.15); box-shadow: 0 8px 24px rgba(0, 0, 0, 0.35); }
      .knowledge-detail-content pre { overflow-x: auto; background: #0b0f19; border: 1px solid rgba(146, 168, 215, 0.15); padding: 1.25rem; border-radius: 12px; margin: 1.5rem 0; }
      .knowledge-detail-content code { font-family: ui-monospace, SFMono-Regular, Menlo, Monaco, Consolas, monospace; font-size: 0.9em; background: rgba(138, 180, 255, 0.1); padding: 0.2rem 0.4rem; border-radius: 4px; color: #a5c7ff; }
      .knowledge-detail-content pre code { background: none; padding: 0; color: #e7edf7; }
      .knowledge-detail-content blockquote { border-left: 4px solid #8ab4ff; margin: 1.5rem 0; padding: 0.75rem 1.25rem; background: rgba(138, 180, 255, 0.05); border-radius: 0 8px 8px 0; color: #b8d2ff; }
      .knowledge-detail-content table { width: 100%; border-collapse: collapse; margin: 1.5rem 0; font-size: 0.95rem; }
      .knowledge-detail-content th, .knowledge-detail-content td { border: 1px solid rgba(146, 168, 215, 0.2); padding: 0.75rem 1rem; text-align: left; }
      .knowledge-detail-content th { background: rgba(138, 180, 255, 0.1); color: #f4f7ff; }
      .knowledge-detail-error { background: rgba(140, 24, 24, 0.15); border: 1px solid rgba(230, 115, 115, 0.35); border-radius: 18px; padding: 2rem; margin-top: 1rem; }
      .knowledge-detail-error h1 { margin-top: 0; color: #ff8a8a; }
      .knowledge-detail-error a { color: #80c7ff; font-weight: 500; }
      .knowledge-detail-fab {
        position: fixed;
        right: 1.5rem;
        bottom: 1.5rem;
        z-index: 20;
        display: inline-flex;
        align-items: center;
        gap: 0.4rem;
        padding: 0.75rem 1.25rem;
        border-radius: 999px;
        background: #1c6dd0;
        color: #ffffff;
        text-decoration: none;
        font-weight: 600;
        font-size: 0.9rem;
        box-shadow: 0 10px 24px rgba(0, 0, 0, 0.35);
        transition: transform 0.15s ease, background 0.15s ease;
      }
      .knowledge-detail-fab:hover { background: #1558a8; transform: translateY(-2px); }
    `,
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class KnowledgeDetail {
  private readonly route = inject(ActivatedRoute);
  private readonly destroyRef = inject(DestroyRef);
  private readonly knowledgeService = inject(KnowledgeService);
  private readonly sanitizer = inject(DomSanitizer);
  private readonly platformId = inject(PLATFORM_ID);

  readonly slug = signal('');
  readonly loading = signal(true);
  readonly error = signal<string | null>(null);
  readonly article = signal<KnowledgeArticleDetailApi | null>(null);

  readonly renderedContent = computed(() => {
    const content = this.article()?.contentMd ?? '';
    const currentSlug = this.slug();
    const isBrowser = isPlatformBrowser(this.platformId);
    const blobBaseUrl = environment.blobStorageUrl || 'https://storagetetxito.blob.core.windows.net/knowledge';
    const assetApiBaseUrl = `${environment.apiUrl}/knowledge-articles/assets`;

    const html = renderMarkdownArticle(content, currentSlug, blobBaseUrl, assetApiBaseUrl, isBrowser);
    return this.sanitizer.bypassSecurityTrustHtml(html);
  });

  constructor() {
    this.route.paramMap
      .pipe(
        switchMap((params) => {
          const currentSlug = params.get('slug') ?? '';
          this.slug.set(currentSlug);
          if (!currentSlug) {
            this.loading.set(false);
            this.error.set('No se encontró el artículo solicitado.');
            return of(null);
          }

          this.loading.set(true);
          this.error.set(null);

          return this.knowledgeService.getKnowledgeBySlug(currentSlug).pipe(
            tap((payload) => {
              this.article.set(payload);
            }),
            catchError(() => {
              this.error.set('No se pudo cargar el contenido del artículo.');
              this.article.set(null);
              return of(null);
            })
          );
        }),
        takeUntilDestroyed(this.destroyRef)
      )
      .subscribe(() => {
        this.loading.set(false);
      });
  }

  formatDate(date: string | null): string {
    if (!date) {
      return 'Sin fecha';
    }

    const parsed = new Date(date);
    return Number.isNaN(parsed.getTime()) ? date : parsed.toLocaleDateString('es-ES', { day: '2-digit', month: 'short', year: 'numeric' });
  }
}

