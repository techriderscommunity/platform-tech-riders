import { ChangeDetectionStrategy, Component, computed, signal, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { AuthService } from '@core/auth/auth.service';
import { catchError, finalize, of, tap } from 'rxjs';
import { UiTextField  } from '@shared/ui/text-field/text-field';
import { UiSelect, UiSelectOption } from '@shared/ui/select/select';
import { UiTextarea  } from '@shared/ui/textarea/textarea';
import { UiButton } from '@shared/ui/button/button';
import { MemberProfileApi, MemberProfileService } from './services/member-profile.service';
import { CapabilityRequestService } from '@core/capability/capability-request.service';
import { REQUESTABLE_CAPABILITIES } from '@core/capability/capability-request.models';
import { PreferencesService } from '@core/preferences/preferences.service';
import { PreferenceDimension } from '@core/preferences/preferences.models';
import { ConsentsService } from '@core/consents/consents.service';
import { ConsentPurposeItem } from '@core/consents/consents.models';

@Component({
  selector: 'app-perfil-usuario',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [FormsModule, RouterLink, UiTextField, UiSelect, UiTextarea, UiButton],
  templateUrl: './perfil-usuario.html',
  styleUrl: './perfil-usuario.scss'
})
export class PerfilUsuario {
  private readonly authService = inject(AuthService);
  private readonly memberProfileService = inject(MemberProfileService);
  private readonly capabilityRequestService = inject(CapabilityRequestService);
  private readonly preferencesService = inject(PreferencesService);
  private readonly consentsService = inject(ConsentsService);

  readonly audienceOptions: UiSelectOption[] = [
    { label: 'Estudiante Tech', value: 'student' },
    { label: 'Profesional Tech', value: 'professional' },
    { label: 'Profesor / orientador', value: 'educator' },
    { label: 'Otro perfil comunitario', value: 'community' }
  ];

  readonly loading = signal(false);
  readonly success = signal<string | null>(null);
  readonly nombre = signal('');
  readonly email = signal('');
  readonly bio = signal('');
  readonly intereses = signal('FP Tour, eventos, comunidad y aprendizaje práctico');
  readonly audience = signal('student');
  readonly communityRole = signal('member');
  readonly organizacion = signal('');

  readonly currentUserName = computed(() => this.authService.user()?.name || this.nombre() || 'Miembro Tech Riders');
  readonly currentRoles = computed(() => this.authService.user()?.roles ?? []);

  readonly requestableRoles = computed(() =>
    REQUESTABLE_CAPABILITIES.filter(item => !this.currentRoles().includes(item.role)),
  );
  readonly roleRequestOptions = computed<UiSelectOption[]>(() =>
    this.requestableRoles().map(item => ({ label: item.label, value: item.role })),
  );
  readonly selectedRoleRequest = signal<string>('');
  readonly roleRequestSending = signal(false);
  readonly roleRequestFeedback = signal<string | null>(null);

  readonly preferenceDimensions = signal<PreferenceDimension[]>([]);
  readonly selectedPreferenceValueIds = signal<Set<string>>(new Set());
  readonly preferencesSaving = signal(false);
  readonly preferencesFeedback = signal<string | null>(null);

  readonly consentPurposes = signal<ConsentPurposeItem[]>([]);
  readonly consentUpdatingCode = signal<string | null>(null);
  readonly consentsFeedback = signal<string | null>(null);

  readonly proximasActividades = signal([
    { titulo: 'Calendario de comunidad', detalle: 'Revisa sesiones y eventos próximos desde intranet.' },
    { titulo: 'Solicitud Ambassador', detalle: 'Puedes iniciar o continuar tu paso a rol activo cuando tenga sentido.' },
    { titulo: 'Conocimiento compartido', detalle: 'El banco de conocimiento llegará en la siguiente release del roadmap.' },
  ]);

  constructor() {
    this.hydrateMemberProfile();
    this.selectedRoleRequest.set(this.requestableRoles()[0]?.role ?? '');
    this.loadPreferences();
    this.loadConsents();
  }

  guardarCambios() {
    if (!this.authService.user()) {
      this.success.set('Debes iniciar sesión para guardar el perfil.');
      return;
    }

    const payload = {
      userKey: this.resolveUserKey(),
      name: this.nombre(),
      email: this.email(),
      bio: this.bio(),
      interests: this.intereses(),
      audience: this.audience(),
      communityRole: this.communityRole(),
      organization: this.organizacion(),
    };

    this.memberProfileService.saveProfile(payload)
      .pipe(
        tap(() => {
          this.success.set('Perfil member guardado en backend.');
        }),
        catchError(() => {
          this.success.set('No se pudo guardar el perfil. Intenta de nuevo.');
          return of(null);
        })
      )
      .subscribe();
  }

  updateNombre(value: string) {
    this.nombre.set(value);
  }

  updateEmail(value: string) {
    this.email.set(value);
  }

  updateBio(value: string) {
    this.bio.set(value);
  }

  updateIntereses(value: string) {
    this.intereses.set(value);
  }

  updateAudience(value: string) {
    this.audience.set(value);
  }

  updateOrganizacion(value: string) {
    this.organizacion.set(value);
  }

  updateSelectedRoleRequest(value: string) {
    this.selectedRoleRequest.set(value);
  }

  solicitarRol() {
    const target = this.requestableRoles().find(item => item.role === this.selectedRoleRequest())
      ?? this.requestableRoles()[0];
    if (!target) {
      return;
    }

    this.roleRequestSending.set(true);
    this.roleRequestFeedback.set(null);
    this.capabilityRequestService.requestCapability(target.capabilityName)
      .pipe(
        tap(() => this.roleRequestFeedback.set(`Solicitud de ${target.label} enviada. Un admin/staff la revisará.`)),
        catchError((error) => {
          const message = error?.error?.Message ?? 'No se pudo enviar la solicitud.';
          this.roleRequestFeedback.set(message);
          return of(null);
        }),
        finalize(() => this.roleRequestSending.set(false)),
      )
      .subscribe();
  }

  private hydrateMemberProfile() {
    const currentUser = this.authService.user();
    this.nombre.set(currentUser?.name ?? '');
    this.email.set(currentUser?.email ?? '');

    if (!currentUser) {
      return;
    }

    this.memberProfileService.getProfile(this.resolveUserKey(), currentUser.email)
      .pipe(
        tap(profile => {
          this.nombre.set(profile.Name);
          this.email.set(profile.Email);
          this.bio.set(profile.Bio);
          this.intereses.set(profile.Interests);
          this.audience.set(profile.Audience);
          this.communityRole.set(profile.CommunityRole);
          this.organizacion.set(profile.Organization);
        }),
        catchError(() => of(null))
      )
      .subscribe();
  }

  private resolveUserKey(): string {
    const email = this.authService.user()?.email;
    if (!email) {
      throw new Error('Authenticated user email is required.');
    }

    return email;
  }

  isPreferenceSelected(valueId: string): boolean {
    return this.selectedPreferenceValueIds().has(valueId);
  }

  togglePreference(valueId: string, checked: boolean) {
    const next = new Set(this.selectedPreferenceValueIds());
    if (checked) {
      next.add(valueId);
    } else {
      next.delete(valueId);
    }
    this.selectedPreferenceValueIds.set(next);
  }

  guardarPreferencias() {
    this.preferencesSaving.set(true);
    this.preferencesFeedback.set(null);
    this.preferencesService.setMine(Array.from(this.selectedPreferenceValueIds()))
      .pipe(
        tap(() => this.preferencesFeedback.set('Preferencias guardadas.')),
        catchError(() => {
          this.preferencesFeedback.set('No se pudieron guardar las preferencias.');
          return of(null);
        }),
        finalize(() => this.preferencesSaving.set(false)),
      )
      .subscribe();
  }

  toggleConsent(code: string, granted: boolean) {
    this.consentUpdatingCode.set(code);
    this.consentsFeedback.set(null);
    const request$ = granted ? this.consentsService.grant(code) : this.consentsService.withdraw(code);
    request$
      .pipe(
        tap(() => {
          this.consentPurposes.update(items =>
            items.map(item => item.code === code ? { ...item, granted, status: granted ? 'Otorgado' : 'Retirado' } : item),
          );
        }),
        catchError(() => {
          this.consentsFeedback.set('No se pudo actualizar el consentimiento.');
          return of(null);
        }),
        finalize(() => this.consentUpdatingCode.set(null)),
      )
      .subscribe();
  }

  private loadPreferences() {
    this.preferencesService.getCatalog()
      .pipe(
        tap(dimensions => this.preferenceDimensions.set(dimensions)),
        catchError(() => of(null)),
      )
      .subscribe();

    this.preferencesService.getMine()
      .pipe(
        tap(ids => this.selectedPreferenceValueIds.set(new Set(ids))),
        catchError(() => of(null)),
      )
      .subscribe();
  }

  private loadConsents() {
    this.consentsService.getMyConsentPurposes()
      .pipe(
        tap(items => this.consentPurposes.set(items)),
        catchError(() => of(null)),
      )
      .subscribe();
  }
}


