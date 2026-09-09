import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '@env/environment';
import { CommunityPartnerScope } from '../models/community-partner.models';

export interface CreateCommunityPartnerApplicationRequest {
  readonly name: string;
  readonly logoUrl?: string;
  readonly website: string;
  readonly contactEmail: string;
  readonly contactName: string;
  readonly whoYouAre: string;
  readonly whatYouDo: string;
  readonly mission: string;
  readonly topics: string;
  readonly scope: CommunityPartnerScope;
  readonly linkedin?: string;
  readonly instagram?: string;
  readonly x?: string;
  readonly youtube?: string;
  readonly github?: string;
  readonly motivation: string;
  readonly collaborationIdeas: string;
}

export interface CommunityPartnerApplicationResponse {
  readonly id: string;
  readonly status: string;
  readonly linkedIn?: string | null;
  readonly instagram?: string | null;
  readonly x?: string | null;
  readonly youTube?: string | null;
  readonly github?: string | null;
}

@Injectable({ providedIn: 'root' })
export class CommunityPartnerApplicationsService {
  private readonly http = inject(HttpClient);
  private readonly url = `${environment.apiUrl}/community-partner-applications`;

  create(request: CreateCommunityPartnerApplicationRequest): Observable<CommunityPartnerApplicationResponse> {
    return this.http.post<CommunityPartnerApplicationResponse>(this.url, request);
  }
}
