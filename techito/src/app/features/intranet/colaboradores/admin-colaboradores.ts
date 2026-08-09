import { ChangeDetectionStrategy, Component, signal, inject, DestroyRef } from '@angular/core';
import { RouterLink } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { tap, catchError } from 'rxjs/operators';
import { of } from 'rxjs';
import { UiButton  } from '@shared/ui/button/button';
import { UiModal } from '@shared/ui/modal/modal';
import { UiTextField } from '@shared/ui/text-field/text-field';
import { ColaboradorItem } from './models/colaboradores.models';
import { ColaboradoresService } from './services/colaboradores.service';

@Component({
  selector: 'app-admin-colaboradores',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [RouterLink, UiButton, UiModal, UiTextField],
  templateUrl: './admin-colaboradores.html',
  styleUrl: './admin-colaboradores.scss'
})
export class AdminColaboradores {
  private readonly colaboradoresService = inject(ColaboradoresService);
  private readonly destroyRef = inject(DestroyRef);

  readonly loading = signal(true);
  readonly saving = signal(false);
  readonly error = signal<string | null>(null);
  readonly feedback = signal<string | null>(null);
  readonly colaboradores = signal<ColaboradorItem[]>([]);

  readonly showCreateModal = signal(false);
  readonly showStatusModal = signal(false);
  readonly modalError = signal<string | null>(null);
  readonly formNombre = signal('');
  readonly formEmail = signal('');
  readonly formPassword = signal('');
  readonly selectedColaborador = signal<ColaboradorItem | null>(null);

  constructor() {
    this.loadColaboradores();
  }

  private loadColaboradores() {
    this.colaboradoresService.getColaboradores()
      .pipe(
        tap((data) => {
          this.colaboradores.set(data);
          this.loading.set(false);
        }),
        catchError(() => {
          this.error.set('No se pudieron cargar los colaboradores');
          this.loading.set(false);
          return of([]);
        }),
        takeUntilDestroyed(this.destroyRef)
      )
      .subscribe();
  }

  agregarColaborador() {
    this.modalError.set(null);
    this.formNombre.set('');
    this.formEmail.set('');
    this.formPassword.set('');
    this.showCreateModal.set(true);
  }

  closeCreateModal() {
    this.showCreateModal.set(false);
    this.modalError.set(null);
  }

  submitCreateColaborador(event?: Event) {
    if (event) event.preventDefault();

    const nombre = this.formNombre().trim();
    const email = this.formEmail().trim();
    const password = this.formPassword();

    if (!nombre || !email || !password) {
      this.modalError.set('Nombre, email y contraseña son obligatorios.');
      return;
    }

    if (password.length < 8) {
      this.modalError.set('La contraseña debe tener al menos 8 caracteres.');
      return;
    }

    this.modalError.set(null);

    this.saving.set(true);
    this.colaboradoresService.createColaborador({
      email,
      nombre,
      password,
      primaryRole: 'colaborador',
      roles: ['colaborador'],
    })
      .pipe(
        tap(() => {
          this.feedback.set('Colaborador creado correctamente.');
          this.showCreateModal.set(false);
          this.loadColaboradores();
        }),
        catchError((err) => {
          this.modalError.set(err?.error?.error ?? 'No se pudo crear el colaborador.');
          return of(null);
        }),
        tap(() => this.saving.set(false)),
        takeUntilDestroyed(this.destroyRef)
      )
      .subscribe();
  }

  editarColaborador(id: string) {
    const item = this.colaboradores().find(colab => colab.id === id);
    if (!item) return;

    this.selectedColaborador.set(item);
    this.modalError.set(null);
    this.showStatusModal.set(true);
  }

  closeStatusModal() {
    this.showStatusModal.set(false);
    this.selectedColaborador.set(null);
    this.modalError.set(null);
  }

  confirmToggleEstado() {
    const item = this.selectedColaborador();
    if (!item) return;

    const activar = item.estado !== 'activo';

    this.saving.set(true);
    this.colaboradoresService.updateEstado(item.id, activar)
      .pipe(
        tap(() => {
          this.feedback.set(`Colaborador ${activar ? 'reactivado' : 'desactivado'} correctamente.`);
          this.colaboradores.update(list => list.map(colab =>
            colab.id === item.id ? { ...colab, estado: activar ? 'activo' : 'inactivo' } : colab
          ));
          this.showStatusModal.set(false);
          this.selectedColaborador.set(null);
        }),
        catchError((err) => {
          this.modalError.set(err?.error?.error ?? 'No se pudo actualizar el estado.');
          return of(null);
        }),
        tap(() => this.saving.set(false)),
        takeUntilDestroyed(this.destroyRef)
      )
      .subscribe();
  }

  verProyectos(id: string) {
    this.feedback.set(`Vista de proyectos para colaborador ${id} queda en siguiente iteración.`);
  }
}


