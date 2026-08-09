export type IntakeType = 'member' | 'ambassador' | 'session';

export interface JoinRequestPayload {
  name: string;
  email: string;
  requestType: IntakeType;
  communityRole: string;
  audience: string | null;
  organization: string | null;
  motivation: string;
  sessionTopic: string | null;
  sessionFormat: string | null;
}

export interface MemberDraftPayload {
  nombre: string;
  email: string;
  bio: string;
  intereses: string;
  organizacion: string | null;
  communityRole: string;
}
