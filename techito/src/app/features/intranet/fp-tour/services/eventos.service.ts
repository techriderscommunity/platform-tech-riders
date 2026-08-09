import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { environment } from '../../../../../environments/environment';
import { EventoConSesionesApi, EventoListApi, EventoResumen } from '../models/eventos.models';
import { PagedResult } from '../models/sesiones.models';
import { EventoCalendario, SesionEnEvento } from '../calendar-eventos';

function toHHmm(timeOnly: string | null): string {
  if (!timeOnly) return '00:00';
  // TimeOnly llega como "HH:mm:ss" o "HH:mm:ss.fffffff"
  return timeOnly.substring(0, 5);
}

function toFechaStr(dateStr: string): string {
  // ISO date → 'YYYY-MM-DD'
  return dateStr.substring(0, 10);
}


const DEFAULT_EVENTO_CALENDARIO_CATEGORIA: EventoCalendario['categoria'] = 'Colaboradores';
const DEFAULT_EVENTO_RESUMEN_CATEGORIA = 'others';

const EVENT_STATUS = {
  active: 'Active',
  inactive: 'Inactive',
} as const;

@Injectable({ providedIn: 'root' })
export class EventosService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/events`;

  getEventosConSesiones(page = 1, pageSize = 100): import('rxjs').Observable<PagedResult<EventoCalendario>> {
    const safePage = this.normalizePositiveInt(page, 1);
    const safePageSize = this.normalizePositiveInt(pageSize, 100);
    const params = this.buildPaginationParams(safePage, safePageSize);

    return this.http
      .get<EventoConSesionesApi[]>(this.baseUrl, { params })
      .pipe(map((result) => ({
        items: (result ?? []).map((evento) => this.mapEventoCalendario(evento)),
        totalCount: result?.length ?? 0,
        page: safePage,
        pageSize: safePageSize,
        totalPages: 1,
        hasNextPage: false,
        hasPreviousPage: false,
      })));
  }

  getEventos(page = 1, pageSize = 20): import('rxjs').Observable<PagedResult<EventoResumen>> {
    const safePage = this.normalizePositiveInt(page, 1);
    const safePageSize = this.normalizePositiveInt(pageSize, 20);
    const params = this.buildPaginationParams(safePage, safePageSize);

    return this.http
      .get<EventoListApi[]>(this.baseUrl, { params })
      .pipe(map((result) => ({
        items: (result ?? []).map((evento) => this.mapEventoResumen(evento)),
        totalCount: result?.length ?? 0,
        page: safePage,
        pageSize: safePageSize,
        totalPages: 1,
        hasNextPage: false,
        hasPreviousPage: false,
      })));
  }

  private buildPaginationParams(page: number, pageSize: number): HttpParams {
    return new HttpParams()
      .set('page', String(page))
      .set('pageSize', String(pageSize));
  }

  private mapEventoCalendario(evento: EventoConSesionesApi): EventoCalendario {
    return {
      id: evento.Id,
      titulo: evento.Name,
      fechaInicio: toFechaStr(evento.StartDate),
      fechaFin: toFechaStr(evento.EndDate),
      categoria: this.getCalendarCategory(evento.Name),
      descripcion: evento.Description,
      centro: evento.Location,
      sesiones: (evento.Sessions ?? []).map((sesion): SesionEnEvento => ({
        id: sesion.Id,
        titulo: sesion.Title,
        fecha: toFechaStr(evento.StartDate),
        horaInicio: toHHmm(sesion.StartTime),
        horaFin: toHHmm(sesion.EndTime),
        descripcion: sesion.Description,
        ponente: sesion.Speaker,
        sala: sesion.Room,
      })),
    };
  }

  private mapEventoResumen(evento: EventoListApi): EventoResumen {
    return {
      id: evento.Id,
      titulo: evento.Name,
      ubicacion: evento.Location ?? '',
      categoria: DEFAULT_EVENTO_RESUMEN_CATEGORIA,
      estado: evento.IsActive ? EVENT_STATUS.active : EVENT_STATUS.inactive,
      url: '',
      esPasado: false,
    };
  }

  private getCalendarCategory(nombreEvento: string): EventoCalendario['categoria'] {
    const normalizedName = nombreEvento.toLowerCase();

    if (normalizedName.includes('fptour') || normalizedName.includes('fp tour')) {
      return 'FPTour';
    }

    if (normalizedName.includes('tajamar')) {
      return 'TajamarTech';
    }

    if (normalizedName.includes('techriders') || normalizedName.includes('tech riders')) {
      return 'TechRiders';
    }

    return DEFAULT_EVENTO_CALENDARIO_CATEGORIA;
  }

  private normalizePositiveInt(value: number, fallback: number): number {
    return Number.isInteger(value) && value > 0 ? value : fallback;
  }
}





