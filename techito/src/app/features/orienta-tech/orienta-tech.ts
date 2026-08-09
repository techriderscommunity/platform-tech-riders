import { ChangeDetectionStrategy, Component, DestroyRef, OnInit, WritableSignal, inject, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { RouterLink } from '@angular/router';
import { catchError, EMPTY, tap } from 'rxjs';
import { PublicContentService } from '@core/content/public-content.service';
import { FeatureCardItem, MetricItem, ProgressCardItem } from '@core/content/public-content.models';
import { UiMetricsStrip } from '@shared/ui/metrics-strip/metrics-strip';
import { UiFeatureCards } from '@shared/ui/feature-cards/feature-cards';
import { UiProgressCards } from '@shared/ui/progress-cards/progress-cards';
import { UiCarouselItem, UiMediaCarousel } from '@shared/ui/media-carousel/media-carousel';
import { OrientaPlaylistKey, YoutubePlaylistSection } from './models/orienta-tech.models';
import { OrientaTechPlaylistsService } from './services/orienta-tech-playlists.service';

function playlistVideoItem(title: string, videoId: string, listId: string): UiCarouselItem {
  return {
    kind: 'video',
    title,
    src: `https://www.youtube-nocookie.com/embed/${videoId}?list=${listId}`,
    link: `https://www.youtube.com/watch?v=${videoId}&list=${listId}`
  };
}

@Component({
  selector: 'app-orienta-tech',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [RouterLink, UiMetricsStrip, UiFeatureCards, UiProgressCards, UiMediaCarousel],
  templateUrl: './orienta-tech.html',
  styleUrl: './orienta-tech.scss'
})
export class OrientaTech implements OnInit {
  private readonly orientaTechPlaylistsService = inject(OrientaTechPlaylistsService);
  private readonly publicContentService = inject(PublicContentService);
  private readonly destroyRef = inject(DestroyRef);

  orientaMetrics: MetricItem[] = [];
  coreFeatures: FeatureCardItem[] = [];
  participationTracks: ProgressCardItem[] = [];
  studySections: FeatureCardItem[] = [];

  readonly youtubePlaylistsUrl = 'https://www.youtube.com/@TechRidersMedia/playlists';
  readonly loadingProfiles = signal(false);
  readonly loadingSuccessStories = signal(false);
  readonly loadingInterviews = signal(false);

  private readonly profilesFallback: UiCarouselItem[] = [
    playlistVideoItem('Perfiles profesionales · Episodio 1', 'J25VQJ7Wx34', 'PLxVLmPZVJwGEdXBvyBphPA_YxYjL8QNKO'),
    playlistVideoItem('Perfiles profesionales · Episodio 2', 'X4mIfCx6XPU', 'PLxVLmPZVJwGEdXBvyBphPA_YxYjL8QNKO'),
    playlistVideoItem('Perfiles profesionales · Episodio 3', 'vncHQDNPjEw', 'PLxVLmPZVJwGEdXBvyBphPA_YxYjL8QNKO'),
    playlistVideoItem('Perfiles profesionales · Episodio 4', 'A856m8nAx6g', 'PLxVLmPZVJwGEdXBvyBphPA_YxYjL8QNKO'),
    playlistVideoItem('Perfiles profesionales · Episodio 5', '5zfaHALRmis', 'PLxVLmPZVJwGEdXBvyBphPA_YxYjL8QNKO'),
  ];
  private readonly successStoriesFallback: UiCarouselItem[] = [
    playlistVideoItem('Historias de éxito · Episodio 1', 'HKgt8H8o-nI', 'PLxVLmPZVJwGESQcMxlUozaHcXVDBkigUo'),
    playlistVideoItem('Historias de éxito · Episodio 2', 'RXRqB_Ul_oI', 'PLxVLmPZVJwGESQcMxlUozaHcXVDBkigUo'),
    playlistVideoItem('Historias de éxito · Episodio 3', 'zlZwB1VlY28', 'PLxVLmPZVJwGESQcMxlUozaHcXVDBkigUo'),
    playlistVideoItem('Historias de éxito · Episodio 4', 'TAxnDg0kyRI', 'PLxVLmPZVJwGESQcMxlUozaHcXVDBkigUo'),
    playlistVideoItem('Historias de éxito · Episodio 5', 'NwEhryRqSio', 'PLxVLmPZVJwGESQcMxlUozaHcXVDBkigUo'),
  ];
  private readonly interviewsFallback: UiCarouselItem[] = [
    playlistVideoItem('Entrevistas · IA, Copilot y el futuro del desarrollo', 'WQp9pZb8shU', 'PLxVLmPZVJwGFXrUIxJdfJ9fB5QYuHyI-q'),
    playlistVideoItem('Entrevistas · Agentes de IA en empresa', 'SvZ50wArtaM', 'PLxVLmPZVJwGFXrUIxJdfJ9fB5QYuHyI-q'),
    playlistVideoItem('Entrevistas · Estudiantes AcademyVerso', 'baKNCZUbvL8', 'PLxVLmPZVJwGFXrUIxJdfJ9fB5QYuHyI-q'),
    playlistVideoItem('Entrevistas · IA con imágenes', '-biqjBJN_cI', 'PLxVLmPZVJwGFXrUIxJdfJ9fB5QYuHyI-q'),
    playlistVideoItem('Entrevistas · Microsoft Student Ambassador', 'Gc2sLw3vcvM', 'PLxVLmPZVJwGFXrUIxJdfJ9fB5QYuHyI-q'),
  ];

  readonly profilesItems = signal<UiCarouselItem[]>(this.profilesFallback);
  readonly successStoriesItems = signal<UiCarouselItem[]>(this.successStoriesFallback);
  readonly interviewsItems = signal<UiCarouselItem[]>(this.interviewsFallback);

  readonly youtubePlaylistSections: YoutubePlaylistSection[] = [
    {
      key: 'profiles',
      title: 'Perfiles profesionales',
      url: 'https://www.youtube.com/watch?v=J25VQJ7Wx34&list=PLxVLmPZVJwGEdXBvyBphPA_YxYjL8QNKO',
    },
    {
      key: 'success-stories',
      title: 'Historias de éxito',
      url: 'https://www.youtube.com/watch?v=HKgt8H8o-nI&list=PLxVLmPZVJwGESQcMxlUozaHcXVDBkigUo',
    },
    {
      key: 'interviews',
      title: 'Entrevistas',
      url: 'https://www.youtube.com/watch?v=WQp9pZb8shU&list=PLxVLmPZVJwGFXrUIxJdfJ9fB5QYuHyI-q',
    },
  ];

  ngOnInit(): void {
    this.publicContentService
      .getPublicContent()
      .pipe(
        tap((content) => {
          this.orientaMetrics = content.orientaTech.metrics;
          this.coreFeatures = content.orientaTech.coreFeatures;
          this.participationTracks = content.orientaTech.participationTracks;
          this.studySections = content.orientaTech.studySections;
        }),
        catchError(() => EMPTY),
        takeUntilDestroyed(this.destroyRef)
      )
      .subscribe();

    this.loadPlaylist('profiles', this.profilesItems, this.profilesFallback, this.loadingProfiles);
    this.loadPlaylist('success-stories', this.successStoriesItems, this.successStoriesFallback, this.loadingSuccessStories);
    this.loadPlaylist('interviews', this.interviewsItems, this.interviewsFallback, this.loadingInterviews);
  }

  private loadPlaylist(
    playlist: OrientaPlaylistKey,
    target: WritableSignal<UiCarouselItem[]>,
    fallback: UiCarouselItem[],
    loading: WritableSignal<boolean>
  ): void {
    loading.set(true);
    this.orientaTechPlaylistsService
      .getVideosByPlaylist(playlist, 8)
      .pipe(
        tap((items) => {
          if (items.length === 0) {
            target.set(fallback);
          } else if (items.length < 5) {
            const existingSrc = new Set(items.map((item) => item.src));
            const missingFromFallback = fallback.filter((item) => !existingSrc.has(item.src));
            target.set([...items, ...missingFromFallback].slice(0, 8));
          } else {
            target.set(items);
          }
          loading.set(false);
        }),
        catchError(() => {
          target.set(fallback);
          loading.set(false);
          return EMPTY;
        }),
        takeUntilDestroyed(this.destroyRef)
      )
      .subscribe();
  }

  sectionItems(section: YoutubePlaylistSection): UiCarouselItem[] {
    if (section.key === 'profiles') {
      return this.profilesItems();
    }
    if (section.key === 'success-stories') {
      return this.successStoriesItems();
    }
    return this.interviewsItems();
  }

  sectionLoading(section: YoutubePlaylistSection): boolean {
    if (section.key === 'profiles') {
      return this.loadingProfiles();
    }
    if (section.key === 'success-stories') {
      return this.loadingSuccessStories();
    }
    return this.loadingInterviews();
  }

}


