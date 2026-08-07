import {
  AfterViewInit,
  ChangeDetectionStrategy,
  Component,
  ElementRef,
  Input,
  OnDestroy,
  ViewChild,
} from '@angular/core';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';

export interface UiCarouselItem {
  kind: 'image' | 'video';
  src: string;
  title: string;
  alt?: string;
}

@Component({
  selector: 'app-ui-media-carousel',
  standalone: true,
  templateUrl: './media-carousel.html',
  styleUrl: './media-carousel.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class UiMediaCarousel implements AfterViewInit, OnDestroy {
  @ViewChild('track', { static: false })
  track?: ElementRef<HTMLDivElement>;

  @Input() ariaLabel = 'Carrusel';
  @Input() autoplay = false;
  @Input() autoplayDelayMs = 3500;
  @Input() showCaptions = true;
  @Input() cardMinWidth = '260px';

  @Input()
  set items(value: UiCarouselItem[]) {
    this._items = value ?? [];
    this.videoSrcMap.clear();
  }

  get items(): UiCarouselItem[] {
    return this._items;
  }

  get hasMultipleItems(): boolean {
    return this._items.length > 1;
  }

  private _items: UiCarouselItem[] = [];
  private autoplayTimer: ReturnType<typeof setInterval> | null = null;
  private touchStartX = 0;
  private touchStartY = 0;
  private readonly videoSrcMap = new Map<string, SafeResourceUrl>();

  constructor(private readonly sanitizer: DomSanitizer) {}

  ngAfterViewInit(): void {
    this.startAutoplay();
  }

  ngOnDestroy(): void {
    this.stopAutoplay();
  }

  prev(): void {
    this.scrollBy(-1);
  }

  next(): void {
    this.scrollBy(1);
  }

  onMouseEnter(): void {
    this.stopAutoplay();
  }

  onMouseLeave(): void {
    this.startAutoplay();
  }

  onFocusIn(): void {
    this.stopAutoplay();
  }

  onFocusOut(): void {
    this.startAutoplay();
  }

  onTouchStart(event: TouchEvent): void {
    const touch = event.touches[0];
    this.touchStartX = touch.clientX;
    this.touchStartY = touch.clientY;
    this.stopAutoplay();
  }

  onTouchEnd(event: TouchEvent): void {
    const touch = event.changedTouches[0];
    const deltaX = touch.clientX - this.touchStartX;
    const deltaY = touch.clientY - this.touchStartY;

    if (Math.abs(deltaX) > 40 && Math.abs(deltaX) > Math.abs(deltaY)) {
      this.scrollBy(deltaX < 0 ? 1 : -1);
    }

    this.startAutoplay();
  }

  getSafeVideoUrl(src: string): SafeResourceUrl {
    const existing = this.videoSrcMap.get(src);
    if (existing) {
      return existing;
    }

    const safeUrl = this.sanitizer.bypassSecurityTrustResourceUrl(src);
    this.videoSrcMap.set(src, safeUrl);
    return safeUrl;
  }

  private startAutoplay(): void {
    if (!this.autoplay || this.autoplayTimer || !this.track?.nativeElement || !this.hasMultipleItems) {
      return;
    }

    this.autoplayTimer = setInterval(() => this.scrollBy(1), this.autoplayDelayMs);
  }

  private stopAutoplay(): void {
    if (!this.autoplayTimer) {
      return;
    }

    clearInterval(this.autoplayTimer);
    this.autoplayTimer = null;
  }

  private scrollBy(direction: 1 | -1): void {
    const trackEl = this.track?.nativeElement;
    if (!trackEl || !this.hasMultipleItems) {
      return;
    }

    const firstSlide = trackEl.querySelector<HTMLElement>('.ui-carousel-slide');
    const gap = 16;
    const scrollAmount = firstSlide ? firstSlide.clientWidth + gap : Math.max(240, Math.floor(trackEl.clientWidth * 0.8));
    const target = trackEl.scrollLeft + direction * scrollAmount;
    const maxScroll = trackEl.scrollWidth - trackEl.clientWidth;

    if (direction > 0 && target >= maxScroll - 4) {
      trackEl.scrollTo({ left: 0, behavior: 'smooth' });
      return;
    }

    if (direction < 0 && target <= 0) {
      trackEl.scrollTo({ left: maxScroll, behavior: 'smooth' });
      return;
    }

    trackEl.scrollBy({ left: direction * scrollAmount, behavior: 'smooth' });
  }
}


