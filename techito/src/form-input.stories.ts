import { Meta, StoryObj } from '@storybook/angular';
import { Component, Input } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-form-input-demo',
  standalone: true,
  imports: [ReactiveFormsModule],
  template: `
    <div class="form-group">
      <label [for]="inputId">{{ label }}</label>
      <input
        [id]="inputId"
        [type]="inputType"
        [class]="inputClass"
        [placeholder]="placeholder"
        [attr.aria-describedby]="helpId"
        [formControl]="control"
      />
      @if (helpText) {
        <p [id]="helpId" style="font-size: 0.875rem; color: var(--text-secondary);">
          {{ helpText }}
        </p>
      }
      @if (errorText) {
        <p [id]="helpId" role="alert" style="color: var(--accent-error);">
          {{ errorText }}
        </p>
      }
    </div>
  `,
  styles: [`
    :host { display: block; }
  `]
})
class FormInputDemoComponent {
  @Input() label = 'Input Label';
  @Input() inputType = 'text';
  @Input() inputClass = 'form-input';
  @Input() placeholder = 'Enter text...';
  @Input() helpText = '';
  @Input() errorText = '';
  @Input() inputId = 'input-1';
  @Input() helpId = 'input-help-1';

  control = new FormControl('');
}

const meta: Meta<FormInputDemoComponent> = {
  title: 'Components/Form Input',
  component: FormInputDemoComponent,
  tags: ['autodocs'],
  argTypes: {
    label: { control: 'text' },
    inputType: { control: 'text' },
    inputClass: { control: 'text' },
    placeholder: { control: 'text' },
    helpText: { control: 'text' },
    errorText: { control: 'text' }
  }
};

export default meta;
type Story = StoryObj<FormInputDemoComponent>;

export const Basic: Story = {
  args: {
    label: 'Full Name',
    inputType: 'text',
    placeholder: 'John Doe',
    helpText: '',
    errorText: ''
  }
};

export const WithHelp: Story = {
  args: {
    label: 'Email Address',
    inputType: 'email',
    placeholder: 'you@example.com',
    helpText: 'We\'ll never share your email.',
    errorText: ''
  }
};

export const WithError: Story = {
  args: {
    label: 'Password',
    inputType: 'password',
    inputClass: 'form-input is-error',
    placeholder: 'Enter password',
    helpText: '',
    errorText: 'Password must be at least 8 characters.'
  }
};

export const Success: Story = {
  args: {
    label: 'Username',
    inputType: 'text',
    inputClass: 'form-input is-success',
    placeholder: 'username',
    helpText: 'Username is available!',
    errorText: ''
  }
};

export const Required: Story = {
  args: {
    label: 'Required Field *',
    inputType: 'text',
    placeholder: 'This field is required',
    helpText: 'Enter a value to proceed.',
    errorText: ''
  }
};
