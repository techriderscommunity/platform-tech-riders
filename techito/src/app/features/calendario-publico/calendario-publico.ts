import { ChangeDetectionStrategy, Component, DestroyRef, OnInit, computed, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { catchError, EMPTY, tap } from 'rxjs';
import { UiTextField } from '@shared/ui/text-field/text-field';
import { UiMetricsStrip } from '@shared/ui/metrics-strip/metrics-strip';
import { UiResourceCardItem, UiResourceCards } from '@shared/ui/resource-cards/resource-cards';
import {
  PUBLIC_EVENT_MODALITIES,
  PUBLIC_EVENT_TOPICS,
  PUBLIC_EVENT_TYPES,
  PublicEvent,
} from './models/public-event.model';
import { PublicEventsService } from './services/public-events.service';

@Component({
  selector: 'app-calendario-publico',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [RouterLink, UiTextField, UiMetricsStrip, UiResourceCards],
  templateUrl: './calendario-publico.html',
  styleUrl: './calendario-publico.scss'
})
export class CalendarioPublico implements OnInit {
  private readonly publicEventsService = inject(PublicEventsService);
  private readonly destroyRef = inject(DestroyRef);

  readonly events = signal<PublicEvent[]>([]);
  readonly loading = signal(false);

  readonly types = PUBLIC_EVENT_TYPES;
  readonly modalities = PUBLIC_EVENT_MODALITIES;
  readonly topics = PUBLIC_EVENT_TOPICS;

  readonly selectedType = signal('');
  readonly selectedModality = signal('');
  readonly selectedTopic = signal('');
  readonly searchText = signal('');

  readonly filteredEvents = computed(() => {
    const query = this.searchText().trim().toLowerCase();
    return this.events().filter((event) => {
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

  readonly metrics = computed(() => [
    { icon: '📅', value: String(this.filteredEvents().length), label: 'Eventos filtrados' },
    { icon: '🧭', value: String(this.types.length), label: 'Tipos de actividad' },
    { icon: '🌍', value: 'Abierto', label: 'Acceso para comunidad' },
  ]);

  readonly activeFilterLabel = computed(() => {
    const labels = [this.selectedType(), this.selectedModality(), this.selectedTopic()].filter(Boolean);
    return labels.length ? labels.join(' · ') : 'Todos';
  });

  readonly eventCards = computed<UiResourceCardItem[]>(() => this.filteredEvents().map((event) => ({
    mode: event.type,
    title: event.title,
    summary: event.summary,
    tags: [event.topic, event.modality],
    meta: `${this.formatDate(event.date)} · ${event.place}`,
    ctaLabel: 'Ver detalles y registro',
    ctaHref: event.url
  })));

  private readonly dateFormatter = new Intl.DateTimeFormat('es-ES', {
    day: '2-digit',
    month: 'short',
    year: 'numeric'
  });

  ngOnInit(): void {
    this.loading.set(true);
    this.publicEventsService
      .getUpcomingEvents()
      .pipe(
        tap((events) => {
          this.events.set(events);
          this.loading.set(false);
        }),
        catchError(() => {
          this.events.set([]);
          this.loading.set(false);
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

  private formatDate(dateText: string): string {
    const date = new Date(dateText);
    return Number.isNaN(date.getTime()) ? dateText : this.dateFormatter.format(date);
  }
}
