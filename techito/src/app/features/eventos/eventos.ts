import { ChangeDetectionStrategy, Component, DestroyRef, computed, inject, OnInit, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { catchError, EMPTY, tap } from 'rxjs';
import { EventoResumen } from '@core/events/public-events.models';
import { PublicEventsService } from '@core/events/public-events.service';
import { PublicContentService } from '@core/content/public-content.service';
import { GalleryGroupItem, ParticipationModeItem } from '@core/content/public-content.models';
import { UiCarouselItem, UiMediaCarousel  } from '@shared/ui/media-carousel/media-carousel';
import { UiMetricsStrip } from '@shared/ui/metrics-strip/metrics-strip';
import { UiProgressCards } from '@shared/ui/progress-cards/progress-cards';
import { PodcastService } from './services/podcast.service';

@Component({
  selector: 'app-eventos',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [RouterLink, UiMediaCarousel, UiMetricsStrip, UiProgressCards],
  templateUrl: './eventos.html',
  styleUrl: './eventos.scss'
})
export class Eventos implements OnInit {
  private readonly publicEventsService = inject(PublicEventsService);
  private readonly publicContentService = inject(PublicContentService);
  private readonly podcastService = inject(PodcastService);
  private readonly destroyRef = inject(DestroyRef);

  readonly eventos = signal<EventoResumen[]>([]);
  readonly loadingEventos = signal(false);
  readonly proximosEventos = computed(() => this.eventos().filter(evento => !evento.esPasado));
  readonly eventosPasados = computed(() => this.eventos().filter(evento => evento.esPasado));
  readonly totalEventos = computed(() => this.eventos().length);
  readonly totalProximos = computed(() => this.proximosEventos().length);
  readonly totalPasados = computed(() => this.eventosPasados().length);
  readonly eventosMetrics = computed(() => [
    { value: String(this.totalEventos()), label: 'Total publicados', icon: '📊' },
    { value: String(this.totalProximos()), label: 'Próximos', icon: '📅' },
    { value: String(this.totalPasados()), label: 'Histórico', icon: '🗂️' },
  ]);
  readonly loadingTalks = signal(false);

  participationModes: ParticipationModeItem[] = [];

  readonly participationCards = computed(() => this.participationModes.map((mode, index) => ({
    title: mode.title,
    detail: mode.detail,
    progress: 70 + (index * 7),
    status: 'Participación',
    ctaLabel: 'Más info',
    ctaLink: '/join',
  })));

  readonly talksPodcastUrl = 'https://www.youtube.com/@TechRidersMedia/podcasts';
  private talksHistoricoFallback: UiCarouselItem[] = [];
  readonly talksHistorico = signal<UiCarouselItem[]>([]);

  galerias: GalleryGroupItem[] = [];

  ngOnInit(): void {
    this.publicContentService
      .getPublicContent()
      .pipe(
        tap((content) => {
          this.participationModes = content.events.participationModes;
          this.galerias = content.events.galleryGroups;
          this.talksHistoricoFallback = content.events.talksFallback.map((item) => ({
            kind: 'video',
            title: item.title,
            src: item.src,
          }));
          this.talksHistorico.set(this.talksHistoricoFallback);
          this.loadTalksHistorico();
        }),
        catchError(() => EMPTY),
        takeUntilDestroyed(this.destroyRef)
      )
      .subscribe();

    this.loadingEventos.set(true);
    this.publicEventsService
      .getEventos(1, 60)
      .pipe(
        tap((result) => {
          this.eventos.set(result.items);
          this.loadingEventos.set(false);
        }),
        catchError(() => {
          this.loadingEventos.set(false);
          return EMPTY;
        }),
        takeUntilDestroyed(this.destroyRef)
      )
      .subscribe();
  }

  private loadTalksHistorico(): void {
    this.loadingTalks.set(true);
    this.podcastService
      .getLatestVideos(8)
      .pipe(
        tap((items) => {
          if (items.length === 0) {
            this.talksHistorico.set(this.talksHistoricoFallback);
          } else if (items.length < 5) {
            const existingSrc = new Set(items.map((item) => item.src));
            const missingFromFallback = this.talksHistoricoFallback.filter((item) => !existingSrc.has(item.src));
            this.talksHistorico.set([...items, ...missingFromFallback].slice(0, 8));
          } else {
            this.talksHistorico.set(items);
          }
          this.loadingTalks.set(false);
        }),
        catchError(() => {
          this.talksHistorico.set(this.talksHistoricoFallback);
          this.loadingTalks.set(false);
          return EMPTY;
        }),
        takeUntilDestroyed(this.destroyRef)
      )
      .subscribe();
  }
}



