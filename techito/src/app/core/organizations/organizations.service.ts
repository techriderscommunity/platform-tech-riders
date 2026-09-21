import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { environment } from '@env/environment';

export interface OrganizationApi {
  Id: string;
  OrganizationType: string;
  Name: string;
  TaxId?: string | null;
  Website?: string | null;
  Address?: string | null;
  Province?: string | null;
  Notes?: string | null;
  Origin?: string | null;
  IsActive: boolean;
}

@Injectable({ providedIn: 'root' })
export class OrganizationsService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/organizations`;

  list(type?: string, onlyActive = true) {
    let params = new HttpParams().set('onlyActive', String(onlyActive));
    if (type) params = params.set('type', type);
    return this.http.get<OrganizationApi[]>(this.baseUrl, { params });
  }

  getPending() {
    return this.http.get<OrganizationApi[]>(`${this.baseUrl}/pending`);
  }

  create(payload: { organizationType: string; name: string; taxId?: string; website?: string; address?: string; province?: string; notes?: string }) {
    return this.http.post<OrganizationApi>(this.baseUrl, {
      OrganizationType: payload.organizationType,
      Name: payload.name,
      TaxId: payload.taxId,
      Website: payload.website,
      Address: payload.address,
      Province: payload.province,
      Notes: payload.notes,
    });
  }

  update(id: string, payload: { name: string; taxId?: string; website?: string; address?: string; province?: string; notes?: string }) {
    return this.http.put<OrganizationApi>(`${this.baseUrl}/${id}`, {
      Name: payload.name,
      TaxId: payload.taxId,
      Website: payload.website,
      Address: payload.address,
      Province: payload.province,
      Notes: payload.notes,
    });
  }

  activate(id: string) {
    return this.http.post(`${this.baseUrl}/${id}/activate`, {});
  }

  suspend(id: string) {
    return this.http.post(`${this.baseUrl}/${id}/suspend`, {});
  }
}

