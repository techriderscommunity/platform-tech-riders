import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { environment } from '\.\./\.\./\.\./\.\./\.\./environments/environment';
import { Embajador, PagedResult } from '../models/embajadores.models';

interface AmbassadorApi {
  Id: string;
  Name: string;
  LastName: string;
  Email: string;
  Phone?: string | null;
  CategoryName?: string | null;
  OtherCategory?: string | null;
  UpdatedAt?: string | null;
  CreatedAt: string;
  IsActive: boolean;
}

@Injectable({ providedIn: 'root' })
export class EmbajadoresService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/ambassadors`;

  getEmbajadores(page = 1, pageSize = 50, estado?: string) {
    const params = new HttpParams();

    return this.http.get<AmbassadorApi[]>(this.baseUrl, { params }).pipe(
      map((items) => {
        const mappedItems = (items ?? []).map((item): Embajador => ({
          id: item.Id,
          nombre: [item.Name, item.LastName].filter(Boolean).join(' ').trim(),
          email: item.Email,
          telefono: item.Phone ?? '',
          categoria: item.CategoryName ?? item.OtherCategory ?? 'General',
          estado: item.IsActive ? 'activo' : 'desactivado',
          totalSesiones: 0,
          ultimaActividad: item.UpdatedAt ?? item.CreatedAt,
        }));

        const filteredItems = estado
          ? mappedItems.filter((item) => item.estado === estado)
          : mappedItems;

        const startIndex = Math.max(0, (page - 1) * pageSize);
        const pagedItems = filteredItems.slice(startIndex, startIndex + pageSize);
        const totalCount = filteredItems.length;
        const totalPages = Math.ceil(totalCount / pageSize);

        return {
          items: pagedItems,
          totalCount,
          page,
          pageSize,
          totalPages,
          hasNextPage: page < totalPages,
          hasPreviousPage: page > 1,
        } satisfies PagedResult<Embajador>;
      })
    );
  }

  createEmbajador(embajador: { nombre: string; email: string; telefono: string; categoria: string; }) {
    return this.http.post<string>(this.baseUrl, embajador);
  }
}



