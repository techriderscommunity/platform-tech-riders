import { ChangeDetectionStrategy, Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { UiProgressCards } from '@shared/ui/progress-cards/progress-cards';
import { UiResourceCards } from '@shared/ui/resource-cards/resource-cards';

@Component({
  selector: 'app-oportunidades',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [RouterLink, UiProgressCards, UiResourceCards],
  templateUrl: './oportunidades.html',
  styleUrl: './oportunidades.scss'
})
export class Oportunidades {
  readonly tracks = [
    {
      title: 'Primer empleo tech',
      detail: 'Rutas para perfiles junior con foco en transición real al mercado.',
      progress: 78,
      status: 'Junior',
      ctaLabel: 'Ver guía',
      ctaLink: '/orienta-tech'
    },
    {
      title: 'Upskilling profesional',
      detail: 'Sesiones y recursos para evolución de perfil técnico y liderazgo.',
      progress: 65,
      status: 'Profesional',
      ctaLabel: 'Explorar recursos',
      ctaLink: '/tutorials'
    },
    {
      title: 'Conexión con empresas',
      detail: 'Canales de colaboración, sesiones y oportunidades compartidas con partners.',
      progress: 71,
      status: 'Empresa',
      ctaLabel: 'Ir a empresas',
      ctaLink: '/companies'
    }
  ];

  readonly resources = [
    {
      mode: 'Comunidad',
      title: 'Banco de conocimiento Tech Riders',
      summary: 'Tutoriales, charlas y materiales prácticos para aprendizaje continuo.',
      tags: ['Tutoriales', 'Recursos', 'Aprendizaje'],
      meta: 'Actualización continua',
      ctaLabel: 'Ir a conocimiento',
      ctaLink: '/tutorials'
    },
    {
      mode: 'Actividad',
      title: 'Próximas sesiones y actividades',
      summary: 'Agenda pública con oportunidades para participar y hacer networking.',
      tags: ['Eventos', 'Sesiones', 'Networking'],
      meta: 'Calendario abierto',
      ctaLabel: 'Ver calendario',
      ctaLink: '/calendar'
    }
  ];
}
