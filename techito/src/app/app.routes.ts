import { Routes } from '@angular/router';
import { authGuard, roleGuard } from './core/auth/auth.guard';

export const routes: Routes = [
  // === Público: Comunidad ===
  { path: '', loadComponent: () => import('./features/home/home').then(m => m.Home) },
  { path: 'join', loadComponent: () => import('./features/unete/unete').then(m => m.Unete) },
  { path: 'about-us', loadComponent: () => import('./features/quienes-somos/quienes-somos').then(m => m.QuienesSomos) },
  { path: 'orienta-tech', loadComponent: () => import('./features/orienta-tech/orienta-tech').then(m => m.OrientaTech) },
  { path: 'events', loadComponent: () => import('./features/eventos/eventos').then(m => m.Eventos) },

  // === Público: Contenido ===
  { path: 'tutorials', loadComponent: () => import('./features/tutoriales/tutoriales').then(m => m.Tutoriales) },

  // === Público: Contacto ===
  { path: 'contact', loadComponent: () => import('./features/contacto/contacto').then(m => m.Contacto) },

  // === Auth ===
  { path: 'login', loadComponent: () => import('./features/login/login-redirect').then(m => m.LoginRedirect) },

  // === Intranet: Shell interno con menu por permisos ===
  {
    path: 'intranet',
    canActivate: [authGuard],
    loadComponent: () => import('./features/intranet/empleo/intranet-layout').then(m => m.IntranetLayout),
    children: [
      { path: '', loadComponent: () => import('./features/intranet/empleo/intranet-home').then(m => m.IntranetHome) },

      // === Intranet: Admin ===
      { path: 'admin', canActivate: [roleGuard(['admin', 'superadmin'])], loadComponent: () => import('./features/intranet/admin-dashboard/admin-dashboard').then(m => m.AdminDashboard) },
      { path: 'admin/staff', redirectTo: 'staff', pathMatch: 'full' },
      { path: 'admin/ambassadors', canActivate: [roleGuard(['admin', 'superadmin'])], loadComponent: () => import('./features/intranet/embajadores/embajador').then(m => m.EmbajadorComponent) },
      { path: 'admin/collaborators', canActivate: [roleGuard(['admin', 'superadmin'])], loadComponent: () => import('./features/intranet/colaboradores/admin-colaboradores').then(m => m.AdminColaboradores) },
      { path: 'admin/fp-tour', canActivate: [roleGuard(['admin', 'superadmin'])], loadComponent: () => import('./features/intranet/fp-tour/admin-sesiones').then(m => m.AdminSesiones) },
      { path: 'admin/events', redirectTo: 'admin/fp-tour', pathMatch: 'full' },
      { path: 'admin/eventos', redirectTo: 'admin/events', pathMatch: 'full' },
      { path: 'admin/sessions', canActivate: [roleGuard(['admin', 'superadmin'])], loadComponent: () => import('./features/intranet/fp-tour/admin-gestion-sesiones').then(m => m.AdminGestionSesiones) },

      // === Intranet: FP Tour (navegacion modular interna) ===
      { path: 'fp-tour/centers', canActivate: [roleGuard(['superadmin', 'staff', 'coordinador', 'centro'])], loadComponent: () => import('./features/intranet/fp-tour/admin-sesiones').then(m => m.AdminSesiones) },
      { path: 'fp-tour/centros', redirectTo: 'fp-tour/centers', pathMatch: 'full' },
      { path: 'fp-tour/my-sessions', canActivate: [roleGuard(['superadmin', 'staff', 'coordinador', 'junior', 'embajador', 'colaborador', 'centro'])], loadComponent: () => import('./features/intranet/fp-tour/sesiones').then(m => m.Sesiones) },
      { path: 'fp-tour/management', canActivate: [roleGuard(['superadmin', 'staff', 'coordinador'])], loadComponent: () => import('./features/intranet/fp-tour/admin-gestion-sesiones').then(m => m.AdminGestionSesiones) },
      { path: 'fp-tour/gestion', redirectTo: 'fp-tour/management', pathMatch: 'full' },

      // === Intranet: Eventos (navegacion modular interna) ===
      { path: 'events/mine', canActivate: [roleGuard(['superadmin', 'staff', 'coordinador', 'embajador', 'colaborador'])], loadComponent: () => import('./features/intranet/fp-tour/calendar-eventos').then(m => m.CalendarEventos) },
      { path: 'events/management', canActivate: [roleGuard(['superadmin', 'staff', 'coordinador'])], loadComponent: () => import('./features/intranet/fp-tour/admin-sesiones').then(m => m.AdminSesiones) },

      // === Intranet: Member / Ambassador ===
      { path: 'member/profile', canActivate: [roleGuard(['superadmin', 'staff', 'coordinador', 'admin', 'empresa', 'junior', 'embajador', 'colaborador', 'centro', 'member'])], loadComponent: () => import('./features/perfil-usuario/perfil-usuario').then(m => m.PerfilUsuario) },
      { path: 'ambassador/portal', canActivate: [roleGuard(['superadmin', 'staff', 'coordinador', 'embajador', 'colaborador'])], loadComponent: () => import('./features/intranet/embajadores/embajador').then(m => m.EmbajadorComponent) },

      // === Intranet: Sesiones (navegacion modular interna) ===
      { path: 'sessions/mine', canActivate: [roleGuard(['superadmin', 'staff', 'coordinador', 'junior', 'embajador', 'colaborador', 'centro'])], loadComponent: () => import('./features/intranet/fp-tour/sesiones').then(m => m.Sesiones) },
      { path: 'sessions/management', canActivate: [roleGuard(['superadmin', 'staff', 'coordinador'])], loadComponent: () => import('./features/intranet/fp-tour/admin-gestion-sesiones').then(m => m.AdminGestionSesiones) },

      // === Intranet: Calendario ===
      { path: 'calendar', canActivate: [roleGuard(['superadmin', 'staff', 'coordinador', 'admin', 'empresa', 'junior', 'embajador', 'colaborador', 'centro'])], loadComponent: () => import('./features/intranet/fp-tour/calendar-eventos').then(m => m.CalendarEventos) },

      // === Intranet: Administracion (superadmin) ===
      { path: 'administration/user-roles', canActivate: [roleGuard('superadmin')], loadComponent: () => import('./features/intranet/staff/admin-staff').then(m => m.AdminStaff) },
      { path: 'administration/centers', canActivate: [roleGuard('superadmin')], loadComponent: () => import('./features/intranet/fp-tour/admin-sesiones').then(m => m.AdminSesiones) },
      { path: 'administration/ambassadors', canActivate: [roleGuard('superadmin')], loadComponent: () => import('./features/intranet/embajadores/embajador').then(m => m.EmbajadorComponent) },
      { path: 'administration/configuration', canActivate: [roleGuard('superadmin')], loadComponent: () => import('./features/intranet/administracion/admin-configuracion').then(m => m.AdminConfiguracion) },
      { path: 'administration/audit', canActivate: [roleGuard('superadmin')], loadComponent: () => import('./features/intranet/administracion/admin-auditoria').then(m => m.AdminAuditoria) },

      // === Intranet: Staff Governance ===
      { path: 'staff', canActivate: [roleGuard(['superadmin', 'staff', 'coordinador'])], loadComponent: () => import('./features/intranet/staff/admin-staff').then(m => m.AdminStaff) },
      { path: 'staff/fp-tour', canActivate: [roleGuard('superadmin')], loadComponent: () => import('./features/intranet/fp-tour/admin-sesiones').then(m => m.AdminSesiones) },
      { path: 'staff/sessions', canActivate: [roleGuard('superadmin')], loadComponent: () => import('./features/intranet/fp-tour/admin-gestion-sesiones').then(m => m.AdminGestionSesiones) },
      { path: 'staff/ambassadors', canActivate: [roleGuard('superadmin')], loadComponent: () => import('./features/intranet/embajadores/embajador').then(m => m.EmbajadorComponent) },
      { path: 'staff/collaborators', canActivate: [roleGuard('superadmin')], loadComponent: () => import('./features/intranet/colaboradores/admin-colaboradores').then(m => m.AdminColaboradores) },
      { path: 'staff/offers', canActivate: [roleGuard('superadmin')], loadComponent: () => import('./features/intranet/empleo/empresa/gestionar-ofertas/gestionar-ofertas').then(m => m.GestionarOfertas) },
      { path: 'staff/candidates', canActivate: [roleGuard('superadmin')], loadComponent: () => import('./features/intranet/empleo/empresa/ver-candidatos/ver-candidatos').then(m => m.VerCandidatos) },

      // === Intranet: Empresa ===
      { path: 'company', canActivate: [roleGuard('empresa')], loadComponent: () => import('./features/intranet/empleo/empresa/dashboard-empresa/dashboard-empresa').then(m => m.DashboardEmpresa) },
      { path: 'company/manage-offers', canActivate: [roleGuard('empresa')], loadComponent: () => import('./features/intranet/empleo/empresa/gestionar-ofertas/gestionar-ofertas').then(m => m.GestionarOfertas) },
      { path: 'company/view-candidates', canActivate: [roleGuard('empresa')], loadComponent: () => import('./features/intranet/empleo/empresa/ver-candidatos/ver-candidatos').then(m => m.VerCandidatos) },

      // === Intranet: Junior ===
      { path: 'junior', canActivate: [roleGuard('junior')], loadComponent: () => import('./features/intranet/empleo/junior/dashboard-junior/dashboard-junior').then(m => m.DashboardJunior) },
      { path: 'junior/edit-profile', canActivate: [roleGuard('junior')], loadComponent: () => import('./features/intranet/empleo/junior/editar-perfil/editar-perfil').then(m => m.EditarPerfil) },
      { path: 'junior/my-offers', canActivate: [roleGuard('junior')], loadComponent: () => import('./features/intranet/empleo/junior/mis-ofertas/mis-ofertas').then(m => m.MisOfertas) },
      { path: 'junior/my-courses', canActivate: [roleGuard('junior')], loadComponent: () => import('./features/intranet/empleo/junior/mis-cursos/mis-cursos').then(m => m.MisCursos) },
      { path: 'junior/profile', canActivate: [roleGuard('junior')], loadComponent: () => import('./features/intranet/empleo/junior/perfil-candidato/perfil-candidato').then(m => m.PerfilCandidato) },
    ],
  },

  // === Fallback ===
  { path: '**', redirectTo: '' }
];


