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
  readonly feedback = signal<string | null>(null);
  readonly eventos = signal<Sesion[]>([]);
  readonly actingId = signal<string | null>(null);

  readonly eventosProgramados = computed(() => this.eventos().filter(s => s.estado === 'Pendiente').length);
  readonly totalAsistentes = computed(() => this.eventos().reduce((sum, s) => sum + s.numAlumnos, 0));
  readonly context = computed(() => {
    const url = this.router.url;

    if (url.includes('/intranet/fp-tour/organizations')) {
      return {
        title: 'FP Tour · Organizaciones',
        subtitle: 'Solicitudes por organización con prioridad en pendientes y gestión de estados.',
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

  publicarEvento(id: string) {
    this.actingId.set(id);
    this.sesionesService.publish(id)
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: () => {
          this.eventos.update(items => items.map(e => e.id === id ? { ...e, estado: 'Realizada' } : e));
          this.feedback.set('Sesión publicada correctamente.');
          this.actingId.set(null);
        },
        error: () => {
          this.feedback.set('No se pudo publicar la sesión.');
          this.actingId.set(null);
        },
      });
  }

  asignarPonente(id: string) {
    const userId = prompt('Introduce el Id (GUID) del usuario a asignar como ponente:');
    if (!userId) {
      return;
    }

    this.actingId.set(id);
    this.sesionesService.addSpeaker(id, userId.trim(), true)
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: () => {
          this.feedback.set('Ponente asignado correctamente.');
          this.actingId.set(null);
        },
        error: (error) => {
          this.feedback.set(error?.error?.Message ?? 'No se pudo asignar el ponente.');
          this.actingId.set(null);
        },
      });
  }

  cancelarEvento(id: string) {
    if (!confirm('¿Confirmas cancelar esta sesión?')) {
      return;
    }

    this.actingId.set(id);
    this.sesionesService.cancel(id)
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: () => {
          this.eventos.update(items => items.map(e => e.id === id ? { ...e, estado: 'Cancelada' } : e));
          this.feedback.set('Sesión cancelada correctamente.');
          this.actingId.set(null);
        },
        error: () => {
          this.feedback.set('No se pudo cancelar la sesión.');
          this.actingId.set(null);
        },
      });
  }

  limpiarFeedback() {
    this.feedback.set(null);
  }
}




