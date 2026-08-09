import { ChangeDetectionStrategy, Component, Input } from '@angular/core';

export interface UiProfileCardItem {
  name: string;
  role: string;
  imageSrc: string;
  imageAlt: string;
  badge?: string;
}

@Component({
  selector: 'app-ui-profile-cards',
  standalone: true,
  templateUrl: './profile-cards.html',
  styleUrl: './profile-cards.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class UiProfileCards {
  @Input() items: UiProfileCardItem[] = [];
}
