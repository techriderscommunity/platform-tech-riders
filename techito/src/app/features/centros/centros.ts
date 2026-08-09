import { ChangeDetectionStrategy, Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { UiFeatureCards } from '@shared/ui/feature-cards/feature-cards';
import { UiMetricsStrip } from '@shared/ui/metrics-strip/metrics-strip';

@Component({
  selector: 'app-centros',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [RouterLink, UiMetricsStrip, UiFeatureCards],
  templateUrl: './centros.html',
  styleUrl: './centros.scss'
})
export class Centros {
  readonly metrics = [
    { icon: '🏫', value: 'Centros', label: 'Red educativa abierta' },
    { icon: '🗺️', value: 'Multi-zona', label: 'Cobertura territorial' },
    { icon: '🎓', value: 'FP + Grado', label: 'Perfiles formativos' },
  ];

  readonly cards = [
    {
      icon: '📍',
      title: 'Dónde estudiar',
      description: 'Explora centros por zona y descubre rutas formativas orientadas a perfiles tech.',
      points: ['Búsqueda por provincia', 'Ficha de centro', 'Programas destacados']
    },
    {
      icon: '🧑‍🏫',
      title: 'Sesiones para aulas',
      description: 'Solicita sesiones para estudiantes con foco en empleabilidad, especialización y realidad sectorial.',
      points: ['Formato presencial/online', 'Temáticas por nivel', 'Coordinación con Staff']
    },
    {
      icon: '🤝',
      title: 'Colaboración educativa',
      description: 'Conecta con la comunidad para actividades, talleres y propuestas conjuntas de alto impacto.',
      points: ['Diseño de iniciativas', 'Calendario coordinado', 'Seguimiento y continuidad']
    }
  ];
}
