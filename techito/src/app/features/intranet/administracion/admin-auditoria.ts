import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { RouterLink } from '@angular/router';
import { catchError, of } from 'rxjs';
import { IntranetAdminService, IntranetAuditRecord } from './services/intranet-admin.service';

interface AuditRow {
  fecha: string;
  usuario: string;
  modulo: string;
  accion: string;
  resultado: 'ok' | 'warning';
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
  private readonly intranetAdminService = inject(IntranetAdminService);

  readonly loading = signal(false);
  readonly rows = signal<AuditRow[]>([]);

  constructor() {
    this.load();
  }

  private load() {
    this.loading.set(true);
    this.intranetAdminService.getAuditLogs()
      .pipe(
        catchError(() => of([] as IntranetAuditRecord[])),
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
