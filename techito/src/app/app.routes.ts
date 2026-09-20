import { Routes } from '@angular/router';
import { authGuard, permissionGuard, roleGuard } from './core/auth/auth.guard';

export const routes: Routes = [
  // === Público: Comunidad ===
  {
    path: '',
    loadComponent: () => import('./features/home/home').then((m) => m.Home),
  },
  {
    path: 'join',
    loadComponent: () => import('./features/unete/unete').then((m) => m.Unete),
  },
  {
    path: 'about-us',
    loadComponent: () =>
      import('./features/about-us/quienes-somos').then((m) => m.QuienesSomos),
  },
  {
    path: 'community-partners',
    loadComponent: () =>
      import('./features/comuneras/comuneras').then((m) => m.Comuneras),
  },
  {
    path: 'community-partners/apply',
    loadComponent: () =>
      import('./features/comuneras/community-partner-apply').then(
        (m) => m.CommunityPartnerApply,
      ),
  },
  {
    path: 'community-partners/:id',
    loadComponent: () =>
      import('./features/comuneras/community-partner-detail').then(
        (m) => m.CommunityPartnerDetail,
      ),
  },
  {
    path: 'events',
    loadComponent: () =>
      import('./features/events/events').then((m) => m.Events),
  },
  {
    path: 'orienta-tech',
    loadComponent: () =>
      import('./features/orienta-tech/orienta-tech').then((m) => m.OrientaTech),
  },
  { path: 'tutorials', redirectTo: 'knowledge', pathMatch: 'full' },
  { path: 'contact', redirectTo: 'join', pathMatch: 'full' },

  // === Público: Contenido ===
  {
    path: 'knowledge',
    loadComponent: () =>
      import('./features/knowledge/knowledge').then((m) => m.Knowledge),
  },
  {
    path: 'knowledge/:slug',
    loadComponent: () =>
      import('./features/knowledge/knowledge-detail').then(
        (m) => m.KnowledgeDetail,
      ),
  },

  // === Auth ===
  {
    path: 'login',
    loadComponent: () =>
      import('./features/login/login-redirect').then((m) => m.LoginRedirect),
  },

  // === Intranet: Shell interno con menu por permisos ===
  {
    path: 'intranet',
    // TEMPORAL: dejar visible la intranet mientras se completa la incorporación del contenido histórico.
    // Los módulos de administración siguen protegidos por sus guards específicos.
    loadComponent: () =>
      import('./features/intranet/empleo/intranet-layout').then(
        (m) => m.IntranetLayout,
      ),
    children: [
      {
        path: '',
        loadComponent: () =>
          import('./features/intranet/empleo/intranet-home').then(
            (m) => m.IntranetHome,
          ),
      },

      // === Intranet: Admin ===
      {
        path: 'admin',
        canActivate: [permissionGuard('platform.manage')],
        loadComponent: () =>
          import('./features/intranet/admin-dashboard/admin-dashboard').then(
            (m) => m.AdminDashboard,
          ),
      },
      { path: 'admin/staff', redirectTo: 'staff', pathMatch: 'full' },
      {
        path: 'admin/ambassadors',
        canActivate: [permissionGuard('users.manage')],
        loadComponent: () =>
          import('./features/intranet/embajadores/embajador').then(
            (m) => m.EmbajadorComponent,
          ),
      },
      {
        path: 'admin/assignments',
        canActivate: [permissionGuard('assignments.manage')],
        loadComponent: () =>
          import('./features/intranet/asignaciones/admin-asignaciones').then(
            (m) => m.AdminAsignaciones,
          ),
      },
      {
        path: 'admin/collaborators',
        canActivate: [permissionGuard('users.manage')],
        loadComponent: () =>
          import('./features/intranet/colaboradores/admin-colaboradores').then(
            (m) => m.AdminColaboradores,
          ),
      },
      {
        path: 'admin/community-partners',
        canActivate: [permissionGuard('community.manage')],
        loadComponent: () =>
          import('./features/intranet/comuneras/admin-comuneras').then(
            (m) => m.AdminComuneras,
          ),
      },
      {
        path: 'admin/fp-tour',
        canActivate: [permissionGuard('sessions.fptour.manage')],
        loadComponent: () =>
          import('./features/intranet/fp-tour/admin-sesiones').then(
            (m) => m.AdminSesiones,
          ),
      },
      { path: 'admin/events', redirectTo: 'admin/fp-tour', pathMatch: 'full' },
      { path: 'admin/events', redirectTo: 'admin/events', pathMatch: 'full' },
      {
        path: 'admin/sessions',
        canActivate: [permissionGuard('sessions.fptour.manage')],
        loadComponent: () =>
          import('./features/intranet/fp-tour/admin-gestion-sesiones').then(
            (m) => m.AdminGestionSesiones,
          ),
      },

      // === Intranet: FP Tour (navegacion modular interna) ===
      {
        path: 'fp-tour/organizations',
        canActivate: [permissionGuard('center.info.manage')],
        loadComponent: () =>
          import('./features/intranet/fp-tour/admin-sesiones').then(
            (m) => m.AdminSesiones,
          ),
      },
      {
        path: 'fp-tour/my-sessions',
        canActivate: [
          permissionGuard(['sessions.assigned.view', 'sessions.request']),
        ],
        loadComponent: () =>
          import('./features/intranet/fp-tour/sesiones').then(
            (m) => m.Sesiones,
          ),
      },
      {
        path: 'fp-tour/management',
        canActivate: [permissionGuard('sessions.fptour.manage')],
        loadComponent: () =>
          import('./features/intranet/fp-tour/admin-gestion-sesiones').then(
            (m) => m.AdminGestionSesiones,
          ),
      },
      {
        path: 'fp-tour/gestion',
        redirectTo: 'fp-tour/management',
        pathMatch: 'full',
      },

      // === Intranet: Eventos (navegacion modular interna) ===
      {
        path: 'events/mine',
        canActivate: [
          permissionGuard(['events.participate', 'events.register']),
        ],
        loadComponent: () =>
          import('./features/intranet/fp-tour/calendar-eventos').then(
            (m) => m.CalendarEventos,
          ),
      },
      {
        path: 'events/management',
        canActivate: [permissionGuard('events.create')],
        loadComponent: () =>
          import('./features/intranet/fp-tour/admin-sesiones').then(
            (m) => m.AdminSesiones,
          ),
      },

      // === Intranet: Member / Ambassador ===
      {
        path: 'member/profile',
        canActivate: [permissionGuard('profile.manage')],
        loadComponent: () =>
          import('./features/intranet/perfil-usuario/perfil-usuario').then(
            (m) => m.PerfilUsuario,
          ),
      },
      {
        path: 'ambassador/portal',
        canActivate: [permissionGuard('ambassador.profile.manage')],
        loadComponent: () =>
          import('./features/intranet/embajadores/embajador').then(
            (m) => m.EmbajadorComponent,
          ),
      },
      {
        path: 'center',
        canActivate: [roleGuard('center')],
        loadComponent: () =>
          import('./features/intranet/center/center-dashboard').then(
            (m) => m.CenterDashboard,
          ),
      },
      {
        path: 'community-partner',
        canActivate: [roleGuard('community-partner')],
        loadComponent: () =>
          import('./features/intranet/community-partner/community-partner-dashboard').then(
            (m) => m.CommunityPartnerDashboard,
          ),
      },

      // === Intranet: Sesiones (navegacion modular interna) ===
      {
        path: 'sessions/mine',
        canActivate: [
          permissionGuard(['sessions.assigned.view', 'sessions.request']),
        ],
        loadComponent: () =>
          import('./features/intranet/fp-tour/sesiones').then(
            (m) => m.Sesiones,
          ),
      },
      {
        path: 'sessions/management',
        canActivate: [permissionGuard('sessions.fptour.manage')],
        loadComponent: () =>
          import('./features/intranet/fp-tour/admin-gestion-sesiones').then(
            (m) => m.AdminGestionSesiones,
          ),
      },

      // === Intranet: Calendario ===
      {
        path: 'calendar',
        canActivate: [permissionGuard('events.register')],
        loadComponent: () =>
          import('./features/intranet/fp-tour/calendar-eventos').then(
            (m) => m.CalendarEventos,
          ),
      },

      // === Intranet: Administracion (admin) ===
      {
        path: 'administration/user-roles',
        canActivate: [permissionGuard('users.manage')],
        loadComponent: () =>
          import('./features/intranet/staff/admin-staff').then(
            (m) => m.AdminStaff,
          ),
      },
      {
        path: 'administration/users',
        canActivate: [permissionGuard('users.manage')],
        loadComponent: () =>
          import('./features/intranet/usuarios/admin-usuarios').then(
            (m) => m.AdminUsuarios,
          ),
      },
      {
        path: 'administration/centers',
        canActivate: [permissionGuard('center.info.manage')],
        loadComponent: () =>
          import('./features/intranet/centros/admin-centros').then(
            (m) => m.AdminCentros,
          ),
      },
      {
        path: 'administration/approvals',
        canActivate: [permissionGuard('approvals.manage')],
        loadComponent: () =>
          import('./features/intranet/aprobaciones/admin-aprobaciones').then(
            (m) => m.AdminAprobaciones,
          ),
      },
      {
        path: 'administration/organizations',
        canActivate: [permissionGuard('center.info.manage')],
        loadComponent: () =>
          import('./features/intranet/fp-tour/admin-sesiones').then(
            (m) => m.AdminSesiones,
          ),
      },
      {
        path: 'administration/ambassadors',
        canActivate: [permissionGuard('users.manage')],
        loadComponent: () =>
          import('./features/intranet/embajadores/embajador').then(
            (m) => m.EmbajadorComponent,
          ),
      },
      {
        path: 'administration/configuration',
        canActivate: [permissionGuard('functional-config.manage')],
        loadComponent: () =>
          import('./features/intranet/administracion/admin-configuracion').then(
            (m) => m.AdminConfiguracion,
          ),
      },
      {
        path: 'administration/audit',
        canActivate: [permissionGuard('audit.manage')],
        loadComponent: () =>
          import('./features/intranet/administracion/admin-auditoria').then(
            (m) => m.AdminAuditoria,
          ),
      },

      // === Intranet: Staff Governance ===
      {
        path: 'staff',
        canActivate: [permissionGuard('community.manage')],
        loadComponent: () =>
          import('./features/intranet/staff/admin-staff').then(
            (m) => m.AdminStaff,
          ),
      },
      {
        path: 'staff/fp-tour',
        canActivate: [permissionGuard('sessions.fptour.manage')],
        loadComponent: () =>
          import('./features/intranet/fp-tour/admin-sesiones').then(
            (m) => m.AdminSesiones,
          ),
      },
      {
        path: 'staff/sessions',
        canActivate: [permissionGuard('sessions.fptour.manage')],
        loadComponent: () =>
          import('./features/intranet/fp-tour/admin-gestion-sesiones').then(
            (m) => m.AdminGestionSesiones,
          ),
      },
      {
        path: 'staff/ambassadors',
        canActivate: [permissionGuard('validations.manage')],
        loadComponent: () =>
          import('./features/intranet/embajadores/embajador').then(
            (m) => m.EmbajadorComponent,
          ),
      },
      {
        path: 'staff/collaborators',
        canActivate: [permissionGuard('users.manage')],
        loadComponent: () =>
          import('./features/intranet/colaboradores/admin-colaboradores').then(
            (m) => m.AdminColaboradores,
          ),
      },
      {
        path: 'staff/offers',
        canActivate: [permissionGuard('community.manage')],
        loadComponent: () =>
          import('./features/intranet/empleo/empresa/gestionar-ofertas/gestionar-ofertas').then(
            (m) => m.GestionarOfertas,
          ),
      },
      {
        path: 'staff/candidates',
        canActivate: [permissionGuard('community.manage')],
        loadComponent: () =>
          import('./features/intranet/empleo/empresa/ver-candidatos/ver-candidatos').then(
            (m) => m.VerCandidatos,
          ),
      },

      // === Intranet: Member (talento) ===
      {
        path: 'junior',
        canActivate: [roleGuard('member')],
        loadComponent: () =>
          import('./features/intranet/empleo/junior/dashboard-junior/dashboard-junior').then(
            (m) => m.DashboardJunior,
          ),
      },
      {
        path: 'junior/edit-profile',
        canActivate: [roleGuard('member')],
        loadComponent: () =>
          import('./features/intranet/empleo/junior/editar-perfil/editar-perfil').then(
            (m) => m.EditarPerfil,
          ),
      },
      {
        path: 'junior/my-offers',
        canActivate: [roleGuard('member')],
        loadComponent: () =>
          import('./features/intranet/empleo/junior/mis-ofertas/mis-ofertas').then(
            (m) => m.MisOfertas,
          ),
      },
      {
        path: 'junior/my-courses',
        canActivate: [roleGuard('member')],
        loadComponent: () =>
          import('./features/intranet/empleo/junior/mis-cursos/mis-cursos').then(
            (m) => m.MisCursos,
          ),
      },
      {
        path: 'junior/profile',
        canActivate: [roleGuard('member')],
        loadComponent: () =>
          import('./features/intranet/empleo/junior/perfil-candidato/perfil-candidato').then(
            (m) => m.PerfilCandidato,
          ),
      },
    ],
  },

  // === Fallback ===
  { path: '**', redirectTo: '' },
];
