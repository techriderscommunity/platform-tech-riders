import { ChangeDetectionStrategy, Component, DestroyRef, OnInit, inject } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { catchError, EMPTY, tap } from 'rxjs';
import { RouterLink } from '@angular/router';
import { PublicContentService } from '@core/content/public-content.service';
import { JourneyStepItem, MetricItem } from '@core/content/public-content.models';
import { UiJourneySteps } from '@shared/ui/journey-steps/journey-steps';
import { UiMetricsStrip } from '@shared/ui/metrics-strip/metrics-strip';

@Component({
  selector: 'app-woman-tech',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [RouterLink, UiMetricsStrip, UiJourneySteps],
  templateUrl: './woman-tech.html',
  styleUrl: './woman-tech.scss'
})
export class WomanTech implements OnInit {
  private readonly publicContentService = inject(PublicContentService);
  private readonly destroyRef = inject(DestroyRef);

  metrics: MetricItem[] = [];
  journey: JourneyStepItem[] = [];

  ngOnInit(): void {
    this.publicContentService
      .getPublicContent()
      .pipe(
        tap((content) => {
          this.metrics = content.womanTech.metrics;
          this.journey = content.womanTech.journey;
        }),
        catchError(() => EMPTY),
        takeUntilDestroyed(this.destroyRef)
      )
      .subscribe();
  }
}
