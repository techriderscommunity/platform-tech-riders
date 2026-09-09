import { ChangeDetectionStrategy, Component, DestroyRef, computed, inject, OnInit, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { catchError, EMPTY, tap } from 'rxjs';
import { EventoResumen } from '@core/events/public-events.models';
import { PublicEventsService } from '@core/events/public-events.service';
import { PublicContentService } from '@core/content/public-content.service';
import { GalleryGroupItem } from '@core/content/public-content.models';
import { UiCarouselItem, UiMediaCarousel  } from '@shared/ui/media-carousel/media-carousel';
import { UiResourceCardItem, UiResourceCards } from '@shared/ui/resource-cards/resource-cards';
import { UiTextField } from '@shared/ui/text-field/text-field';
import {
  PUBLIC_EVENT_MODALITIES,
  PUBLIC_EVENT_TOPICS,
  PUBLIC_EVENT_TYPES,
  PublicEvent,
} from './models/public-event.model';
import { PublicEventsAgendaService } from './services/public-events.service';
import { PodcastService } from './services/podcast.service';

@Component({
  selector: 'app-events',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [RouterLink, UiMediaCarousel, UiResourceCards, UiTextField],
  templateUrl: './events.html',
  styleUrl: './events.scss'
})
export class Events implements OnInit {
  private readonly publicEventsService = inject(PublicEventsService);
  private readonly publicEventsAgendaService = inject(PublicEventsAgendaService);
  private readonly publicContentService = inject(PublicContentService);
  private readonly podcastService = inject(PodcastService);
  private readonly destroyRef = inject(DestroyRef);

  readonly events = signal<EventoResumen[]>([]);
  readonly eventsPasados = computed(() => this.events().filter(evento => evento.esPasado));
  readonly loadingTalks = signal(false);

  readonly featuredEvents = [
    {
      eyebrow: 'Empleabilidad',
      title: 'EmpleaTech',
      description: 'Conecta talento, empresas y oportunidades para dar el siguiente paso profesional.',
      accent: 'coral',
    },
    {
      eyebrow: 'Inteligencia artificial',
      title: 'Copilot Dev Days',
      description: 'Descubre nuevas formas de crear software con GitHub Copilot y herramientas de IA.',
      accent: 'cyan',
    },
    {
      eyebrow: 'Comunidad',
      title: 'TechRiders League',
      description: 'Aprende, compite y comparte retos técnicos con la comunidad TechRiders.',
      accent: 'gold',
    },
  ] as const;

  readonly agendaEvents = signal<PublicEvent[]>([]);
  readonly agendaTypes = PUBLIC_EVENT_TYPES;
  readonly agendaModalities = PUBLIC_EVENT_MODALITIES;
  readonly agendaTopics = PUBLIC_EVENT_TOPICS;
  readonly selectedType = signal('');
  readonly selectedModality = signal('');
  readonly selectedTopic = signal('');
  readonly searchText = signal('');

  readonly filteredEvents = computed(() => {
    const query = this.searchText().trim().toLowerCase();
    return this.agendaEvents().filter((event) => {
      const byType = !this.selectedType() || event.type === this.selectedType();
      const byModality = !this.selectedModality() || event.modality === this.selectedModality();
      const byTopic = !this.selectedTopic() || event.topic === this.selectedTopic();
      const bySearch = !query
        || event.title.toLowerCase().includes(query)
        || event.summary.toLowerCase().includes(query)
        || event.place.toLowerCase().includes(query);

      return byType && byModality && byTopic && bySearch;
    });
  });

  readonly activeFilterLabel = computed(() => {
    const labels = [this.selectedType(), this.selectedModality(), this.selectedTopic()].filter(Boolean);
    return labels.length ? labels.join(' · ') : 'Todos';
  });

  readonly eventCards = computed<UiResourceCardItem[]>(() => this.filteredEvents().map((event) => ({
    mode: event.type,
    title: event.title,
    summary: event.summary,
    tags: [event.topic, event.modality],
    meta: `${this.formatEventDate(event.date)} · ${event.place}`,
    ctaLabel: 'Ver detalles y registro',
    ctaHref: event.url,
  })));

  private readonly eventDateFormatter = new Intl.DateTimeFormat('es-ES', {
    day: '2-digit',
    month: 'short',
    year: 'numeric',
  });

  readonly talksPodcastUrl = 'https://www.youtube.com/@TechRidersMedia/podcasts';
  private talksHistoricoFallback: UiCarouselItem[] = [];
  readonly talksHistorico = signal<UiCarouselItem[]>([]);

  galerias: GalleryGroupItem[] = [];

  ngOnInit(): void {
    this.publicContentService
      .getPublicContent()
      .pipe(
        tap((content) => {
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

    this.publicEventsService
      .getEventos(1, 60)
      .pipe(
        tap((result) => {
          this.events.set(result.items);
        }),
        catchError(() => EMPTY),
        takeUntilDestroyed(this.destroyRef)
      )
      .subscribe();

    this.publicEventsAgendaService
      .getUpcomingEvents()
      .pipe(
        tap((events) => this.agendaEvents.set(events)),
        catchError(() => {
          this.agendaEvents.set([]);
          return EMPTY;
        }),
        takeUntilDestroyed(this.destroyRef)
      )
      .subscribe();
  }

  setType(type: string): void {
    this.selectedType.set(type);
  }

  setModality(modality: string): void {
    this.selectedModality.set(modality);
  }

  setTopic(topic: string): void {
    this.selectedTopic.set(topic);
  }

  updateSearch(value: string): void {
    this.searchText.set(value);
  }

  clearFilters(): void {
    this.selectedType.set('');
    this.selectedModality.set('');
    this.selectedTopic.set('');
    this.searchText.set('');
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

  private formatEventDate(dateText: string): string {
    const date = new Date(dateText);
    return Number.isNaN(date.getTime()) ? dateText : this.eventDateFormatter.format(date);
  }
}



