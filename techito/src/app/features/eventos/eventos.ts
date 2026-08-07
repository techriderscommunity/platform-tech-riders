import { ChangeDetectionStrategy, Component, DestroyRef, computed, inject, OnInit, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { catchError, EMPTY, tap } from 'rxjs';
import { EventosService, EventoResumen } from '../intranet/fp-tour/services/eventos.service';
import { UiCarouselItem, UiMediaCarousel  } from '@shared/ui/media-carousel/media-carousel';
import { PodcastService } from './services/podcast.service';

interface GaleriaItem {
  src: string;
  alt: string;
}

interface GaleriaGrupo {
  title: string;
  subtitle: string;
  items: GaleriaItem[];
}

@Component({
  selector: 'app-eventos',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [RouterLink, UiMediaCarousel],
  templateUrl: './eventos.html',
  styleUrl: './eventos.scss'
})
export class Eventos implements OnInit {
  private readonly eventosService = inject(EventosService);
  private readonly podcastService = inject(PodcastService);
  private readonly destroyRef = inject(DestroyRef);

  readonly eventos = signal<EventoResumen[]>([]);
  readonly loadingEventos = signal(false);
  readonly proximosEventos = computed(() => this.eventos().filter(evento => !evento.esPasado));
  readonly eventosPasados = computed(() => this.eventos().filter(evento => evento.esPasado));
  readonly loadingTalks = signal(false);

  readonly talksPodcastUrl = 'https://www.youtube.com/@TechRidersMedia/podcasts';
  private readonly talksHistoricoFallback: UiCarouselItem[] = [
    {
      kind: 'video',
      title: 'Comunidad, aprendizaje y cerrar ciclos: Tech Riders Talks | Salero de Ming',
      src: 'https://www.youtube-nocookie.com/embed/YekC-fVM3Ig'
    },
    {
      kind: 'video',
      title: 'Liderazgo técnico, comunidad y crecimiento profesional | Sergio Hernández',
      src: 'https://www.youtube-nocookie.com/embed/NHkw3rh1BO8'
    },
    {
      kind: 'video',
      title: 'IA, liderazgo y comunidad: experiencia sin filtros | Javier Pallo',
      src: 'https://www.youtube-nocookie.com/embed/qJUUlvvH3_g'
    },
    {
      kind: 'video',
      title: 'Ciberseguridad real: pentesting, red team y LockShields | Marco Carrasco',
      src: 'https://www.youtube-nocookie.com/embed/IOi91LjE0m4'
    },
    {
      kind: 'video',
      title: 'De junior a senior: claves reales para crecer en tecnología | María & Elías',
      src: 'https://www.youtube-nocookie.com/embed/o6bGKi8y2eY'
    }
  ];
  readonly talksHistorico = signal<UiCarouselItem[]>(this.talksHistoricoFallback);

  readonly galerias: GaleriaGrupo[] = [
    {
      title: 'Talks',
      subtitle: 'Charlas y encuentros de la comunidad técnica.',
      items: [
        { src: 'assets/techito_salero_ming.jpg', alt: 'Talk en evento TechRiders' },
        { src: 'assets/techito_salero_ming.jpg', alt: 'Comunidad participando en una charla' },
        { src: 'assets/techito_salero_ming.jpg', alt: 'Ponencia técnica en TechRiders' }
      ]
    },
    {
      title: '#FPTour',
      subtitle: 'Meetups y sesiones en centros de formación.',
      items: [
        { src: 'assets/techito_karmela.jpg', alt: 'Evento #FPTour en aula' },
        { src: 'assets/techito_karmela.jpg', alt: 'Networking durante #FPTour' },
        { src: 'assets/techito_karmela.jpg', alt: 'Participantes de #FPTour' }
      ]
    },
    {
      title: 'Eventos externos',
      subtitle: 'Conferencias, webinars y colaboraciones con otras comunidades.',
      items: [
        { src: 'assets/techito_piscineo.jpg', alt: 'Evento externo de la comunidad TechRiders' },
        { src: 'assets/techito_bici_tajamar.jpg', alt: 'Conferencia y networking de TechRiders' },
        { src: 'assets/techito_piscineo.jpg', alt: 'Participación de TechRiders en evento externo' }
      ]
    }
  ];

  ngOnInit(): void {
    this.loadTalksHistorico();

    this.loadingEventos.set(true);
    this.eventosService
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



