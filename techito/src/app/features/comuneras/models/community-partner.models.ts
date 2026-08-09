export type CommunityPartnerScope = 'local' | 'national' | 'international';

export type CommunityPartnerStatus =
  | 'pending'
  | 'review'
  | 'more-info'
  | 'approved'
  | 'rejected'
  | 'suspended';

export interface CommunityPartner {
  id: string;
  name: string;
  logoUrl: string;
  shortDescription: string;
  description: string;
  mission: string;
  cityOrScope: string;
  website?: string;
  linkedin?: string;
  instagram?: string;
  x?: string;
  youtube?: string;
  discord?: string;
  telegram?: string;
  topics: string[];
  scope: CommunityPartnerScope;
  contactName: string;
  contactEmail: string;
  requestedAt?: Date;
  memberCount?: number;
  joinedAt?: Date;
  collaborations: string[];
  status: CommunityPartnerStatus;
}

export interface CommunityPartnerApplication {
  name: string;
  logoUrl: string;
  website: string;
  contactEmail: string;
  contactName: string;
  whoYouAre: string;
  whatYouDo: string;
  mission: string;
  topics: string[];
  scope: CommunityPartnerScope;
  linkedin?: string;
  instagram?: string;
  x?: string;
  youtube?: string;
  discord?: string;
  telegram?: string;
  motivation: string;
  collaborationIdeas: string;
}
