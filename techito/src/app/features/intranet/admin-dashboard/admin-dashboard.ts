import { ChangeDetectionStrategy, Component, computed, signal, inject, DestroyRef } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { RouterLink } from '@angular/router';
import { tap, catchError } from 'rxjs/operators';
import { of } from 'rxjs';
import { AdminDashboardService, AdminDashboardApiResponse } from './services/admin-dashboard.service';
import { UiMetricsStrip } from '@shared/ui/metrics-strip/metrics-strip';
import { UiProgressCards } from '@shared/ui/progress-cards/progress-cards';

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [UiMetricsStrip, UiProgressCards, RouterLink],
  templateUrl: './admin-dashboard.html',
  styleUrl: './admin-dashboard.scss'
})
export class AdminDashboard {
  private readonly adminDashboardService = inject(AdminDashboardService);
  private readonly destroyRef = inject(DestroyRef);

  readonly loading = signal(true);
  readonly error = signal<string | null>(null);
  readonly stats = signal({ totalUsuarios: 0, activos: 0, superadmins: 0, eventos: 0, sesiones: 0, embajadores: 0, ofertas: 0, candidaturas: 0 });
  readonly ultimasAcciones = signal<Array<{ accion: string; detalle: string; fecha: string }>>([]);
  readonly systemeHealth = signal({ servers: '', database: '', uploads: '', cpu: '' });

  readonly dashboardMetrics = computed(() => {
    const stats = this.stats();
    return [
      { icon: '👥', value: String(stats.totalUsuarios), label: 'Usuarios totales' },
      { icon: '⭐', value: String(stats.superadmins), label: 'Superadmins' },
      { icon: '📅', value: String(stats.eventos), label: 'Eventos' },
      { icon: '🧩', value: String(stats.sesiones), label: 'Sesiones' },
    ];
  });

  readonly systemHealthCards = computed(() => {
    const health = this.systemeHealth();
    return [
      { title: 'Servidores', value: health.servers || 'N/D', detail: 'Estado de infraestructura', progress: health.servers === 'OK' ? 100 : 50, status: health.servers === 'OK' ? 'Operativo' : 'Revisar' },
      { title: 'Base de datos', value: health.database || 'N/D', detail: 'Disponibilidad y rendimiento', progress: health.database === 'OK' ? 100 : 50, status: health.database === 'OK' ? 'Operativo' : 'Revisar' },
      { title: 'Cargas', value: health.uploads || 'N/D', detail: 'Canal de subida de contenidos', progress: health.uploads === 'OK' ? 100 : 50, status: health.uploads === 'OK' ? 'Operativo' : 'Revisar' },
      { title: 'CPU', value: health.cpu || 'N/D', detail: 'Consumo general del sistema', progress: health.cpu === 'OK' ? 100 : 50, status: health.cpu === 'OK' ? 'Operativo' : 'Revisar' },
    ];
  });

  constructor() {
    this.loadDashboard();
  }

  private loadDashboard() {
    this.adminDashboardService.getDashboard()
      .pipe(
        tap((data: AdminDashboardApiResponse) => {
          const nextStats = data.Stats;
          this.stats.set({
            totalUsuarios: Number(nextStats.TotalUsers ?? 0),
            activos: Number(nextStats.ActiveUsers ?? 0),
            superadmins: Number(nextStats.SuperAdmins ?? 0),
            eventos: Number(nextStats.Events ?? 0),
            sesiones: Number(nextStats.Sessions ?? 0),
            embajadores: Number(nextStats.Ambassadors ?? 0),
            ofertas: Number(nextStats.JobOffers ?? 0),
            candidaturas: Number(nextStats.Applications ?? 0),
          });
          this.ultimasAcciones.set(data.RecentActions.map(item => ({
            accion: item.Action,
            detalle: item.Detail,
            fecha: item.CreatedUtc,
          })));
          this.systemeHealth.set({
            servers: data.SystemHealth.Servers,
            database: data.SystemHealth.Database,
            uploads: data.SystemHealth.Uploads,
            cpu: data.SystemHealth.Cpu,
          });
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


