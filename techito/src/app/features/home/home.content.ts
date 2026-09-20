import { HomePastEventPhotoItem, HomeProfileCardItem } from '@shared/ui/public-content.types';

/** Metadata estatica (icono/label) emparejada en orden con HomeStats del backend (ambassadors, events, sessions, centers). */
export const HOME_STATS_META: ReadonlyArray<{ icon: string; label: string }> = [
  { icon: 'users', label: 'Ambassadors activos' },
  { icon: 'calendar', label: 'Eventos' },
  { icon: 'book', label: 'Sesiones' },
  { icon: 'map', label: 'Centros' },
];

export const HOME_PROFILE_PANEL_CARDS: HomeProfileCardItem[] = [
  { title: 'Docentes', description: 'Impulsa el talento tecnológico de tu alumnado. Comparte conocimiento y accede a recursos para el aula.', icon: '🎓', cta: 'Explorar', link: '/centers', accent: 'violet' },
  { title: 'Estudiantes', description: 'Aprende tecnologías, descubre formación y accede a oportunidades para desarrollar tu futuro.', icon: '🧑‍💻', cta: 'Explorar', link: '/orienta-tech', accent: 'cyan' },
  { title: 'Profesionales', description: 'Impulsa tu carrera en tecnología, comparte experiencia y amplía tu red de contactos.', icon: '💼', cta: 'Explorar', link: '/events', accent: 'blue' },
  { title: 'Empresas', description: 'Conecta con el talento, participa en eventos y comparte conocimiento real con la comunidad.', icon: '🏢', cta: 'Explorar', link: '/companies', accent: 'amber' },
  { title: 'Orientadores', description: 'Accede a recursos y actividades tecnológicas para tu alumnado y descubre iniciativas STEM.', icon: '🧭', cta: 'Explorar', link: '/orienta-tech', accent: 'pink' },
  { title: 'Starters', description: 'Descubre profesiones, formaciones y tus primeros pasos en el mundo tech. No necesitas experiencia.', icon: '🚀', cta: 'Explorar', link: '/knowledge', accent: 'teal' },
  { title: 'Women in Tech', description: 'Referentes, ayudas, comunidad y oportunidades para mujeres que quieren crecer en tecnología.', icon: '♀️', cta: 'Explorar', link: '/woman-tech', accent: 'fuchsia' },
  { title: 'Conócenos', description: 'Descubre quiénes somos, nuestra misión, valores y cómo trabajamos para impulsar el talento tech.', icon: '👥', cta: 'Explorar', link: '/about-us', accent: 'sky' },
];

export const HOME_PAST_EVENT_PHOTOS: HomePastEventPhotoItem[] = [
  { src: 'assets/techito_salero_ming.jpg', alt: 'Talk de TechRiders', label: 'Talks' },
  { src: 'assets/techito_karmela.jpg', alt: 'Encuentro #FPTour', label: '#FPTour' },
  { src: 'assets/techito_bici_tajamar.jpg', alt: 'Evento en Tajamar Tech', label: 'Tajamar Tech' },
  { src: 'assets/techito_piscineo.jpg', alt: 'Evento externo de comunidad', label: 'Eventos externos' },
];
