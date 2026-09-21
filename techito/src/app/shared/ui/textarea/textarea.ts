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
  private static nextId = 0;

  @Input() label = '';
  @Input() id = '';
  @Input() name = '';
  @Input() placeholder = '';
  @Input() rows = 4;
  @Input() required = false;
  @Input() disabled = false;
  @Input() invalid = false;
  @Input() describedBy = '';
  @Input() errorMessage = '';
  @Input() value = '';

  @Output() readonly valueChange = new EventEmitter<string>();

  readonly generatedId = `ui-textarea-${UiTextarea.nextId++}`;

  get controlId(): string {
    return this.id || this.name || this.generatedId;
  }
}


