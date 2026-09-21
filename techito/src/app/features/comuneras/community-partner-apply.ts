import { ChangeDetectionStrategy, Component, computed, inject, signal } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { UiButton } from '@shared/ui/button/button';
import { UiSelect, UiSelectOption } from '@shared/ui/select/select';
import { UiTextField } from '@shared/ui/text-field/text-field';
import { UiTextarea } from '@shared/ui/textarea/textarea';
import { CommunityPartnerAnalyticsService } from './services/community-partner-analytics.service';
import { CommunityPartnerApplicationsService } from './services/community-partner-applications.service';
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
  private readonly applicationsService = inject(CommunityPartnerApplicationsService);
  private readonly analytics = inject(CommunityPartnerAnalyticsService);
  private readonly router = inject(Router);

  readonly saving = signal(false);
  readonly submitted = signal(false);
  readonly error = signal<string | null>(null);

  readonly name = signal('');
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
  readonly github = signal('');
  readonly motivation = signal('');
  readonly collaborationIdeas = signal('');

  readonly socialProfileUrls = {
    linkedin: 'https://www.linkedin.com/company/',
    instagram: 'https://www.instagram.com/',
    x: 'https://x.com/',
    youtube: 'https://www.youtube.com/@',
    github: 'https://github.com/',
  } as const;

  readonly linkedinPreview = computed(() => this.buildSocialUrl(this.socialProfileUrls.linkedin, this.linkedin()));
  readonly instagramPreview = computed(() => this.buildSocialUrl(this.socialProfileUrls.instagram, this.instagram()));
  readonly xPreview = computed(() => this.buildSocialUrl(this.socialProfileUrls.x, this.x()));
  readonly youtubePreview = computed(() => this.buildSocialUrl(this.socialProfileUrls.youtube, this.youtube()));
  readonly githubPreview = computed(() => this.buildSocialUrl(this.socialProfileUrls.github, this.github()));

  constructor() {
    this.router.navigate(['/'], {
      queryParams: {
        login: '1',
        authMode: 'register',
        returnUrl: '/intranet/community-partner',
      },
      replaceUrl: true,
    });
  }

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

    this.applicationsService.create({
        name: this.name().trim(),
        website: this.website().trim(),
        contactEmail: this.contactEmail().trim(),
        contactName: this.contactName().trim(),
        whoYouAre: this.whoYouAre().trim(),
        whatYouDo: this.whatYouDo().trim(),
        mission: this.mission().trim(),
        topics: this.topicsRaw().trim(),
        scope: this.scope(),
        linkedin: this.linkedin().trim() || undefined,
        instagram: this.instagram().trim() || undefined,
        x: this.x().trim() || undefined,
        youtube: this.youtube().trim() || undefined,
        github: this.github().trim() || undefined,
        motivation: this.motivation().trim(),
        collaborationIdeas: this.collaborationIdeas().trim(),
      })
      .subscribe({
        next: () => {
          this.submitted.set(true);
          this.analytics.track('community_partner_application_submitted', {
            name: this.name().trim(),
            scope: this.scope(),
          });
          this.saving.set(false);
        },
        error: (error: { status?: number; error?: { detail?: string } }) => {
          this.error.set(error.status === 409
            ? 'Ya existe una solicitud con esos datos.'
            : error.error?.detail ?? 'No se pudo registrar la solicitud.');
          this.saving.set(false);
        },
      });
  }

  onScopeChange(value: string): void {
    if (value === 'local' || value === 'national' || value === 'international') {
      this.scope.set(value);
    }
  }

  private isValidEmail(email: string): boolean {
    return /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email.trim());
  }

  private buildSocialUrl(baseUrl: string, identifier: string): string | null {
    const normalized = identifier.trim().replace(/^@/, '').replace(/^\/+|\/+$/g, '');
    return normalized ? `${baseUrl}${normalized}` : null;
  }
}
