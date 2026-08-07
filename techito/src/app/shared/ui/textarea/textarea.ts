import { ChangeDetectionStrategy, Component, EventEmitter, Input, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-ui-textarea',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './textarea.html',
  styleUrl: './textarea.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class UiTextarea {
  @Input() label = '';
  @Input() name = '';
  @Input() placeholder = '';
  @Input() rows = 4;
  @Input() required = false;
  @Input() value = '';

  @Output() readonly valueChange = new EventEmitter<string>();
}


