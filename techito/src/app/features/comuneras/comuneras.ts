import { ChangeDetectionStrategy, Component, computed, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { UiSelect, UiSelectOption } from '@shared/ui/select/select';
import { UiTextField } from '@shared/ui/text-field/text-field';
import { CommunityPartnersStore } from './services/community-partners.store';

@Component({
  selector: 'app-comuneras',
  standalone: true,
  imports: [RouterLink, UiSelect, UiTextField],
  templateUrl: './comuneras.html',
  styleUrl: './comuneras.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class Comuneras {
  private readonly store = inject(CommunityPartnersStore);
  readonly searchTerm = signal('');
  readonly scopeFilter = signal<'all' | 'local' | 'national' | 'international'>('all');

  readonly partners = this.store.approvedPartners;
  readonly subtitle =
    'Comunidades compañeras con las que compartimos camino, ideas y ganas de hacer cosas grandes.';

  readonly topicSummary = computed(() => {
    const topics = this.partners().flatMap(partner => partner.topics);
    return Array.from(new Set(topics)).slice(0, 7);
  });

  readonly filteredPartners = computed(() => {
    const term = this.searchTerm().trim().toLowerCase();
    const scope = this.scopeFilter();

    return this.partners().filter(partner => {
      const matchesScope = scope === 'all' || partner.scope === scope;
      const matchesTerm = !term
        || partner.name.toLowerCase().includes(term)
        || partner.shortDescription.toLowerCase().includes(term)
        || partner.cityOrScope.toLowerCase().includes(term)
        || partner.topics.some(topic => topic.toLowerCase().includes(term));

      return matchesScope && matchesTerm;
    });
  });

  readonly scopeOptions: UiSelectOption[] = [
    { label: 'Todos los ámbitos', value: 'all' },
    { label: 'Local', value: 'local' },
    { label: 'Nacional', value: 'national' },
    { label: 'Internacional', value: 'international' },
  ];

  onCardClick(id: string, name: string): void {
    this.store.trackCardClick(id, name);
  }

  onApplyClick(): void {
    this.store.trackApplyClick('directory');
  }

  onScopeFilterChange(value: string): void {
    if (value === 'all' || value === 'local' || value === 'national' || value === 'international') {
      this.scopeFilter.set(value);
    }
  }
}
