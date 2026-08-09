import { Meta, StoryObj } from '@storybook/angular';
import { UiJourneySteps } from './journey-steps';

const meta: Meta<UiJourneySteps> = {
  title: 'Shared/Journey Steps',
  component: UiJourneySteps,
  tags: ['autodocs']
};

export default meta;
type Story = StoryObj<UiJourneySteps>;

export const Default: Story = {
  args: {
    kicker: 'Journey',
    title: 'Ruta de incorporación',
    steps: [
      {
        step: 'Paso 01',
        title: 'Selecciona intención',
        text: 'Escoges el flujo que mejor representa tu objetivo en la comunidad.'
      },
      {
        step: 'Paso 02',
        title: 'Comparte contexto',
        text: 'Enviamos al staff la información mínima para actuar con rapidez.'
      },
      {
        step: 'Paso 03',
        title: 'Recibes siguiente acción',
        text: 'Te devolvemos un siguiente paso operativo claro y accionable.'
      }
    ]
  }
};
