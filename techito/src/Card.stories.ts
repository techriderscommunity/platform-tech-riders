import { Meta, StoryObj } from '@storybook/angular';
import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-card-demo',
  standalone: true,
  template: `<div [class]="cardClass">
    <h3>{{ title }}</h3>
    <p>{{ content }}</p>
  </div>`,
  styles: [`
    :host { display: block; }
  `]
})
class CardDemoComponent {
  @Input() title = 'Card Title';
  @Input() content = 'This is card content with default padding and shadow.';
  @Input() cardClass = 'card';
}

const meta: Meta<CardDemoComponent> = {
  title: 'Components/Card',
  component: CardDemoComponent,
  tags: ['autodocs'],
  argTypes: {
    title: { control: 'text', description: 'Card title' },
    content: { control: 'text', description: 'Card content' },
    cardClass: { control: 'text', description: 'CSS classes' }
  }
};

export default meta;
type Story = StoryObj<CardDemoComponent>;

export const Default: Story = {
  args: {
    title: 'Default Card',
    content: 'This is a card with default styling, shadow, and hover effect.',
    cardClass: 'card'
  }
};

export const Compact: Story = {
  args: {
    title: 'Compact Card',
    content: 'This is a compact card with less shadow and padding.',
    cardClass: 'card card-compact'
  }
};

export const WithHover: Story = {
  args: {
    title: 'Hover Card',
    content: 'Hover over this card to see the shadow and lift effect.',
    cardClass: 'card'
  }
};

export const Stacked: Story = {
  args: {
    title: 'Card Stack',
    content: 'Multiple cards can be stacked in a grid layout.',
    cardClass: 'card'
  }
};
