import { Meta, StoryObj } from '@storybook/angular';
import { UiMetricsStrip } from './metrics-strip';

const meta: Meta<UiMetricsStrip> = {
  title: 'Shared/Metrics Strip',
  component: UiMetricsStrip,
  tags: ['autodocs']
};

export default meta;
type Story = StoryObj<UiMetricsStrip>;

export const Default: Story = {
  args: {
    items: [
      { icon: '📅', value: '13', label: 'Años de comunidad' },
      { icon: '📚', value: '1300+', label: 'Tutoriales' },
      { icon: '🏫', value: '50+', label: 'Centros' },
      { icon: '👥', value: '1500+', label: 'Alumnos' }
    ]
  }
};
