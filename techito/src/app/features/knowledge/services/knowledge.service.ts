import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import {
  PagedResult,
  KnowledgeArticleDetailApi,
  KnowledgeArticleDetailApiRaw,
  KnowledgeModel,
  KnowledgePagedApiRaw,
  KnowledgeQueryParams,
} from '../models/knowledge.models';
import { environment } from '@env/environment';

@Injectable({ providedIn: 'root' })
export class KnowledgeService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/knowledge-articles`;

  getKnowledges(params: KnowledgeQueryParams) {
    let httpParams = new HttpParams()
      .set('page', String(params.page))
      .set('pageSize', String(params.pageSize));

    if (params.busqueda) {
      httpParams = httpParams.set('search', params.busqueda);
    }

    if (params.categoria) {
      httpParams = httpParams.set('categoryName', params.categoria);
    }

    return this.http.get<KnowledgePagedApiRaw>(this.baseUrl, { params: httpParams }).pipe(
      map((result) => {
        const rawItems = result?.Items ?? result?.items ?? [];
        const items = rawItems.map((item): KnowledgeModel => {
          const categorias = item.Categories ?? item.categories ?? [];
          return {
            id: (item.Id ?? item.id)!,
            slug: (item.Slug ?? item.slug)!,
            titulo: (item.Title ?? item.title)!,
            extracto: categorias.length > 0
              ? `Artículo de ${categorias.join(', ')}`
              : 'Artículo de conocimiento',
            autor: (item.AuthorName ?? item.authorName) || 'TechRiders',
            fechaPublicacion: (item.PublishedAt ?? item.publishedAt) ?? new Date().toISOString(),
            categorias,
            url: `/knowledge/${item.Slug ?? item.slug}`,
          };
        });

        const page = (result?.Page ?? result?.page) ?? params.page;
        const totalCount = (result?.TotalCount ?? result?.totalCount) ?? items.length;
        const totalPages = Math.max(1, Math.ceil(totalCount / params.pageSize));

        return {
          items,
          totalCount,
          page,
          pageSize: (result?.PageSize ?? result?.pageSize) ?? params.pageSize,
          totalPages,
          hasNextPage: page < totalPages,
          hasPreviousPage: page > 1,
        } satisfies PagedResult<KnowledgeModel>;
      })
    );
  }

  getKnowledgeBySlug(slug: string): Observable<KnowledgeArticleDetailApi> {
    return this.http.get<KnowledgeArticleDetailApiRaw>(`${this.baseUrl}/${encodeURIComponent(slug)}`).pipe(
      map((raw): KnowledgeArticleDetailApi => ({
        id: (raw.Id ?? raw.id)!,
        slug: (raw.Slug ?? raw.slug)!,
        title: (raw.Title ?? raw.title)!,
        contentMd: (raw.ContentMd ?? raw.contentMd) ?? '',
        publishedAt: (raw.PublishedAt ?? raw.publishedAt) ?? null,
        authorName: (raw.AuthorName ?? raw.authorName) ?? '',
        statusName: (raw.StatusName ?? raw.statusName) ?? null,
        categories: (raw.Categories ?? raw.categories) ?? [],
        skills: (raw.Skills ?? raw.skills) ?? [],
      }))
    );
  }
}



