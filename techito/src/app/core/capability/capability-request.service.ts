import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { environment } from '@env/environment';
import { map } from 'rxjs/operators';
import { CapabilityRequestApi, CapabilityRequestItem } from './capability-request.models';

@Injectable({ providedIn: 'root' })
export class CapabilityRequestService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/capability-requests`;

  /** Un Member solicita ascender a un rol de comunidad (Staff, Community Leader, Ambassador, Center, Community Partner). */
  requestCapability(capabilityName: string) {
    return this.http.post(this.baseUrl, { CapabilityName: capabilityName });
  }

  getPending() {
    return this.http.get<CapabilityRequestApi[]>(`${this.baseUrl}/pending`).pipe(
      map((items) => (items ?? []).map(toItem)),
    );
  }

  approve(id: string) {
    return this.http.post(`${this.baseUrl}/${id}/approve`, {});
  }

  reject(id: string) {
    return this.http.post(`${this.baseUrl}/${id}/reject`, {});
  }
}

function toItem(api: CapabilityRequestApi): CapabilityRequestItem {
  return {
    id: api.Id,
    userId: api.UserId,
    userName: api.UserName ?? '',
    userEmail: api.UserEmail ?? '',
    capabilityName: api.CapabilityName,
    status: api.Status,
    requestedAt: api.RequestedAt,
  };
}
