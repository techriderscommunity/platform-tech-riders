import { ChangeDetectionStrategy, Component, signal, inject, DestroyRef } from '@angular/core';
import { RouterLink } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { EmpresaService } from '../services/empresa.service';
import { Candidato, EmpresaDashboardStats, Oferta } from '../models/empresa.models';
import { catchError, finalize, tap } from 'rxjs/operators';
import { of } from 'rxjs';
import { UiButton  } from '@shared/ui/button/button';

@Component({
  selector: 'app-dashboard-empresa',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [RouterLink, UiButton],
  templateUrl: './dashboard-empresa.html',
  styleUrl: './dashboard-empresa.scss'
})
export class DashboardEmpresa {
  private readonly empresaService = inject(EmpresaService);
  private readonly destroyRef = inject(DestroyRef);

  readonly loading = signal(true);
  readonly error = signal<string | null>(null);
  readonly info = signal<string | null>(null);
  readonly nombreEmpresa = signal('');
  readonly stats = signal<EmpresaDashboardStats>({ ofertasActivas: 0, candidatosTotal: 0, enProceso: 0, contratados: 0 });
  readonly ultimasOfertas = signal<Oferta[]>([]);
  readonly ultimosCandidatos = signal<Candidato[]>([]);

  constructor() {
    this.loadDashboard();
  }

  private loadDashboard() {
    this.loading.set(true);
    this.error.set(null);
    this.empresaService.getDashboard()
      .pipe(
        tap((data) => {
          if (!data) {
            return;
          }
          this.nombreEmpresa.set(data.nombreEmpresa);
          this.stats.set(data.stats);
          this.ultimasOfertas.set(data.ultimasOfertas);
          this.ultimosCandidatos.set(data.ultimosCandidatos);
          this.info.set(null);
        }),
        catchError(() => {
          this.error.set('No se pudo cargar el dashboard');
          return of(null);
        }),
        finalize(() => this.loading.set(false)),
        takeUntilDestroyed(this.destroyRef)
      )
      .subscribe();
  }

  reintentarCarga() {
    this.loadDashboard();
  }
}




