import { HttpClient } from '@angular/common/http';
import { ChangeDetectionStrategy, Component, DestroyRef, computed, inject } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { AuthService } from '@core/auth/auth.service';
import { environment } from '@env/environment';
import { catchError, interval, of, startWith, switchMap } from 'rxjs';
import { INTRANET_NAV_SECTIONS, IntranetNavItem, IntranetNavSection } from './intranet-nav.config';

@Component({
  selector: 'app-intranet-layout',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [RouterOutlet, RouterLink, RouterLinkActive],
  templateUrl: './intranet-layout.html',
  styleUrl: './intranet-layout.scss'
})
export class IntranetLayout {
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);
  private readonly http = inject(HttpClient);
  private readonly destroyRef = inject(DestroyRef);
  private readonly baseUrl = environment.apiUrl;

  private readonly navSections: IntranetNavSection[] = INTRANET_NAV_SECTIONS;

  readonly userType = computed(() => this.authService.userType() || 'junior');
  readonly currentUserName = computed(() => this.authService.user()?.name || 'Usuario');
  readonly visibleSections = computed(() =>
    this.navSections
      .map(section => ({
        ...section,
        items: section.items.filter(item => this.authService.hasRole(item.roles)),
      }))
      .filter(section => section.items.length > 0),
  );

  constructor() {
    this.startHeartbeatTrace();
  }

  trackSection(_index: number, section: IntranetNavSection): string {
    return section.title;
  }

  trackItem(_index: number, item: IntranetNavItem): string {
    return `${item.label}-${item.route ?? 'disabled'}`;
  }

  goToDefaultArea() {
    this.router.navigateByUrl(this.authService.getDefaultRoute());
  }

  private startHeartbeatTrace() {
    interval(5 * 60 * 1000)
      .pipe(
        startWith(0),
        switchMap(() => this.http.post(`${this.baseUrl}/intranet/trazas`, {
          kind: 'heartbeat',
          route: this.router.url,
          detail: 'intranet_layout_alive',
        }).pipe(catchError(() => of(null)))),
        takeUntilDestroyed(this.destroyRef),
      )
      .subscribe();
  }
}


