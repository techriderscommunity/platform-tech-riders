import { ChangeDetectionStrategy, Component, DestroyRef, OnInit, computed, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { tap, catchError } from 'rxjs/operators';
import { EMPTY } from 'rxjs';
import { EventosService, EventoResumen } from '../intranet/fp-tour/services/eventos.service';
import { UiCarouselItem, UiMediaCarousel  } from '@shared/ui/media-carousel/media-carousel';

@Component({
  selector: 'app-home',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [RouterLink, UiMediaCarousel],
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

  readonly roles = [
    {
      icon: '👩‍💻',
      title: 'Particular',
      subtitle: 'Voluntario / Ponente',
      desc: '¿Quieres compartir tu conocimiento? Únete como voluntario o propón una charla técnica.',
      cta: 'Quiero colaborar',
      link: '/join'
    },
    {
      icon: '🎓',
      title: 'Centro Formador',
      subtitle: 'Solicita una sesión gratuita',
      desc: '¿Diriges un centro de formación? Solicita sesiones gratuitas de expertos del sector.',
      cta: 'Solicitar sesión',
      link: '/join'
    }
  ];

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



