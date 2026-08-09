import { ChangeDetectionStrategy, Component, computed, inject, signal } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { FormsModule } from '@angular/forms';
import { PublicContentService } from '@core/content/public-content.service';
import { UiTextField  } from '@shared/ui/text-field/text-field';
import { UiTextarea  } from '@shared/ui/textarea/textarea';
import { UiSelect, UiSelectOption  } from '@shared/ui/select/select';
import { UiButton  } from '@shared/ui/button/button';
import { PerfilPrivado, PerfilPublico } from '../models/junior.models';

@Component({
  selector: 'app-editar-perfil',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [FormsModule, UiTextField, UiTextarea, UiSelect, UiButton],
  templateUrl: './editar-perfil.html',
  styleUrl: './editar-perfil.scss'
})
export class EditarPerfil {
  private readonly publicContentService = inject(PublicContentService);
  private readonly publicContent = toSignal(this.publicContentService.getPublicContent(), { initialValue: null });

  readonly tabActiva = signal<'publico' | 'privado'>('publico');

  readonly perfilPublico = signal<PerfilPublico>({
    nombre: 'Ana García',
    titulo: 'Desarrollador Frontend',
    ubicacion: 'Madrid, España',
    resumen: 'Desarrolladora frontend apasionada por React y crear interfaces intuitivas...',
    habilidades: ['JavaScript', 'React', 'CSS', 'HTML', 'Git'],
    experiencia: 'Junior con 6 meses de experiencia en desarrollo web',
    foto: 'AG'
  });

  readonly perfilPrivado = signal<PerfilPrivado>({
    email: 'ana.garcia@email.com',
    telefono: '+34 612 345 678',
    edad: 22,
    gradoAcademico: 'Grado en Ingeniería Informática',
    universidad: 'Universidad Autónoma de Madrid',
    disponibilidad: 'Inmediata',
    pretensionSalarial: '24.000 - 30.000€'
  });

  readonly habilidadesDisponibles = computed(() => this.publicContent()?.intranet.juniorSkillOptions ?? []);

  readonly habilidadesOptions = computed<UiSelectOption[]>(() => [
    { label: 'Selecciona una habilidad...', value: '' },
    ...this.habilidadesDisponibles().map(hab => ({ label: hab, value: hab }))
  ]);

  readonly disponibilidadOptions = computed<UiSelectOption[]>(
    () => this.publicContent()?.intranet.juniorAvailabilityOptions ?? []
  );

  readonly nuevaHabilidad = signal('');

  guardarCambios(tipo: string) {
    alert(`Cambios en ${tipo} guardados correctamente`);
  }

  updatePerfilPublico(field: keyof PerfilPublico, value: string) {
    this.perfilPublico.update(v => ({ ...v, [field]: value }));
  }

  updatePerfilPrivado(field: keyof PerfilPrivado, value: string | number) {
    this.perfilPrivado.update(v => ({ ...v, [field]: value }));
  }

  agregarHabilidad() {
    const hab = this.nuevaHabilidad();
    if (hab && !this.perfilPublico().habilidades.includes(hab)) {
      this.perfilPublico.update(v => ({ ...v, habilidades: [...v.habilidades, hab] }));
      this.nuevaHabilidad.set('');
    }
  }

  eliminarHabilidad(habilidad: string) {
    this.perfilPublico.update(v => ({ ...v, habilidades: v.habilidades.filter(h => h !== habilidad) }));
  }
}




