import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { environment } from '@env/environment';
import { AssignmentCandidateApi } from './assignments.models';

@Injectable({ providedIn: 'root' })
export class AssignmentsService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/assignments`;

  getCandidates(skillId?: string, availabilityValueId?: string) {
    let params = new HttpParams();
    if (skillId) params = params.set('skillId', skillId);
    if (availabilityValueId) params = params.set('availabilityValueId', availabilityValueId);

    return this.http.get<AssignmentCandidateApi[]>(`${this.baseUrl}/candidates`, { params });
  }
}
