import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { environment } from '@env/environment';
import { map } from 'rxjs/operators';
import { GpfPersonLinkApi, GpfPersonLinkItem, PersonOrganizationApi, PersonOrganizationItem } from './gpf.models';

@Injectable({ providedIn: 'root' })
export class GpfLinkService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/gpf-person-links`;

  getAll() {
    return this.http.get<GpfPersonLinkApi[]>(this.baseUrl).pipe(map((items) => (items ?? []).map(toLinkItem)));
  }

  link(userId: string, codUnico: string) {
    return this.http.post(this.baseUrl, { UserId: userId, CodUnico: codUnico });
  }

  unlink(id: string) {
    return this.http.post(`${this.baseUrl}/${id}/unlink`, {});
  }
}

@Injectable({ providedIn: 'root' })
export class OrganizationRelationsService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/organizations`;

  getPendingRelations() {
    return this.http.get<PersonOrganizationApi[]>(`${this.baseUrl}/relations/pending`).pipe(
      map((items) => (items ?? []).map(toRelationItem)),
    );
  }

  approve(id: string) {
    return this.http.post(`${this.baseUrl}/relations/${id}/approve`, {});
  }

  reject(id: string) {
    return this.http.post(`${this.baseUrl}/relations/${id}/reject`, {});
  }
}

function toLinkItem(api: GpfPersonLinkApi): GpfPersonLinkItem {
  return {
    id: api.Id,
    userId: api.UserId,
    userName: api.UserName ?? '',
    userEmail: api.UserEmail ?? '',
    codUnico: api.CodUnico,
    status: api.Status,
    linkedAt: api.LinkedAt,
  };
}

function toRelationItem(api: PersonOrganizationApi): PersonOrganizationItem {
  return {
    id: api.Id,
    userName: api.UserName ?? '',
    organizationName: api.OrganizationName,
    relationType: api.RelationType,
    requestedAt: api.RequestedAt,
  };
}
