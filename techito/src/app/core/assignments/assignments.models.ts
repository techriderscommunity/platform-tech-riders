export interface AssignmentCandidateApi {
  UserId: string;
  Name: string;
  Email: string;
  SkillLevel?: string | null;
  HasRequestedAvailability: boolean;
  SessionsAsSpeaker: number;
  EventsParticipated: number;
  FPToursAsAmbassador: number;
}
