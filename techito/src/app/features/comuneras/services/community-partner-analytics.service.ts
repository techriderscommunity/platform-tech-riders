import { Injectable } from '@angular/core';

type AnalyticsEventName =
  | 'community_partner_viewed'
  | 'community_partner_card_clicked'
  | 'community_partner_apply_clicked'
  | 'community_partner_application_submitted'
  | 'community_partner_approved';

@Injectable({ providedIn: 'root' })
export class CommunityPartnerAnalyticsService {
  track(event: AnalyticsEventName, payload: Record<string, unknown> = {}): void {
    // Local instrumentation until central analytics endpoint is connected.
    console.info('[analytics]', event, payload);
  }
}
