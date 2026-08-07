import { ChangeDetectionStrategy, Component, signal, inject, DestroyRef } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { HttpClient } from '@angular/common/http';
import { environment } from '@env/environment';
import { tap, catchError } from 'rxjs/operators';
import { of } from 'rxjs';

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [],
  templateUrl: './admin-dashboard.html',
  styleUrl: './admin-dashboard.scss'
})
export class AdminDashboard {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = environment.apiUrl;
  private readonly destroyRef = inject(DestroyRef);

  readonly loading = signal(true);
  readonly error = signal<string | null>(null);
  readonly stats = signal({ totalUsuarios: 0, activos: 0, superadmins: 0, eventos: 0, sesiones: 0, embajadores: 0, ofertas: 0, candidaturas: 0 });
  readonly ultimasAcciones = signal<any[]>([]);
  readonly systemeHealth = signal({ servers: '', database: '', uploads: '', cpu: '' });

  constructor() {
    this.loadDashboard();
  }

  private loadDashboard() {
    this.http.get(`${this.baseUrl}/admin/dashboard`)
      .pipe(
        tap((data: any) => {
          const nextStats = data?.stats ?? data;
          this.stats.set({
            totalUsuarios: Number(nextStats?.totalUsuarios ?? 0),
            activos: Number(nextStats?.usuariosActivos ?? 0),
            superadmins: Number(nextStats?.totalSuperadmins ?? 0),
            eventos: Number(nextStats?.totalEventos ?? 0),
            sesiones: Number(nextStats?.totalSesiones ?? 0),
            embajadores: Number(nextStats?.totalEmbajadores ?? 0),
            ofertas: Number(nextStats?.totalOfertas ?? 0),
            candidaturas: Number(nextStats?.totalCandidaturas ?? 0),
          });
          this.ultimasAcciones.set(data.ultimasAcciones);
          this.systemeHealth.set(data.systemeHealth);
          this.loading.set(false);
        }),
        catchError(() => {
          this.error.set('No se pudo cargar el dashboard de admin');
          this.loading.set(false);
          return of(null);
        }),
        takeUntilDestroyed(this.destroyRef)
      )
      .subscribe();
  }
}


