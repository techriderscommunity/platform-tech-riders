import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { RouterLink } from '@angular/router';

export interface UiFeatureCardItem {
  icon?: string;
  title: string;
  description: string;
  points?: string[];
  badge?: string;
  ctaLabel?: string;
  ctaLink?: string;
  ctaHref?: string;
}

@Component({
  selector: 'app-ui-feature-cards',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './feature-cards.html',
  styleUrl: './feature-cards.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class UiFeatureCards {
  @Input() items: UiFeatureCardItem[] = [];
  @Input() singleLineTitles = false;
  @Input() columns: number | null = null;
}
