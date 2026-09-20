import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, catchError, of, shareReplay } from 'rxjs';
import { environment } from '@env/environment';

export interface AboutStats {
  activeAmbassadors: number;
}

@Injectable({ providedIn: 'root' })
export class AboutStatsService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/public/about/stats`;

  private readonly stats$ = this.http.get<AboutStats>(this.baseUrl).pipe(
    catchError(() => of<AboutStats>({ activeAmbassadors: 0 })),
    shareReplay({ bufferSize: 1, refCount: true }),
  );

  getStats(): Observable<AboutStats> {
    return this.stats$;
  }
}
