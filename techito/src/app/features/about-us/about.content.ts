import { SocialLinkItem } from '@shared/ui/public-content.types';

/** Cuentas oficiales de la comunidad (no dependen de BD). */
export const ABOUT_SOCIAL_LINKS: SocialLinkItem[] = [
  { platform: 'linkedin', href: 'https://www.linkedin.com/company/techriders' },
  { platform: 'github', href: 'https://github.com/techriders' },
  { platform: 'x', href: 'https://x.com' },
  { platform: 'instagram', href: 'https://www.instagram.com' },
  { platform: 'youtube', href: 'https://www.youtube.com/@TechRidersMedia' },
];

/** Titulo/descripcion por zona de equipo (estatico); los miembros vienen del backend (Users reales por Capability). */
export const ABOUT_ZONE_META: Readonly<Record<string, { title: string; description: string }>> = {
  staff: { title: 'Staff', description: 'Personas que lideran y coordinan la comunidad.' },
  'community-leaders': { title: 'Community Leaders', description: 'Personas que ayudan a dar forma y operar iniciativas de Tech Riders.' },
  ambassador: { title: 'Ambassador', description: 'Personas que participan activamente en actividades y ayudan a extender comunidad.' },
  member: { title: 'Member', description: 'Personas que se unen y participan en sesiones, actividades y comunidad.' },
};

/** Segunda metrica es puramente editorial (no es un count real). La primera se rellena con el dato dinamico. */
export const ABOUT_METRICS_META: ReadonlyArray<{ icon: string; label: string; staticValue?: string }> = [
  { icon: 'people', label: 'Comunidad' },
  { icon: 'wrench', label: 'Soporte', staticValue: '24/7' },
];
