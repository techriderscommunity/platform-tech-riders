import { SelectOptionItem } from '@shared/ui/public-content.types';

export const JUNIOR_SKILL_OPTIONS: string[] = [
  'JavaScript', 'TypeScript', 'React', 'Angular', 'Vue.js',
  'Node.js', 'Python', 'Java', 'C++', 'HTML', 'CSS',
  'Sass', 'Bootstrap', 'Tailwind', 'Git', 'Docker',
  'SQL', 'MongoDB', 'REST APIs', 'GraphQL',
];

export const JUNIOR_AVAILABILITY_OPTIONS: SelectOptionItem[] = [
  { label: 'Inmediata', value: 'Inmediata' },
  { label: 'En 1 semana', value: 'En 1 semana' },
  { label: 'En 2 semanas', value: 'En 2 semanas' },
  { label: 'En 1 mes', value: 'En 1 mes' },
];
