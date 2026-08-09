import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, catchError, of, shareReplay } from 'rxjs';
import { environment } from '@env/environment';
import { PublicContentPayload } from './public-content.models';
import { PUBLIC_CONTENT_FALLBACK } from './public-content.fallback';

@Injectable({ providedIn: 'root' })
export class PublicContentService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/publiccontent`;

  private readonly publicContentRequest$ = this.http
    .get<PublicContentPayload>(this.baseUrl)
    .pipe(catchError(() => of(PUBLIC_CONTENT_FALLBACK)))
    .pipe(shareReplay({ bufferSize: 1, refCount: true }));

  getPublicContent(): Observable<PublicContentPayload> {
    return this.publicContentRequest$;
  }
}
