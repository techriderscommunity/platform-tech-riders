import { HttpClient } from '@angular/common/http';
import { ChangeDetectionStrategy, Component, computed, inject, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { RouterLink } from '@angular/router';
import { AuthService } from '@core/auth/auth.service';
import { environment } from '@env/environment';
import { catchError, of } from 'rxjs';

interface DashboardNotification {
  title: string;
  detail: string;
}

interface QuickAccess {
  label: string;
  description: string;
  route: string;
}

interface MySpaceItem {
  label: string;
  route: string;
}

interface ActivitySummaryItem {
  module: string;
  pending: string;
}

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
  private readonly http = inject(HttpClient);
  private readonly baseUrl = environment.apiUrl;
  readonly selectedCategories = signal<string[]>(['FP Tour', 'Eventos']);
  readonly availableCategories = ['FP Tour', 'Eventos', 'Mentorias', 'Podcast', 'Comunidad'];
  readonly categoriesSaving = signal(false);

  readonly userName = computed(() => this.authService.user()?.name || 'Usuario');
  readonly userRoles = computed(() => this.authService.user()?.roles ?? []);
  readonly canManageCategories = computed(() => this.authService.hasRole(['embajador', 'colaborador']));

  readonly profileRoute = computed(() => {
    if (this.authService.hasRole('junior')) return '/intranet/junior/edit-profile';
    if (this.authService.hasRole('empresa')) return '/intranet/company';
    if (this.authService.hasRole(['embajador', 'colaborador', 'centro'])) return '/intranet/fp-tour/my-sessions';
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
        { label: 'Staff Governance', description: 'Control de usuarios, roles y estado de cuentas.', route: '/intranet/staff' },
        { label: 'Collaborators', description: 'Gestionar alta, activacion y desactivacion.', route: '/intranet/staff/collaborators' },
        { label: 'Candidates', description: 'Revisar y actualizar estado de candidaturas.', route: '/intranet/staff/candidates' },
      ];
    }

    if (this.authService.hasRole('empresa')) {
      return [
        { label: 'Company Dashboard', description: 'Resumen de actividad y seguimiento.', route: '/intranet/company' },
        { label: 'Manage Offers', description: 'Crear y editar ofertas de empleo.', route: '/intranet/company/manage-offers' },
        { label: 'View Candidates', description: 'Revisar postulaciones recibidas.', route: '/intranet/company/view-candidates' },
      ];
    }

    return [
      { label: 'Mi Dashboard', description: 'Resumen de oportunidades y actividad.', route: '/intranet/junior' },
      { label: 'Edit Profile', description: 'Actualiza tu informacion profesional.', route: '/intranet/junior/edit-profile' },
      { label: 'My Offers', description: 'Consulta estado de tus postulaciones.', route: '/intranet/junior/my-offers' },
    ];
  });

  readonly mySpace = computed<MySpaceItem[]>(() => [
    { label: 'Mi perfil (ver/editar)', route: this.profileRoute() },
    { label: 'Dashboard personal', route: '/intranet' },
    { label: 'Sesiones impartidas', route: this.authService.hasRole('junior') ? '/intranet/junior/my-courses' : '/intranet/sessions/mine' },
    { label: 'Proximas sesiones', route: this.authService.hasRole('junior') ? '/intranet/junior/my-courses' : '/intranet/fp-tour/my-sessions' },
    { label: 'Actividad reciente', route: this.authService.hasRole(['superadmin', 'admin']) ? '/intranet/admin' : '/intranet' },
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
    this.http.get<string[]>(`${this.baseUrl}/intranet/mis-categorias`)
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
    this.http.put(`${this.baseUrl}/intranet/mis-categorias`, { categories })
      .pipe(
        catchError(() => of(null)),
        takeUntilDestroyed(),
      )
      .subscribe(() => {
        this.categoriesSaving.set(false);
      });
  }

  private emitLandingTrace() {
    this.http.post(`${this.baseUrl}/intranet/trazas`, {
      kind: 'landing',
      route: '/intranet',
      detail: 'home_loaded',
    })
      .pipe(
        catchError(() => of(null)),
        takeUntilDestroyed(),
      )
      .subscribe();
  }
}


