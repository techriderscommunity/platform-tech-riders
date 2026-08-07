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
  @Input() label = '';
  @Input() options: UiSelectOption[] = [];
  @Input() value = '';

  @Output() readonly valueChange = new EventEmitter<string>();
}


