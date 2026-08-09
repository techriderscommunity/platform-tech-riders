import { AppRole } from '@core/auth/auth.service';

export interface IntranetNavItem {
  label: string;
  route: string | null;
  roles: AppRole[];
  exact?: boolean;
}

export interface IntranetNavSection {
  title: string;
  icon: string;
  items: IntranetNavItem[];
}

export const INTRANET_NAV_SECTIONS: IntranetNavSection[] = [
  {
    title: 'Inicio',
    icon: '🏠',
    items: [
      { label: 'Panel principal', route: '/intranet', roles: ['admin', 'superadmin', 'staff', 'coordinador', 'empresa', 'junior', 'embajador', 'colaborador', 'centro'], exact: true },
    ],
  },
  {
    title: 'Mi área',
    icon: '🧭',
    items: [
      { label: 'Mi perfil', route: '/intranet/member/profile', roles: ['admin', 'superadmin', 'staff', 'coordinador', 'empresa', 'junior', 'embajador', 'colaborador', 'centro', 'member'] },
      { label: 'Portal embajadores', route: '/intranet/ambassador/portal', roles: ['superadmin', 'staff', 'coordinador', 'embajador', 'colaborador'] },
      { label: 'Panel empresa', route: '/intranet/company', roles: ['empresa'] },
      { label: 'Panel junior', route: '/intranet/junior', roles: ['junior'] },
    ],
  },
  {
    title: 'FP Tour',
    icon: '🎓',
    items: [
      { label: 'Centros', route: '/intranet/fp-tour/centers', roles: ['superadmin', 'staff', 'coordinador', 'centro'] },
      { label: 'Mis sesiones', route: '/intranet/fp-tour/my-sessions', roles: ['superadmin', 'staff', 'coordinador', 'junior', 'embajador', 'colaborador', 'centro'] },
      { label: 'Gestión FP Tour', route: '/intranet/fp-tour/management', roles: ['superadmin', 'staff', 'coordinador'] },
      { label: 'Admin FP Tour', route: '/intranet/admin/fp-tour', roles: ['admin', 'superadmin'] },
    ],
  },
  {
    title: 'Eventos',
    icon: '🎤',
    items: [
      { label: 'Mis eventos', route: '/intranet/events/mine', roles: ['superadmin', 'staff', 'coordinador', 'embajador', 'colaborador'] },
      { label: 'Gestión eventos', route: '/intranet/events/management', roles: ['superadmin', 'staff', 'coordinador'] },
    ],
  },
  {
    title: 'Sesiones',
    icon: '📚',
    items: [
      { label: 'Mis sesiones', route: '/intranet/sessions/mine', roles: ['superadmin', 'staff', 'coordinador', 'junior', 'embajador', 'colaborador', 'centro'] },
      { label: 'Gestión sesiones', route: '/intranet/sessions/management', roles: ['superadmin', 'staff', 'coordinador'] },
      { label: 'Admin sesiones', route: '/intranet/admin/sessions', roles: ['admin', 'superadmin'] },
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
    title: 'Empleo',
    icon: '💼',
    items: [
      { label: 'Gestionar ofertas', route: '/intranet/company/manage-offers', roles: ['empresa'] },
      { label: 'Ver candidatos', route: '/intranet/company/view-candidates', roles: ['empresa'] },
      { label: 'Editar perfil junior', route: '/intranet/junior/edit-profile', roles: ['junior'] },
      { label: 'Mis ofertas', route: '/intranet/junior/my-offers', roles: ['junior'] },
      { label: 'Mis cursos', route: '/intranet/junior/my-courses', roles: ['junior'] },
      { label: 'Mi perfil candidato', route: '/intranet/junior/profile', roles: ['junior'] },
    ],
  },
  {
    title: 'Administración',
    icon: '⚙️',
    items: [
      { label: 'Dashboard admin', route: '/intranet/admin', roles: ['admin', 'superadmin'] },
      { label: 'Comuñeras', route: '/intranet/admin/community-partners', roles: ['admin', 'superadmin'] },
      { label: 'Colaboradores', route: '/intranet/admin/collaborators', roles: ['admin', 'superadmin'] },
      { label: 'Embajadores', route: '/intranet/admin/ambassadors', roles: ['admin', 'superadmin'] },
      { label: 'Usuarios y roles', route: '/intranet/administration/user-roles', roles: ['superadmin'] },
      { label: 'Centros (gobierno)', route: '/intranet/administration/centers', roles: ['superadmin'] },
      { label: 'Configuración', route: '/intranet/administration/configuration', roles: ['superadmin'] },
      { label: 'Auditoría', route: '/intranet/administration/audit', roles: ['superadmin'] },
    ],
  },
  {
    title: 'Gobierno staff',
    icon: '🛡️',
    items: [
      { label: 'Staff principal', route: '/intranet/staff', roles: ['superadmin', 'staff', 'coordinador'] },
      { label: 'Staff FP Tour', route: '/intranet/staff/fp-tour', roles: ['superadmin'] },
      { label: 'Staff sesiones', route: '/intranet/staff/sessions', roles: ['superadmin'] },
      { label: 'Staff embajadores', route: '/intranet/staff/ambassadors', roles: ['superadmin'] },
      { label: 'Staff colaboradores', route: '/intranet/staff/collaborators', roles: ['superadmin'] },
      { label: 'Staff ofertas', route: '/intranet/staff/offers', roles: ['superadmin'] },
      { label: 'Staff candidatos', route: '/intranet/staff/candidates', roles: ['superadmin'] },
    ],
  },
];
