import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { environment } from '@env/environment';
import { SkillApi, UserSkillApi } from './skills.models';

@Injectable({ providedIn: 'root' })
export class SkillsService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/skills`;

  getCatalog() {
    return this.http.get<SkillApi[]>(this.baseUrl);
  }

  getMine() {
    return this.http.get<UserSkillApi[]>(`${this.baseUrl}/me`);
  }

  addOrUpdateMine(skillId: string, level: string, isSpeakerSkill: boolean, isMentorSkill: boolean) {
    return this.http.post<UserSkillApi>(`${this.baseUrl}/me`, {
      SkillId: skillId,
      Level: level,
      IsSpeakerSkill: isSpeakerSkill,
      IsMentorSkill: isMentorSkill,
    });
  }

  removeMine(skillId: string) {
    return this.http.delete(`${this.baseUrl}/me/${skillId}`);
  }
}
