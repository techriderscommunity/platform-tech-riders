import { HttpClient } from '@angular/common/http';
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
  IsActive: boolean;
}

@Injectable({ providedIn: 'root' })
export class OrganizationsService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/organizations`;

  create(payload: { organizationType: string; name: string; taxId?: string; website?: string; address?: string; province?: string }) {
    return this.http.post<OrganizationApi>(this.baseUrl, {
      OrganizationType: payload.organizationType,
      Name: payload.name,
      TaxId: payload.taxId,
      Website: payload.website,
      Address: payload.address,
      Province: payload.province,
    });
  }
}
