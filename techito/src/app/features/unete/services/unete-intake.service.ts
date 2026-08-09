import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { environment } from '@env/environment';
import { JoinRequestPayload } from '../models/unete.models';

@Injectable({ providedIn: 'root' })
export class UneteIntakeService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = environment.apiUrl;

  submitJoinRequest(payload: JoinRequestPayload) {
    return this.http.post(`${this.baseUrl}/join`, payload);
  }
}
