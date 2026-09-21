import { Component, OnInit, ChangeDetectionStrategy, DestroyRef, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { signal, computed } from '@angular/core';
import { Router } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { EventosService } from './services/eventos.service';
import { tap, catchError } from 'rxjs/operators';
import { EMPTY } from 'rxjs';
import { UiButton  } from '@shared/ui/button/button';

export interface SesionEnEvento {
  id: string;
  titulo: string;
  fecha: string; // 'YYYY-MM-DD' — fecha dentro del evento
  horaInicio: string; // 'HH:mm'
  horaFin: string; // 'HH:mm'
  descripcion?: string;
  ponente?: string;
  sala?: string;
}

export interface EventoCalendario {
  id: string;
  titulo: string;
  fechaInicio: string; // 'YYYY-MM-DD'
  fechaFin: string; // 'YYYY-MM-DD'
  categoria: 'FPTour' | 'TajamarTech' | 'TechRiders' | 'Colaboradores';
  descripcion?: string;
  centro?: string;
  sesiones: SesionEnEvento[];
}

export interface SesionEnCarril {
  sesion: SesionEnEvento;
  evento: EventoCalendario;
  posicionX: number; // % de desplazamiento
  ancho: number; // % de ancho
  carril: number; // índice de fila en el día
}

export interface DiaEventos {
  fecha: string; // 'YYYY-MM-DD'
  diasEnEventoActual?: number; // qué día del evento es este
  sesionesEnCarril: SesionEnCarril[];
}

@Component({
  selector: 'app-calendar-eventos',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [CommonModule, UiButton],
  templateUrl: './calendar-eventos.html',
  styleUrl: './calendar-eventos.scss',
})
export class CalendarEventos implements OnInit {
  private readonly eventosService = inject(EventosService);
  private readonly destroyRef = inject(DestroyRef);
  private readonly router = inject(Router);

  readonly currentDate = signal(new Date());
  readonly eventos = signal<EventoCalendario[]>([]);
  readonly loading = signal(false);
  readonly error = signal<string | null>(null);

  // Categorías disponibles en orden
  readonly categorias = signal<Array<'FPTour' | 'TajamarTech' | 'TechRiders' | 'Colaboradores'>>([
    'FPTour',
    'TajamarTech',
    'TechRiders',
    'Colaboradores',
  ]);

  // Eventos agrupados por fecha en el mes actual
  readonly eventosAgrupados = computed(() => {
    const eventosList = this.eventos();
    const current = this.currentDate();
    const mes = current.getMonth();
    const año = current.getFullYear();

    // Obtener todos los días del mes actual
    const diasDelMes = new Date(año, mes + 1, 0).getDate();
    const dias: DiaEventos[] = [];

    for (let dia = 1; dia <= diasDelMes; dia++) {
      const fecha = new Date(año, mes, dia);
      const fechaStr = fecha.toISOString().split('T')[0];

      // Encontrar todas las sesiones que ocurren en este día
      const sesionesDelDia: SesionEnCarril[] = [];

      eventosList.forEach((evento) => {
        // Verificar si este evento incluye este día
        const fechaInicioEvento = new Date(evento.fechaInicio);
        const fechaFinEvento = new Date(evento.fechaFin);
        const fechaActual = new Date(año, mes, dia);

        if (fechaActual >= fechaInicioEvento && fechaActual <= fechaFinEvento) {
          // Este evento ocurre en este día, agregar sus sesiones
          evento.sesiones.forEach((sesion) => {
            if (sesion.fecha === fechaStr) {
              const posicion = this.calcularPosicionSesion(sesion);
              sesionesDelDia.push({
                sesion,
                evento,
                posicionX: posicion.posicionX,
                ancho: posicion.ancho,
                carril: posicion.carril,
              });
            }
          });
        }
      });

      if (sesionesDelDia.length > 0) {
        dias.push({
          fecha: fechaStr,
          sesionesEnCarril: sesionesDelDia,
        });
      }
    }

    return dias;
  });

  // Estadísticas
  readonly totalEventos = computed(() => this.eventos().length);

  readonly sesionesEstesMes = computed(() => {
    const eventosList = this.eventos();
    const current = this.currentDate();
    const mes = current.getMonth();
    const año = current.getFullYear();

    let totalSesiones = 0;
    eventosList.forEach((evento) => {
      evento.sesiones.forEach((sesion) => {
        const fechaSesion = new Date(sesion.fecha);
        if (fechaSesion.getMonth() === mes && fechaSesion.getFullYear() === año) {
          totalSesiones++;
        }
      });
    });
    return totalSesiones;
  });

  readonly mesActual = computed(() => {
    const date = this.currentDate();
    const meses = [
      'Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio',
      'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre',
    ];
    return `${meses[date.getMonth()]} ${date.getFullYear()}`;
  });

  readonly context = computed(() => {
    const url = this.router.url;

    if (url.includes('/intranet/events/mine')) {
      return {
        title: 'Eventos · Mis eventos',
        subtitle: 'Calendario operativo de eventos y sesiones vinculadas para tu participacion.',
        legend: 'Calendario de eventos y sesiones por dia.',
      };
    }

    return {
      title: 'Calendario · Vista unificada',
      subtitle: 'Vista consolidada de eventos, sesiones y actividad planificada de intranet.',
      legend: 'Calendario unificado por dia y franja horaria.',
    };
  });

  ngOnInit(): void {
    this.loadEventos();
  }

  private loadEventos(): void {
    this.loading.set(true);
    this.error.set(null);

    this.eventosService
      .getEventosConSesiones(1, 100)
      .pipe(
        tap((result) => {
          this.eventos.set(result.items);
          this.loading.set(false);
        }),
        catchError(() => {
          this.error.set('No se pudieron cargar los eventos. Comprueba la conexión con la API.');
          this.loading.set(false);
          return EMPTY;
        }),
        takeUntilDestroyed(this.destroyRef)
      )
      .subscribe();
  }

  /**
   * Calcula posición y ancho para una sesión (basado en horario del día)
   */
  private calcularPosicionSesion(sesion: SesionEnEvento): { posicionX: number; ancho: number; carril: number } {
    const [hInicio, mInicio] = sesion.horaInicio.split(':').map(Number);
    const [hFin, mFin] = sesion.horaFin.split(':').map(Number);

    const minInicio = hInicio * 60 + mInicio;
    const minFin = hFin * 60 + mFin;
    const minTotalesDia = 24 * 60;

    const posicionX = (minInicio / minTotalesDia) * 100;
    const duracion = minFin - minInicio;
    const ancho = (duracion / minTotalesDia) * 100;

    // Por ahora carril 0; puede extenderse para detectar solapamientos
    const carril = 0;

    return { posicionX, ancho, carril };
  }

  // Navegación
  mesAnterior(): void {
    const nueva = new Date(this.currentDate());
    nueva.setMonth(nueva.getMonth() - 1);
    this.currentDate.set(nueva);
  }

  mesSiguiente(): void {
    const nueva = new Date(this.currentDate());
    nueva.setMonth(nueva.getMonth() + 1);
    this.currentDate.set(nueva);
  }

  // Color por categoría
  getColorCategoria(categoria: string): string {
    const colores: Record<string, string> = {
      FPTour: 'var(--badge-info-bg)',
      TajamarTech: 'var(--badge-success-bg)',
      TechRiders: 'var(--badge-warning-bg)',
      Colaboradores: 'var(--badge-error-bg)',
    };
    return colores[categoria] || 'var(--bg-elevated)';
  }

  getColorCategoriaBorde(categoria: string): string {
    const colores: Record<string, string> = {
      FPTour: 'var(--badge-info-text)',
      TajamarTech: 'var(--badge-success-text)',
      TechRiders: 'var(--badge-warning-text)',
      Colaboradores: 'var(--badge-error-text)',
    };
    return colores[categoria] || 'var(--text-primary)';
  }
}




