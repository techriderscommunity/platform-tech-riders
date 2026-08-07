import { ChangeDetectionStrategy, Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { UiButton  } from '@shared/ui/button/button';

@Component({
  selector: 'app-perfil-candidato',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [RouterLink, UiButton],
  templateUrl: './perfil-candidato.html',
  styleUrl: './perfil-candidato.scss'
})
export class PerfilCandidato {
  candidato = {
    id: 1,
    nombre: 'Ana García López',
    titulo: 'Desarrollador Frontend Junior',
    ubicacion: 'Madrid, España',
    foto: 'AG',
    disponibilidad: 'Disponible inmediatamente',
    pretensionSalarial: '22,000 - 26,000 €',
    resumen: 'Desarrolladora frontend junior apasionada por crear interfaces intuitivas y accesibles. Con experiencia en React y un portfolio de proyectos personales. Busco mi primer trabajo en una empresa que me permita crecer profesionalmente.',

    habilidades: [
      'JavaScript',
      'React',
      'TypeScript',
      'CSS',
      'HTML',
      'Git',
      'Responsive Design',
      'Figma'
    ],

    experiencia: [
      {
        titulo: 'Desarrolladora Frontend Junior - Bootcamp Tech Riders',
        empresa: 'Tech Riders Academy',
        periodo: 'Ene 2025 - Actualidad',
        descripcion: 'Participante en programa intensivo de 3 meses enfocado en desarrollo frontend con React, TypeScript y buenas prácticas de código.'
      },
      {
        titulo: 'Proyectos Personales',
        empresa: 'Portfolio Propio',
        periodo: 'Ago 2024 - Presente',
        descripcion: 'Desarrollo de aplicaciones web usando React, con enfoque en UI/UX y accesibilidad.'
      }
    ],

    educacion: [
      {
        titulo: 'Bootcamp Full Stack Development',
        institucion: 'Tech Riders',
        fecha: '2024 - 2025',
        estado: 'En progreso'
      },
      {
        titulo: 'Grado en Ingeniería Técnica en Informática',
        institucion: 'Universidad Autónoma de Madrid',
        fecha: '2020 - 2023',
        estado: 'Completado'
      }
    ],

    redes: [
      { red: 'GitHub', url: 'github.com/anagarcia' },
      { red: 'LinkedIn', url: 'linkedin.com/in/anagarcia' },
      { red: 'Portfolio', url: 'anagarcia.dev' }
    ]
  };

  contactarCandidato() {
    alert(`Iniciando conversación con ${this.candidato.nombre}...`);
  }

  guardarCandidato() {
    alert('Candidato guardado para revisar después');
  }
}




