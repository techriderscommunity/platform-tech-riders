import { FeatureCardItem } from '@shared/ui/public-content.types';
import { UiProgressCardItem } from '@shared/ui/progress-cards/progress-cards';

/** Segunda metrica es puramente editorial (no es un count real). La primera se rellena con el dato dinamico. */
export const ORIENTA_TECH_METRICS_META: ReadonlyArray<{ icon: string; label: string; staticValue?: string }> = [
  { icon: 'book', label: 'Sesiones' },
  { icon: 'sparkles', label: 'Mentorías', staticValue: '1:1' },
];

export const ORIENTA_TECH_CORE_FEATURES: FeatureCardItem[] = [
  { icon: '📚', title: 'Formaciones regladas', description: 'Programas formales para iniciar o transformar tu carrera tecnológica.', points: ['Ciclos formativos', 'Bootcamps certificados', 'Rutas guiadas'] },
  { icon: '💼', title: 'Empleo Tech', description: 'Conexión con empresas reales que contratan talento junior y en transición.', points: ['Ofertas curadas', 'Prácticas', 'Networking de hiring'] },
  { icon: '🎯', title: 'Mentoría personalizada', description: 'Acompañamiento por profesionales en activo para acelerar tu evolución.', points: ['Mentor asignado', 'Sesiones periódicas', 'Seguimiento de objetivos'] },
  { icon: '🚀', title: 'Orientación estratégica', description: 'Plan de carrera con objetivos accionables y revisión continua.', points: ['Especialización', 'Roadmap', 'Revisión trimestral'] },
];

// TODO(gap): "progress" fake eliminado; si aparece una metrica real, anadir aqui como campo dinamico.
export const ORIENTA_TECH_PARTICIPATION_TRACKS: UiProgressCardItem[] = [
  { title: 'Empresas colaboradoras', status: 'Activa', detail: 'Red de organizaciones que abren oportunidades reales de empleabilidad.', ctaLabel: 'Ver oportunidades', ctaLink: '/join' },
  { title: 'Recruiters y RRHH', status: 'Activa', detail: 'Sesiones de mercado laboral, procesos de selección y feedback estructurado.', ctaLabel: 'Participar', ctaLink: '/join' },
  { title: 'Recursos y contenidos', status: 'En crecimiento', detail: 'Videoteca, guías y casos para crecer en soft skills y carrera profesional.', ctaLabel: 'Explorar', ctaLink: '/knowledge' },
];

export const ORIENTA_TECH_STUDY_SECTIONS: FeatureCardItem[] = [
  { icon: '', title: 'FP', description: 'Itinerarios base en desarrollo, sistemas, data y ciberseguridad para iniciar carrera tech.', points: ['SMR', 'ASIR', 'DAW', 'DAM'] },
  { icon: '', title: 'Másteres FP', description: 'Especialización en áreas con alta demanda y enfoque práctico para empleabilidad.', points: ['Big Data', 'Ciberseguridad', 'Cloud', 'IA aplicada'] },
  { icon: '', title: 'Certificados', description: 'Rutas cortas para validar competencias y acelerar inserción laboral.', points: ['Qué son', 'Cuándo elegirlos', 'FAQ'] },
  { icon: '', title: 'Grados y Másteres', description: 'Opciones universitarias orientadas a perfiles técnicos y de especialización avanzada.', points: ['Grados base', 'Másteres de especialidad', 'Comparativa por perfil'] },
  { icon: '', title: 'Certificaciones', description: 'Credenciales por fabricante para reforzar tu perfil profesional.', points: ['Ruta por proveedor', 'Nivel recomendado', 'Preparación guiada'] },
];
