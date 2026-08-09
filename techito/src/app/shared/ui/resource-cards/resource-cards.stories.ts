import { Meta, StoryObj } from '@storybook/angular';
import { UiResourceCards } from './resource-cards';

const meta: Meta<UiResourceCards> = {
  title: 'Shared/UI/Resource Cards',
  component: UiResourceCards,
  tags: ['autodocs'],
};

export default meta;
type Story = StoryObj<UiResourceCards>;

export const Default: Story = {
  args: {
    items: [
      {
        mode: 'Digital',
        title: 'Microsoft Virtual Briefing - Explorando Copilot Chat',
        summary: 'Sesión orientada a escenarios reales para organizaciones con foco en adopción segura.',
        tags: ['IA', 'Copilot', 'Microsoft 365'],
        meta: '2026-01-21 · 16:00 - 17:00 CET',
        ctaLabel: 'Registro y detalles',
        ctaHref: '#',
      },
      {
        mode: 'En persona',
        title: 'Celebración 30 Años Microsoft',
        summary: 'Encuentro presencial para networking de comunidad y visión de plataforma.',
        tags: ['Comunidad', 'Evento'],
        meta: '2026-02-03 · 18:00 - 20:00 CET',
        ctaLabel: 'Registro y detalles',
        ctaHref: '#',
      },
    ],
  },
};
