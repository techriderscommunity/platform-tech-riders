import type { AppRole } from '@core/auth/auth.service';

export interface IntranetNavItem {
  label: string;
  route: string;
  exact?: boolean;
}

export interface IntranetNavSection {
  title: string;
  icon: string;
  items: readonly IntranetNavItem[];
}

export interface IntranetWorkspace {
  role: AppRole;
  label: string;
  homeRoute: string;
  sections: readonly IntranetNavSection[];
}

const HOME_SECTION: IntranetNavSection = {
  title: 'Inicio',
  icon: 'Inicio',
  items: [{ label: 'Panel principal', route: '/intranet', exact: true }],
};

export const INTRANET_WORKSPACES: Record<AppRole, IntranetWorkspace> = {
  admin: {
    role: 'admin',
    label: 'Administrador',
    homeRoute: '/intranet',
    sections: [
      HOME_SECTION,
      {
        title: 'Gobierno',
        icon: 'Gobierno',
        items: [
          {
            label: 'Usuarios y roles',
            route: '/intranet/administration/user-roles',
          },
          {
            label: 'Organizaciones',
            route: '/intranet/administration/organizations',
          },
          {
            label: 'Configuración',
            route: '/intranet/administration/configuration',
          },
          { label: 'Auditoría', route: '/intranet/administration/audit' },
        ],
      },
      {
        title: 'Operativa',
        icon: 'Operativa',
        items: [
          { label: 'FP Tour', route: '/intranet/admin/fp-tour' },
          { label: 'Eventos', route: '/intranet/events/management' },
          { label: 'Sesiones', route: '/intranet/admin/sessions' },
          { label: 'Comuñeras', route: '/intranet/admin/community-partners' },
          { label: 'Colaboradores', route: '/intranet/admin/collaborators' },
          { label: 'Embajadores', route: '/intranet/admin/ambassadors' },
        ],
      },
    ],
  },
  staff: {
    role: 'staff',
    label: 'Staff',
    homeRoute: '/intranet/staff',
    sections: [
      HOME_SECTION,
      {
        title: 'Coordinación',
        icon: 'Coordinación',
        items: [
          { label: 'Gobierno de staff', route: '/intranet/staff' },
          { label: 'FP Tour', route: '/intranet/staff/fp-tour' },
          { label: 'Sesiones', route: '/intranet/staff/sessions' },
        ],
      },
      {
        title: 'Seguimiento',
        icon: 'Seguimiento',
        items: [
          { label: 'Embajadores', route: '/intranet/staff/ambassadors' },
          { label: 'Ofertas', route: '/intranet/staff/offers' },
          { label: 'Candidatos', route: '/intranet/staff/candidates' },
        ],
      },
    ],
  },
  'community-leader': {
    role: 'community-leader',
    label: 'Responsable de comunidad',
    homeRoute: '/intranet/staff',
    sections: [
      HOME_SECTION,
      {
        title: 'Comunidad',
        icon: 'Comunidad',
        items: [
          { label: 'Gestión de comunidad', route: '/intranet/staff' },
          { label: 'Eventos', route: '/intranet/events/management' },
          { label: 'Ofertas', route: '/intranet/staff/offers' },
          { label: 'Candidatos', route: '/intranet/staff/candidates' },
        ],
      },
    ],
  },
  ambassador: {
    role: 'ambassador',
    label: 'Embajador',
    homeRoute: '/intranet/ambassador/portal',
    sections: [
      HOME_SECTION,
      {
        title: 'Mi actividad',
        icon: 'Actividad',
        items: [
          {
            label: 'Portal de embajadores',
            route: '/intranet/ambassador/portal',
          },
          { label: 'Mis sesiones', route: '/intranet/sessions/mine' },
          { label: 'Mis eventos', route: '/intranet/events/mine' },
        ],
      },
    ],
  },
  center: {
    role: 'center',
    label: 'Centro educativo',
    homeRoute: '/intranet/center',
    sections: [
      HOME_SECTION,
      {
        title: 'Mi centro',
        icon: 'Centro',
        items: [
          { label: 'Resumen', route: '/intranet/center' },
          { label: 'Organización', route: '/intranet/fp-tour/organizations' },
          { label: 'Mis sesiones', route: '/intranet/fp-tour/my-sessions' },
        ],
      },
    ],
  },
  'community-partner': {
    role: 'community-partner',
    label: 'Entidad colaboradora',
    homeRoute: '/intranet/community-partner',
    sections: [
      HOME_SECTION,
      {
        title: 'Mi entidad',
        icon: 'Entidad',
        items: [{ label: 'Resumen', route: '/intranet/community-partner' }],
      },
    ],
  },
  member: {
    role: 'member',
    label: 'Miembro',
    homeRoute: '/intranet/junior',
    sections: [
      HOME_SECTION,
      {
        title: 'Mi perfil',
        icon: 'Perfil',
        items: [
          {
            label: 'Perfil profesional',
            route: '/intranet/junior/edit-profile',
          },
          { label: 'Panel de talento', route: '/intranet/junior' },
        ],
      },
      {
        title: 'Mi actividad',
        icon: 'Actividad',
        items: [
          { label: 'Mis sesiones', route: '/intranet/sessions/mine' },
          { label: 'Calendario', route: '/intranet/calendar' },
        ],
      },
    ],
  },
};

const WORKSPACE_PRIORITY: readonly AppRole[] = [
  'admin',
  'staff',
  'community-leader',
  'ambassador',
  'center',
  'community-partner',
  'member',
];

export function resolveIntranetWorkspace(
  roles: readonly AppRole[],
): IntranetWorkspace {
  const activeRole =
    WORKSPACE_PRIORITY.find((role) => roles.includes(role)) ?? 'member';
  return INTRANET_WORKSPACES[activeRole];
}
