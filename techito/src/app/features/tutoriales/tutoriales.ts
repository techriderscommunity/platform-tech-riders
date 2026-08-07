import { ChangeDetectionStrategy, Component, DestroyRef, computed, inject, signal, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { takeUntilDestroyed, toObservable } from '@angular/core/rxjs-interop';
import { catchError, of, switchMap, tap } from 'rxjs';
import { TutorialesService } from './services/tutoriales.service';
import { PagedResult, Tutorial } from './models/tutoriales.models';
import { UiTextField  } from '@shared/ui/text-field/text-field';

@Component({
  selector: 'app-tutoriales',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [UiTextField],
  templateUrl: './tutoriales.html',
  styleUrl: './tutoriales.scss'
})

export class Tutoriales {
  readonly destroyRef = inject(DestroyRef);
  readonly tutorialesService = inject(TutorialesService);
  private readonly platformId = inject(PLATFORM_ID);

  readonly featuredCategories = [
    'Azure', '.NET', 'C#', 'Desarrollo', 'Windows Server',
    'Docker', 'Kubernetes', 'Full Stack', 'Seguridad'
  ];

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

  private readonly dateFormatter = new Intl.DateTimeFormat('es-ES', {
    day: '2-digit',
    month: 'short',
    year: 'numeric'
  });

  constructor() {
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


