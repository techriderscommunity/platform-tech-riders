import { ChangeDetectionStrategy, Component, DestroyRef, OnInit, computed, inject } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { catchError, EMPTY, tap } from 'rxjs';
import { AboutStatsService } from './about-stats.service';
import { AboutTeamService } from './about-team.service';
import { ABOUT_METRICS_META, ABOUT_SOCIAL_LINKS, ABOUT_ZONE_META } from './about.content';
import { MetricItem, SocialLinkItem, TeamMemberItem, TeamZoneItem } from '@shared/ui/public-content.types';
import { UiMetricsStrip } from '@shared/ui/metrics-strip/metrics-strip';
import { UiCarouselItem, UiMediaCarousel } from '@shared/ui/media-carousel/media-carousel';
import { CommunityPartnersStore } from '../comuneras/services/community-partners.store';

const COMMUNITY_PARTNER_FALLBACK_AVATAR = '/assets/avatar-comuneras.png';

@Component({
  selector: 'app-quienes-somos',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [UiMetricsStrip, UiMediaCarousel],
  templateUrl: './quienes-somos.html',
  styleUrl: './quienes-somos.scss'
})
export class QuienesSomos implements OnInit {
  private readonly communityPartnersStore = inject(CommunityPartnersStore);
  private readonly aboutStatsService = inject(AboutStatsService);
  private readonly aboutTeamService = inject(AboutTeamService);
  private readonly destroyRef = inject(DestroyRef);

  allSocials: SocialLinkItem[] = ABOUT_SOCIAL_LINKS;
  communityMetrics: MetricItem[] = [];
  teamZones: TeamZoneItem[] = Object.entries(ABOUT_ZONE_META).map(([key, meta]) => ({
    key: key as TeamZoneItem['key'],
    title: meta.title,
    description: meta.description,
    members: [],
  }));

  readonly comunerasSubtitle = 'Comunidades compañeras con las que compartimos camino, ideas y ganas de hacer cosas grandes.';

  ngOnInit(): void {
    this.aboutStatsService
      .getStats()
      .pipe(
        tap((stats) => {
          this.communityMetrics = ABOUT_METRICS_META.map((meta) => ({
            icon: meta.icon,
            label: meta.label,
            value: String(stats[meta.key]),
          }));
        }),
        catchError(() => EMPTY),
        takeUntilDestroyed(this.destroyRef)
      )
      .subscribe();

    this.aboutTeamService
      .getTeamZones()
      .pipe(
        tap((teamZones) => {
          this.teamZones = teamZones;
        }),
        catchError(() => EMPTY),
        takeUntilDestroyed(this.destroyRef)
      )
      .subscribe();
  }

  readonly comunerasCarouselItems = computed<UiCarouselItem[]>(() =>
    this.communityPartnersStore
      .approvedPartners()
      .map(partner => ({
        kind: 'image' as const,
        src: partner.logoUrl,
        fallbackSrc: COMMUNITY_PARTNER_FALLBACK_AVATAR,
        title: partner.name,
        subtitle: `${partner.shortDescription} · ${partner.cityOrScope}`,
        alt: `Logo de ${partner.name}`,
        socials: [
          partner.linkedin ? { platform: 'linkedin' as const, href: partner.linkedin } : null,
          partner.github ? { platform: 'github' as const, href: partner.github } : null,
          partner.x ? { platform: 'x' as const, href: partner.x } : null,
          partner.instagram ? { platform: 'instagram' as const, href: partner.instagram } : null,
          partner.youtube ? { platform: 'youtube' as const, href: partner.youtube } : null,
        ].filter((social): social is { platform: 'linkedin' | 'github' | 'x' | 'instagram' | 'youtube'; href: string } => social !== null),
      })),
  );

  toCarouselItems(members: TeamMemberItem[]): UiCarouselItem[] {
    return members.map(member => ({
      kind: 'image',
      src: member.photo ?? '',
      fallbackSrc: member.photoFallback,
      title: member.name,
      subtitle: member.role,
      alt: member.photoAlt,
      socials: member.socials,
    }));
  }

}


