import { ChangeDetectionStrategy, Component, DestroyRef, computed, inject, signal } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { map } from 'rxjs/operators';
import { CommunityPartnersStore } from './services/community-partners.store';

@Component({
  selector: 'app-community-partner-detail',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './community-partner-detail.html',
  styleUrl: './community-partner-detail.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CommunityPartnerDetail {
  private readonly route = inject(ActivatedRoute);
  private readonly destroyRef = inject(DestroyRef);
  private readonly store = inject(CommunityPartnersStore);

  readonly partnerId = signal('');

  readonly partner = computed(() => {
    const id = this.partnerId();
    if (!id) {
      return undefined;
    }
    return this.store.findById(id);
  });

  constructor() {
    this.route.paramMap
      .pipe(
        map(params => params.get('id') ?? ''),
        takeUntilDestroyed(this.destroyRef),
      )
      .subscribe(id => this.partnerId.set(id));
  }

  onCardClick(): void {
    const current = this.partner();
    if (current) {
      this.store.trackCardClick(current.id, current.name);
    }
  }

  formatDate(date?: Date): string {
    if (!date) {
      return 'Pendiente';
    }

    return new Date(date).toLocaleDateString('es-ES');
  }
}
