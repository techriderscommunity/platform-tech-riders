import { ChangeDetectionStrategy, Component, DestroyRef, computed, inject, signal } from '@angular/core';
import { NgClass } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { takeUntilDestroyed, toObservable } from '@angular/core/rxjs-interop';
import { catchError, of, switchMap, tap } from 'rxjs';
import { EmbajadoresService } from './services/embajadores.service';
import { Embajador } from './models/embajadores.models';
import { UiTextField  } from '@shared/ui/text-field/text-field';
import { UiSelect, UiSelectOption  } from '@shared/ui/select/select';
import { UiButton  } from '@shared/ui/button/button';

@Component({
  selector: 'app-embajador',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [FormsModule, NgClass, UiTextField, UiSelect, UiButton],
  templateUrl: './embajador.html',
  styleUrl: './embajador.scss'
})
export class EmbajadorComponent {
  private readonly embajadoresService = inject(EmbajadoresService);
  private readonly destroyRef = inject(DestroyRef);

  readonly searchName = signal('');
  readonly searchStatus = signal('pendiente');
  readonly loading = signal(false);
  readonly error = signal<string | null>(null);
  readonly embajadores = signal<Embajador[]>([]);

  readonly estados = [
    { label: 'Todos', value: '' },
    { label: 'Activos', value: 'activo' },
    { label: 'Desactivados', value: 'desactivado' },
    { label: 'Pendientes', value: 'pendiente' }
  ];

  readonly estadoOptions: UiSelectOption[] = this.estados.map(estado => ({
    label: estado.label,
    value: estado.value
  }));

  readonly query = computed(() => ({
    estado: this.searchStatus()
  }));

  constructor() {
    toObservable(this.query)
      .pipe(
        tap(() => {
          this.loading.set(true);
          this.error.set(null);
        }),
        switchMap(({ estado }) => this.embajadoresService.getEmbajadores(1, 100, estado || undefined).pipe(
          catchError(() => {
            this.error.set('No se pudieron cargar los embajadores.');
            return of({ items: [] as Embajador[] });
          })
        )),
        takeUntilDestroyed(this.destroyRef)
      )
      .subscribe(result => {
        this.embajadores.set(result.items);
        this.loading.set(false);
      });
  }

  readonly embajadoresFiltrados = computed(() => {
    const name = this.searchName().toLowerCase();

    return this.embajadores().filter(e => {
      const matchName = e.nombre.toLowerCase().includes(name);
      return matchName;
    });
  });

  normalizarEstado(estado: string): 'activo' | 'desactivado' | 'pendiente' {
    const normalized = estado.toLowerCase();
    if (normalized === 'activo') return 'activo';
    if (normalized === 'desactivado') return 'desactivado';
    return 'pendiente';
  }

  formatUltimaActividad(value: string | null): string {
    if (!value) return '-';
    return new Intl.DateTimeFormat('es-ES', {
      day: '2-digit',
      month: '2-digit',
      year: 'numeric'
    }).format(new Date(value));
  }

  addEmbajador() {
    // TODO: abrir modal/formulario real cuando se implemente UX de alta.
  }

  editarEmbajador(_: string) {
    // TODO: conectar edición real cuando exista endpoint de actualización.
  }

  activarEmbajador(_: string) {
    // TODO: conectar activación real cuando exista endpoint de cambio de estado.
  }

  reactivarEmbajador(_: string) {
    // TODO: conectar reactivación real cuando exista endpoint de cambio de estado.
  }

  darDeBajaEmbajador(_: string) {
    // TODO: conectar baja real cuando exista endpoint de cambio de estado.
  }
}


