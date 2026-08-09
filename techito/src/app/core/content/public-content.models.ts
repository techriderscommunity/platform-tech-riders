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

export interface ProgressCardItem {
  title: string;
  detail: string;
  progress: number;
  status: string;
  ctaLabel?: string;
  ctaLink?: string;
}

export interface ResourceCardItem {
  mode: string;
  title: string;
  summary: string;
  tags: string[];
  meta: string;
  ctaLabel: string;
  ctaLink: string;
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

export interface ParticipationModeItem {
  title: string;
  detail: string;
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

export interface VideoCarouselItem {
  title: string;
  src: string;
}

export interface JourneyStepItem {
  step: string;
  title: string;
  text: string;
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
  role: string;
  photo: string;
  photoAlt: string;
  socials: SocialLinkItem[];
}

export interface TeamZoneItem {
  key: 'staff' | 'community-leaders' | 'ambassador' | 'member';
  title: string;
  description: string;
  members: TeamMemberItem[];
}

export interface PublicContentPayload {
  home: {
    stats: MetricItem[];
    profilePanelCards: HomeProfileCardItem[];
    pastEventPhotos: HomePastEventPhotoItem[];
  };
  events: {
    participationModes: ParticipationModeItem[];
    galleryGroups: GalleryGroupItem[];
    talksFallback: VideoCarouselItem[];
  };
  centers: {
    metrics: MetricItem[];
    cards: FeatureCardItem[];
  };
  companies: {
    valueCards: FeatureCardItem[];
    processCards: ProgressCardItem[];
  };
  opportunities: {
    tracks: ProgressCardItem[];
    resources: ResourceCardItem[];
  };
  womanTech: {
    metrics: MetricItem[];
    journey: JourneyStepItem[];
  };
  join: {
    metrics: MetricItem[];
    intakeOptions: SelectOptionItem[];
  };
  orientaTech: {
    metrics: MetricItem[];
    coreFeatures: FeatureCardItem[];
    participationTracks: ProgressCardItem[];
    studySections: FeatureCardItem[];
  };
  about: {
    metrics: MetricItem[];
    socialLinks: SocialLinkItem[];
    teamZones: TeamZoneItem[];
  };
  tutorials: {
    featuredCategories: string[];
  };
  intranet: {
    ambassadorStatusOptions: SelectOptionItem[];
    ambassadorAvailabilityOptions: SelectOptionItem[];
    staffPeriodOptions: SelectOptionItem[];
    memberCategoryOptions: string[];
    sessionStatusOptions: string[];
    juniorSkillOptions: string[];
    juniorAvailabilityOptions: SelectOptionItem[];
  };
}
