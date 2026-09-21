import { ChangeDetectionStrategy, Component, EventEmitter, Input, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';

export interface UiSelectOption {
  label: string;
  value: string;
}

@Component({
  selector: 'app-ui-select',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './select.html',
  styleUrl: './select.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class UiSelect {
  private static nextId = 0;

  @Input() label = '';
  @Input() id = '';
  @Input() name = '';
  @Input() options: UiSelectOption[] = [];
  @Input() value = '';
  @Input() disabled = false;
  @Input() invalid = false;
  @Input() describedBy = '';
  @Input() errorMessage = '';

  @Output() readonly valueChange = new EventEmitter<string>();

  readonly generatedId = `ui-select-${UiSelect.nextId++}`;

  get controlId(): string {
    return this.id || this.name || this.generatedId;
  }
}


