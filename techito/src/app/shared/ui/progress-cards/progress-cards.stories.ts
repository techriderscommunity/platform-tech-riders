import { Meta, StoryObj } from '@storybook/angular';
import { UiProgressCards } from './progress-cards';

const meta: Meta<UiProgressCards> = {
  title: 'Shared/Progress Cards',
  component: UiProgressCards,
  tags: ['autodocs']
};

export default meta;
type Story = StoryObj<UiProgressCards>;

export const Default: Story = {
  args: {
    items: [
      {
        title: 'Experiencia web',
        value: 'MVP',
        detail: 'Mejoras visuales y navegación pública alineadas a design system.',
        progress: 72,
        status: 'En ejecución',
        ctaLabel: 'Ver tutoriales',
        ctaLink: '/tutorials'
      },
      {
        title: 'Comunidad',
        value: 'Activa',
        detail: 'Eventos y recursos en ritmo de publicación continuo.',
        progress: 81,
        status: 'Activa',
        ctaLabel: 'Ir a eventos',
        ctaLink: '/events'
      }
    ]
  }
};
