import { Meta, StoryObj } from '@storybook/angular';
import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-badge-demo',
  standalone: true,
  template: `<span [class]="badgeClass">{{ label }}</span>`,
  styles: [`
    :host { display: inline-block; }
  `]
})
class BadgeDemoComponent {
  @Input() label = 'Badge';
  @Input() badgeClass = 'badge';
}

const meta: Meta<BadgeDemoComponent> = {
  title: 'Components/Badge',
  component: BadgeDemoComponent,
  tags: ['autodocs'],
  argTypes: {
    label: { control: 'text', description: 'Badge text' },
    badgeClass: { control: 'text', description: 'CSS classes' }
  }
};

export default meta;
type Story = StoryObj<BadgeDemoComponent>;

export const Default: Story = {
  args: {
    label: 'Default',
    badgeClass: 'badge'
  }
};

export const Info: Story = {
  args: {
    label: 'Info',
    badgeClass: 'badge badge-info'
  }
};

export const Success: Story = {
  args: {
    label: 'Success',
    badgeClass: 'badge badge-success'
  }
};

export const Warning: Story = {
  args: {
    label: 'Warning',
    badgeClass: 'badge badge-warning'
  }
};

export const Error: Story = {
  args: {
    label: 'Error',
    badgeClass: 'badge badge-error'
  }
};

export const AllVariants: Story = {
  render: () => ({
    template: `
      <div class="flex gap-2">
        <span class="badge">Default</span>
        <span class="badge badge-info">Info</span>
        <span class="badge badge-success">Success</span>
        <span class="badge badge-warning">Warning</span>
        <span class="badge badge-error">Error</span>
      </div>
    `,
    styles: [`
      :host { display: block; }
    `]
  })
};
