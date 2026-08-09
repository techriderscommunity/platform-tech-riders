export const PUBLIC_EVENT_TYPES = [
  'Sesion tecnica',
  'Orientacion',
  'Empleabilidad',
  'Podcast',
  'Workshop',
  'Woman Tech',
] as const;

export const PUBLIC_EVENT_MODALITIES = ['Online', 'Presencial', 'Hibrido'] as const;

export const PUBLIC_EVENT_TOPICS = [
  'Azure',
  '.NET',
  'Datos',
  'Ciberseguridad',
  'Carrera',
  'Soft Skills',
  'Comunidad',
] as const;

export type PublicEventType = (typeof PUBLIC_EVENT_TYPES)[number];
export type PublicEventModality = (typeof PUBLIC_EVENT_MODALITIES)[number];
export type PublicEventTopic = (typeof PUBLIC_EVENT_TOPICS)[number];

export interface PublicEvent {
  title: string;
  summary: string;
  type: PublicEventType;
  modality: PublicEventModality;
  topic: PublicEventTopic;
  date: string;
  place: string;
  url: string;
}
