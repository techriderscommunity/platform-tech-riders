import { ChangeDetectionStrategy, Component, DestroyRef, OnInit, inject } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { catchError, EMPTY, tap } from 'rxjs';
import { RouterLink } from '@angular/router';
import { PublicContentService } from '@core/content/public-content.service';
import { ProgressCardItem, ResourceCardItem } from '@core/content/public-content.models';
import { UiProgressCards } from '@shared/ui/progress-cards/progress-cards';
import { UiResourceCards } from '@shared/ui/resource-cards/resource-cards';

@Component({
  selector: 'app-oportunidades',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [RouterLink, UiProgressCards, UiResourceCards],
  templateUrl: './oportunidades.html',
  styleUrl: './oportunidades.scss'
})
export class Oportunidades implements OnInit {
  private readonly publicContentService = inject(PublicContentService);
  private readonly destroyRef = inject(DestroyRef);

  tracks: ProgressCardItem[] = [];
  resources: ResourceCardItem[] = [];

  ngOnInit(): void {
    this.publicContentService
      .getPublicContent()
      .pipe(
        tap((content) => {
          this.tracks = content.opportunities.tracks;
          this.resources = content.opportunities.resources;
        }),
        catchError(() => EMPTY),
        takeUntilDestroyed(this.destroyRef)
      )
      .subscribe();
  }
}
