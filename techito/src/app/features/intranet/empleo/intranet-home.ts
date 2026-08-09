import { ChangeDetectionStrategy, Component, computed, inject, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { toSignal } from '@angular/core/rxjs-interop';
import { NavigationEnd, Router, RouterLink } from '@angular/router';
import { AuthService } from '@core/auth/auth.service';
import { PublicContentService } from '@core/content/public-content.service';
import { catchError, of } from 'rxjs';
import { filter, startWith } from 'rxjs/operators';
import {
  ActivitySummaryItem,
  DashboardModuleCard,
  DashboardNotification,
  MySpaceItem,
  QuickAccess,
  RecentActivityItem,
  RoleHeroContent,
} from './models/intranet-home.models';
import { INTRANET_NAV_SECTIONS } from './intranet-nav.config';
import { IntranetHomeService } from './services/intranet-home.service';

const ROLE_LABELS: Record<string, string> = {
  superadmin: 'Superadmin',
  admin: 'Admin',
  staff: 'Staff',
  coordinador: 'Coordinador',
  empresa: 'Empresa',
  junior: 'Junior',
  embajador: 'Embajador',
  colaborador: 'Colaborador',
  centro: 'Centro',
};

@Component({
  selector: 'app-intranet-home',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [RouterLink],
  templateUrl: './intranet-home.html',
  styleUrl: './intranet-home.scss'
})
export class IntranetHome {
  private readonly authService = inject(AuthService);
  private readonly publicContentService = inject(PublicContentService);
  private readonly intranetHomeService = inject(IntranetHomeService);
  private readonly router = inject(Router);
  private readonly navigationDone = toSignal(
    this.router.events.pipe(
      filter((event) => event instanceof NavigationEnd),
      startWith(null),
    ),
  );
  readonly selectedCategories = signal<string[]>([]);
  availableCategories: string[] = [];
  readonly categoriesSaving = signal(false);

  readonly userName = computed(() => this.authService.user()?.name || 'Usuario');
  readonly userRoles = computed(() => this.authService.user()?.roles ?? []);
  readonly userEmail = computed(() => this.authService.user()?.email || 'usuario@techriders.local');
  readonly userInitials = computed(() => {
    const name = this.userName().trim();
    if (!name) return 'TR';
    const parts = name.split(' ').filter(Boolean);
    const first = parts[0]?.[0] || 'T';
    const second = parts[1]?.[0] || 'R';
    return `${first}${second}`.toUpperCase();
  });
  readonly canManageCategories = computed(() => this.authService.hasRole(['embajador', 'colaborador']));
  readonly activeRoleLabel = computed(() => ROLE_LABELS[this.userRoles()[0]] || 'Invitado');
  readonly visibleSubmenuSections = computed(() =>
    INTRANET_NAV_SECTIONS
      .map(section => ({
        title: section.title,
        icon: section.icon,
        items: section.items.filter(item => item.route && this.authService.hasRole(item.roles)),
      }))
      .filter(section => section.items.length > 0),
  );
  readonly totalVisibleModules = computed(() =>
    this.visibleSubmenuSections().reduce((total, section) => total + section.items.length, 0),
  );

  readonly recentActivity = computed<RecentActivityItem[]>(() => {
    const fromNotifications = this.notifications().map((item, index) => ({
      label: item.title,
      detail: item.detail,
      time: index === 0 ? 'Ahora' : 'Hace unos minutos',
    }));

    const fromModules = this.activitySummary().slice(0, 2).map(item => ({
      label: `Seguimiento de ${item.module}`,
      detail: item.pending,
      time: 'Hoy',
    }));

    return [...fromNotifications, ...fromModules].slice(0, 4);
  });

  readonly interestTags: string[] = ['Programación', 'IA', 'Cloud', 'Ciberseguridad', 'DevOps', 'Educación', 'Innovación'];

  readonly currentRoute = computed(() => {
    this.navigationDone();
    return this.router.url;
  });

  readonly currentSection = computed(() => {
    const route = this.currentRoute();
    return this.visibleSubmenuSections().find(section =>
      section.items.some(item => route === item.route || route.startsWith(`${item.route}/`)),
    );
  });

  readonly dashboardModules = computed<DashboardModuleCard[]>(() =>
    this.quickAccess().map(item => ({
      title: item.label,
      description: item.description,
      route: item.route,
    })),
  );

  readonly roleHero = computed<RoleHeroContent>(() => {
    if (this.authService.hasRole(['superadmin', 'admin'])) {
      return {
        title: 'Centro de control de administración',
        subtitle: 'Supervisa gobernanza, estados operativos y flujos críticos desde una única intranet.',
        contextLabel: 'Vista de administración',
      };
    }

    if (this.authService.hasRole(['staff', 'coordinador'])) {
      return {
        title: 'Centro de coordinación interna',
        subtitle: 'Gestiona tareas operativas, sesiones y seguimiento del equipo desde tu vista de intranet.',
        contextLabel: 'Vista de coordinación',
      };
    }

    if (this.authService.hasRole('empresa')) {
      return {
        title: 'Panel operativo de empresa',
        subtitle: 'Controla ofertas, candidaturas y actividad de contratación desde esta intranet única.',
        contextLabel: 'Vista de empresa',
      };
    }

    return {
      title: 'Tu panel personal de intranet',
      subtitle: 'Consulta oportunidades, cursos y actividad desde una única experiencia adaptada a tu rol.',
      contextLabel: 'Vista de talento',
    };
  });

  readonly profileRoute = computed(() => {
    if (this.authService.hasRole('junior')) return '/intranet/junior/edit-profile';
    if (this.authService.hasRole('empresa')) return '/intranet/company';
    if (this.authService.hasRole(['embajador', 'colaborador', 'centro'])) return '/intranet/member/profile';
    return '/intranet/administration/user-roles';
  });

  readonly notifications = computed<DashboardNotification[]>(() => {
    if (this.authService.hasRole(['superadmin', 'admin'])) {
      return [
        { title: 'Moderacion pendiente', detail: 'Tienes solicitudes de alta y cambios de estado por revisar.' },
        { title: 'Gobernanza', detail: 'Valida permisos de usuarios y cobertura de roles internos.' },
      ];
    }

    if (this.authService.hasRole(['staff', 'coordinador'])) {
      return [
        { title: 'Seguimiento interno', detail: 'Hay candidaturas y validaciones internas pendientes.' },
        { title: 'Coordinacion', detail: 'Revisa agenda de sesiones y tareas asignadas al equipo.' },
      ];
    }

    if (this.authService.hasRole('empresa')) {
      return [
        { title: 'Candidaturas pendientes', detail: 'Hay nuevos perfiles esperando valoracion.' },
        { title: 'Ofertas activas', detail: 'Comprueba estado de tus ofertas publicadas.' },
      ];
    }

    return [
      { title: 'Perfil pendiente', detail: 'Completa tu perfil para mejorar tu visibilidad.' },
      { title: 'Nuevas oportunidades', detail: 'Hay ofertas recomendadas para tu perfil.' },
    ];
  });

  readonly quickAccess = computed<QuickAccess[]>(() => {
    if (this.authService.hasRole(['superadmin', 'staff', 'coordinador'])) {
      return [
        { label: 'Gobierno de staff', description: 'Control de usuarios, roles y estado de cuentas.', route: '/intranet/staff' },
        { label: 'Módulo de colaboradores', description: 'Gestionar altas, activaciones y desactivaciones.', route: '/intranet/staff/collaborators' },
        { label: 'Módulo de candidaturas', description: 'Revisar y actualizar estado de candidaturas.', route: '/intranet/staff/candidates' },
      ];
    }

    if (this.authService.hasRole('admin')) {
      return [
        { label: 'Panel de administración', description: 'Vista central con estado de módulos internos.', route: '/intranet/admin' },
        { label: 'Módulo de colaboradores', description: 'Gestionar altas, activaciones y desactivaciones.', route: '/intranet/admin/collaborators' },
        { label: 'Módulo de centros y sesiones', description: 'Revisar solicitudes de charlas y sesiones FP Tour.', route: '/intranet/admin/fp-tour' },
      ];
    }

    if (this.authService.hasRole('empresa')) {
      return [
        { label: 'Panel de empresa', description: 'Resumen de actividad y seguimiento.', route: '/intranet/company' },
        { label: 'Módulo de ofertas', description: 'Crear y editar ofertas de empleo.', route: '/intranet/company/manage-offers' },
        { label: 'Módulo de candidatos', description: 'Revisar postulaciones recibidas.', route: '/intranet/company/view-candidates' },
      ];
    }

    return [
      { label: 'Panel de talento', description: 'Resumen de oportunidades y actividad.', route: '/intranet/junior' },
      { label: 'Módulo de perfil', description: 'Actualiza tu información profesional.', route: '/intranet/junior/edit-profile' },
      { label: 'Módulo de ofertas', description: 'Consulta estado de tus postulaciones.', route: '/intranet/junior/my-offers' },
    ];
  });

  readonly talkRequestRoute = computed(() => {
    if (this.authService.hasRole(['superadmin', 'staff', 'coordinador'])) return '/intranet/fp-tour/management';
    if (this.authService.hasRole('admin')) return '/intranet/admin/fp-tour';
    if (this.authService.hasRole('empresa')) return '/intranet/company/manage-offers';
    return '/intranet/junior/my-courses';
  });

  readonly resourcesRoute = computed(() => {
    if (this.authService.hasRole('admin')) return '/intranet/admin';
    return this.quickAccess()[0]?.route || '/intranet';
  });

  readonly requestsRoute = computed(() => {
    if (this.authService.hasRole(['superadmin', 'staff', 'coordinador'])) return '/intranet/staff/candidates';
    if (this.authService.hasRole('admin')) return '/intranet/admin/fp-tour';
    if (this.authService.hasRole('empresa')) return '/intranet/company/view-candidates';
    return '/intranet/junior/my-offers';
  });

  readonly mySpace = computed<MySpaceItem[]>(() => [
    { label: 'Mi perfil', route: this.profileRoute() },
    { label: 'Inicio de intranet', route: '/intranet' },
    { label: 'Módulo de embajadores', route: this.authService.hasRole(['embajador', 'colaborador']) ? '/intranet/ambassador/portal' : this.profileRoute() },
    { label: 'Módulo de sesiones', route: this.authService.hasRole('junior') ? '/intranet/junior/my-courses' : '/intranet/sessions/mine' },
    { label: 'Agenda de sesiones', route: this.authService.hasRole('junior') ? '/intranet/junior/my-courses' : '/intranet/fp-tour/my-sessions' },
    { label: 'Actividad de módulos', route: this.authService.hasRole(['superadmin', 'admin']) ? '/intranet/admin' : '/intranet' },
  ]);

  readonly activitySummary = computed<ActivitySummaryItem[]>(() => {
    const base: ActivitySummaryItem[] = [];

    if (this.authService.hasRole(['superadmin', 'staff', 'coordinador', 'admin'])) {
      base.push(
        { module: 'FP Tour', pending: '2 solicitudes pendientes' },
        { module: 'Eventos', pending: '3 eventos proximos' },
        { module: 'Embajadores', pending: '1 sesion pendiente de aceptar' },
        { module: 'Comunidad', pending: '4 notificaciones' },
      );
      return base;
    }

    if (this.authService.hasRole('empresa')) {
      base.push(
        { module: 'FP Tour', pending: '1 solicitud pendiente' },
        { module: 'Eventos', pending: '2 eventos proximos' },
        { module: 'Comunidad', pending: '3 notificaciones' },
      );
      return base;
    }

    return [
      { module: 'FP Tour', pending: '1 convocatoria pendiente' },
      { module: 'Eventos', pending: '2 eventos recomendados' },
      { module: 'Comunidad', pending: '2 notificaciones' },
    ];
  });

  constructor() {
    this.publicContentService
      .getPublicContent()
      .pipe(
        catchError(() => of(null)),
        takeUntilDestroyed(),
      )
      .subscribe(content => {
        if (!content) {
          return;
        }

        this.availableCategories = content.intranet.memberCategoryOptions;
        if (this.selectedCategories().length === 0) {
          this.selectedCategories.set(this.availableCategories.slice(0, 2));
        }
      });

    this.emitLandingTrace();

    if (this.canManageCategories()) {
      this.loadMyCategories();
    }
  }

  toggleCategory(category: string) {
    if (this.categoriesSaving()) return;

    const current = this.selectedCategories();
    let next: string[];
    if (current.includes(category)) {
      next = current.filter(item => item !== category);
    }
    else {
      next = [...current, category];
    }

    this.selectedCategories.set(next);
    this.saveMyCategories(next);
  }

  private loadMyCategories() {
    this.intranetHomeService.getMyCategories(this.resolveUserKey())
      .pipe(
        catchError(() => of([] as string[])),
        takeUntilDestroyed(),
      )
      .subscribe(categories => {
        if (categories.length > 0) {
          this.selectedCategories.set(categories);
        }
      });
  }

  private saveMyCategories(categories: string[]) {
    this.categoriesSaving.set(true);
    this.intranetHomeService.saveMyCategories(this.resolveUserKey(), categories)
      .pipe(
        catchError(() => of(null)),
        takeUntilDestroyed(),
      )
      .subscribe(() => {
        this.categoriesSaving.set(false);
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

  private resolveUserKey(): string {
    return this.authService.user()?.email || 'local-user@techriders.local';
  }
}


