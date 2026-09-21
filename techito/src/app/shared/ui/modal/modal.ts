import { AfterViewInit, ChangeDetectionStrategy, Component, ElementRef, EventEmitter, HostListener, Output, ViewChild, inject } from '@angular/core';
import { DOCUMENT, isPlatformBrowser } from '@angular/common';
import { PLATFORM_ID } from '@angular/core';

@Component({
  selector: 'app-ui-modal',
  standalone: true,
  templateUrl: './modal.html',
  styleUrl: './modal.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class UiModal implements AfterViewInit {
  private static nextId = 0;
  private readonly documentRef = inject(DOCUMENT);
  private readonly platformId = inject(PLATFORM_ID);
  private readonly elementRef = inject(ElementRef<HTMLElement>);
  private readonly triggerElement: HTMLElement | null = isPlatformBrowser(this.platformId)
    ? this.documentRef.activeElement as HTMLElement | null
    : null;

  @ViewChild('dialog', { static: true }) private readonly dialogRef!: ElementRef<HTMLElement>;

  @Output() readonly close = new EventEmitter<void>();

  readonly titleId = `ui-modal-title-${UiModal.nextId++}`;
  readonly descriptionId = `ui-modal-description-${UiModal.nextId++}`;

  constructor() {
    if (isPlatformBrowser(this.platformId)) {
      this.documentRef.body.classList.add('has-ui-modal');
    }
  }

  ngAfterViewInit(): void {
    if (!isPlatformBrowser(this.platformId)) {
      return;
    }

    queueMicrotask(() => this.focusFirstElement());
  }

  @HostListener('document:keydown.escape')
  onEscape(): void {
    this.close.emit();
  }

  @HostListener('document:keydown', ['$event'])
  onKeydown(event: KeyboardEvent): void {
    if (event.key !== 'Tab') {
      return;
    }

    const focusableElements = this.getFocusableElements();
    if (focusableElements.length === 0) {
      event.preventDefault();
      return;
    }

    const firstElement = focusableElements[0];
    const lastElement = focusableElements[focusableElements.length - 1];
    if (event.shiftKey && this.documentRef.activeElement === firstElement) {
      event.preventDefault();
      lastElement.focus();
    } else if (!event.shiftKey && this.documentRef.activeElement === lastElement) {
      event.preventDefault();
      firstElement.focus();
    }
  }

  onClose(): void {
    this.close.emit();
  }

  ngOnDestroy(): void {
    if (isPlatformBrowser(this.platformId)) {
      this.documentRef.body.classList.remove('has-ui-modal');
      this.triggerElement?.focus();
    }
  }

  private focusFirstElement(): void {
    const focusableElements = this.getFocusableElements();
    (focusableElements[0] ?? this.dialogRef.nativeElement).focus();
  }

  private getFocusableElements(): HTMLElement[] {
    const selector = [
      'button:not([disabled])',
      '[href]',
      'input:not([disabled])',
      'select:not([disabled])',
      'textarea:not([disabled])',
      '[tabindex]:not([tabindex="-1"])',
    ].join(',');

    return Array.from(this.dialogRef.nativeElement.querySelectorAll<HTMLElement>(selector));
  }
}


