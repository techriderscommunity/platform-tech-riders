import { ChangeDetectionStrategy, Component, EventEmitter, HostListener, Output, inject } from '@angular/core';
import { DOCUMENT, isPlatformBrowser } from '@angular/common';
import { PLATFORM_ID } from '@angular/core';

@Component({
  selector: 'app-ui-modal',
  standalone: true,
  templateUrl: './modal.html',
  styleUrl: './modal.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class UiModal {
  private static nextId = 0;
  private readonly documentRef = inject(DOCUMENT);
  private readonly platformId = inject(PLATFORM_ID);

  @Output() readonly close = new EventEmitter<void>();

  readonly titleId = `ui-modal-title-${UiModal.nextId++}`;

  constructor() {
    if (isPlatformBrowser(this.platformId)) {
      this.documentRef.body.classList.add('has-ui-modal');
    }
  }

  @HostListener('document:keydown.escape')
  onEscape() {
    this.close.emit();
  }

  onClose() {
    this.close.emit();
  }

  ngOnDestroy() {
    if (isPlatformBrowser(this.platformId)) {
      this.documentRef.body.classList.remove('has-ui-modal');
    }
  }
}


