import { ChangeDetectionStrategy, Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-ui-file-input',
  standalone: true,
  templateUrl: './file-input.html',
  styleUrl: './file-input.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class UiFileInput {
  @Input() label = '';
  @Input() accept = '*/*';
  @Output() readonly fileSelected = new EventEmitter<File | null>();

  onFileChange(event: Event): void {
    const input = event.target as HTMLInputElement;
    const file = input.files?.[0] ?? null;
    this.fileSelected.emit(file);
  }
}


