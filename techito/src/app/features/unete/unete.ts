import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { environment } from '@env/environment';
import { UiTextField  } from '@shared/ui/text-field/text-field';
import { UiTextarea  } from '@shared/ui/textarea/textarea';
import { UiButton  } from '@shared/ui/button/button';
import { UiSelect, UiSelectOption } from '@shared/ui/select/select';
import { UiMetricsStrip } from '@shared/ui/metrics-strip/metrics-strip';

type IntakeType = 'member' | 'ambassador' | 'session';

@Component({
  selector: 'app-unete',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [CommonModule, FormsModule, RouterLink, UiTextField, UiTextarea, UiButton, UiSelect, UiMetricsStrip],
  templateUrl: './unete.html',
  styleUrl: './unete.scss'
})
export class Unete {
  private readonly http = inject(HttpClient);
  private readonly router = inject(Router);
  private readonly baseUrl = environment.apiUrl;

  readonly intakeOptions: UiSelectOption[] = [
    { label: 'Quiero unirme como miembro', value: 'member' },
    { label: 'Quiero solicitar ser Ambassador', value: 'ambassador' },
    { label: 'Quiero solicitar una sesión', value: 'session' }
  ];

  readonly joinMetrics = [
    { value: '13', label: 'Años de comunidad', icon: '📅' },
    { value: '1300+', label: 'Recursos compartidos', icon: '📚' },
    { value: '80+', label: 'Sesiones #FPTOUR', icon: '🎤' },
    { value: '1500+', label: 'Alumnos impactados', icon: '👥' },
  ];

  // Estado de flujo activo
  flujoActivo = signal<IntakeType>('member');

  // Estado del formulario
  formulario = signal({
    nombre: '',
    email: '',
    requestType: 'member' as IntakeType,
    communityRole: 'member',
    organizacion: '',
    motivacion: '',
    sessionTopic: '',
  });

  enviado = signal(false);
  loading = signal(false);
  error = signal('');
  successMessage = signal('');

  seleccionarFlujo(requestType: IntakeType) {
    this.flujoActivo.set(requestType);
    this.formulario.update(current => ({
      ...current,
      requestType,
      communityRole: this.getCommunityRoleForRequest(requestType)
    }));
  }

  seleccionarFlujoDesdeSelect(value: string) {
    if (value === 'member' || value === 'ambassador' || value === 'session') {
      this.seleccionarFlujo(value);
    }
  }

  flowTitle(): string {
    switch (this.flujoActivo()) {
      case 'ambassador':
        return 'Comparte tu experiencia con impacto';
      case 'session':
        return 'Traemos una sesion adaptada a tu contexto';
      default:
        return 'Empieza por donde estes hoy';
    }
  }

  flowHint(): string {
    switch (this.flujoActivo()) {
      case 'ambassador':
        return 'Si te apetece aportar, difundimos tu conocimiento en formatos que encajen contigo.';
      case 'session':
        return 'Nos cuentas tu necesidad y co-disenamos una sesion util para tu grupo.';
      default:
        return 'No necesitas tenerlo todo claro. Te ayudamos a encontrar tu camino dentro de la comunidad.';
    }
  }

  onSubmit(event: Event) {
    event.preventDefault();
    this.loading.set(true);
    this.error.set('');

    const payload = {
      name: this.formulario().nombre,
      email: this.formulario().email,
      requestType: this.formulario().requestType,
      communityRole: this.formulario().communityRole,
      audience: null,
      organization: this.formulario().organizacion || null,
      motivation: this.formulario().motivacion,
      sessionTopic: this.formulario().requestType === 'session' ? this.formulario().sessionTopic || null : null,
      sessionFormat: this.formulario().requestType === 'session' ? 'por-definir' : null,
    };

    this.http.post(`${this.baseUrl}/join`, payload).subscribe({
      next: () => {
        this.persistLocalDraft(payload);
        this.loading.set(false);
        this.enviado.set(true);
        this.successMessage.set(this.buildSuccessMessage(this.formulario().requestType));
        this.formulario.set({
          nombre: '',
          email: '',
          requestType: this.flujoActivo(),
          communityRole: this.getCommunityRoleForRequest(this.flujoActivo()),
          organizacion: '',
          motivacion: '',
          sessionTopic: '',
        });
        setTimeout(() => this.enviado.set(false), 3000);
      },
      error: (err) => {
        this.loading.set(false);
        this.error.set('Oops, algo salió mal. ¡Reinténtalo en un momento! 🚀');
        console.error(err);
      }
    });
  }

  updateNombre(value: string) {
    this.formulario.update(f => ({ ...f, nombre: value }));
  }

  updateEmail(value: string) {
    this.formulario.update(f => ({ ...f, email: value }));
  }

  updateOrganizacion(value: string) {
    this.formulario.update(f => ({ ...f, organizacion: value }));
  }

  updateSessionTopic(value: string) {
    this.formulario.update(f => ({ ...f, sessionTopic: value }));
  }

  updateMotivacion(value: string) {
    this.formulario.update(f => ({ ...f, motivacion: value }));
  }

  private getCommunityRoleForRequest(requestType: IntakeType): string {
    switch (requestType) {
      case 'ambassador':
        return 'ambassador';
      case 'session':
        return 'member';
      default:
        return 'member';
    }
  }

  irAlPortalAmbassador() {
    this.router.navigateByUrl('/intranet/ambassador/portal');
  }

  private persistLocalDraft(payload: {
    name: string;
    email: string;
    requestType: IntakeType;
    communityRole: string;
    audience: string | null;
    organization: string | null;
    motivation: string;
    sessionTopic: string | null;
    sessionFormat: string | null;
  }) {
    if (typeof localStorage === 'undefined') {
      return;
    }

    if (payload.requestType === 'ambassador') {
      localStorage.setItem('techriders.mvp.ambassadorDraft', JSON.stringify(payload));
    }

    if (payload.requestType === 'member') {
      localStorage.setItem('techriders.mvp.memberProfile', JSON.stringify({
        nombre: payload.name,
        email: payload.email,
        bio: payload.motivation,
        intereses: 'por-definir',
        organizacion: payload.organization,
        communityRole: payload.communityRole,
      }));
    }
  }

  private buildSuccessMessage(requestType: IntakeType): string {
    if (requestType === 'ambassador') {
      return 'Solicitud enviada. Si ya tienes acceso a la intranet, el portal Ambassador quedará precargado con tu borrador.';
    }

    if (requestType === 'session') {
      return 'Solicitud enviada. El equipo revisará el contexto y propondrá coordinación para la sesión.';
    }

      return 'Solicitud enviada. Tu perfil de member ha quedado registrado.';
  }
}


