import { ChangeDetectionStrategy, Component, DestroyRef, computed, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { catchError, combineLatest, of, tap } from 'rxjs';
import { AuthService } from '@core/auth/auth.service';
import { EmbajadoresService } from '../embajadores/services/embajadores.service';
import { Embajador } from '../embajadores/models/embajadores.models';
import { Sesion } from './models/sesiones.models';
import { SesionesService } from './services/sesiones.service';
import { UiTextField  } from '@shared/ui/text-field/text-field';
import { UiSelect, UiSelectOption  } from '@shared/ui/select/select';
import { UiButton } from '@shared/ui/button/button';

@Component({
  selector: 'app-sesiones',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [FormsModule, UiTextField, UiSelect, UiButton],
  templateUrl: './sesiones.html',
  styleUrl: './sesiones.scss'
})
export class Sesiones {
  private readonly authService = inject(AuthService);
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
  readonly success = signal<string | null>(null);
  readonly sessionOverrides = signal<Record<string, Partial<Sesion>>>({});

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

    this.sesionesService.getSessionWorkflow()
      .pipe(
        tap(actions => {
          const mapped = Object.fromEntries(Object.entries(actions).map(([key, value]) => [key, {
            estado: value.Status,
            embajadorAsignadoId: value.AmbassadorAssignedId ?? null,
          }]));
          this.sessionOverrides.update(current => ({ ...current, ...mapped }));
        }),
        catchError(() => of(null))
      )
      .subscribe();
  }

  readonly sesionesBase = computed(() => this.sesiones().map(session => ({
    ...session,
    ...(this.sessionOverrides()[session.id] ?? {})
  })));

  readonly resumen = computed(() => {
    const sesiones = this.sesionesBase();
    return {
      total: sesiones.length,
      pendientes: sesiones.filter(item => item.estado === 'Pendiente').length,
      confirmadas: sesiones.filter(item => item.estado === 'Confirmada').length,
      canceladas: sesiones.filter(item => item.estado === 'Cancelada').length,
    };
  });

  readonly sesionesFiltradas = computed(() => {
    const centro = this.filtroCentro();
    const fecha = this.filtroFecha();
    const categoria = this.filtroCategoria();
    const estado = this.filtroEstado();
    const min = this.filtroAlumnosMin();
    const max = this.filtroAlumnosMax();

    return this.sesionesBase().filter(s => {
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

  canSelfAssign(): boolean {
    return this.authService.hasRole(['embajador', 'colaborador']);
  }

  canManageSessions(): boolean {
    return this.authService.hasRole(['superadmin', 'staff', 'coordinador']);
  }

  asignarmeSesion(id: string) {
    const ownEmbajador = this.resolveCurrentEmbajador();
    if (!ownEmbajador) {
      this.success.set('No se encontró un perfil Ambassador asociado a tu usuario actual.');
      return;
    }

    this.patchSession(id, {
      embajadorAsignadoId: ownEmbajador.id,
      estado: 'Confirmada'
    }, 'Te has asignado la sesión en local para el MVP.');
  }

  liberarSesion(id: string) {
    this.patchSession(id, {
      embajadorAsignadoId: null,
      estado: 'Pendiente'
    }, 'La sesión vuelve a estado pendiente en local para el MVP.');
  }

  confirmarSesion(id: string) {
    this.patchSession(id, { estado: 'Confirmada' }, 'Sesión marcada como confirmada en local para el MVP.');
  }

  cancelarSesion(id: string) {
    this.patchSession(id, { estado: 'Cancelada' }, 'Sesión marcada como cancelada en local para el MVP.');
  }

  private patchSession(id: string, patch: Partial<Sesion>, successMessage: string) {
    this.sesionesService.updateSessionWorkflow(id, {
      status: patch.estado,
      ambassadorAssignedId: patch.embajadorAsignadoId ?? null,
    })
      .pipe(
        tap(() => {
          this.sessionOverrides.update(current => ({
            ...current,
            [id]: {
              ...(current[id] ?? {}),
              ...patch,
            }
          }));
          this.success.set(successMessage);
          this.error.set(null);
        }),
        catchError(() => {
          this.error.set('No se pudo guardar el estado de la sesión en backend.');
          return of(null);
        }),
        takeUntilDestroyed(this.destroyRef)
      )
      .subscribe();
  }

  private resolveCurrentEmbajador(): Embajador | null {
    const userEmail = this.authService.user()?.email?.toLowerCase();
    if (!userEmail) {
      return this.embajadores()[0] ?? null;
    }

    return this.embajadores().find(item => item.email.toLowerCase() === userEmail) ?? this.embajadores()[0] ?? null;
  }

}


