import { ChangeDetectionStrategy, Component, DestroyRef, OnInit, inject } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { RouterLink } from '@angular/router';
import { catchError, EMPTY, tap } from 'rxjs';
import { OrientaTechStatsService } from './orienta-tech-stats.service';
import {
  ORIENTA_TECH_METRICS_META,
  ORIENTA_TECH_CORE_FEATURES,
  ORIENTA_TECH_PARTICIPATION_TRACKS,
  ORIENTA_TECH_STUDY_SECTIONS,
} from './orienta-tech.content';
import { FeatureCardItem, MetricItem } from '@shared/ui/public-content.types';
import { UiProgressCardItem } from '@shared/ui/progress-cards/progress-cards';
import { UiMetricsStrip } from '@shared/ui/metrics-strip/metrics-strip';
import { UiFeatureCards } from '@shared/ui/feature-cards/feature-cards';
import { UiProgressCards } from '@shared/ui/progress-cards/progress-cards';

@Component({
  selector: 'app-orienta-tech',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [RouterLink, UiMetricsStrip, UiFeatureCards, UiProgressCards],
  templateUrl: './orienta-tech.html',
  styleUrl: './orienta-tech.scss'
})
export class OrientaTech implements OnInit {
  private readonly orientaTechStatsService = inject(OrientaTechStatsService);
  private readonly destroyRef = inject(DestroyRef);

  orientaMetrics: MetricItem[] = [];
  coreFeatures: FeatureCardItem[] = ORIENTA_TECH_CORE_FEATURES;
  participationTracks: UiProgressCardItem[] = ORIENTA_TECH_PARTICIPATION_TRACKS;
  studySections: FeatureCardItem[] = ORIENTA_TECH_STUDY_SECTIONS;

  ngOnInit(): void {
    this.orientaTechStatsService
      .getStats()
      .pipe(
        tap((stats) => {
          this.orientaMetrics = ORIENTA_TECH_METRICS_META.map((meta) => ({
            icon: meta.icon,
            label: meta.label,
            value: meta.staticValue ?? String(stats.activeSessions),
          }));
        }),
        catchError(() => EMPTY),
        takeUntilDestroyed(this.destroyRef)
      )
      .subscribe();

  }
}


