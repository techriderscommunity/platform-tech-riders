import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, catchError, of, shareReplay, map } from 'rxjs';
import { environment } from '@env/environment';
import { PublicContentPayload } from './public-content.models';
import { PUBLIC_CONTENT_FALLBACK } from './public-content.fallback';

@Injectable({ providedIn: 'root' })
export class PublicContentService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/publiccontent`;

  private readonly publicContentRequest$ = this.http
    .get<PublicContentPayload>(this.baseUrl)
    .pipe(
      map((payload) => this.normalizePayload(payload)),
      catchError(() => of(PUBLIC_CONTENT_FALLBACK)),
      shareReplay({ bufferSize: 1, refCount: true })
    );

  getPublicContent(): Observable<PublicContentPayload> {
    return this.publicContentRequest$;
  }

  private normalizePayload(payload?: Partial<PublicContentPayload> | null): PublicContentPayload {
    if (!payload) {
      return PUBLIC_CONTENT_FALLBACK;
    }

    const homeLooksMinimal =
      !payload.home ||
      !payload.home.stats ||
      payload.home.stats.length < 6 ||
      !payload.home.profilePanelCards ||
      payload.home.profilePanelCards.length < 5;

    const eventsLooksMinimal =
      !payload.events ||
      !payload.events.galleryGroups ||
      payload.events.galleryGroups.length < 3;

    if (homeLooksMinimal || eventsLooksMinimal) {
      return PUBLIC_CONTENT_FALLBACK;
    }

    return payload as PublicContentPayload;
  }
}
