import { ChangeDetectionStrategy, Component, DestroyRef, OnInit, inject } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { catchError, EMPTY, tap } from 'rxjs';
import { RouterLink } from '@angular/router';
import { PublicContentService } from '@core/content/public-content.service';
import { FeatureCardItem, ProgressCardItem } from '@core/content/public-content.models';
import { UiFeatureCards } from '@shared/ui/feature-cards/feature-cards';
import { UiProgressCards } from '@shared/ui/progress-cards/progress-cards';

@Component({
  selector: 'app-empresas',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [RouterLink, UiFeatureCards, UiProgressCards],
  templateUrl: './empresas.html',
  styleUrl: './empresas.scss'
})
export class Empresas implements OnInit {
  private readonly publicContentService = inject(PublicContentService);
  private readonly destroyRef = inject(DestroyRef);

  valueCards: FeatureCardItem[] = [];
  processCards: ProgressCardItem[] = [];

  ngOnInit(): void {
    this.publicContentService
      .getPublicContent()
      .pipe(
        tap((content) => {
          this.valueCards = content.companies.valueCards;
          this.processCards = content.companies.processCards;
        }),
        catchError(() => EMPTY),
        takeUntilDestroyed(this.destroyRef)
      )
      .subscribe();
  }
}
