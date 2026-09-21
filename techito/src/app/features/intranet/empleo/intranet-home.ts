import { ChangeDetectionStrategy, Component, computed, inject, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { RouterLink } from '@angular/router';
import { DatePipe } from '@angular/common';
import { AuthService } from '@core/auth/auth.service';
import { catchError, of } from 'rxjs';
import {
  DashboardModuleCard,
  QuickAccess,
  RoleHeroContent,
} from './models/intranet-home.models';
import { resolveIntranetWorkspace } from './intranet-nav.config';
import { IntranetHomeService } from './services/intranet-home.service';
import { AdminDashboardService } from '../admin-dashboard/services/admin-dashboard.service';

@Component({
  selector: 'app-intranet-home',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [RouterLink, DatePipe],
  templateUrl: './intranet-home.html',
  styleUrl: './intranet-home.scss'
})
export class IntranetHome {
  private readonly authService = inject(AuthService);
  private readonly intranetHomeService = inject(IntranetHomeService);
  private readonly adminDashboardService = inject(AdminDashboardService);

  readonly pendingApprovals = signal<{ type: string; count: number }[]>([]);
  readonly upcomingEvents = signal<{ id: string; title: string; startDateTime: string }[]>([]);
  readonly upcomingSessions = signal<{ id: string; title: string; startDateTime: string }[]>([]);
  readonly totalPendingApprovals = computed(() => this.pendingApprovals().reduce((sum, item) => sum + item.count, 0));

  readonly userName = computed(() => this.authService.user()?.name || 'Usuario');
  readonly workspace = computed(() => {
    const user = this.authService.user();
    const roles = user?.roles?.length ? user.roles : user ? [user.role] : [];
    return resolveIntranetWorkspace(roles);
  });
  readonly activeRoleLabel = computed(() => this.workspace().label);

  readonly dashboardModules = computed<DashboardModuleCard[]>(() =>
    this.quickAccess().map(item => ({
      title: item.label,
      description: item.description,
      route: item.route,
    })),
  );
  readonly primaryAction = computed(() => this.quickAccess()[0]);

  readonly roleHero = computed<RoleHeroContent>(() => {
    switch (this.workspace().role) {
      case 'admin':
        return {
        title: 'Centro de control de administración',
        subtitle: 'Supervisa gobernanza, estados operativos y flujos críticos desde una única intranet.',
        contextLabel: 'Vista de administración',
        };
      case 'staff':
        return {
        title: 'Centro de coordinación interna',
        subtitle: 'Coordina FP Tour, sesiones y seguimiento operativo del equipo.',
        contextLabel: 'Vista de coordinación',
        };
      case 'community-leader':
        return {
          title: 'Espacio de coordinación comunitaria',
          subtitle: 'Gestiona actividad comunitaria, eventos y seguimiento de oportunidades.',
          contextLabel: 'Vista de comunidad',
        };
      case 'ambassador':
        return {
          title: 'Espacio de embajador',
          subtitle: 'Consulta tu actividad, sesiones y eventos asignados.',
          contextLabel: 'Vista de embajador',
        };
      case 'center':
        return {
          title: 'Espacio de centro educativo',
          subtitle: 'Gestiona la actividad y las sesiones de tu centro.',
          contextLabel: 'Vista de centro',
        };
      case 'community-partner':
        return {
          title: 'Espacio de entidad colaboradora',
          subtitle: 'Consulta la información y actividad disponible para tu entidad.',
          contextLabel: 'Vista de entidad',
        };
      default:
        return {
          title: 'Tu espacio de talento',
          subtitle: 'Gestiona tu perfil profesional y consulta tu actividad.',
          contextLabel: 'Vista de miembro',
        };
    }
  });

  readonly quickAccess = computed<QuickAccess[]>(() => {
    switch (this.workspace().role) {
      case 'admin':
        return [
        { label: 'Usuarios y roles', description: 'Gestiona usuarios, roles y estado de las cuentas.', route: '/intranet/staff' },
        { label: 'Organizaciones', description: 'Revisa relaciones pendientes y gestiona organizaciones.', route: '/intranet/administration/organizations' },
        { label: 'FP Tour', description: 'Administra solicitudes, centros y sesiones.', route: '/intranet/admin/fp-tour' },
        { label: 'Configuración', description: 'Gestiona los parámetros activos de la intranet.', route: '/intranet/administration/configuration' },
        ];
      case 'staff':
        return [
        { label: 'Gobierno de staff', description: 'Control de usuarios, roles y estado de cuentas.', route: '/intranet/staff' },
        { label: 'FP Tour', description: 'Gestiona las solicitudes y sesiones del programa.', route: '/intranet/staff/fp-tour' },
        { label: 'Módulo de candidaturas', description: 'Revisar y actualizar estado de candidaturas.', route: '/intranet/staff/candidates' },
        ];
      case 'community-leader':
        return [
          { label: 'Gestión de comunidad', description: 'Coordina la actividad de la comunidad.', route: '/intranet/staff' },
          { label: 'Eventos', description: 'Gestiona los eventos oficiales de la comunidad.', route: '/intranet/events/management' },
          { label: 'Candidatos', description: 'Consulta el seguimiento de candidaturas.', route: '/intranet/staff/candidates' },
        ];
      case 'ambassador':
        return [
          { label: 'Portal de embajadores', description: 'Gestiona tu información y actividad como embajador.', route: '/intranet/ambassador/portal' },
          { label: 'Mis sesiones', description: 'Consulta las sesiones que tienes asignadas.', route: '/intranet/sessions/mine' },
          { label: 'Mis eventos', description: 'Consulta tus eventos y participación.', route: '/intranet/events/mine' },
        ];
      case 'center':
        return [
          { label: 'Mi centro', description: 'Consulta la organización vinculada a tu centro.', route: '/intranet/fp-tour/organizations' },
          { label: 'Mis sesiones', description: 'Consulta las sesiones de tu centro.', route: '/intranet/fp-tour/my-sessions' },
        ];
      case 'community-partner':
        return [];
      default:
        return [
          { label: 'Panel de talento', description: 'Consulta tu resumen profesional y actividad.', route: '/intranet/junior' },
          { label: 'Perfil profesional', description: 'Actualiza tu información profesional.', route: '/intranet/junior/edit-profile' },
          { label: 'Mis sesiones', description: 'Consulta tus sesiones asignadas o solicitadas.', route: '/intranet/sessions/mine' },
        ];
    }
  });

  readonly resourcesRoute = computed(() => {
    return this.quickAccess()[0]?.route || '/intranet';
  });

  constructor() {
    this.emitLandingTrace();
    if (this.workspace().role === 'admin') {
      this.loadDashboardExtras();
    }
  }

  private loadDashboardExtras() {
    this.adminDashboardService.getDashboard()
      .pipe(
        catchError(() => of(null)),
        takeUntilDestroyed(),
      )
      .subscribe(dashboard => {
        if (!dashboard) return;
        this.pendingApprovals.set((dashboard.PendingApprovals ?? []).map(p => ({ type: p.Type, count: p.Count })));
        this.upcomingEvents.set((dashboard.UpcomingEvents ?? []).map(e => ({ id: e.Id, title: e.Title, startDateTime: e.StartDateTime })));
        this.upcomingSessions.set((dashboard.UpcomingSessions ?? []).map(s => ({ id: s.Id, title: s.Title, startDateTime: s.StartDateTime })));
      });
  }

  private emitLandingTrace() {
    this.intranetHomeService.emitLandingTrace()
      .pipe(
        catchError(() => of(null)),
        takeUntilDestroyed(),
      )
      .subscribe();
  }
}


