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

  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  private _eventosMockLegacy(): void {
    // Mock eliminado — datos cargados desde /api/events/with-sessions
    const eventosMock: EventoCalendario[] = [
      {
        id: '1',
        titulo: '#FPTour - Tour by Tajamar',
        fechaInicio: '2026-05-24',
        fechaFin: '2026-05-24',
        categoria: 'FPTour',
        centro: 'IES Tajamar',
        descripcion: 'Jornada de orientación profesional',
        sesiones: [
          {
            id: '1-1',
            titulo: 'Charla: Node.js en Producción',
            fecha: '2026-05-24',
            horaInicio: '09:00',
            horaFin: '11:00',
            ponente: 'Carlos López',
            sala: 'Aula A',
          },
          {
            id: '1-2',
            titulo: 'Panel de Experiencias',
            fecha: '2026-05-24',
            horaInicio: '11:30',
            horaFin: '13:00',
            ponente: 'Varios',
            sala: 'Aula A',
          },
        ],
      },
      {
        id: '2',
        titulo: '#TajamarTech Hackathon',
        fechaInicio: '2026-05-24',
        fechaFin: '2026-05-25',
        categoria: 'TajamarTech',
        centro: 'IES Tajamar - Aula Lab',
        descripcion: 'Competición de desarrollo de 24 horas',
        sesiones: [
          {
            id: '2-1',
            titulo: 'Presentación de retos',
            fecha: '2026-05-24',
            horaInicio: '09:00',
            horaFin: '10:00',
            ponente: 'Organizadores',
            sala: 'Aula Lab',
          },
          {
            id: '2-2',
            titulo: 'Desarrollo en equipos',
            fecha: '2026-05-24',
            horaInicio: '10:00',
            horaFin: '18:00',
            ponente: 'Equipos participantes',
            sala: 'Aula Lab',
          },
          {
            id: '2-3',
            titulo: 'Continuación Hackathon',
            fecha: '2026-05-25',
            horaInicio: '09:00',
            horaFin: '12:00',
            ponente: 'Equipos participantes',
            sala: 'Aula Lab',
          },
          {
            id: '2-4',
            titulo: 'Presentaciones finales y premiación',
            fecha: '2026-05-25',
            horaInicio: '14:00',
            horaFin: '16:00',
            ponente: 'Jurado',
            sala: 'Aula A',
          },
        ],
      },
      {
        id: '3',
        titulo: '#TechRiders Summit 2026',
        fechaInicio: '2026-05-24',
        fechaFin: '2026-05-26',
        categoria: 'TechRiders',
        centro: 'Centro de Convenciones',
        descripcion: 'Conferencia de tres días sobre tendencias tech',
        sesiones: [
          {
            id: '3-1',
            titulo: 'Keynote: El futuro del AI',
            fecha: '2026-05-24',
            horaInicio: '10:00',
            horaFin: '11:30',
            ponente: 'Dr. María García',
            sala: 'Salón Principal',
          },
          {
            id: '3-2',
            titulo: 'Taller: Angular 18+ Signals',
            fecha: '2026-05-24',
            horaInicio: '12:00',
            horaFin: '14:00',
            ponente: 'Miguel Ruiz',
            sala: 'Sala B',
          },
          {
            id: '3-3',
            titulo: 'Demo: Cloud-Native Architecture',
            fecha: '2026-05-25',
            horaInicio: '10:00',
            horaFin: '11:00',
            ponente: 'Alex Turner',
            sala: 'Salón Principal',
          },
          {
            id: '3-4',
            titulo: 'Networking Lunch',
            fecha: '2026-05-25',
            horaInicio: '13:00',
            horaFin: '14:30',
            ponente: 'Community',
            sala: 'Cafetería',
          },
        ],
      },
      {
        id: '4',
        titulo: '#Colaboradores - Partner Day',
        fechaInicio: '2026-05-26',
        fechaFin: '2026-05-26',
        categoria: 'Colaboradores',
        centro: 'Oficinas centrales',
        descripcion: 'Reunión con empresas colaboradoras',
        sesiones: [
          {
            id: '4-1',
            titulo: 'Presentación de partners',
            fecha: '2026-05-26',
            horaInicio: '09:30',
            horaFin: '10:30',
            ponente: 'Director',
            sala: 'Sala de Juntas',
          },
          {
            id: '4-2',
            titulo: 'Mesa redonda: Oportunidades 2026',
            fecha: '2026-05-26',
            horaInicio: '11:00',
            horaFin: '12:30',
            ponente: 'Empresas',
            sala: 'Sala de Juntas',
          },
          {
            id: '4-3',
            titulo: 'Cóctel de cierre',
            fecha: '2026-05-26',
            horaInicio: '17:00',
            horaFin: '19:00',
            ponente: 'N/A',
            sala: 'Terraza',
          },
        ],
      },
    ];
    void eventosMock; // referencia retenida sólo para no romper el método heredado
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




