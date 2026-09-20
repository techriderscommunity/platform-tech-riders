export interface SkillApi {
  Id: string;
  Name: string;
  Description?: string | null;
  ParentSkillId?: string | null;
}

export interface UserSkillApi {
  SkillId: string;
  SkillName: string;
  Level: string;
  IsSpeakerSkill: boolean;
  IsMentorSkill: boolean;
}

export const SKILL_LEVELS = [
  { label: 'Principiante', value: 'Beginner' },
  { label: 'Intermedio', value: 'Intermediate' },
  { label: 'Avanzado', value: 'Advanced' },
  { label: 'Experto', value: 'Expert' },
] as const;
