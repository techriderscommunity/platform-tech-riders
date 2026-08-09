import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { environment } from '@env/environment';

export interface MemberProfileApi {
  Name: string;
  Email: string;
  Bio: string;
  Interests: string;
  Audience: string;
  CommunityRole: string;
  Organization: string;
}

export interface SaveMemberProfilePayload {
  userKey: string;
  name: string;
  email: string;
  bio: string;
  interests: string;
  audience: string;
  communityRole: string;
  organization: string;
}

@Injectable({ providedIn: 'root' })
export class MemberProfileService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/intranet/perfil`;

  getProfile(userKey: string, email: string) {
    const params = new HttpParams()
      .set('userKey', userKey)
      .set('email', email);

    return this.http.get<MemberProfileApi>(this.baseUrl, { params });
  }

  saveProfile(payload: SaveMemberProfilePayload) {
    return this.http.put<MemberProfileApi>(this.baseUrl, payload);
  }
}
