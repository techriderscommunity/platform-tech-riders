import { ChangeDetectionStrategy, Component, DestroyRef, OnInit, computed, inject } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { catchError, EMPTY, tap } from 'rxjs';
import { PublicContentService } from '@core/content/public-content.service';
import { MetricItem, SocialLinkItem, TeamMemberItem, TeamZoneItem } from '@core/content/public-content.models';
import { UiMetricsStrip } from '@shared/ui/metrics-strip/metrics-strip';
import { UiCarouselItem, UiMediaCarousel } from '@shared/ui/media-carousel/media-carousel';
import { CommunityPartnersStore } from '../comuneras/services/community-partners.store';

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
  private readonly publicContentService = inject(PublicContentService);
  private readonly destroyRef = inject(DestroyRef);

  allSocials: SocialLinkItem[] = [];
  communityMetrics: MetricItem[] = [];
  teamZones: TeamZoneItem[] = [];

  readonly comunerasSubtitle = 'Comunidades compañeras con las que compartimos camino, ideas y ganas de hacer cosas grandes.';

  ngOnInit(): void {
    this.publicContentService
      .getPublicContent()
      .pipe(
        tap((content) => {
          this.communityMetrics = content.about.metrics;
          this.allSocials = content.about.socialLinks;
          this.teamZones = content.about.teamZones;
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
        title: partner.name,
        subtitle: `${partner.shortDescription} · ${partner.cityOrScope}`,
        alt: `Logo de ${partner.name}`,
        link: `/community-partners/${partner.id}`,
      })),
  );

  toCarouselItems(members: TeamMemberItem[]): UiCarouselItem[] {
    return members.map(member => ({
      kind: 'image',
      src: member.photo,
      title: member.name,
      subtitle: member.role,
      alt: member.photoAlt,
      socials: member.socials,
    }));
  }

}


