import { ChangeDetectionStrategy, Component, computed, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { UiTextField  } from '@shared/ui/text-field/text-field';
import { UiTextarea  } from '@shared/ui/textarea/textarea';
import { UiSelect, UiSelectOption  } from '@shared/ui/select/select';
import { UiButton  } from '@shared/ui/button/button';

interface PerfilPublico {
  nombre: string;
  titulo: string;
  ubicacion: string;
  resumen: string;
  habilidades: string[];
  experiencia: string;
  foto: string;
}

interface PerfilPrivado {
  email: string;
  telefono: string;
  edad: number;
  gradoAcademico: string;
  universidad: string;
  disponibilidad: string;
  pretensionSalarial: string;
}

@Component({
  selector: 'app-editar-perfil',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [FormsModule, UiTextField, UiTextarea, UiSelect, UiButton],
  templateUrl: './editar-perfil.html',
  styleUrl: './editar-perfil.scss'
})
export class EditarPerfil {
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

  readonly habilidadesDisponibles = [
    'JavaScript', 'TypeScript', 'React', 'Angular', 'Vue.js',
    'Node.js', 'Python', 'Java', 'C++', 'HTML', 'CSS',
    'Sass', 'Bootstrap', 'Tailwind', 'Git', 'Docker',
    'SQL', 'MongoDB', 'REST APIs', 'GraphQL'
  ];

  readonly habilidadesOptions = computed<UiSelectOption[]>(() => [
    { label: 'Selecciona una habilidad...', value: '' },
    ...this.habilidadesDisponibles.map(hab => ({ label: hab, value: hab }))
  ]);

  readonly disponibilidadOptions: UiSelectOption[] = [
    { label: 'Inmediata', value: 'Inmediata' },
    { label: 'En 1 semana', value: 'En 1 semana' },
    { label: 'En 2 semanas', value: 'En 2 semanas' },
    { label: 'En 1 mes', value: 'En 1 mes' }
  ];

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




