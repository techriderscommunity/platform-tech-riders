import { GalleryGroupItem } from '@shared/ui/public-content.types';

export const EVENTS_GALLERY_GROUPS: GalleryGroupItem[] = [
  {
    title: '#FPTour',
    subtitle: 'Meetups y sesiones en centros de formación.',
    items: [
      { src: 'assets/techito_karmela.jpg', alt: 'Evento #FPTour en aula' },
      { src: 'assets/techito_karmela.jpg', alt: 'Networking durante #FPTour' },
      { src: 'assets/techito_karmela.jpg', alt: 'Participantes de #FPTour' },
    ],
  },
  {
    title: 'Eventos externos',
    subtitle: 'Conferencias, webinars y colaboraciones con otras comunidades.',
    items: [
      { src: 'assets/techito_piscineo.jpg', alt: 'Evento externo de la comunidad TechRiders' },
      { src: 'assets/techito_bici_tajamar.jpg', alt: 'Conferencia y networking de TechRiders' },
      { src: 'assets/techito_piscineo.jpg', alt: 'Participación de TechRiders en evento externo' },
    ],
  },
];
