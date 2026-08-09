import { ChangeDetectionStrategy, Component, computed, inject, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { RouterLink } from '@angular/router';
import { catchError, of } from 'rxjs';
import { finalize } from 'rxjs/operators';
import { ConfigItem, IntranetSettingRecord } from './models/intranet-admin.models';
import { IntranetAdminService } from './services/intranet-admin.service';
import { UiMetricsStrip } from '@shared/ui/metrics-strip/metrics-strip';

@Component({
  selector: 'app-admin-configuracion',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [RouterLink, UiMetricsStrip],
  templateUrl: './admin-configuracion.html',
  styleUrl: './admin-configuracion.scss'
})
export class AdminConfiguracion {
  private readonly intranetAdminService = inject(IntranetAdminService);

  readonly loading = signal(false);
  readonly error = signal<string | null>(null);
  readonly feedback = signal<string | null>(null);
  readonly items = signal<ConfigItem[]>([]);
  readonly savingByKey = signal<Record<string, boolean>>({});

  readonly inRevisionCount = computed(() => this.items().filter(item => item.estado === 'revision').length);
  readonly configMetrics = computed(() => [
    { icon: '🧱', value: String(this.items().length), label: 'Bloques de configuración' },
    { icon: '🧪', value: String(this.inRevisionCount()), label: 'En revisión' },
  ]);

  constructor() {
    this.load();
  }

  private load() {
    this.loading.set(true);
    this.error.set(null);

    this.intranetAdminService.getSettings()
      .pipe(
        catchError(() => {
          this.error.set('No se pudo cargar configuracion de intranet.');
          return of([] as IntranetSettingRecord[]);
        }),
        takeUntilDestroyed(),
      )
      .subscribe(rows => {
        this.items.set(rows.map(row => ({
          key: row.key,
          modulo: row.module,
          valor: row.value,
          estado: row.status === 'revision' ? 'revision' : 'activo',
        })));
        this.loading.set(false);
      });
  }

  updateModulo(key: string, value: string) {
    this.updateItem(key, { modulo: value });
  }

  updateValor(key: string, value: string) {
    this.updateItem(key, { valor: value });
  }

  updateEstado(key: string, value: string) {
    const estado = value === 'revision' ? 'revision' : 'activo';
    this.updateItem(key, { estado });
  }

  saveRow(item: ConfigItem) {
    this.feedback.set(null);
    this.setSaving(item.key, true);

    this.intranetAdminService.updateSetting({
      key: item.key,
      module: item.modulo,
      value: item.valor,
      status: item.estado,
    })
      .pipe(
        catchError(() => {
          this.feedback.set(`No se pudo guardar la fila ${item.key}.`);
          return of(null);
        }),
        finalize(() => this.setSaving(item.key, false)),
        takeUntilDestroyed(),
      )
      .subscribe(result => {
        if (result !== null) {
          this.feedback.set(`Fila ${item.key} guardada correctamente.`);
        }
      });
  }

  isSaving(key: string): boolean {
    return !!this.savingByKey()[key];
  }

  private updateItem(key: string, patch: Partial<ConfigItem>) {
    this.items.update(items => items.map(item => item.key === key ? { ...item, ...patch } : item));
  }

  private setSaving(key: string, value: boolean) {
    this.savingByKey.update(state => ({ ...state, [key]: value }));
  }
}
