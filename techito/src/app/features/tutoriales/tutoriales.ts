import { ChangeDetectionStrategy, Component, DestroyRef, computed, inject, signal, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { takeUntilDestroyed, toObservable } from '@angular/core/rxjs-interop';
import { catchError, of, switchMap, tap } from 'rxjs';
import { TutorialesService } from './services/tutoriales.service';
import { PagedResult, Tutorial } from './models/tutoriales.models';
import { PublicContentService } from '@core/content/public-content.service';
import { UiTextField  } from '@shared/ui/text-field/text-field';
import { UiMetricsStrip } from '@shared/ui/metrics-strip/metrics-strip';
import { UiResourceCardItem, UiResourceCards } from '@shared/ui/resource-cards/resource-cards';

@Component({
  selector: 'app-tutoriales',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [UiTextField, UiMetricsStrip, UiResourceCards],
  templateUrl: './tutoriales.html',
  styleUrl: './tutoriales.scss'
})

export class Tutoriales {
  readonly destroyRef = inject(DestroyRef);
  readonly tutorialesService = inject(TutorialesService);
  readonly publicContentService = inject(PublicContentService);
  private readonly platformId = inject(PLATFORM_ID);

  featuredCategories: string[] = [];

  readonly pageSize = 12;
  readonly selectedCategoria = signal('');
  readonly searchText = signal('');
  readonly currentPage = signal(1);
  readonly loadingTutoriales = signal(false);
  readonly tutorialesError = signal<string | null>(null);
  readonly tutorialesPaged = signal<PagedResult<Tutorial>>({
    items: [],
    totalCount: 0,
    page: 1,
    pageSize: this.pageSize,
    totalPages: 0,
    hasNextPage: false,
    hasPreviousPage: false
  });

  readonly tutorialesQuery = computed(() => ({
    page: this.currentPage(),
    pageSize: this.pageSize,
    categoria: this.selectedCategoria() || undefined,
    busqueda: this.searchText().trim() || undefined
  }));

  readonly tutorialesMetrics = computed(() => [
    { icon: '📚', value: String(this.tutorialesPaged().totalCount), label: 'Recursos disponibles' },
    { icon: '🏷️', value: String(this.featuredCategories.length), label: 'Categorías destacadas' },
    { icon: '🔎', value: this.searchText().trim() ? 'Activa' : 'General', label: 'Búsqueda' },
  ]);

  readonly activeFilterLabel = computed(() => this.selectedCategoria() || 'Todos');

  readonly tutorialCards = computed<UiResourceCardItem[]>(() => this.tutorialesPaged().items.map(tutorial => ({
    mode: tutorial.categorias[0] || 'Digital',
    title: tutorial.titulo,
    summary: tutorial.extracto,
    tags: tutorial.categorias,
    meta: `${this.formatFecha(tutorial.fechaPublicacion)} · ${tutorial.autor}`,
    ctaLabel: 'Registro y detalles',
    ctaHref: tutorial.url
  })));

  private readonly dateFormatter = new Intl.DateTimeFormat('es-ES', {
    day: '2-digit',
    month: 'short',
    year: 'numeric'
  });

  constructor() {
    this.publicContentService
      .getPublicContent()
      .pipe(
        tap((content) => {
          this.featuredCategories = content.tutorials.featuredCategories;
        }),
        catchError(() => of(null)),
        takeUntilDestroyed(this.destroyRef)
      )
      .subscribe();

    if (isPlatformBrowser(this.platformId)) toObservable(this.tutorialesQuery)
      .pipe(
        tap(() => {
          this.loadingTutoriales.set(true);
          this.tutorialesError.set(null);
        }),
        switchMap(query => this.tutorialesService.getTutoriales(query).pipe(
          catchError(() => {
            this.tutorialesError.set('No se pudieron cargar los tutoriales.');
            return of({
              items: [],
              totalCount: 0,
              page: query.page,
              pageSize: query.pageSize,
              totalPages: 0,
              hasNextPage: false,
              hasPreviousPage: false
            } satisfies PagedResult<Tutorial>);
          })
        )),
        takeUntilDestroyed(this.destroyRef)
      )
      .subscribe(result => {
        this.tutorialesPaged.set(result);
        this.loadingTutoriales.set(false);
      });
  }

  setCategoria(categoria: string): void {
    this.selectedCategoria.set(categoria);
    this.currentPage.set(1);
  }

  updateSearch(term: string): void {
    this.searchText.set(term);
    this.currentPage.set(1);
  }

  nextTutorialesPage(): void {
    if (this.tutorialesPaged().hasNextPage) {
      this.currentPage.update(page => page + 1);
    }
  }

  prevTutorialesPage(): void {
    if (this.tutorialesPaged().hasPreviousPage) {
      this.currentPage.update(page => page - 1);
    }
  }

  formatFecha(fechaPublicacion: string): string {
    const date = new Date(fechaPublicacion);
    return Number.isNaN(date.getTime()) ? fechaPublicacion : this.dateFormatter.format(date);
  }
}


