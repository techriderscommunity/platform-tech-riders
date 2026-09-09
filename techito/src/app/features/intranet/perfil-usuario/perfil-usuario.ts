import { ChangeDetectionStrategy, Component, computed, signal, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { AuthService } from '@core/auth/auth.service';
import { catchError, of, tap } from 'rxjs';
import { UiTextField  } from '@shared/ui/text-field/text-field';
import { UiSelect, UiSelectOption } from '@shared/ui/select/select';
import { UiTextarea  } from '@shared/ui/textarea/textarea';
import { UiButton } from '@shared/ui/button/button';
import { MemberProfileApi, MemberProfileService } from './services/member-profile.service';

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

  readonly audienceOptions: UiSelectOption[] = [
    { label: 'Estudiante / junior', value: 'junior' },
    { label: 'Profesional senior', value: 'senior' },
    { label: 'Profesor / orientador', value: 'educator' },
    { label: 'Centro educativo', value: 'centre' },
    { label: 'Empresa', value: 'company' }
  ];

  readonly loading = signal(false);
  readonly success = signal<string | null>(null);
  readonly nombre = signal('');
  readonly email = signal('');
  readonly bio = signal('');
  readonly intereses = signal('FP Tour, eventos, comunidad y aprendizaje práctico');
  readonly audience = signal('junior');
  readonly communityRole = signal('member');
  readonly organizacion = signal('');

  readonly currentUserName = computed(() => this.authService.user()?.name || this.nombre() || 'Miembro Tech Riders');
  readonly currentRoles = computed(() => this.authService.user()?.roles ?? []);
  readonly proximasActividades = signal([
    { titulo: 'Calendario de comunidad', detalle: 'Revisa sesiones y eventos próximos desde intranet.' },
    { titulo: 'Solicitud Ambassador', detalle: 'Puedes iniciar o continuar tu paso a rol activo cuando tenga sentido.' },
    { titulo: 'Conocimiento compartido', detalle: 'El banco de conocimiento llegará en la siguiente release del roadmap.' },
  ]);

  constructor() {
    this.hydrateMemberProfile();
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
}


