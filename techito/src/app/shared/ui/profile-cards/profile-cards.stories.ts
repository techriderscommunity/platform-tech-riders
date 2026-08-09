import { Meta, StoryObj } from '@storybook/angular';
import { UiProfileCards } from './profile-cards';

const meta: Meta<UiProfileCards> = {
  title: 'Shared/Profile Cards',
  component: UiProfileCards,
  tags: ['autodocs']
};

export default meta;
type Story = StoryObj<UiProfileCards>;

export const Default: Story = {
  args: {
    items: [
      {
        name: 'Sergio Hierro',
        role: 'Coordinador',
        imageSrc: 'assets/staff/sergio-hierro.png',
        imageAlt: 'Sergio Hierro',
        badge: 'Staff Lead'
      },
      {
        name: 'Ana Pereira',
        role: 'Mentora',
        imageSrc: 'assets/staff/ana-pereira.jpg',
        imageAlt: 'Ana Pereira',
        badge: 'Staff'
      }
    ]
  }
};
