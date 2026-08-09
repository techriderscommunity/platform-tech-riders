import { ChangeDetectionStrategy, Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { UiJourneySteps } from '@shared/ui/journey-steps/journey-steps';
import { UiMetricsStrip } from '@shared/ui/metrics-strip/metrics-strip';

@Component({
  selector: 'app-woman-tech',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [RouterLink, UiMetricsStrip, UiJourneySteps],
  templateUrl: './woman-tech.html',
  styleUrl: './woman-tech.scss'
})
export class WomanTech {
  readonly metrics = [
    { icon: '💜', value: 'Woman Tech', label: 'Línea de comunidad' },
    { icon: '🎙️', value: 'Sesiones', label: 'Referentes y experiencias' },
    { icon: '🤝', value: 'Red', label: 'Acompañamiento y visibilidad' },
  ];

  readonly journey = [
    {
      step: '01',
      title: 'Inspiración',
      text: 'Historias y trayectorias de mujeres en tecnología con contexto real.'
    },
    {
      step: '02',
      title: 'Aprendizaje',
      text: 'Sesiones técnicas y recursos para fortalecer habilidades y confianza.'
    },
    {
      step: '03',
      title: 'Conexión',
      text: 'Vínculo con comunidad, redes profesionales y nuevas oportunidades.'
    }
  ];
}
