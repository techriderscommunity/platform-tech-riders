import { ChangeDetectionStrategy, Component, DestroyRef, computed, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { takeUntilDestroyed, toObservable } from '@angular/core/rxjs-interop';
import { catchError, combineLatest, of, switchMap, tap } from 'rxjs';
import { EmbajadoresService } from '../embajadores/services/embajadores.service';
import { Embajador } from '../embajadores/models/embajadores.models';
import { Sesion } from './models/sesiones.models';
import { SesionesService } from './services/sesiones.service';
import { UiTextField  } from '@shared/ui/text-field/text-field';
import { UiSelect, UiSelectOption  } from '@shared/ui/select/select';

@Component({
  selector: 'app-sesiones',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [FormsModule, UiTextField, UiSelect],
  templateUrl: './sesiones.html',
  styleUrl: './sesiones.scss'
})
export class Sesiones {
  private readonly sesionesService = inject(SesionesService);
  private readonly embajadoresService = inject(EmbajadoresService);
  private readonly destroyRef = inject(DestroyRef);
  private readonly router = inject(Router);

  readonly filtroCentro = signal('');
  readonly filtroFecha = signal('');
  readonly filtroCategoria = signal('');
  readonly filtroEstado = signal('');
  readonly filtroAlumnosMin = signal<number | null>(null);
  readonly filtroAlumnosMax = signal<number | null>(null);
  readonly loading = signal(false);
  readonly error = signal<string | null>(null);

  readonly estados = ['Pendiente', 'Realizada', 'Cancelada'];
  readonly sesiones = signal<Sesion[]>([]);
  readonly embajadores = signal<Embajador[]>([]);

  readonly categorias = computed(() => {
    const categories = this.sesiones().map(s => s.categoria);
    return [...new Set(categories)].sort();
  });

  readonly categoriasOptions = computed<UiSelectOption[]>(() => [
    { label: 'Todas categorías', value: '' },
    ...this.categorias().map(cat => ({ label: cat, value: cat }))
  ]);

  readonly estadoOptions: UiSelectOption[] = [
    { label: 'Todos estados', value: '' },
    ...this.estados.map(est => ({ label: est, value: est }))
  ];

  readonly context = computed(() => {
    const url = this.router.url;

    if (url.includes('/intranet/fp-tour/mis-sesiones')) {
      return {
        title: 'FP Tour · Mis sesiones',
        subtitle: 'Solicitudes y sesiones de FP Tour, ordenadas por estado operativo.',
      };
    }

    return {
      title: 'Sesiones · Mis sesiones',
      subtitle: 'Listado general de sesiones (independientes y vinculadas a eventos).',
    };
  });

  constructor() {
    combineLatest([
      this.sesionesService.getSesiones(1, 100),
      this.embajadoresService.getEmbajadores(1, 100)
    ])
      .pipe(
        tap(() => {
          this.loading.set(true);
          this.error.set(null);
        }),
        catchError(() => {
          this.error.set('No se pudieron cargar las sesiones.');
          return of([
            { items: [] as Sesion[] },
            { items: [] as Embajador[] }
          ] as const);
        }),
        takeUntilDestroyed(this.destroyRef)
      )
      .subscribe(([sesionesResult, embajadoresResult]) => {
        this.sesiones.set(sesionesResult.items);
        this.embajadores.set(embajadoresResult.items);
        this.loading.set(false);
      });
  }

  readonly sesionesFiltradas = computed(() => {
    const centro = this.filtroCentro();
    const fecha = this.filtroFecha();
    const categoria = this.filtroCategoria();
    const estado = this.filtroEstado();
    const min = this.filtroAlumnosMin();
    const max = this.filtroAlumnosMax();

    return this.sesiones().filter(s => {
      const centroMatch = centro ? s.centro.toLowerCase().includes(centro.toLowerCase()) : true;
      const fechaMatch = fecha ? s.fecha === fecha : true;
      const categoriaMatch = categoria ? s.categoria === categoria : true;
      const estadoMatch = estado ? s.estado === estado : true;
      const alumnosMatch = (min !== null ? s.numAlumnos >= min : true) && (max !== null ? s.numAlumnos <= max : true);
      return centroMatch && fechaMatch && categoriaMatch && estadoMatch && alumnosMatch;
    });
  });

  getEmbajadorNombre(embajadorId: string | null): string {
    if (!embajadorId) return '';
    return this.embajadores().find(e => e.id === embajadorId)?.nombre ?? '';
  }
}


