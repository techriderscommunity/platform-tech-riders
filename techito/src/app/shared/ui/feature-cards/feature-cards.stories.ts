import { Meta, StoryObj } from '@storybook/angular';
import { UiFeatureCards } from './feature-cards';

const meta: Meta<UiFeatureCards> = {
  title: 'Shared/Feature Cards',
  component: UiFeatureCards,
  tags: ['autodocs']
};

export default meta;
type Story = StoryObj<UiFeatureCards>;

export const Default: Story = {
  args: {
    items: [
      {
        icon: '📚',
        title: 'Formaciones regladas',
        description: 'Programas formales para iniciar o transformar tu carrera tech.',
        points: ['Ciclos formativos', 'Bootcamps certificados', 'Rutas de aprendizaje']
      },
      {
        icon: '💼',
        title: 'Empleo tech',
        description: 'Conexión directa con empresas que contratan perfiles junior.',
        points: ['Ofertas curadas', 'Prácticas', 'Eventos de hiring']
      }
    ]
  }
};
