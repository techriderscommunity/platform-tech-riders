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
import { RouterLink } from '@angular/router';

export interface UiCarouselItem {
  kind: 'image' | 'video';
  src: string;
  title: string;
  alt?: string;
  subtitle?: string;
  link?: string | readonly string[];
  socials?: Array<{
    platform: 'linkedin' | 'github' | 'x' | 'instagram' | 'youtube';
    href: string;
  }>;
}

@Component({
  selector: 'app-ui-media-carousel',
  standalone: true,
  imports: [RouterLink],
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
  @Input() fixedColumnsDesktop: number | null = null;

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

  get showControls(): boolean {
    if (this.fixedColumnsDesktop === null || this.fixedColumnsDesktop === undefined) {
      return this.hasMultipleItems;
    }

    return this._items.length > this.fixedColumnsDesktop;
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
    this.scrollByDirection(-1);
  }

  next(): void {
    this.scrollByDirection(1);
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
      this.scrollByDirection(deltaX < 0 ? 1 : -1);
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
    if (!this.autoplay || this.autoplayTimer || !this.track?.nativeElement || !this.showControls) {
      return;
    }

    this.autoplayTimer = setInterval(() => this.scrollByDirection(1), this.autoplayDelayMs);
  }

  private stopAutoplay(): void {
    if (!this.autoplayTimer) {
      return;
    }

    clearInterval(this.autoplayTimer);
    this.autoplayTimer = null;
  }

  private scrollByDirection(direction: 1 | -1): void {
    const trackEl = this.track?.nativeElement;
    if (!trackEl || !this.showControls) {
      return;
    }

    const maxScrollLeft = trackEl.scrollWidth - trackEl.clientWidth;
    const edgeTolerance = 2;

    if (direction > 0 && trackEl.scrollLeft >= maxScrollLeft - edgeTolerance) {
      trackEl.scrollTo({ left: 0, behavior: 'smooth' });
      return;
    }

    if (direction < 0 && trackEl.scrollLeft <= edgeTolerance) {
      trackEl.scrollTo({ left: maxScrollLeft, behavior: 'smooth' });
      return;
    }

    const slides = Array.from(trackEl.querySelectorAll<HTMLElement>('.ui-carousel-slide'));
    if (!slides.length) {
      return;
    }

    const currentScroll = trackEl.scrollLeft;
    let currentIndex = 0;
    let smallestDistance = Number.POSITIVE_INFINITY;

    for (let index = 0; index < slides.length; index += 1) {
      const distance = Math.abs(slides[index].offsetLeft - currentScroll);
      if (distance < smallestDistance) {
        smallestDistance = distance;
        currentIndex = index;
      }
    }

    const nextIndex = direction > 0
      ? (currentIndex + 1) % slides.length
      : (currentIndex - 1 + slides.length) % slides.length;

    trackEl.scrollTo({
      left: slides[nextIndex].offsetLeft,
      behavior: 'smooth',
    });
  }
}


