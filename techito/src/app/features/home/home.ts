import { ChangeDetectionStrategy, Component, DestroyRef, OnInit, PLATFORM_ID, computed, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { tap, catchError } from 'rxjs/operators';
import { EMPTY } from 'rxjs';
import { isPlatformBrowser } from '@angular/common';
import { EventoResumen } from '@core/events/public-events.models';
import { PublicEventsService } from '@core/events/public-events.service';
import { HomeStatsService } from './home-stats.service';
import { HOME_STATS_META, HOME_PROFILE_PANEL_CARDS, HOME_PAST_EVENT_PHOTOS } from './home.content';
import { HomePastEventPhotoItem, HomeProfileCardItem, MetricItem } from '@shared/ui/public-content.types';
import { UiCarouselItem, UiMediaCarousel  } from '@shared/ui/media-carousel/media-carousel';
import { UiMetricsStrip } from '@shared/ui/metrics-strip/metrics-strip';

@Component({
  selector: 'app-home',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [RouterLink, UiMediaCarousel, UiMetricsStrip],
  templateUrl: './home.html',
  styleUrl: './home.scss'
})
export class Home implements OnInit {
  private readonly publicEventsService = inject(PublicEventsService);
  private readonly homeStatsService = inject(HomeStatsService);
  private readonly destroyRef = inject(DestroyRef);
  private readonly platformId = inject(PLATFORM_ID);

  readonly eventos = signal<EventoResumen[]>([]);
  readonly loadingEventos = signal(false);
  readonly showLumaCalendar = signal(false);
  readonly proximosEventos = computed(() => this.eventos().filter(evento => !evento.esPasado).slice(0, 6));
  eventosPasadosFotos: HomePastEventPhotoItem[] = HOME_PAST_EVENT_PHOTOS;
  readonly eventosPasadosSlides = computed<UiCarouselItem[]>(() => this.eventosPasadosFotos.map(foto => ({
    kind: 'image',
    src: foto.src,
    alt: foto.alt,
    title: foto.label,
  })));
  stats: MetricItem[] = [];
  profilePanelCards: HomeProfileCardItem[] = HOME_PROFILE_PANEL_CARDS;

  ngOnInit(): void {
    this.showLumaCalendar.set(isPlatformBrowser(this.platformId));

    this.homeStatsService
      .getStats()
      .pipe(
        tap((stats) => {
          const values = [stats.activeAmbassadors, stats.activeEvents, stats.activeSessions, stats.activeTrainingCenters];
          this.stats = HOME_STATS_META.map((meta, index) => ({
            icon: meta.icon,
            label: meta.label,
            value: String(values[index]),
          }));
        }),
        catchError(() => EMPTY),
        takeUntilDestroyed(this.destroyRef)
      )
      .subscribe();

    this.loadingEventos.set(true);
    this.publicEventsService
      .getEventos(1, 20)
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

}



