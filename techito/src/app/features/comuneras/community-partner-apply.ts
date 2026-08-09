import { ChangeDetectionStrategy, Component, computed, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { UiButton } from '@shared/ui/button/button';
import { UiSelect, UiSelectOption } from '@shared/ui/select/select';
import { UiTextField } from '@shared/ui/text-field/text-field';
import { UiTextarea } from '@shared/ui/textarea/textarea';
import { CommunityPartnersStore } from './services/community-partners.store';
import { CommunityPartnerScope } from './models/community-partner.models';

@Component({
  selector: 'app-community-partner-apply',
  standalone: true,
  imports: [RouterLink, UiButton, UiSelect, UiTextField, UiTextarea],
  templateUrl: './community-partner-apply.html',
  styleUrl: './community-partner-apply.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CommunityPartnerApply {
  private readonly store = inject(CommunityPartnersStore);

  readonly saving = signal(false);
  readonly submitted = signal(false);
  readonly error = signal<string | null>(null);

  readonly name = signal('');
  readonly logoUrl = signal('');
  readonly website = signal('');
  readonly contactEmail = signal('');
  readonly contactName = signal('');
  readonly whoYouAre = signal('');
  readonly whatYouDo = signal('');
  readonly mission = signal('');
  readonly topicsRaw = signal('');
  readonly scope = signal<CommunityPartnerScope>('local');
  readonly linkedin = signal('');
  readonly instagram = signal('');
  readonly x = signal('');
  readonly youtube = signal('');
  readonly discord = signal('');
  readonly telegram = signal('');
  readonly motivation = signal('');
  readonly collaborationIdeas = signal('');

  readonly scopeOptions: UiSelectOption[] = [
    { label: 'Local', value: 'local' },
    { label: 'Nacional', value: 'national' },
    { label: 'Internacional', value: 'international' },
  ];

  readonly canSubmit = computed(() => {
    return Boolean(
      this.name().trim() &&
      this.website().trim() &&
      this.contactEmail().trim() &&
      this.contactName().trim() &&
      this.whoYouAre().trim() &&
      this.whatYouDo().trim() &&
      this.mission().trim() &&
      this.motivation().trim() &&
      this.collaborationIdeas().trim(),
    );
  });

  submit(event: Event): void {
    event.preventDefault();

    if (!this.canSubmit()) {
      this.error.set('Completa los campos obligatorios antes de enviar la solicitud.');
      return;
    }

    if (!this.isValidEmail(this.contactEmail())) {
      this.error.set('El email de contacto no tiene un formato válido.');
      return;
    }

    this.error.set(null);
    this.saving.set(true);

    try {
      this.store.submitApplication({
        name: this.name().trim(),
        logoUrl: this.logoUrl().trim() || 'assets/logo.png',
        website: this.website().trim(),
        contactEmail: this.contactEmail().trim(),
        contactName: this.contactName().trim(),
        whoYouAre: this.whoYouAre().trim(),
        whatYouDo: this.whatYouDo().trim(),
        mission: this.mission().trim(),
        topics: this.topicsRaw()
          .split(',')
          .map(item => item.trim())
          .filter(Boolean),
        scope: this.scope(),
        linkedin: this.linkedin().trim() || undefined,
        instagram: this.instagram().trim() || undefined,
        x: this.x().trim() || undefined,
        youtube: this.youtube().trim() || undefined,
        discord: this.discord().trim() || undefined,
        telegram: this.telegram().trim() || undefined,
        motivation: this.motivation().trim(),
        collaborationIdeas: this.collaborationIdeas().trim(),
      });

      this.submitted.set(true);
    } catch (error) {
      this.error.set(error instanceof Error ? error.message : 'No se pudo registrar la solicitud.');
    } finally {
      this.saving.set(false);
    }
  }

  onScopeChange(value: string): void {
    if (value === 'local' || value === 'national' || value === 'international') {
      this.scope.set(value);
    }
  }

  private isValidEmail(email: string): boolean {
    return /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email.trim());
  }
}
