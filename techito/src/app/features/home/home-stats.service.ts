import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, catchError, of, shareReplay } from 'rxjs';
import { environment } from '@env/environment';

export interface HomeStats {
  activeAmbassadors: number;
  activeEvents: number;
  activeSessions: number;
  activeTrainingCenters: number;
}

const EMPTY_STATS: HomeStats = { activeAmbassadors: 0, activeEvents: 0, activeSessions: 0, activeTrainingCenters: 0 };

@Injectable({ providedIn: 'root' })
export class HomeStatsService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/public/home/stats`;

  private readonly stats$ = this.http.get<HomeStats>(this.baseUrl).pipe(
    catchError(() => of(EMPTY_STATS)),
    shareReplay({ bufferSize: 1, refCount: true }),
  );

  getStats(): Observable<HomeStats> {
    return this.stats$;
  }
}
