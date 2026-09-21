import { ChangeDetectionStrategy, Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-ui-checkbox',
  standalone: true,
  templateUrl: './checkbox.html',
  styleUrl: './checkbox.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class UiCheckbox {
  @Input() label = '';
  @Input() checked = false;
  @Input() disabled = false;
  @Output() readonly checkedChange = new EventEmitter<boolean>();

  toggle(): void {
    if (this.disabled) {
      return;
    }

    this.checkedChange.emit(!this.checked);
  }
}
