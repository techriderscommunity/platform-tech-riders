import { ChangeDetectionStrategy, Component, signal, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { environment } from '@env/environment';
import { UiTextField  } from '@shared/ui/text-field/text-field';
import { UiTextarea  } from '@shared/ui/textarea/textarea';
import { UiButton  } from '@shared/ui/button/button';

@Component({
  selector: 'app-contacto',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [FormsModule, UiTextField, UiTextarea, UiButton],
  templateUrl: './contacto.html',
  styleUrl: './contacto.scss'
})
export class Contacto {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = environment.apiUrl;

  contacto = signal({ nombre: '', email: '', mensaje: '' });
  sugerencia = signal({ nombre: '', texto: '' });

  enviadoContacto = signal(false);
  enviadoSugerencia = signal(false);
  loadingContacto = signal(false);
  loadingSugerencia = signal(false);
  errorContacto = signal('');
  errorSugerencia = signal('');

  onSubmitContacto(event: Event) {
    event.preventDefault();
    this.loadingContacto.set(true);
    this.errorContacto.set('');

    const payload = {
      name: this.contacto().nombre,
      email: this.contacto().email,
      message: this.contacto().mensaje,
    };

    this.http.post(`${this.baseUrl}/contact`, payload).subscribe({
      next: () => {
        this.loadingContacto.set(false);
        this.enviadoContacto.set(true);
        this.contacto.set({ nombre: '', email: '', mensaje: '' });
        setTimeout(() => this.enviadoContacto.set(false), 2200);
      },
      error: () => {
        this.loadingContacto.set(false);
        this.errorContacto.set('Error al enviar el mensaje');
      }
    });
  }

  onSubmitSugerencia(event: Event) {
    event.preventDefault();
    this.loadingSugerencia.set(true);
    this.errorSugerencia.set('');

    const payload = {
      name: this.sugerencia().nombre,
      text: this.sugerencia().texto,
    };

    this.http.post(`${this.baseUrl}/suggestions`, payload).subscribe({
      next: () => {
        this.loadingSugerencia.set(false);
        this.enviadoSugerencia.set(true);
        this.sugerencia.set({ nombre: '', texto: '' });
        setTimeout(() => this.enviadoSugerencia.set(false), 2200);
      },
      error: () => {
        this.loadingSugerencia.set(false);
        this.errorSugerencia.set('Error al enviar la sugerencia');
      }
    });
  }

  updateContactoNombre(value: string) {
    this.contacto.update(current => ({ ...current, nombre: value }));
  }

  updateContactoEmail(value: string) {
    this.contacto.update(current => ({ ...current, email: value }));
  }

  updateContactoMensaje(value: string) {
    this.contacto.update(current => ({ ...current, mensaje: value }));
  }

  updateSugerenciaNombre(value: string) {
    this.sugerencia.update(current => ({ ...current, nombre: value }));
  }

  updateSugerenciaTexto(value: string) {
    this.sugerencia.update(current => ({ ...current, texto: value }));
  }
}


