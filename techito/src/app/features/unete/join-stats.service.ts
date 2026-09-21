import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, catchError, of, shareReplay } from 'rxjs';
import { environment } from '@env/environment';

export interface JoinStats {
  activeAmbassadors: number;
}

@Injectable({ providedIn: 'root' })
export class JoinStatsService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/public/join/stats`;

  private readonly stats$ = this.http.get<JoinStats>(this.baseUrl).pipe(
    catchError(() => of<JoinStats>({ activeAmbassadors: 0 })),
    shareReplay({ bufferSize: 1, refCount: true }),
  );

  getStats(): Observable<JoinStats> {
    return this.stats$;
  }
}
