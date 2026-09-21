import { ChangeDetectionStrategy, Component, EventEmitter, Input, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-ui-text-field',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './text-field.html',
  styleUrl: './text-field.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class UiTextField {
  private static nextId = 0;

  @Input() label = '';
  @Input() id = '';
  @Input() type = 'text';
  @Input() name = '';
  @Input() placeholder = '';
  @Input() autocomplete = 'off';
  @Input() required = false;
  @Input() disabled = false;
  @Input() invalid = false;
  @Input() describedBy = '';
  @Input() errorMessage = '';
  @Input() value = '';

  @Output() readonly valueChange = new EventEmitter<string>();

  readonly generatedId = `ui-text-field-${UiTextField.nextId++}`;

  get controlId(): string {
    return this.id || this.name || this.generatedId;
  }
}


