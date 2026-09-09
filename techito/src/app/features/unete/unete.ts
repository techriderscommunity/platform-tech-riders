import { ChangeDetectionStrategy, Component, DestroyRef, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { catchError, EMPTY, tap } from 'rxjs';
import { PublicContentService } from '@core/content/public-content.service';
import { MetricItem } from '@core/content/public-content.models';
import { UiTextField  } from '@shared/ui/text-field/text-field';
import { UiTextarea  } from '@shared/ui/textarea/textarea';
import { UiButton  } from '@shared/ui/button/button';
import { UiSelect, UiSelectOption } from '@shared/ui/select/select';
import { UiModal } from '@shared/ui/modal/modal';
import { IntakeType, JoinRequestPayload } from './models/unete.models';
import { UneteIntakeService } from './services/unete-intake.service';

@Component({
  selector: 'app-unete',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [CommonModule, FormsModule, UiTextField, UiTextarea, UiButton, UiSelect, UiModal],
  templateUrl: './unete.html',
  styleUrl: './unete.scss'
})
export class Unete implements OnInit {
  private readonly uneteIntakeService = inject(UneteIntakeService);
  private readonly publicContentService = inject(PublicContentService);
  private readonly destroyRef = inject(DestroyRef);

  intakeOptions: UiSelectOption[] = [];

  joinMetrics: MetricItem[] = [];

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
  showJoinModal = signal(false);

  ngOnInit(): void {
    this.publicContentService
      .getPublicContent()
      .pipe(
        tap((content) => {
          this.joinMetrics = content.join.metrics;
          this.intakeOptions = content.join.intakeOptions;
        }),
        catchError(() => EMPTY),
        takeUntilDestroyed(this.destroyRef)
      )
      .subscribe();
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

  abrirSolicitud(requestType: IntakeType = 'member') {
    this.seleccionarFlujo(requestType);
    this.error.set('');
    this.enviado.set(false);
    this.showJoinModal.set(true);
  }

  cerrarSolicitud() {
    if (this.loading()) {
      return;
    }

    this.showJoinModal.set(false);
    this.error.set('');
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

    const payload: JoinRequestPayload = {
      name: this.formulario().nombre,
      email: this.formulario().email,
      requestType: this.formulario().requestType,
      communityRole: this.formulario().communityRole,
      audience: null,
      organization: this.formulario().organizacion || null,
      motivation: this.formulario().motivacion,
      sessionTopic: null,
      sessionFormat: null,
    };

    this.uneteIntakeService.submitJoinRequest(payload).subscribe({
      next: () => {
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
        setTimeout(() => {
          this.enviado.set(false);
          this.showJoinModal.set(false);
        }, 2400);
      },
      error: () => {
        this.loading.set(false);
        this.error.set('No se pudo enviar la solicitud. Reinténtalo en un momento.');
      }
    });
  }

  updateNombre(value: string) {
    this.formulario.update(f => ({ ...f, nombre: value }));
  }

  updateEmail(value: string) {
    this.formulario.update(f => ({ ...f, email: value }));
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


