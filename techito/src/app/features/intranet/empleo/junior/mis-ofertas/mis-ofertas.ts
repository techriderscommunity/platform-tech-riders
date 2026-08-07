import { ChangeDetectionStrategy, Component, DestroyRef, computed, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { catchError, finalize, of } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { JuniorService } from '../services/junior.service';
import { OfertaJunior } from '../models/junior.models';
import { UiSelect, UiSelectOption  } from '@shared/ui/select/select';
import { UiButton  } from '@shared/ui/button/button';

@Component({
  selector: 'app-mis-ofertas',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [FormsModule, UiSelect, UiButton],
  templateUrl: './mis-ofertas.html',
  styleUrl: './mis-ofertas.scss'
})
export class MisOfertas {
  private readonly juniorService = inject(JuniorService);
  private readonly destroyRef = inject(DestroyRef);

  readonly filtroActual = signal('todas');
  readonly ordenar = signal('recientes');
  readonly loading = signal(false);
  readonly error = signal<string | null>(null);
  readonly info = signal<string | null>(null);
  readonly ofertas = signal<OfertaJunior[]>([]);
  readonly ordenarOptions: UiSelectOption[] = [
    { label: 'Más Recientes', value: 'recientes' },
    { label: 'Mayor Compatibilidad', value: 'match' },
    { label: 'Mayor Salario', value: 'salario' }
  ];

  constructor() {
    this.loading.set(true);
    this.error.set(null);
    this.juniorService.getOfertas(1, 100)
      .pipe(
        catchError(() => {
          this.error.set('No se pudieron cargar las ofertas.');
          return of({ items: [] as OfertaJunior[] });
        }),
        finalize(() => this.loading.set(false)),
        takeUntilDestroyed(this.destroyRef)
      )
      .subscribe(result => {
        this.ofertas.set(result.items);
      });
  }

  readonly ofertasFiltradas = computed(() => {
    const filtro = this.filtroActual();
    const orden = this.ordenar();
    let resultado = filtro === 'todas' ? [...this.ofertas()] : this.ofertas().filter(o => o.estado === filtro);

    if (orden === 'salario') {
      resultado = [...resultado].sort((a, b) => {
        const minA = parseInt(a.salario.split('-')[0] ?? '0', 10);
        const minB = parseInt(b.salario.split('-')[0] ?? '0', 10);
        return minB - minA;
      });
    }

    return resultado;
  });

  guardarOferta(_: string) {
    // TODO: endpoint de guardado de oferta por junior.
  }

  enviarSolicitud(ofertaId: string) {
    this.info.set(null);
    this.error.set(null);
    this.juniorService
      .enviarSolicitud(ofertaId, 'junior-demo', 'Junior Demo', 'junior.demo@techriders.local')
      .pipe(
        catchError(() => {
          this.error.set('No se pudo enviar tu solicitud. Intenta de nuevo.');
          return of(null);
        }),
        takeUntilDestroyed(this.destroyRef)
      )
      .subscribe(result => {
        if (result) {
          this.info.set('Solicitud enviada correctamente.');
        }
      });
  }

  eliminarOferta(id: string) {
    this.ofertas.update(list => list.filter(o => o.id !== id));
  }
}




