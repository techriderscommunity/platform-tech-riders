import { SelectOptionItem } from '@shared/ui/public-content.types';

/** Segunda metrica es puramente editorial (no es un count real). La primera se rellena con el dato dinamico. */
export const JOIN_METRICS_META: ReadonlyArray<{ icon: string; label: string; staticValue?: string }> = [
  { icon: 'rocket', label: 'Proceso de ingreso', staticValue: '4 pasos' },
  { icon: 'people', label: 'Miembros activos' },
];

export const JOIN_INTAKE_OPTIONS: SelectOptionItem[] = [
  { label: 'Quiero unirme como miembro', value: 'member' },
  { label: 'Quiero solicitar ser Ambassador', value: 'ambassador' },
  { label: 'Quiero solicitar una sesión', value: 'session' },
];
