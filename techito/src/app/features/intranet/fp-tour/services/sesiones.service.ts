import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { environment } from '\.\./\.\./\.\./\.\./\.\./environments/environment';
import { PagedResult, Sesion } from '../models/sesiones.models';

interface SessionApi {
  Id: string;
  Title: string;
  Level?: string | null;
  MaxCapacity?: number | null;
  IsActive: boolean;
  Event?: {
    Name: string;
    StartDate: string;
  } | null;
}

interface SessionWorkflowApi {
  SessionId: string;
  Status?: string;
  AmbassadorAssignedId?: string | null;
  UpdatedAt: string;
}

interface UpdateSessionWorkflowRequest {
  status?: string;
  ambassadorAssignedId?: string | null;
}

@Injectable({ providedIn: 'root' })
export class SesionesService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/sessions`;

  getSesiones(page = 1, pageSize = 50, estado?: string) {
    let params = new HttpParams()
      .set('page', String(page))
      .set('pageSize', String(pageSize));

    if (estado) {
      params = params.set('estado', estado);
    }

    return this.http.get<SessionApi[]>(this.baseUrl, { params }).pipe(
      map((items) => {
        const mappedItems = (items ?? []).map((item): Sesion => ({
          id: item.Id,
          titulo: item.Title,
          centro: item.Event?.Name ?? 'Unassigned event',
          fecha: item.Event?.StartDate?.substring(0, 10) ?? '',
          categoria: item.Level ?? 'General',
          estado: item.IsActive ? 'Pendiente' : 'Cancelada',
          numAlumnos: item.MaxCapacity ?? 0,
          embajadorAsignadoId: null,
        }));

        return {
          items: mappedItems,
          totalCount: mappedItems.length,
          page,
          pageSize,
          totalPages: 1,
          hasNextPage: false,
          hasPreviousPage: false,
        } satisfies PagedResult<Sesion>;
      })
    );
  }

  createSesion(sesion: Omit<Sesion, 'id' | 'estado' | 'embajadorAsignadoId'>) {
    return this.http.post<string>(this.baseUrl, sesion);
  }

  getSessionWorkflow() {
    return this.http.get<Record<string, SessionWorkflowApi>>(`${this.baseUrl}/workflow`);
  }

  updateSessionWorkflow(sessionId: string, request: UpdateSessionWorkflowRequest) {
    return this.http.put<SessionWorkflowApi>(`${this.baseUrl}/${sessionId}/workflow`, request);
  }

  updateSesion(id: string, sesion: { titulo: string; centro: string; fecha: string; categoria: string; numAlumnos: number; }) {
    return this.http.put<void>(`${this.baseUrl}/${id}`, sesion);
  }
}



