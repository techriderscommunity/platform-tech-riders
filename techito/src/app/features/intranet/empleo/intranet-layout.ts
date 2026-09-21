import { ChangeDetectionStrategy, Component, DestroyRef, computed, inject } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { AuthService } from '@core/auth/auth.service';
import { catchError, interval, of, startWith, switchMap } from 'rxjs';
import { IntranetLayoutTraceService } from './services/intranet-layout-trace.service';
import { IntranetNavItem, IntranetNavSection, resolveIntranetWorkspace } from './intranet-nav.config';

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
  private readonly intranetLayoutTraceService = inject(IntranetLayoutTraceService);
  private readonly destroyRef = inject(DestroyRef);

  readonly currentUserName = computed(() => this.authService.user()?.name || 'Usuario');
  readonly workspace = computed(() => {
    const user = this.authService.user();
    const roles = user?.roles?.length ? user.roles : user ? [user.role] : [];
    return resolveIntranetWorkspace(roles);
  });
  readonly activeRoleLabel = computed(() => this.workspace().label);
  readonly visibleSections = computed<readonly IntranetNavSection[]>(() => this.workspace().sections);

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
        switchMap(() => this.intranetLayoutTraceService.emitHeartbeatTrace(this.router.url).pipe(catchError(() => of(null)))),
        takeUntilDestroyed(this.destroyRef),
      )
      .subscribe();
  }
}


