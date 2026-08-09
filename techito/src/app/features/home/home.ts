import { ChangeDetectionStrategy, Component, DestroyRef, OnInit, computed, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { tap, catchError } from 'rxjs/operators';
import { EMPTY } from 'rxjs';
import { EventosService, EventoResumen } from '../intranet/fp-tour/services/eventos.service';
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
  private readonly eventosService = inject(EventosService);
  private readonly destroyRef = inject(DestroyRef);

  readonly eventos = signal<EventoResumen[]>([]);
  readonly loadingEventos = signal(false);
  readonly proximosEventos = computed(() => this.eventos().filter(evento => !evento.esPasado).slice(0, 6));
  readonly eventosPasadosFotos = [
    { src: 'assets/techito_salero_ming.jpg', alt: 'Talk de TechRiders', label: 'Talks' },
    { src: 'assets/techito_karmela.jpg', alt: 'Encuentro #FPTour', label: '#FPTour' },
    { src: 'assets/techito_bici_tajamar.jpg', alt: 'Evento en Tajamar Tech', label: 'Tajamar Tech' },
    { src: 'assets/techito_piscineo.jpg', alt: 'Evento externo de comunidad', label: 'Eventos externos' },
  ];
  readonly eventosPasadosSlides: UiCarouselItem[] = this.eventosPasadosFotos.map(foto => ({
    kind: 'image',
    src: foto.src,
    alt: foto.alt,
    title: foto.label,
  }));
  readonly stats = [
    { value: '13', label: 'Años de comunidad', icon: '📅' },
    { value: '1300+', label: 'Tutoriales publicados', icon: '📚' },
    { value: '50+', label: 'Centros inscritos #FPTOUR', icon: '🏫' },
    { value: '1500+', label: 'Alumnos #FPTOUR', icon: '👥' },
    { value: '80+', label: 'Sesiones #FPTOUR', icon: '🎤' },
    { value: '67', label: 'Sesiones en Tajamar Tech', icon: '🎤' },
    { value: '20+', label: 'Colaboraciones realizadas con otras comunidades', icon: '🫂' },
    { value: '5', label: 'Eventos propios', icon: '🎉' }

  ];

  readonly profilePanelCards = [
    {
      title: 'Docentes',
      description: 'Impulsa el talento tecnológico de tu alumnado. Comparte conocimiento y accede a recursos para el aula.',
      icon: '🎓',
      cta: 'Explorar',
      link: '/centers',
      accent: 'violet'
    },
    {
      title: 'Estudiantes',
      description: 'Aprende tecnologías, descubre formación y accede a oportunidades para desarrollar tu futuro.',
      icon: '🧑‍💻',
      cta: 'Explorar',
      link: '/orienta-tech',
      accent: 'cyan'
    },
    {
      title: 'Profesionales',
      description: 'Impulsa tu carrera en tecnología, comparte experiencia y amplía tu red de contactos.',
      icon: '💼',
      cta: 'Explorar',
      link: '/events',
      accent: 'blue'
    },
    {
      title: 'Empresas',
      description: 'Conecta con el talento, participa en eventos y comparte conocimiento real con la comunidad.',
      icon: '🏢',
      cta: 'Explorar',
      link: '/companies',
      accent: 'amber'
    },
    {
      title: 'Orientadores',
      description: 'Accede a recursos y actividades tecnológicas para tu alumnado y descubre iniciativas STEM.',
      icon: '🧭',
      cta: 'Explorar',
      link: '/orienta-tech',
      accent: 'pink'
    },
    {
      title: 'Starters',
      description: 'Descubre profesiones, formaciones y tus primeros pasos en el mundo tech. No necesitas experiencia.',
      icon: '🚀',
      cta: 'Explorar',
      link: '/tutorials',
      accent: 'teal'
    },
    {
      title: 'Women in Tech',
      description: 'Referentes, ayudas, comunidad y oportunidades para mujeres que quieren crecer en tecnología.',
      icon: '♀️',
      cta: 'Explorar',
      link: '/woman-tech',
      accent: 'fuchsia'
    },
    {
      title: 'Conócenos',
      description: 'Descubre quiénes somos, nuestra misión, valores y cómo trabajamos para impulsar el talento tech.',
      icon: '👥',
      cta: 'Explorar',
      link: '/about-us',
      accent: 'sky'
    }
  ] as const;

  ngOnInit(): void {
    this.loadingEventos.set(true);
    this.eventosService
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



