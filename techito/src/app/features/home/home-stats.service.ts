import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, catchError, map, of, shareReplay } from 'rxjs';
import { environment } from '@env/environment';

export interface HomeStats {
  activeAmbassadors: number;
  activeEvents: number;
  activeSessions: number;
  activeTrainingCenters: number;
}

interface HomeStatsApiResponse {
  ActiveAmbassadors: number;
  ActiveEvents: number;
  ActiveSessions: number;
  ActiveTrainingCenters: number;
}

const EMPTY_STATS: HomeStats = { activeAmbassadors: 0, activeEvents: 0, activeSessions: 0, activeTrainingCenters: 0 };

@Injectable({ providedIn: 'root' })
export class HomeStatsService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/public/home/stats`;

  private readonly stats$ = this.http.get<HomeStatsApiResponse>(this.baseUrl).pipe(
    map((response) => ({
      activeAmbassadors: response.ActiveAmbassadors,
      activeEvents: response.ActiveEvents,
      activeSessions: response.ActiveSessions,
      activeTrainingCenters: response.ActiveTrainingCenters,
    })),
    catchError(() => of(EMPTY_STATS)),
    shareReplay({ bufferSize: 1, refCount: true }),
  );

  getStats(): Observable<HomeStats> {
    return this.stats$;
  }
}
