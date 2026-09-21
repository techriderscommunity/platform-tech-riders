import { SelectOptionItem } from '@shared/ui/public-content.types';

export const AMBASSADOR_STATUS_OPTIONS: SelectOptionItem[] = [
  { label: 'Todos', value: '' },
  { label: 'Activos', value: 'activo' },
  { label: 'Desactivados', value: 'desactivado' },
  { label: 'Pendientes', value: 'pendiente' },
];

export const AMBASSADOR_AVAILABILITY_OPTIONS: SelectOptionItem[] = [
  { label: 'Baja disponibilidad', value: '1 bloque semanal' },
  { label: 'Disponibilidad media', value: '2 o 3 bloques semanales' },
  { label: 'Alta disponibilidad', value: '4 o más bloques semanales' },
];

export const STAFF_PERIOD_OPTIONS: SelectOptionItem[] = [
  { label: 'Este mes', value: 'month' },
  { label: 'Este año', value: 'year' },
  { label: 'Todo', value: 'all' },
];
