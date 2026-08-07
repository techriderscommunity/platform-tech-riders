import { ChangeDetectionStrategy, Component, ViewChild, ElementRef, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { Renderer2 } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '@env/environment';
import { UiTextField  } from '@shared/ui/text-field/text-field';
import { UiTextarea  } from '@shared/ui/textarea/textarea';
import { UiButton  } from '@shared/ui/button/button';
import { UiSelect, UiSelectOption } from '@shared/ui/select/select';

type IntakeType = 'member' | 'ambassador' | 'session';

@Component({
  selector: 'app-unete',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [CommonModule, FormsModule, RouterLink, UiTextField, UiTextarea, UiButton, UiSelect],
  templateUrl: './unete.html',
  styleUrl: './unete.scss'
})
export class Unete {
  @ViewChild('carruselTrack', { static: false }) carruselTrack!: ElementRef<HTMLDivElement>;

  private readonly renderer = inject(Renderer2);
  private readonly http = inject(HttpClient);
  private readonly router = inject(Router);
  private readonly baseUrl = environment.apiUrl;

  readonly intakeOptions: UiSelectOption[] = [
    { label: 'Quiero unirme como miembro', value: 'member' },
    { label: 'Quiero solicitar ser Ambassador', value: 'ambassador' },
    { label: 'Quiero solicitar una sesión', value: 'session' }
  ];

  readonly audienceOptions: UiSelectOption[] = [
    { label: 'Soy estudiante o perfil junior', value: 'junior' },
    { label: 'Soy profesional senior', value: 'senior' },
    { label: 'Soy profesor u orientador', value: 'educator' },
    { label: 'Represento a un centro educativo', value: 'centre' },
    { label: 'Represento a una empresa', value: 'company' }
  ];

  // Estado de flujo activo
  flujoActivo = signal<IntakeType>('member');

  // Estado del formulario
  formulario = signal({
    nombre: '',
    email: '',
    requestType: 'member' as IntakeType,
    communityRole: 'member',
    audience: 'junior',
    organizacion: '',
    motivacion: '',
    sessionTopic: '',
    sessionFormat: 'tech-talk'
  });

  enviado = signal(false);
  loading = signal(false);
  error = signal('');
  successMessage = signal('');

  scrollCarrusel(direction: number) {
    const track = this.carruselTrack?.nativeElement;
    if (track) {
      const imgs = track.querySelectorAll('img');
      if (imgs.length < 2) return;
      const img = imgs[0];
      const scrollAmount = img.clientWidth + 24;
      if (direction > 0) {
        track.scrollBy({ left: scrollAmount, behavior: 'smooth' });
        setTimeout(() => {
          if (track.scrollLeft + track.clientWidth >= track.scrollWidth - 2) {
            this.renderer.appendChild(track, imgs[0]);
            track.scrollLeft -= scrollAmount;
          }
        }, 350);
      } else {
        track.scrollBy({ left: -scrollAmount, behavior: 'smooth' });
        setTimeout(() => {
          if (track.scrollLeft <= 2) {
            this.renderer.insertBefore(track, imgs[imgs.length - 1], imgs[0]);
            track.scrollLeft += scrollAmount;
          }
        }, 350);
      }
    }
  }

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

  onSubmit(event: Event) {
    event.preventDefault();
    this.loading.set(true);
    this.error.set('');

    const payload = {
      name: this.formulario().nombre,
      email: this.formulario().email,
      requestType: this.formulario().requestType,
      communityRole: this.formulario().communityRole,
      audience: this.formulario().audience,
      organization: this.formulario().organizacion || null,
      motivation: this.formulario().motivacion,
      sessionTopic: this.formulario().requestType === 'session' ? this.formulario().sessionTopic || null : null,
      sessionFormat: this.formulario().requestType === 'session' ? this.formulario().sessionFormat || null : null,
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
          audience: 'junior',
          organizacion: '',
          motivacion: '',
          sessionTopic: '',
          sessionFormat: 'tech-talk'
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

  updateAudience(value: string) {
    this.formulario.update(f => ({ ...f, audience: value }));
  }

  updateSessionTopic(value: string) {
    this.formulario.update(f => ({ ...f, sessionTopic: value }));
  }

  updateSessionFormat(value: string) {
    this.formulario.update(f => ({ ...f, sessionFormat: value }));
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
    audience: string;
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
        intereses: payload.audience,
        organizacion: payload.organization,
        communityRole: payload.communityRole,
      }));
    }
  }

  private buildSuccessMessage(requestType: IntakeType): string {
    if (requestType === 'ambassador') {
      return 'Solicitud enviada. Si ya tienes acceso a intranet, el portal Ambassador quedará precargado con tu borrador.';
    }

    if (requestType === 'session') {
      return 'Solicitud enviada. El equipo revisará el contexto y propondrá coordinación para la sesión.';
    }

    return 'Solicitud enviada. Tu perfil de member queda registrado como base del MVP.';
  }
}


