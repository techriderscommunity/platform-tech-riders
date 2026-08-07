import { HttpClient } from '@angular/common/http';
import { ChangeDetectionStrategy, Component, DestroyRef, computed, inject } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { AppRole, AuthService } from '@core/auth/auth.service';
import { environment } from '@env/environment';
import { catchError, interval, of, startWith, switchMap } from 'rxjs';

interface IntranetNavItem {
  label: string;
  route: string | null;
  roles: AppRole[];
  exact?: boolean;
}

interface IntranetNavSection {
  title: string;
  icon: string;
  items: IntranetNavItem[];
}

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

  private readonly navSections: IntranetNavSection[] = [
    {
      title: 'Home',
      icon: '🏠',
      items: [
        { label: 'Mi dashboard', route: '/intranet', roles: ['admin', 'superadmin', 'staff', 'coordinador', 'empresa', 'junior', 'embajador', 'colaborador', 'centro'], exact: true },
      ],
    },
    {
      title: 'Mi espacio',
      icon: '🧭',
      items: [
        { label: 'Mi perfil', route: '/intranet/member/profile', roles: ['admin', 'superadmin', 'staff', 'coordinador', 'empresa', 'junior', 'embajador', 'colaborador', 'centro'] },
        { label: 'Portal Ambassador', route: '/intranet/ambassador/portal', roles: ['superadmin', 'staff', 'coordinador', 'embajador', 'colaborador'] },
      ],
    },
    {
      title: 'FP Tour',
      icon: '🎓',
      items: [
        { label: 'Centros', route: '/intranet/fp-tour/centers', roles: ['superadmin', 'staff', 'coordinador', 'centro'] },
        { label: 'Mis Sesiones', route: '/intranet/fp-tour/my-sessions', roles: ['superadmin', 'staff', 'coordinador', 'junior', 'embajador', 'colaborador', 'centro'] },
        { label: 'Gestion FP Tour', route: '/intranet/fp-tour/management', roles: ['superadmin', 'staff', 'coordinador'] },
      ],
    },
    {
      title: 'Eventos',
      icon: '🎤',
      items: [
        { label: 'Mis Eventos', route: '/intranet/events/mine', roles: ['superadmin', 'staff', 'coordinador', 'embajador', 'colaborador'] },
        { label: 'Gestion Eventos', route: '/intranet/events/management', roles: ['superadmin', 'staff', 'coordinador'] },
      ],
    },
    {
      title: 'Sesiones',
      icon: '📚',
      items: [
        { label: 'Mis Sesiones', route: '/intranet/sessions/mine', roles: ['superadmin', 'staff', 'coordinador', 'junior', 'embajador', 'colaborador', 'centro'] },
        { label: 'Gestion Sesiones', route: '/intranet/sessions/management', roles: ['superadmin', 'staff', 'coordinador'] },
      ],
    },
    {
      title: 'Calendario',
      icon: '📅',
      items: [
        { label: 'Vista unificada', route: '/intranet/calendar', roles: ['superadmin', 'staff', 'coordinador', 'admin', 'empresa', 'junior', 'embajador', 'colaborador', 'centro'] },
      ],
    },
    {
      title: 'Administracion',
      icon: '⚙️',
      items: [
        { label: 'Usuarios y Roles', route: '/intranet/administration/user-roles', roles: ['superadmin'] },
        { label: 'Centros', route: '/intranet/administration/centers', roles: ['superadmin'] },
        { label: 'Embajadores', route: '/intranet/administration/ambassadors', roles: ['superadmin'] },
        { label: 'Configuracion', route: '/intranet/administration/configuration', roles: ['superadmin'] },
        { label: 'Auditoria', route: '/intranet/administration/audit', roles: ['superadmin'] },
      ],
    },
  ];

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


