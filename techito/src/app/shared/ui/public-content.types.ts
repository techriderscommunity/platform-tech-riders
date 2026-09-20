/** Tipos de presentacion puros (sin datos), compartidos entre features publicas. */

export interface MetricItem {
  icon: string;
  value: string;
  label: string;
}

export interface FeatureCardItem {
  icon: string;
  title: string;
  description: string;
  points: string[];
}

export interface HomeProfileCardItem {
  title: string;
  description: string;
  icon: string;
  cta: string;
  link: string;
  accent: string;
}

export interface HomePastEventPhotoItem {
  src: string;
  alt: string;
  label: string;
}

export interface GalleryItem {
  src: string;
  alt: string;
}

export interface GalleryGroupItem {
  title: string;
  subtitle: string;
  items: GalleryItem[];
}

export interface SelectOptionItem {
  label: string;
  value: string;
}

export interface SocialLinkItem {
  platform: 'linkedin' | 'github' | 'x' | 'instagram' | 'youtube';
  href: string;
}

export interface TeamMemberItem {
  name: string;
  role?: string;
  photo?: string;
  photoAlt?: string;
  socials: SocialLinkItem[];
}

export interface TeamZoneItem {
  key: 'staff' | 'community-leaders' | 'ambassador' | 'member';
  title: string;
  description: string;
  members: TeamMemberItem[];
}
