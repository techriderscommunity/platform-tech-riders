import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { RouterLink } from '@angular/router';

export interface UiResourceCardItem {
  mode?: string;
  title: string;
  summary: string;
  meta?: string;
  tags?: string[];
  ctaLabel?: string;
  ctaLink?: string;
  ctaHref?: string;
}

@Component({
  selector: 'app-ui-resource-cards',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './resource-cards.html',
  styleUrl: './resource-cards.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class UiResourceCards {
  @Input() items: UiResourceCardItem[] = [];
}
