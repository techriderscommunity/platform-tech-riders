import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { environment } from '@env/environment';

export interface IntranetAuditRecord {
  id: string;
  createdUtc: string;
  actorUserId?: string | null;
  actorEmail?: string | null;
  module: string;
  action: string;
  result: string;
  detail?: string | null;
}

export interface IntranetSettingRecord {
  id: string;
  key: string;
  module: string;
  value: string;
  status: string;
  updatedUtc: string;
  updatedBy?: string | null;
}

export interface UpdateIntranetSettingPayload {
  key: string;
  module: string;
  value: string;
  status: 'activo' | 'revision';
}

@Injectable({ providedIn: 'root' })
export class IntranetAdminService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/intranet`;

  getAuditLogs() {
    return this.http.get<IntranetAuditRecord[]>(`${this.baseUrl}/audit-logs`);
  }

  getSettings() {
    return this.http.get<IntranetSettingRecord[]>(`${this.baseUrl}/settings`);
  }

  updateSetting(payload: UpdateIntranetSettingPayload) {
    return this.http.put<IntranetSettingRecord>(`${this.baseUrl}/settings`, payload);
  }
}
