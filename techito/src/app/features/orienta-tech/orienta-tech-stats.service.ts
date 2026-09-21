import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, catchError, of, shareReplay } from 'rxjs';
import { environment } from '@env/environment';

export interface OrientaTechStats {
  activeSessions: number;
}

@Injectable({ providedIn: 'root' })
export class OrientaTechStatsService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/public/orienta-tech/stats`;

  private readonly stats$ = this.http.get<OrientaTechStats>(this.baseUrl).pipe(
    catchError(() => of<OrientaTechStats>({ activeSessions: 0 })),
    shareReplay({ bufferSize: 1, refCount: true }),
  );

  getStats(): Observable<OrientaTechStats> {
    return this.stats$;
  }
}
