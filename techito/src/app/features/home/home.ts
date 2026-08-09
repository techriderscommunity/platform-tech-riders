import { ChangeDetectionStrategy, Component, DestroyRef, OnInit, computed, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { tap, catchError } from 'rxjs/operators';
import { EMPTY } from 'rxjs';
import { EventoResumen } from '@core/events/public-events.models';
import { PublicEventsService } from '@core/events/public-events.service';
import { PublicContentService } from '@core/content/public-content.service';
import { HomePastEventPhotoItem, HomeProfileCardItem, MetricItem } from '@core/content/public-content.models';
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
  private readonly publicContentService = inject(PublicContentService);
  private readonly destroyRef = inject(DestroyRef);

  readonly eventos = signal<EventoResumen[]>([]);
  readonly loadingEventos = signal(false);
  readonly proximosEventos = computed(() => this.eventos().filter(evento => !evento.esPasado).slice(0, 6));
  eventosPasadosFotos: HomePastEventPhotoItem[] = [];
  readonly eventosPasadosSlides = computed<UiCarouselItem[]>(() => this.eventosPasadosFotos.map(foto => ({
    kind: 'image',
    src: foto.src,
    alt: foto.alt,
    title: foto.label,
  })));
  stats: MetricItem[] = [];
  profilePanelCards: HomeProfileCardItem[] = [];

  ngOnInit(): void {
    this.publicContentService
      .getPublicContent()
      .pipe(
        tap((content) => {
          this.stats = content.home.stats;
          this.profilePanelCards = content.home.profilePanelCards;
          this.eventosPasadosFotos = content.home.pastEventPhotos;
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



