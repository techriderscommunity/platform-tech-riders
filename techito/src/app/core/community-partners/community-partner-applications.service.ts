import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { environment } from '@env/environment';
import { CommunityPartnerApplicationAdminApi } from './community-partner-applications.models';

@Injectable({ providedIn: 'root' })
export class CommunityPartnerApplicationsService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/community-partner-applications`;

  getPending() {
    return this.http.get<CommunityPartnerApplicationAdminApi[]>(`${this.baseUrl}/pending`);
  }

  approve(id: string) {
    return this.http.post(`${this.baseUrl}/${id}/approve`, {});
  }

  reject(id: string) {
    return this.http.post(`${this.baseUrl}/${id}/reject`, {});
  }
}
