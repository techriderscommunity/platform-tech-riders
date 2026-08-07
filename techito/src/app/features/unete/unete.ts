import { ChangeDetectionStrategy, Component, ViewChild, ElementRef, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { Renderer2 } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '@env/environment';
import { UiTextField  } from '@shared/ui/text-field/text-field';
import { UiTextarea  } from '@shared/ui/textarea/textarea';
import { UiButton  } from '@shared/ui/button/button';

type RolType = 'candidato' | 'centro';

@Component({
  selector: 'app-unete',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [CommonModule, FormsModule, RouterLink, UiTextField, UiTextarea, UiButton],
  templateUrl: './unete.html',
  styleUrl: './unete.scss'
})
export class Unete {
  @ViewChild('carruselTrack', { static: false }) carruselTrack!: ElementRef<HTMLDivElement>;

  private readonly renderer = inject(Renderer2);
  private readonly http = inject(HttpClient);
  private readonly baseUrl = environment.apiUrl;

  // Estado de pestaña activa
  rolActivo = signal<RolType>('candidato');

  // Estado del formulario
  formulario = signal({
    nombre: '',
    email: '',
    rol: 'candidato' as RolType,
    organizacion: '',
    motivacion: ''
  });

  enviado = signal(false);
  loading = signal(false);
  error = signal('');

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

  seleccionarRol(rol: RolType) {
    this.rolActivo.set(rol);
    this.formulario.update(f => ({ ...f, rol }));
  }

  onSubmit(event: Event) {
    event.preventDefault();
    this.loading.set(true);
    this.error.set('');

    const payload = {
      name: this.formulario().nombre,
      email: this.formulario().email,
      role: this.formulario().rol,
      organization: this.formulario().organizacion || null,
      motivation: this.formulario().motivacion,
    };

    this.http.post(`${this.baseUrl}/join`, payload).subscribe({
      next: () => {
        this.loading.set(false);
        this.enviado.set(true);
        this.formulario.set({
          nombre: '',
          email: '',
          rol: this.rolActivo(),
          organizacion: '',
          motivacion: ''
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

  updateMotivacion(value: string) {
    this.formulario.update(f => ({ ...f, motivacion: value }));
  }
}


