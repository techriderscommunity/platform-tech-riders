import { ChangeDetectionStrategy, Component, computed, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { UiTextField } from '@shared/ui/text-field/text-field';
import { UiMetricsStrip } from '@shared/ui/metrics-strip/metrics-strip';
import { UiResourceCardItem, UiResourceCards } from '@shared/ui/resource-cards/resource-cards';

type PublicEvent = {
  title: string;
  summary: string;
  type: 'Sesion tecnica' | 'Orientacion' | 'Empleabilidad' | 'Podcast' | 'Workshop' | 'Woman Tech';
  modality: 'Online' | 'Presencial' | 'Hibrido';
  topic: 'Azure' | '.NET' | 'Datos' | 'Ciberseguridad' | 'Carrera' | 'Soft Skills' | 'Comunidad';
  date: string;
  place: string;
  url: string;
};

@Component({
  selector: 'app-calendario-publico',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [RouterLink, UiTextField, UiMetricsStrip, UiResourceCards],
  templateUrl: './calendario-publico.html',
  styleUrl: './calendario-publico.scss'
})
export class CalendarioPublico {
  readonly events: PublicEvent[] = [
    {
      title: 'Arquitecturas modernas con .NET',
      summary: 'Sesion tecnica para revisar patrones de arquitectura, modularidad y despliegue de APIs.',
      type: 'Sesion tecnica',
      modality: 'Hibrido',
      topic: '.NET',
      date: '2026-09-18',
      place: 'Tech Riders Hub',
      url: 'https://luma.com/cal-24k3WtXkbkzA2tg'
    },
    {
      title: 'Ruta de entrada a Cloud con Azure',
      summary: 'Sesion de orientacion para perfiles junior sobre itinerarios reales de aprendizaje cloud.',
      type: 'Orientacion',
      modality: 'Online',
      topic: 'Azure',
      date: '2026-09-22',
      place: 'Streaming',
      url: 'https://luma.com/cal-24k3WtXkbkzA2tg'
    },
    {
      title: 'CV Tech y entrevistas sin humo',
      summary: 'Practica guiada para mejorar CV, portfolio y narrativa profesional en entrevistas.',
      type: 'Empleabilidad',
      modality: 'Presencial',
      topic: 'Carrera',
      date: '2026-09-27',
      place: 'Aula 3',
      url: 'https://luma.com/cal-24k3WtXkbkzA2tg'
    },
    {
      title: 'Woman Tech: referentes y trayectorias',
      summary: 'Encuentro abierto con profesionales para compartir experiencias y abrir nuevas oportunidades.',
      type: 'Woman Tech',
      modality: 'Hibrido',
      topic: 'Comunidad',
      date: '2026-10-02',
      place: 'Auditorio principal',
      url: 'https://luma.com/cal-24k3WtXkbkzA2tg'
    },
    {
      title: 'Podcast en directo: Data y decision',
      summary: 'Conversacion tecnica sobre datos aplicados a producto, negocio y operaciones.',
      type: 'Podcast',
      modality: 'Online',
      topic: 'Datos',
      date: '2026-10-08',
      place: 'Canal Tech Riders',
      url: 'https://luma.com/cal-24k3WtXkbkzA2tg'
    },
    {
      title: 'Workshop de seguridad para equipos',
      summary: 'Taller practico de hardening basico, checklist OWASP y analisis de riesgos frecuentes.',
      type: 'Workshop',
      modality: 'Presencial',
      topic: 'Ciberseguridad',
      date: '2026-10-15',
      place: 'Lab de ciber',
      url: 'https://luma.com/cal-24k3WtXkbkzA2tg'
    },
    {
      title: 'Soft Skills para equipos tecnicos',
      summary: 'Sesion para mejorar comunicacion, feedback y colaboracion entre perfiles tecnicos.',
      type: 'Orientacion',
      modality: 'Online',
      topic: 'Soft Skills',
      date: '2026-10-21',
      place: 'Streaming',
      url: 'https://luma.com/cal-24k3WtXkbkzA2tg'
    },
    {
      title: 'Ingenieria de APIs con Azure',
      summary: 'Practica de diseno de APIs, versionado y observabilidad para ecosistemas cloud.',
      type: 'Sesion tecnica',
      modality: 'Hibrido',
      topic: 'Azure',
      date: '2026-10-29',
      place: 'Tech Riders Hub',
      url: 'https://luma.com/cal-24k3WtXkbkzA2tg'
    }
  ];

  readonly types = ['Sesion tecnica', 'Orientacion', 'Empleabilidad', 'Podcast', 'Workshop', 'Woman Tech'] as const;
  readonly modalities = ['Online', 'Presencial', 'Hibrido'] as const;
  readonly topics = ['Azure', '.NET', 'Datos', 'Ciberseguridad', 'Carrera', 'Soft Skills', 'Comunidad'] as const;

  readonly selectedType = signal('');
  readonly selectedModality = signal('');
  readonly selectedTopic = signal('');
  readonly searchText = signal('');

  readonly filteredEvents = computed(() => {
    const query = this.searchText().trim().toLowerCase();
    return this.events.filter(event => {
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

  readonly eventCards = computed<UiResourceCardItem[]>(() => this.filteredEvents().map(event => ({
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
