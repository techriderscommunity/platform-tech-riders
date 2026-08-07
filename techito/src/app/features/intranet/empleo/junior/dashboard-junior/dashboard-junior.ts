import { ChangeDetectionStrategy, Component, signal, inject, DestroyRef, computed } from '@angular/core';
import { RouterLink } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { JuniorService } from '../services/junior.service';
import { tap, catchError } from 'rxjs/operators';
import { of } from 'rxjs';
import { UiButton  } from '@shared/ui/button/button';

@Component({
  selector: 'app-dashboard-junior',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [RouterLink, UiButton],
  templateUrl: './dashboard-junior.html',
  styleUrl: './dashboard-junior.scss'
})
export class DashboardJunior {
  private readonly juniorService = inject(JuniorService);
  private readonly destroyRef = inject(DestroyRef);

  readonly loading = signal(true);
  readonly error = signal<string | null>(null);
  readonly nombreUsuario = signal('');
  readonly especializacion = signal('');
  readonly stats = signal({ ofertasGuardadas: 0, solicitudesEnviadas: 0, enProceso: 0, mensajes: 0 });
  readonly ofertasRecomendadas = signal<any[]>([]);
  readonly misCursos = signal<any[]>([]);
  readonly ultimosEventos = signal<any[]>([]);

  constructor() {
    this.loadDashboard();
  }

  private loadDashboard() {
    this.juniorService.getOfertas(1, 6)
      .pipe(
        tap((data) => {
          this.nombreUsuario.set('Junior TechRider');
          this.especializacion.set('Tecnologia');
          this.stats.set({
            ofertasGuardadas: data.items.length,
            solicitudesEnviadas: 0,
            enProceso: data.items.filter(oferta => oferta.estado === 'Activa').length,
            mensajes: 0
          });
          this.ofertasRecomendadas.set(data.items.map(oferta => ({ ...oferta, guardada: false })));
          this.misCursos.set([]);
          this.ultimosEventos.set([]);
          this.loading.set(false);
        }),
        catchError(() => {
          this.error.set('No se pudo cargar el dashboard');
          this.loading.set(false);
          return of(null);
        }),
        takeUntilDestroyed(this.destroyRef)
      )
      .subscribe();
  }
}




