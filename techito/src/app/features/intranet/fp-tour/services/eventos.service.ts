import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { environment } from '\.\./\.\./\.\./\.\./\.\./environments/environment';
import { PagedResult } from '../models/sesiones.models';
import { EventoCalendario, SesionEnEvento } from '../calendar-eventos';

interface SesionEnEventoApi {
  Id: string;
  Title: string;
  StartTime: string;
  EndTime: string;
  Description?: string;
  Speaker?: string;
  Room?: string;
}

interface EventoConSesionesApi {
  Id: string;
  Name: string;
  Location?: string;
  StartDate: string;
  EndDate: string;
  Description?: string;
  IsActive: boolean;
  Sessions?: SesionEnEventoApi[];
}

function toHHmm(timeOnly: string | null): string {
  if (!timeOnly) return '00:00';
  // TimeOnly llega como "HH:mm:ss" o "HH:mm:ss.fffffff"
  return timeOnly.substring(0, 5);
}

function toFechaStr(dateStr: string): string {
  // ISO date → 'YYYY-MM-DD'
  return dateStr.substring(0, 10);
}

export interface EventoResumen {
  id: string;
  titulo: string;
  ubicacion: string;
  categoria: string;
  estado: string;
  url: string;
  esPasado: boolean;
}

interface EventoListApi {
  Id: string;
  Name: string;
  Location?: string;
  IsActive: boolean;
}

@Injectable({ providedIn: 'root' })
export class EventosService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/events`;

  getEventosConSesiones(page = 1, pageSize = 100) {
    const params = new HttpParams()
      .set('page', String(page))
      .set('pageSize', String(pageSize));

    return this.http
      .get<EventoConSesionesApi[]>(this.baseUrl, { params })
      .pipe(map((result) => ({
        items: (result ?? []).map((e): EventoCalendario => ({
          id: e.Id,
          titulo: e.Name,
          fechaInicio: toFechaStr(e.StartDate),
          fechaFin: toFechaStr(e.EndDate),
          categoria: 'otros' as EventoCalendario['categoria'],
          descripcion: e.Description,
          centro: e.Location,
          sesiones: (e.Sessions ?? []).map((s): SesionEnEvento => ({
            id: s.Id,
            titulo: s.Title,
            fecha: toFechaStr(e.StartDate),
            horaInicio: toHHmm(s.StartTime),
            horaFin: toHHmm(s.EndTime),
            descripcion: s.Description,
            ponente: s.Speaker,
            sala: s.Room,
          })),
        })),
        totalCount: result?.length ?? 0,
        page,
        pageSize,
        totalPages: 1,
        hasNextPage: false,
        hasPreviousPage: false,
      })));
  }

  getEventos(page = 1, pageSize = 20) {
    const params = new HttpParams()
      .set('page', String(page))
      .set('pageSize', String(pageSize));

    return this.http
      .get<EventoListApi[]>(this.baseUrl, { params })
      .pipe(map((result) => ({
        items: (result ?? []).map((e): EventoResumen => ({
          id: e.Id,
          titulo: e.Name,
          ubicacion: e.Location ?? '',
          categoria: 'others',
          estado: e.IsActive ? 'Active' : 'Inactive',
          url: '',
          esPasado: false,
        })),
        totalCount: result?.length ?? 0,
        page,
        pageSize,
        totalPages: 1,
        hasNextPage: false,
        hasPreviousPage: false,
      })));
  }
}





