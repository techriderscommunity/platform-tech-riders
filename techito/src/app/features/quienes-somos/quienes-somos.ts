import { ChangeDetectionStrategy, Component, computed, inject } from '@angular/core';
import { UiMetricsStrip } from '@shared/ui/metrics-strip/metrics-strip';
import { UiCarouselItem, UiMediaCarousel } from '@shared/ui/media-carousel/media-carousel';
import { CommunityPartnersStore } from '../comuneras/services/community-partners.store';

type SocialLink = {
  platform: 'linkedin' | 'github' | 'x' | 'instagram' | 'youtube';
  href: string;
};

type TeamMember = {
  name: string;
  role: string;
  photo: string;
  photoAlt: string;
  socials: SocialLink[];
};

type TeamZone = {
  key: 'staff' | 'community-leaders' | 'ambassador' | 'member';
  title: string;
  description: string;
  members: TeamMember[];
};

@Component({
  selector: 'app-quienes-somos',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [UiMetricsStrip, UiMediaCarousel],
  templateUrl: './quienes-somos.html',
  styleUrl: './quienes-somos.scss'
})
export class QuienesSomos {
  private readonly communityPartnersStore = inject(CommunityPartnersStore);

  private readonly allSocials: SocialLink[] = [
    { platform: 'linkedin', href: 'https://www.linkedin.com' },
    { platform: 'github', href: 'https://github.com' },
    { platform: 'x', href: 'https://x.com' },
    { platform: 'instagram', href: 'https://www.instagram.com' },
    { platform: 'youtube', href: 'https://www.youtube.com/@TechRidersMedia' },
  ];

  readonly communityMetrics = [
    { icon: '🧭', value: '4', label: 'Líneas de comunidad' },
    { icon: '👥', value: '1500+', label: 'Participantes' },
    { icon: '🤝', value: '20+', label: 'Colaboraciones' },
    { icon: '🎤', value: '80+', label: 'Sesiones' },
  ];

  readonly staffMembers: TeamMember[] = [
    {
      name: 'Sergio Hierro',
      role: 'Founder & Community Lead',
      photo: 'assets/staff/sergio-hierro.png',
      photoAlt: 'Foto de Sergio Hierro',
      socials: this.allSocials,
    },
    {
      name: 'Juan Bou',
      role: 'Program Coordinator',
      photo: 'assets/staff/Juan Bou.jpg',
      photoAlt: 'Foto de Juan Bou',
      socials: this.allSocials,
    },
    {
      name: 'Diego Zapico',
      role: 'Learning Initiatives',
      photo: 'assets/staff/diego-zapico.png',
      photoAlt: 'Foto de Diego Zapico',
      socials: this.allSocials,
    },
    {
      name: 'Ana Pereira',
      role: 'Operations & Community Programs',
      photo: 'assets/staff/ana-pereira.jpg',
      photoAlt: 'Foto de Ana Pereira',
      socials: this.allSocials,
    },
    {
      name: 'Borja Piris',
      role: 'Engineering Mentor',
      photo: 'assets/staff/borja-piris.jpg',
      photoAlt: 'Foto de Borja Piris',
      socials: this.allSocials,
    },
  ];

  readonly communityLeadersMembers: TeamMember[] = [
    {
      name: 'Mónica Delgado',
      role: 'Community Leader',
      photo: 'assets/community-leaders/Mónica Delgado.jpg',
      photoAlt: 'Foto de Mónica Delgado',
      socials: this.allSocials,
    },
    {
      name: 'Rodrigo Liberoff',
      role: 'Community Leader',
      photo: 'assets/community-leaders/Rodrigo Liberoff.jpg',
      photoAlt: 'Foto de Rodrigo Liberoff',
      socials: this.allSocials,
    },
  ];

  readonly ambassadorMembers: TeamMember[] = [
    {
      name: 'María Reina',
      role: 'Ambassador · Community Speaker',
      photo: 'assets/ambassadors/María Reina.jpg',
      photoAlt: 'Foto de María Reina',
      socials: this.allSocials,
    },
    {
      name: 'Estefany Duran',
      role: 'Ambassador · Career Talks',
      photo: 'assets/ambassadors/Estefany Duran.jpg',
      photoAlt: 'Foto de Estefany Duran',
      socials: this.allSocials,
    },
    {
      name: 'Celeste Sánchez',
      role: 'Ambassador · Learning Sessions',
      photo: 'assets/ambassadors/Celeste Sánchez.jpg',
      photoAlt: 'Foto de Celeste Sánchez',
      socials: this.allSocials,
    },
  ];

  readonly memberMembers: TeamMember[] = [
    {
      name: 'Marta Moreno',
      role: 'Member · Frontend Developer',
      photo: 'assets/member/Marta Moreno.png',
      photoAlt: 'Foto de Marta Moreno',
      socials: this.allSocials,
    },
    {
      name: 'Macarena Mamolar',
      role: 'Member · Product Design',
      photo: 'assets/member/Macarena Mamolar.jpg',
      photoAlt: 'Foto de Macarena Mamolar',
      socials: this.allSocials,
    },
    {
      name: 'Jorge Rodríguez',
      role: 'Member · Backend Engineer',
      photo: 'assets/member/Jorge Rodríguez.png',
      photoAlt: 'Foto de Jorge Rodríguez',
      socials: this.allSocials,
    },
    {
      name: 'Diego Pérez',
      role: 'Member · Data & AI',
      photo: 'assets/member/Diego Pérez.png',
      photoAlt: 'Foto de Diego Pérez',
      socials: this.allSocials,
    },
  ];

  readonly teamZones: TeamZone[] = [
    {
      key: 'staff',
      title: 'Staff',
      description: 'Personas que lideran y coordinan la comunidad.',
      members: this.staffMembers,
    },
    {
      key: 'community-leaders',
      title: 'Community Leaders',
      description: 'Personas que ayudan a dar forma y operar iniciativas de Tech Riders.',
      members: this.communityLeadersMembers,
    },
    {
      key: 'ambassador',
      title: 'Ambassador',
      description: 'Personas que participan activamente en actividades y ayudan a extender comunidad.',
      members: this.ambassadorMembers,
    },
    {
      key: 'member',
      title: 'Member',
      description: 'Personas que se unen y participan en sesiones, actividades y comunidad.',
      members: this.memberMembers,
    },
  ];

  readonly comunerasSubtitle = 'Comunidades compañeras con las que compartimos camino, ideas y ganas de hacer cosas grandes.';

  readonly comunerasCarouselItems = computed<UiCarouselItem[]>(() =>
    this.communityPartnersStore
      .approvedPartners()
      .map(partner => ({
        kind: 'image' as const,
        src: partner.logoUrl,
        title: partner.name,
        subtitle: `${partner.shortDescription} · ${partner.cityOrScope}`,
        alt: `Logo de ${partner.name}`,
        link: `/community-partners/${partner.id}`,
      })),
  );

  toCarouselItems(members: TeamMember[]): UiCarouselItem[] {
    return members.map(member => ({
      kind: 'image',
      src: member.photo,
      title: member.name,
      subtitle: member.role,
      alt: member.photoAlt,
      socials: member.socials,
    }));
  }

}


