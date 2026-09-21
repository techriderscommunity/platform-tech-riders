import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, catchError, map, of } from 'rxjs';
import { environment } from '@env/environment';
import { TeamZoneItem } from '@shared/ui/public-content.types';
import { getDefaultAvatar } from '@shared/utils/default-avatar';
import { ABOUT_ZONE_META } from './about.content';

interface PublicTeamMemberDto {
  id?: string;
  Id?: string;
  name?: string;
  Name?: string;
  lastName?: string;
  LastName?: string;
  about?: string | null;
  About?: string | null;
  linkedIn?: string;
  LinkedIn?: string;
  instagram?: string;
  Instagram?: string;
  x?: string;
  X?: string;
  youTube?: string;
  YouTube?: string;
  github?: string;
  Github?: string;
}

interface PublicTeamZoneDto {
  key?: string;
  Key?: string;
  members?: PublicTeamMemberDto[];
  Members?: PublicTeamMemberDto[];
}

@Injectable({ providedIn: 'root' })
export class AboutTeamService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/public/about/team`;

  private readonly zones$ = this.http.get<PublicTeamZoneDto[]>(this.baseUrl).pipe(
    map((zones) => {
      const receivedZones = new Map((zones ?? []).map((zone) => [zone.key ?? zone.Key ?? '', zone]));
      return Object.entries(ABOUT_ZONE_META).map(([key, meta]): TeamZoneItem => {
        const zone = receivedZones.get(key);
        const members = zone?.members ?? zone?.Members ?? [];
      return {
          key: key as TeamZoneItem['key'],
          title: meta.title,
          description: meta.description,
          members: members.map((member) => ({
          name: `${member.name ?? member.Name ?? ''} ${member.lastName ?? member.LastName ?? ''}`.trim(),
          photo: member.id ?? member.Id
            ? `${this.baseUrl}/${member.id ?? member.Id}/photo`
            : getDefaultAvatar([key]),
          photoFallback: getDefaultAvatar([key]),
          photoAlt: `Avatar por defecto de ${meta.title}`,
          role: this.toSubtitle(member.about ?? member.About, meta.description, meta.title),
          socials: [
            member.linkedIn ?? member.LinkedIn ? { platform: 'linkedin' as const, href: member.linkedIn ?? member.LinkedIn! } : null,
            member.github ?? member.Github ? { platform: 'github' as const, href: member.github ?? member.Github! } : null,
            member.x ?? member.X ? { platform: 'x' as const, href: member.x ?? member.X! } : null,
            member.instagram ?? member.Instagram ? { platform: 'instagram' as const, href: member.instagram ?? member.Instagram! } : null,
            member.youTube ?? member.YouTube ? { platform: 'youtube' as const, href: member.youTube ?? member.YouTube! } : null,
          ].filter((social): social is { platform: 'linkedin' | 'github' | 'x' | 'instagram' | 'youtube'; href: string } => social !== null),
          })),
        };
      });
    }),
    catchError(() => of<TeamZoneItem[]>([])),
  );

  getTeamZones(): Observable<TeamZoneItem[]> {
    return this.zones$;
  }

  private toSubtitle(about: string | null | undefined, zoneDescription: string, zoneTitle: string): string {
    const trimmedAbout = about?.trim();
    return trimmedAbout ? `${trimmedAbout} · ${zoneTitle}` : zoneDescription;
  }
}
