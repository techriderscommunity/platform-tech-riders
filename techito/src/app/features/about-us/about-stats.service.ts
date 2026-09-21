import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, catchError, map, of } from 'rxjs';
import { environment } from '@env/environment';

export interface AboutStats {
  activeStaff: number;
  activeCommunityLeaders: number;
  activeAmbassadors: number;
  activeMembers: number;
}

interface AboutStatsApiResponse {
  ActiveStaff: number;
  ActiveCommunityLeaders: number;
  ActiveAmbassadors: number;
  ActiveMembers: number;
}

@Injectable({ providedIn: 'root' })
export class AboutStatsService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/public/about/stats`;

  private readonly stats$ = this.http.get<AboutStatsApiResponse>(this.baseUrl).pipe(
    map((response) => ({
      activeStaff: response.ActiveStaff,
      activeCommunityLeaders: response.ActiveCommunityLeaders,
      activeAmbassadors: response.ActiveAmbassadors,
      activeMembers: response.ActiveMembers,
    })),
    catchError(() => of<AboutStats>({ activeStaff: 0, activeCommunityLeaders: 0, activeAmbassadors: 0, activeMembers: 0 })),
  );

  getStats(): Observable<AboutStats> {
    return this.stats$;
  }
}
