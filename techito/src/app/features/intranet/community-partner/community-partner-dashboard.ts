import { ChangeDetectionStrategy, Component, computed, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { CommunityPartnersStore } from '../../comuneras/services/community-partners.store';

@Component({
  selector: 'app-community-partner-dashboard',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './community-partner-dashboard.html',
  styleUrl: './community-partner-dashboard.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CommunityPartnerDashboard {
  private readonly partnersStore = inject(CommunityPartnersStore);

  readonly approvedPartners = computed(() => this.partnersStore.approvedPartners().slice(0, 6));
}
