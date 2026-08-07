import { ChangeDetectionStrategy, Component, signal, inject, DestroyRef } from '@angular/core';
import { NgClass, TitleCasePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { environment } from '@env/environment';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { tap, catchError } from 'rxjs/operators';
import { of } from 'rxjs';
import { UiTextField  } from '@shared/ui/text-field/text-field';
import { UiTextarea  } from '@shared/ui/textarea/textarea';
import { UiFileInput  } from '@shared/ui/file-input/file-input';

interface CharlaItem {
  titulo: string;
  fecha: string;
  centro: string;
  estado: string;
  valoracion: number | null;
}

interface ValoracionItem {
  centro: string;
  texto: string;
}

@Component({
  selector: 'app-perfil-usuario',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [FormsModule, TitleCasePipe, NgClass, UiTextField, UiTextarea, UiFileInput],
  templateUrl: './perfil-usuario.html',
  styleUrl: './perfil-usuario.scss'
})
export class PerfilUsuario {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = environment.apiUrl;
  private readonly destroyRef = inject(DestroyRef);

  readonly loading = signal(true);
  readonly nombre = signal('');
  readonly email = signal('');
  readonly feedback = signal('');
  readonly foto = signal('../../assets/user.svg');
  readonly charlas = signal<CharlaItem[]>([]);
  readonly valoraciones = signal<ValoracionItem[]>([]);

  readonly mostrarModalFoto = signal(false);
  readonly fotoPreview = signal<string | null>(null);
  fotoFile: File | null = null;

  constructor() {
    this.loadProfile();
  }

  private loadProfile() {
    this.http.get(`${this.baseUrl}/perfil`)
      .pipe(
        tap((data: any) => {
          this.nombre.set(data.nombre);
          this.email.set(data.email);
          this.feedback.set(data.feedback);
          if (data.foto) this.foto.set(data.foto);
          this.charlas.set(data.charlas);
          this.valoraciones.set(data.valoraciones);
          this.loading.set(false);
        }),
        catchError(() => {
          this.loading.set(false);
          return of(null);
        }),
        takeUntilDestroyed(this.destroyRef)
      )
      .subscribe();
  }

  guardarCambios() {
    const data = {
      nombre: this.nombre(),
      email: this.email(),
      feedback: this.feedback(),
      foto: this.fotoFile ? 'nuevo-archivo' : null
    };
    this.http.put(`${this.baseUrl}/perfil`, data).subscribe({
      next: () => alert('Cambios guardados'),
      error: () => alert('Error al guardar cambios')
    });
  }

  abrirModalFoto() {
    this.mostrarModalFoto.set(true);
    this.fotoPreview.set(null);
    this.fotoFile = null;
  }

  cerrarModalFoto() {
    this.mostrarModalFoto.set(false);
    this.fotoPreview.set(null);
    this.fotoFile = null;
  }

  onFotoSeleccionada(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files[0]) {
      this.fotoFile = input.files[0];
      const reader = new FileReader();
      reader.onload = (e: ProgressEvent<FileReader>) => {
        this.fotoPreview.set(e.target?.result as string);
      };
      reader.readAsDataURL(this.fotoFile);
    }
  }

  onFotoSeleccionadaArchivo(file: File | null) {
    if (!file) {
      this.fotoFile = null;
      this.fotoPreview.set(null);
      return;
    }

    this.fotoFile = file;
    const reader = new FileReader();
    reader.onload = (e: ProgressEvent<FileReader>) => {
      this.fotoPreview.set(e.target?.result as string);
    };
    reader.readAsDataURL(file);
  }

  guardarFoto() {
    if (this.fotoPreview()) {
      this.foto.set(this.fotoPreview()!);
      alert('Foto cambiada');
    }
    this.cerrarModalFoto();
  }

  cambiarPassword() {
    alert('Función de cambiar contraseña');
  }
}


