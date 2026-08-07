import { ChangeDetectionStrategy, Component, DestroyRef, inject, signal } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { AuthService } from '@core/auth/auth.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { catchError, map, of, switchMap, tap } from 'rxjs';
import { Candidato } from '../models/empresa.models';
import { EmpresaService } from '../services/empresa.service';
import { UiSelect, UiSelectOption  } from '@shared/ui/select/select';
import { UiButton  } from '@shared/ui/button/button';

@Component({
  selector: 'app-ver-candidatos',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [RouterLink, FormsModule, UiSelect, UiButton],
  templateUrl: './ver-candidatos.html',
  styleUrl: './ver-candidatos.scss'
})
export class VerCandidatos {
  private readonly route = inject(ActivatedRoute);
  private readonly empresaService = inject(EmpresaService);
  private readonly authService = inject(AuthService);
  private readonly destroyRef = inject(DestroyRef);

  readonly ofertaId = signal('');
  readonly candidatos = signal<Candidato[]>([]);
  readonly loading = signal(false);
  readonly error = signal<string | null>(null);
  readonly estadoOptions: UiSelectOption[] = [
    { label: 'Marcar Pendiente', value: 'pendiente' },
    { label: 'Marcar para Entrevista', value: 'entrevista' },
    { label: 'Hacer Oferta', value: 'oferta' }
  ];

  constructor() {
    this.route.queryParamMap
      .pipe(
        map(params => params.get('ofertaId') ?? ''),
        tap(ofertaId => {
          this.ofertaId.set(ofertaId);
          this.loading.set(true);
          this.error.set(null);
        }),
        switchMap(ofertaId => {
          if (!ofertaId) {
            if (this.authService.hasRole('superadmin')) {
              return this.empresaService.getCandidatosGovernance().pipe(
                catchError(() => {
                  this.error.set('No se pudieron cargar las candidaturas globales.');
                  return of([] as Candidato[]);
                })
              );
            }

            return of([] as Candidato[]);
          }

          return this.empresaService.getCandidatos(ofertaId).pipe(
            catchError(() => {
              this.error.set('No se pudieron cargar los candidatos.');
              return of([] as Candidato[]);
            })
          );
        }),
        takeUntilDestroyed(this.destroyRef)
      )
      .subscribe(candidatos => {
        this.candidatos.set(candidatos);
        this.loading.set(false);
      });
  }

  cambiarEstado(id: string, nuevoEstado: string) {
    if (this.authService.hasRole('superadmin')) {
      this.empresaService.updateCandidaturaEstado(id, nuevoEstado)
        .pipe(
          catchError(() => {
            this.error.set('No se pudo actualizar el estado de la candidatura.');
            return of(void 0);
          }),
          takeUntilDestroyed(this.destroyRef)
        )
        .subscribe(() => {
          this.candidatos.update(list => list.map(c =>
            c.id === id ? { ...c, estado: nuevoEstado } : c
          ));
        });
      return;
    }

    this.candidatos.update(list => list.map(c =>
      c.id === id ? { ...c, estado: nuevoEstado } : c
    ));
  }

  contactarCandidato(_: string) {
    // TODO: conectar flujo real de contacto.
  }

  rechazarCandidato(id: string) {
    this.candidatos.update(list => list.filter(c => c.id !== id));
  }

  getInitials(nombre: string): string {
    return nombre
      .split(' ')
      .filter(Boolean)
      .slice(0, 2)
      .map(part => part[0]!.toUpperCase())
      .join('');
  }
}




