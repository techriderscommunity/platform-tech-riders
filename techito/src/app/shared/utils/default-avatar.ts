import { AppRole } from '@core/auth/auth.service';

const DEFAULT_AVATARS: Partial<Record<AppRole, string>> = {
  admin: '/assets/avatar-staff.png',
  staff: '/assets/avatar-staff.png',
  ambassador: '/assets/avatar-ambassadors.png',
  center: '/assets/avatar-center.png',
  'community-leader': '/assets/avatar-center.png',
  'community-partner': '/assets/avatar-comuneras.png',
};

export function getDefaultAvatar(roles: readonly string[] | null | undefined): string {
  for (const role of roles ?? []) {
    const normalizedRole = role.trim().toLowerCase() as AppRole;
    const avatar = DEFAULT_AVATARS[normalizedRole];
    if (avatar) return avatar;
  }

  return '/assets/avatar.png';
}
