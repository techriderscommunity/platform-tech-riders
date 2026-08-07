import { Meta, StoryObj } from '@storybook/angular';
import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-button-demo',
  standalone: true,
  template: `<button [class]="buttonClass" [disabled]="disabled">{{ label }}</button>`,
  styles: [`
    :host { display: inline-block; }
  `]
})
class ButtonDemoComponent {
  @Input() label = 'Button';
  @Input() buttonClass = 'btn btn-primary';
  @Input() disabled = false;
}

const meta: Meta<ButtonDemoComponent> = {
  title: 'Components/Button',
  component: ButtonDemoComponent,
  tags: ['autodocs'],
  argTypes: {
    label: { control: 'text', description: 'Button text' },
    buttonClass: { control: 'text', description: 'CSS classes' },
    disabled: { control: 'boolean', description: 'Disabled state' }
  }
};

export default meta;
type Story = StoryObj<ButtonDemoComponent>;

export const Primary: Story = {
  args: {
    label: 'Primary Button',
    buttonClass: 'btn btn-primary'
  }
};

export const Secondary: Story = {
  args: {
    label: 'Secondary Button',
    buttonClass: 'btn btn-secondary'
  }
};

export const Outline: Story = {
  args: {
    label: 'Outline Button',
    buttonClass: 'btn btn-outline'
  }
};

export const Small: Story = {
  args: {
    label: 'Small Button',
    buttonClass: 'btn btn-primary btn-sm'
  }
};

export const Large: Story = {
  args: {
    label: 'Large Button (Touch-Friendly)',
    buttonClass: 'btn btn-primary btn-lg'
  }
};

export const Disabled: Story = {
  args: {
    label: 'Disabled Button',
    buttonClass: 'btn btn-primary',
    disabled: true
  }
};

export const Loading: Story = {
  args: {
    label: 'Loading...',
    buttonClass: 'btn btn-primary is-loading',
    disabled: true
  }
};
