import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { environment } from '@env/environment';

@Injectable({ providedIn: 'root' })
export class IntranetLayoutTraceService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = environment.apiUrl;

  emitHeartbeatTrace(route: string) {
    return this.http.post(`${this.baseUrl}/intranet/trazas`, {
      kind: 'heartbeat',
      route,
      detail: 'intranet_layout_alive',
    });
  }
}
