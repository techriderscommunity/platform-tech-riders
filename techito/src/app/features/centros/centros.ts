import { ChangeDetectionStrategy, Component, DestroyRef, OnInit, inject } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { catchError, EMPTY, tap } from 'rxjs';
import { RouterLink } from '@angular/router';
import { PublicContentService } from '@core/content/public-content.service';
import { FeatureCardItem, MetricItem } from '@core/content/public-content.models';
import { UiFeatureCards } from '@shared/ui/feature-cards/feature-cards';
import { UiMetricsStrip } from '@shared/ui/metrics-strip/metrics-strip';

@Component({
  selector: 'app-centros',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [RouterLink, UiMetricsStrip, UiFeatureCards],
  templateUrl: './centros.html',
  styleUrl: './centros.scss'
})
export class Centros implements OnInit {
  private readonly publicContentService = inject(PublicContentService);
  private readonly destroyRef = inject(DestroyRef);

  metrics: MetricItem[] = [];
  cards: FeatureCardItem[] = [];

  ngOnInit(): void {
    this.publicContentService
      .getPublicContent()
      .pipe(
        tap((content) => {
          this.metrics = content.centers.metrics;
          this.cards = content.centers.cards;
        }),
        catchError(() => EMPTY),
        takeUntilDestroyed(this.destroyRef)
      )
      .subscribe();
  }
}
