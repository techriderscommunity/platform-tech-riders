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
  @Input() label = '';
  @Input() type = 'text';
  @Input() name = '';
  @Input() placeholder = '';
  @Input() autocomplete = 'off';
  @Input() required = false;
  @Input() value = '';

  @Output() readonly valueChange = new EventEmitter<string>();
}


