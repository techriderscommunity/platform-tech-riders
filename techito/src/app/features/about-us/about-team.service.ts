import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, catchError, map, of, shareReplay } from 'rxjs';
import { environment } from '@env/environment';
import { TeamZoneItem } from '@shared/ui/public-content.types';
import { ABOUT_ZONE_META } from './about.content';

interface PublicTeamMemberDto {
  name: string;
  lastName: string;
  linkedIn?: string;
  instagram?: string;
  x?: string;
  youTube?: string;
  github?: string;
}

interface PublicTeamZoneDto {
  key: string;
  members: PublicTeamMemberDto[];
}

@Injectable({ providedIn: 'root' })
export class AboutTeamService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/public/about/team`;

  private readonly zones$ = this.http.get<PublicTeamZoneDto[]>(this.baseUrl).pipe(
    map((zones) => (zones ?? []).map((zone): TeamZoneItem => {
      const meta = ABOUT_ZONE_META[zone.key] ?? { title: zone.key, description: '' };
      return {
        key: zone.key as TeamZoneItem['key'],
        title: meta.title,
        description: meta.description,
        members: zone.members.map((member) => ({
          name: `${member.name} ${member.lastName}`.trim(),
          // TODO(gap): sin campo de foto/avatar en User todavia.
          socials: [
            member.linkedIn ? { platform: 'linkedin' as const, href: member.linkedIn } : null,
            member.github ? { platform: 'github' as const, href: member.github } : null,
            member.x ? { platform: 'x' as const, href: member.x } : null,
            member.instagram ? { platform: 'instagram' as const, href: member.instagram } : null,
            member.youTube ? { platform: 'youtube' as const, href: member.youTube } : null,
          ].filter((social): social is { platform: 'linkedin' | 'github' | 'x' | 'instagram' | 'youtube'; href: string } => social !== null),
        })),
      };
    })),
    catchError(() => of<TeamZoneItem[]>([])),
    shareReplay({ bufferSize: 1, refCount: true }),
  );

  getTeamZones(): Observable<TeamZoneItem[]> {
    return this.zones$;
  }
}
