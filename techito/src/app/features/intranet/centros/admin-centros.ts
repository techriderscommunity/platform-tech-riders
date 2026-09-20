import { ChangeDetectionStrategy, Component, DestroyRef, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { UiButton } from '@shared/ui/button/button';
import { UiSelect, UiSelectOption } from '@shared/ui/select/select';
import { UiTextField } from '@shared/ui/text-field/text-field';
import { UiTextarea } from '@shared/ui/textarea/textarea';
import { catchError, finalize, of } from 'rxjs';
import { OrganizationApi, OrganizationsService } from '@core/organizations/organizations.service';

const CENTER_TYPES: UiSelectOption[] = [
  { label: 'Centro de Formación', value: 'CentroFormacion' },
  { label: 'Centro Educativo', value: 'CentroEducativo' },
];

@Component({
  selector: 'app-admin-centros',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [RouterLink, UiButton, UiSelect, UiTextField, UiTextarea],
  templateUrl: './admin-centros.html',
  styleUrl: './admin-centros.scss',
})
export class AdminCentros {
  private readonly organizationsService = inject(OrganizationsService);
  private readonly destroyRef = inject(DestroyRef);

  readonly feedback = signal<string | null>(null);
  readonly loading = signal(false);
  readonly savingId = signal<string | null>(null);

  readonly centers = signal<OrganizationApi[]>([]);
  readonly pending = signal<OrganizationApi[]>([]);

  readonly typeOptions = CENTER_TYPES;
  readonly showCreateForm = signal(false);
  readonly newType = signal<string>('CentroEducativo');
  readonly newName = signal('');
  readonly newWebsite = signal('');
  readonly newAddress = signal('');
  readonly newProvince = signal('Madrid');
  readonly newNotes = signal('');
  readonly creating = signal(false);

  constructor() {
    this.load();
  }

  load() {
    this.loading.set(true);
    this.organizationsService.list(undefined, true)
      .pipe(
        catchError(() => of([] as OrganizationApi[])),
        finalize(() => this.loading.set(false)),
        takeUntilDestroyed(this.destroyRef),
      )
      .subscribe(items => this.centers.set(items.filter(o => o.OrganizationType === 'CentroFormacion' || o.OrganizationType === 'CentroEducativo')));

    this.organizationsService.getPending()
      .pipe(
        catchError(() => of([] as OrganizationApi[])),
        takeUntilDestroyed(this.destroyRef),
      )
      .subscribe(items => this.pending.set(items));
  }

  toggleCreateForm() {
    this.showCreateForm.update(v => !v);
  }

  updateNewType(value: string) { this.newType.set(value); }
  updateNewName(value: string) { this.newName.set(value); }
  updateNewWebsite(value: string) { this.newWebsite.set(value); }
  updateNewAddress(value: string) { this.newAddress.set(value); }
  updateNewProvince(value: string) { this.newProvince.set(value); }
  updateNewNotes(value: string) { this.newNotes.set(value); }

  createCenter() {
    const name = this.newName().trim();
    if (!name) {
      this.feedback.set('Indica el nombre del centro.');
      return;
    }

    this.creating.set(true);
    this.organizationsService.create({
      organizationType: this.newType(),
      name,
      website: this.newWebsite() || undefined,
      address: this.newAddress() || undefined,
      province: this.newProvince() || undefined,
      notes: this.newNotes() || undefined,
    })
      .pipe(
        finalize(() => this.creating.set(false)),
        takeUntilDestroyed(this.destroyRef),
      )
      .subscribe({
        next: () => {
          this.feedback.set(`Centro ${name} creado correctamente.`);
          this.newName.set('');
          this.newWebsite.set('');
          this.newAddress.set('');
          this.newNotes.set('');
          this.showCreateForm.set(false);
          this.load();
        },
        error: (error) => this.feedback.set(error?.error?.Message ?? 'No se pudo crear el centro.'),
      });
  }

  approvePending(item: OrganizationApi) {
    this.savingId.set(item.Id);
    this.organizationsService.activate(item.Id)
      .pipe(
        finalize(() => this.savingId.set(null)),
        takeUntilDestroyed(this.destroyRef),
      )
      .subscribe({
        next: () => {
          this.feedback.set(`Centro ${item.Name} aprobado.`);
          this.load();
        },
        error: () => this.feedback.set('No se pudo aprobar el centro.'),
      });
  }

  toggleActivation(item: OrganizationApi) {
    const action = item.IsActive ? 'suspender' : 'activar';
    if (!confirm(`¿Confirmas ${action} el centro ${item.Name}?`)) {
      return;
    }

    this.savingId.set(item.Id);
    const request$ = item.IsActive ? this.organizationsService.suspend(item.Id) : this.organizationsService.activate(item.Id);
    request$
      .pipe(
        finalize(() => this.savingId.set(null)),
        takeUntilDestroyed(this.destroyRef),
      )
      .subscribe({
        next: () => {
          this.centers.update(list => list.map(c => c.Id === item.Id ? { ...c, IsActive: !item.IsActive } : c));
          this.feedback.set(`Centro ${item.IsActive ? 'suspendido' : 'activado'} correctamente.`);
        },
        error: () => this.feedback.set('No se pudo actualizar el estado del centro.'),
      });
  }

  limpiarFeedback() {
    this.feedback.set(null);
  }
}
