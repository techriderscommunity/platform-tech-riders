import { HttpClient } from '@angular/common/http';
import { ChangeDetectionStrategy, Component, computed, signal, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { AuthService } from '@core/auth/auth.service';
import { environment } from '@env/environment';
import { catchError, of, tap } from 'rxjs';
import { UiTextField  } from '@shared/ui/text-field/text-field';
import { UiSelect, UiSelectOption } from '@shared/ui/select/select';
import { UiTextarea  } from '@shared/ui/textarea/textarea';
import { UiButton } from '@shared/ui/button/button';

interface MemberProfileApi {
  Name: string;
  Email: string;
  Bio: string;
  Interests: string;
  Audience: string;
  CommunityRole: string;
  Organization: string;
}

@Component({
  selector: 'app-perfil-usuario',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [FormsModule, RouterLink, UiTextField, UiSelect, UiTextarea, UiButton],
  templateUrl: './perfil-usuario.html',
  styleUrl: './perfil-usuario.scss'
})
export class PerfilUsuario {
  private readonly http = inject(HttpClient);
  private readonly authService = inject(AuthService);
  private readonly baseUrl = environment.apiUrl;

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

    this.http.put<MemberProfileApi>(`${this.baseUrl}/intranet/perfil`, payload)
      .pipe(
        tap(() => {
          if (typeof localStorage !== 'undefined') {
            localStorage.setItem('techriders.mvp.memberProfile', JSON.stringify({
              nombre: this.nombre(),
              email: this.email(),
              bio: this.bio(),
              intereses: this.intereses(),
              audience: this.audience(),
              communityRole: this.communityRole(),
              organizacion: this.organizacion(),
            }));
          }
          this.success.set('Perfil member guardado en backend MVP y en caché local.');
        }),
        catchError(() => {
          if (typeof localStorage !== 'undefined') {
            localStorage.setItem('techriders.mvp.memberProfile', JSON.stringify({
              nombre: this.nombre(),
              email: this.email(),
              bio: this.bio(),
              intereses: this.intereses(),
              audience: this.audience(),
              communityRole: this.communityRole(),
              organizacion: this.organizacion(),
            }));
          }
          this.success.set('Perfil guardado solo en caché local; el backend MVP no respondió.');
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

    this.http.get<MemberProfileApi>(`${this.baseUrl}/intranet/perfil`, {
      params: {
        userKey: this.resolveUserKey(),
        email: currentUser?.email ?? this.resolveFallbackEmail(),
      },
    })
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

    if (typeof localStorage === 'undefined') {
      return;
    }

    const stored = localStorage.getItem('techriders.mvp.memberProfile');
    if (!stored) {
      return;
    }

    try {
      const parsed = JSON.parse(stored) as {
        nombre?: string;
        email?: string;
        bio?: string;
        intereses?: string;
        audience?: string;
        communityRole?: string;
        organizacion?: string;
      };

      if (parsed.nombre) this.nombre.set(parsed.nombre);
      if (parsed.email) this.email.set(parsed.email);
      if (parsed.bio) this.bio.set(parsed.bio);
      if (parsed.intereses) this.intereses.set(parsed.intereses);
      if (parsed.audience) this.audience.set(parsed.audience);
      if (parsed.communityRole) this.communityRole.set(parsed.communityRole);
      if (parsed.organizacion) this.organizacion.set(parsed.organizacion);
    }
    catch {
      // Ignore malformed local MVP profile data.
    }
  }

  private resolveUserKey(): string {
    return this.authService.user()?.email || this.resolveFallbackEmail();
  }

  private resolveFallbackEmail(): string {
    if (typeof localStorage === 'undefined') {
      return 'local-user@techriders.local';
    }

    const stored = localStorage.getItem('techriders.mvp.memberProfile');
    if (!stored) {
      return 'local-user@techriders.local';
    }

    try {
      const parsed = JSON.parse(stored) as { email?: string };
      return parsed.email || 'local-user@techriders.local';
    }
    catch {
      return 'local-user@techriders.local';
    }
  }
}


