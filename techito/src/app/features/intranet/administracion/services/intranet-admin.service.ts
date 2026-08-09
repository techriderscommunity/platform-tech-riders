import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { environment } from '@env/environment';
import {
  IntranetAuditRecord,
  IntranetSettingRecord,
  UpdateIntranetSettingPayload,
} from '../models/intranet-admin.models';

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
