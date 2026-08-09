import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { environment } from '@env/environment';
import { UiCarouselItem } from '@shared/ui/media-carousel/media-carousel';
import { OrientaPlaylistKey, PlaylistVideoDto } from '../models/orienta-tech.models';

@Injectable({ providedIn: 'root' })
export class OrientaTechPlaylistsService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/events/podcast/videos`;

  getVideosByPlaylist(
    playlist: OrientaPlaylistKey,
    maxResults = 8
  ): Observable<UiCarouselItem[]> {
    const params = new HttpParams()
      .set('maxResults', String(maxResults))
      .set('playlist', playlist);

    return this.http.get<PlaylistVideoDto[]>(this.baseUrl, { params }).pipe(
      map((videos) =>
        (videos ?? []).map((video) => ({
          kind: 'video' as const,
          title: video.title,
          src: video.embedUrl,
        }))
      )
    );
  }
}
