import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { environment } from '@env/environment';
import { map } from 'rxjs/operators';
import { ApprovalItem, ApprovalItemApi } from './approvals.models';

@Injectable({ providedIn: 'root' })
export class ApprovalsService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/approvals`;

  getPending() {
    return this.http.get<ApprovalItemApi[]>(`${this.baseUrl}/pending`).pipe(
      map((items) => (items ?? []).map(toItem)),
    );
  }
}

function toItem(api: ApprovalItemApi): ApprovalItem {
  return {
    id: api.Id,
    type: api.Type,
    title: api.Title,
    requestedBy: api.RequestedBy ?? '',
    requestedAt: api.RequestedAt,
    module: api.Module,
  };
}
