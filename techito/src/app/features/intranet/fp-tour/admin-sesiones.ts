import { ChangeDetectionStrategy, Component, DestroyRef, computed, inject, signal } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { catchError, of, tap } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { Sesion } from './models/sesiones.models';
import { SesionesService } from './services/sesiones.service';
import { UiButton  } from '@shared/ui/button/button';

@Component({
  selector: 'app-admin-sesiones',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [RouterLink, UiButton],
  templateUrl: './admin-sesiones.html',
  styleUrl: './admin-sesiones.scss'
})
export class AdminSesiones {
  private readonly sesionesService = inject(SesionesService);
  private readonly destroyRef = inject(DestroyRef);
  private readonly router = inject(Router);

  readonly loading = signal(false);
  readonly error = signal<string | null>(null);
  readonly eventos = signal<Sesion[]>([]);

  readonly eventosProgramados = computed(() => this.eventos().filter(s => s.estado === 'Pendiente').length);
  readonly totalAsistentes = computed(() => this.eventos().reduce((sum, s) => sum + s.numAlumnos, 0));
  readonly context = computed(() => {
    const url = this.router.url;

    if (url.includes('/intranet/fp-tour/centers')) {
      return {
        title: 'FP Tour · Centros',
        subtitle: 'Solicitudes por centro con prioridad en pendientes y gestion de estados.',
        createLabel: '➕ Solicitar sesion',
      };
    }

    if (url.includes('/intranet/events/management')) {
      return {
        title: 'Eventos · Gestion',
        subtitle: 'Panel operativo para alta, edicion y seguimiento de eventos internos y externos.',
        createLabel: '➕ Crear evento',
      };
    }

    if (url.includes('/intranet/administration/centers')) {
      return {
        title: 'Administracion · Centros',
        subtitle: 'Control maestro de centros y su actividad operativa en la intranet.',
        createLabel: '➕ Alta de centro',
      };
    }

    return {
      title: 'FP Tour · Gestion',
      subtitle: 'Administra sesiones y solicitudes operativas dentro de Intranet.',
      createLabel: '➕ Crear registro',
    };
  });

  constructor() {
    this.sesionesService.getSesiones(1, 100)
      .pipe(
        tap(() => {
          this.loading.set(true);
          this.error.set(null);
        }),
        catchError(() => {
          this.error.set('No se pudieron cargar los eventos de administracion.');
          return of({ items: [] as Sesion[] });
        }),
        takeUntilDestroyed(this.destroyRef)
      )
      .subscribe(result => {
        this.eventos.set(result.items);
        this.loading.set(false);
      });
  }

  crearEvento() {
    // TODO: conectar creacion real de evento.
  }

  editarEvento(_: string) {
    // TODO: conectar edicion real de evento.
  }

  verAsistentes(_: string) {
    // TODO: conectar vista detallada de asistentes.
  }

  cancelarEvento(_: string) {
    // TODO: conectar cancelacion real de evento.
  }
}




