import { ChangeDetectionStrategy, Component, DestroyRef, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { catchError, of, tap } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { EmpresaService } from '../services/empresa.service';
import { Oferta } from '../models/empresa.models';
import { UiTextField  } from '@shared/ui/text-field/text-field';
import { UiTextarea  } from '@shared/ui/textarea/textarea';
import { UiSelect, UiSelectOption  } from '@shared/ui/select/select';
import { UiButton  } from '@shared/ui/button/button';

interface NuevaOfertaForm {
  titulo: string;
  descripcion: string;
  requisitos: string;
  ubicacion: string;
  salario: string;
  tipo: string;
  modalidad: string;
}

@Component({
  selector: 'app-gestionar-ofertas',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [RouterLink, FormsModule, UiTextField, UiTextarea, UiSelect, UiButton],
  templateUrl: './gestionar-ofertas.html',
  styleUrl: './gestionar-ofertas.scss'
})
export class GestionarOfertas {
  private readonly empresaService = inject(EmpresaService);
  private readonly destroyRef = inject(DestroyRef);

  readonly vistaActual = signal<'lista' | 'crear' | 'editar'>('lista');
  readonly loading = signal(false);
  readonly error = signal<string | null>(null);
  readonly ofertas = signal<Oferta[]>([]);

  readonly nuevaOferta = signal<NuevaOfertaForm>({
    titulo: '',
    descripcion: '',
    requisitos: '',
    ubicacion: '',
    salario: '',
    tipo: 'Jornada completa',
    modalidad: 'Remoto'
  });

  readonly modalidadOptions: UiSelectOption[] = [
    { label: 'Remoto', value: 'Remoto' },
    { label: 'Presencial', value: 'Presencial' },
    { label: 'Híbrido', value: 'Híbrido' }
  ];

  readonly tipoOptions: UiSelectOption[] = [
    { label: 'Jornada completa', value: 'Jornada completa' },
    { label: 'Media jornada', value: 'Media jornada' },
    { label: 'Prácticas', value: 'Prácticas' }
  ];

  constructor() {
    this.cargarOfertas();
  }

  cargarOfertas(): void {
    this.empresaService.getOfertas(1, 100)
      .pipe(
        tap(() => {
          this.loading.set(true);
          this.error.set(null);
        }),
        catchError(() => {
          this.error.set('No se pudieron cargar las ofertas.');
          return of({ items: [] as Oferta[] });
        }),
        takeUntilDestroyed(this.destroyRef)
      )
      .subscribe(result => {
        this.ofertas.set(result.items);
        this.loading.set(false);
      });
  }

  mostrarCrearOferta() {
    this.vistaActual.set('crear');
  }

  volverALista() {
    this.vistaActual.set('lista');
    this.resetFormulario();
  }

  guardarOferta() {
    const form = this.nuevaOferta();
    this.empresaService.createOferta({
      titulo: form.titulo,
      empresa: 'TechRiders',
      salario: form.salario,
      ubicacion: form.ubicacion,
      modalidad: form.modalidad
    }).subscribe(() => {
      this.cargarOfertas();
      this.volverALista();
    });
  }

  publicarOferta(_: string) {
    // TODO: conectar publicación real cuando exista endpoint dedicado.
  }

  archivarOferta(_: string) {
    // TODO: conectar archivado real cuando exista endpoint dedicado.
  }

  resetFormulario() {
    this.nuevaOferta.set({
      titulo: '',
      descripcion: '',
      requisitos: '',
      ubicacion: '',
      salario: '',
      tipo: 'Jornada completa',
      modalidad: 'Remoto'
    });
  }

  updateNuevaOferta(field: keyof NuevaOfertaForm, value: string) {
    this.nuevaOferta.update(v => ({ ...v, [field]: value }));
  }
}




