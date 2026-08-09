import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { RouterLink } from '@angular/router';

export interface UiProgressCardItem {
  title: string;
  value?: string;
  detail: string;
  progress: number;
  status?: string;
  ctaLabel?: string;
  ctaLink?: string;
}

@Component({
  selector: 'app-ui-progress-cards',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './progress-cards.html',
  styleUrl: './progress-cards.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class UiProgressCards {
  @Input() items: UiProgressCardItem[] = [];
}
