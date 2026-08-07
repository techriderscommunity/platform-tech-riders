import { HttpClient } from '@angular/common/http';
import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { RouterLink } from '@angular/router';
import { environment } from '@env/environment';
import { catchError, of } from 'rxjs';

interface AuditRow {
  fecha: string;
  usuario: string;
  modulo: string;
  accion: string;
  resultado: 'ok' | 'warning';
}

interface AuditResponse {
  id: string;
  createdUtc: string;
  actorUserId?: string | null;
  actorEmail?: string | null;
  module: string;
  action: string;
  result: string;
  detail?: string | null;
}

@Component({
  selector: 'app-admin-auditoria',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [RouterLink],
  templateUrl: './admin-auditoria.html',
  styleUrl: './admin-auditoria.scss'
})
export class AdminAuditoria {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = environment.apiUrl;

  readonly loading = signal(false);
  readonly rows = signal<AuditRow[]>([]);

  constructor() {
    this.load();
  }

  private load() {
    this.loading.set(true);
    this.http.get<AuditResponse[]>(`${this.baseUrl}/admin/intranet/auditoria?take=200`)
      .pipe(
        catchError(() => of([] as AuditResponse[])),
        takeUntilDestroyed(),
      )
      .subscribe(rows => {
        this.rows.set(rows.map(row => ({
          fecha: this.formatDate(row.createdUtc),
          usuario: row.actorEmail ?? 'sistema',
          modulo: row.module,
          accion: row.action,
          resultado: row.result === 'warning' ? 'warning' : 'ok',
        })));
        this.loading.set(false);
      });
  }

  private formatDate(raw: string): string {
    return new Intl.DateTimeFormat('es-ES', {
      year: 'numeric',
      month: '2-digit',
      day: '2-digit',
      hour: '2-digit',
      minute: '2-digit',
      hour12: false,
    }).format(new Date(raw));
  }
}
