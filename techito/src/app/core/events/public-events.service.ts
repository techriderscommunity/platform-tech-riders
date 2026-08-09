import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { environment } from '@env/environment';
import { map } from 'rxjs/operators';
import { EventoListApi, EventoResumen, PagedResult } from './public-events.models';

const DEFAULT_EVENTO_RESUMEN_CATEGORIA = 'others';

const EVENT_STATUS = {
  active: 'Active',
  inactive: 'Inactive',
} as const;

@Injectable({ providedIn: 'root' })
export class PublicEventsService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/events`;

  getEventos(page = 1, pageSize = 20) {
    const safePage = this.normalizePositiveInt(page, 1);
    const safePageSize = this.normalizePositiveInt(pageSize, 20);
    const params = new HttpParams()
      .set('page', String(safePage))
      .set('pageSize', String(safePageSize));

    return this.http
      .get<EventoListApi[]>(this.baseUrl, { params })
      .pipe(map((result) => ({
        items: (result ?? []).map((evento): EventoResumen => ({
          id: evento.Id,
          titulo: evento.Name,
          ubicacion: evento.Location ?? '',
          categoria: DEFAULT_EVENTO_RESUMEN_CATEGORIA,
          estado: evento.IsActive ? EVENT_STATUS.active : EVENT_STATUS.inactive,
          url: '',
          esPasado: false,
        })),
        totalCount: result?.length ?? 0,
        page: safePage,
        pageSize: safePageSize,
        totalPages: 1,
        hasNextPage: false,
        hasPreviousPage: false,
      } satisfies PagedResult<EventoResumen>)));
  }

  private normalizePositiveInt(value: number, fallback: number): number {
    return Number.isInteger(value) && value > 0 ? value : fallback;
  }
}
