import { ChangeDetectionStrategy, Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { UiFeatureCards } from '@shared/ui/feature-cards/feature-cards';
import { UiProgressCards } from '@shared/ui/progress-cards/progress-cards';

@Component({
  selector: 'app-empresas',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [RouterLink, UiFeatureCards, UiProgressCards],
  templateUrl: './empresas.html',
  styleUrl: './empresas.scss'
})
export class Empresas {
  readonly valueCards = [
    {
      icon: '🎤',
      title: 'Propón una sesión',
      description: 'Comparte casos reales y aprendizajes prácticos desde tu organización.',
      points: ['Formato adaptable', 'Audiencias concretas', 'Coordinación operativa']
    },
    {
      icon: '🧠',
      title: 'Participa en actividades',
      description: 'Impulsa workshops, retos y formatos de comunidad con impacto formativo.',
      points: ['Co-creación con Tech Riders', 'Visibilidad de marca técnica', 'Continuidad anual']
    },
    {
      icon: '🚀',
      title: 'Conecta con talento',
      description: 'Activa itinerarios para detectar perfiles junior y senior alineados con tu stack.',
      points: ['Perfiles filtrados', 'Canales de contacto', 'Seguimiento de pipeline']
    }
  ];

  readonly processCards = [
    {
      title: 'Definición de colaboración',
      detail: 'Identificamos objetivo, formato y audiencia de la iniciativa.',
      progress: 100,
      status: 'Paso 1'
    },
    {
      title: 'Planificación y calendario',
      detail: 'Alineamos fechas, recursos y coordinación con la comunidad.',
      progress: 100,
      status: 'Paso 2'
    },
    {
      title: 'Ejecución y seguimiento',
      detail: 'Publicamos, ejecutamos y medimos resultados para repetir impacto.',
      progress: 100,
      status: 'Paso 3'
    }
  ];
}
