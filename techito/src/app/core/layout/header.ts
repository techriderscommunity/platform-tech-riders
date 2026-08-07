import {
  ChangeDetectionStrategy,
  Component,
  computed,
  inject,
  signal,
  OnInit,
  OnDestroy,
  HostListener,
  PLATFORM_ID,
} from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { RouterLink, RouterLinkActive, Router } from '@angular/router';
import { AuthService } from '../auth/auth.service';

@Component({
  selector: 'app-header',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [RouterLink, RouterLinkActive],
  templateUrl: './header.html',
  styleUrl: './header.scss',
})
export class Header implements OnInit, OnDestroy {
  private readonly router = inject(Router);
  private readonly authService = inject(AuthService);
  private readonly platformId = inject(PLATFORM_ID);
  private get isBrowser() { return isPlatformBrowser(this.platformId); }

  readonly isLoggedIn = computed(() => this.authService.isAuthenticated());
  readonly mobileMenuOpen = signal(false);
  readonly isScrolled = signal(false);
  readonly isDarkMode = signal(true);

  @HostListener('window:scroll')
  onScroll() {
    if (this.isBrowser) {
      this.isScrolled.set(window.scrollY > 10);
    }
  }

  toggleMobileMenu() {
    this.mobileMenuOpen.update((v) => !v);
  }

  toggleTheme() {
    const newMode = !this.isDarkMode();
    this.isDarkMode.set(newMode);
    if (this.isBrowser) {
      document.documentElement.setAttribute('data-theme', newMode ? 'dark' : 'light');
      localStorage.setItem('tr-theme', newMode ? 'dark' : 'light');
    }
  }

  closeMobileMenu() {
    this.mobileMenuOpen.set(false);
  }

  goToLogin() {
    this.closeMobileMenu();
    this.router.navigate([], {
      queryParams: {
        login: '1',
        returnUrl: this.router.url,
      },
      queryParamsHandling: 'merge',
    });
  }

  logout() {
    this.authService.logout();
    this.closeMobileMenu();
    this.router.navigate(['/']);
  }

  goToProfile(event: Event) {
    event?.stopPropagation();
    this.router.navigate(['/intranet/perfil-usuario']);
  }

  ngOnInit() {
    if (this.isBrowser) {
      // Restore theme preference
      const savedTheme = localStorage.getItem('tr-theme') || 'dark';
      this.isDarkMode.set(savedTheme === 'dark');
      document.documentElement.setAttribute('data-theme', savedTheme);
      this.onScroll();
    }
  }

  ngOnDestroy() {
    // Cleanup if needed for future subscriptions
  }
}


