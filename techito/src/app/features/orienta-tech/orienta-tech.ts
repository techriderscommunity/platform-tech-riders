import { ChangeDetectionStrategy, Component, DestroyRef, OnInit, inject } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { RouterLink } from '@angular/router';
import { catchError, EMPTY, tap } from 'rxjs';
import { PublicContentService } from '@core/content/public-content.service';
import { FeatureCardItem, MetricItem, ProgressCardItem } from '@core/content/public-content.models';
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
  private readonly publicContentService = inject(PublicContentService);
  private readonly destroyRef = inject(DestroyRef);

  orientaMetrics: MetricItem[] = [];
  coreFeatures: FeatureCardItem[] = [];
  participationTracks: ProgressCardItem[] = [];
  studySections: FeatureCardItem[] = [];

  ngOnInit(): void {
    this.publicContentService
      .getPublicContent()
      .pipe(
        tap((content) => {
          this.orientaMetrics = content.orientaTech.metrics;
          this.coreFeatures = content.orientaTech.coreFeatures;
          this.participationTracks = content.orientaTech.participationTracks;
          this.studySections = content.orientaTech.studySections;
        }),
        catchError(() => EMPTY),
        takeUntilDestroyed(this.destroyRef)
      )
      .subscribe();

  }
}


