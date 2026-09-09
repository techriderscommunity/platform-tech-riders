import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from '@env/environment';
import { UiCarouselItem } from '@shared/ui/media-carousel/media-carousel';
import { KnowledgePlaylistKey, PlaylistVideoDto } from '../models/knowledge-playlists.models';

@Injectable({ providedIn: 'root' })
export class KnowledgePlaylistsService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/events/podcast/videos`;

  getVideosByPlaylist(playlist: KnowledgePlaylistKey, maxResults = 8): Observable<UiCarouselItem[]> {
    const params = new HttpParams()
      .set('maxResults', String(maxResults))
      .set('playlist', playlist);

    return this.http.get<PlaylistVideoDto[]>(this.baseUrl, { params }).pipe(
      map((videos) => (videos ?? []).map((video) => ({
        kind: 'video' as const,
        title: video.title,
        src: video.embedUrl,
      })))
    );
  }
}
