import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from '@env/environment';
import { map } from 'rxjs/operators';
import { PagedResult, Tutorial, TutorialApi, TutorialesQueryParams, TutorialPagedApi } from '../models/tutoriales.models';

@Injectable({ providedIn: 'root' })
export class TutorialesService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/tutorials`;

  getTutoriales(params: TutorialesQueryParams) {
    let httpParams = new HttpParams()
      .set('page', String(params.page))
      .set('pageSize', String(params.pageSize));

    if (params.categoria) {
      httpParams = httpParams.set('category', params.categoria);
    }

    if (params.busqueda) {
      httpParams = httpParams.set('search', params.busqueda);
    }

    return this.http.get<TutorialPagedApi>(`${this.baseUrl}/paginated`, { params: httpParams }).pipe(
      map((result) => {
        const items = (result?.items ?? []).map((item): Tutorial => ({
          id: item.Id,
          slug: item.Slug,
          titulo: item.Titulo,
          extracto: item.Extracto,
          autor: item.Autor,
          fechaPublicacion: item.FechaPublicacion,
          categorias: JSON.parse(item.CategoriasJson || '[]') as string[],
          url: item.Url,
        }));

        const totalCount = result?.totalCount ?? 0;
        const page = result?.pageNumber ?? params.page;
        const totalPages = Math.ceil(totalCount / params.pageSize);

        return {
          items,
          totalCount,
          page,
          pageSize: result?.pageSize ?? params.pageSize,
          totalPages,
          hasNextPage: page < totalPages,
          hasPreviousPage: page > 1,
        } satisfies PagedResult<Tutorial>;
      })
    );
  }
}


