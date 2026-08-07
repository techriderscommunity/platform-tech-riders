import { ChangeDetectionStrategy, Component, computed, inject, signal } from '@angular/core';
import { JuniorService } from '../services/junior.service';

@Component({
  selector: 'app-mis-cursos',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [],
  templateUrl: './mis-cursos.html',
  styleUrl: './mis-cursos.scss'
})
export class MisCursos {
  private readonly juniorService = inject(JuniorService);

  readonly filtroActual = signal('todos');
  readonly cursos = this.juniorService.cursos;

  readonly cursosFiltrados = computed(() => {
    const filtro = this.filtroActual();
    const all = this.cursos();
    return filtro === 'todos' ? all : all.filter(c => (c as { estado?: string }).estado === filtro);
  });

  iniciarCurso(_: string) {
    // TODO: endpoint de inscripción/inicio de curso.
  }

  continuarCurso(_: string) {
    // TODO: navegación al detalle del curso cuando exista.
  }
}


