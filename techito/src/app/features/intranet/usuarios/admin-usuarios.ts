import { ChangeDetectionStrategy, Component, DestroyRef, computed, inject, signal } from '@angular/core';
import { DatePipe } from '@angular/common';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { RouterLink } from '@angular/router';
import { UiButton } from '@shared/ui/button/button';
import { UiSelect, UiSelectOption } from '@shared/ui/select/select';
import { UiTextField } from '@shared/ui/text-field/text-field';
import { catchError, finalize, of } from 'rxjs';
import { UsersService } from '@core/users/users.service';
import { CreateUserPayload, UserListItem } from '@core/users/users.models';

@Component({
  selector: 'app-admin-usuarios',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [RouterLink, UiButton, UiSelect, UiTextField, DatePipe],
  templateUrl: './admin-usuarios.html',
  styleUrl: './admin-usuarios.scss',
})
export class AdminUsuarios {
  private readonly usersService = inject(UsersService);
  private readonly destroyRef = inject(DestroyRef);

  readonly feedback = signal<string | null>(null);
  readonly loading = signal(false);
  readonly savingId = signal<string | null>(null);

  readonly items = signal<UserListItem[]>([]);
  readonly totalCount = signal(0);
  readonly searchTerm = signal('');

  readonly showCreateForm = signal(false);
  readonly newNickname = signal('');
  readonly newName = signal('');
  readonly newLastName = signal('');
  readonly newEmail = signal('');
  readonly creating = signal(false);

  readonly membershipStatusOptions: UiSelectOption[] = [
    { label: 'Todos los estados', value: '' },
    { label: 'Activa', value: 'Activa' },
    { label: 'Pendiente', value: 'Pendiente' },
    { label: 'Suspendida', value: 'Suspendida' },
    { label: 'Baja', value: 'Baja' },
  ];
  readonly membershipFilter = signal('');

  readonly activeCount = computed(() => this.items().filter(u => u.membershipStatus === 'Activa').length);

  constructor() {
    this.load();
  }

  load() {
    this.loading.set(true);
    this.usersService.list({ search: this.searchTerm(), membershipStatus: this.membershipFilter() || undefined })
      .pipe(
        catchError(() => {
          this.feedback.set('No se pudo cargar el listado de usuarios.');
          return of({ items: [] as UserListItem[], totalCount: 0 });
        }),
        finalize(() => this.loading.set(false)),
        takeUntilDestroyed(this.destroyRef),
      )
      .subscribe(({ items, totalCount }) => {
        this.items.set(items);
        this.totalCount.set(totalCount);
      });
  }

  updateSearchTerm(value: string) {
    this.searchTerm.set(value);
  }

  updateMembershipFilter(value: string) {
    this.membershipFilter.set(value);
    this.load();
  }

  toggleCreateForm() {
    this.showCreateForm.update(v => !v);
  }

  updateNewNickname(value: string) { this.newNickname.set(value); }
  updateNewName(value: string) { this.newName.set(value); }
  updateNewLastName(value: string) { this.newLastName.set(value); }
  updateNewEmail(value: string) { this.newEmail.set(value); }

  createUser() {
    const payload: CreateUserPayload = {
      nickname: this.newNickname().trim(),
      name: this.newName().trim(),
      lastName: this.newLastName().trim(),
      email: this.newEmail().trim(),
    };

    if (!payload.nickname || !payload.name || !payload.lastName || !payload.email) {
      this.feedback.set('Completa nickname, nombre, apellidos y email para dar de alta.');
      return;
    }

    this.creating.set(true);
    this.usersService.create(payload)
      .pipe(
        finalize(() => this.creating.set(false)),
        takeUntilDestroyed(this.destroyRef),
      )
      .subscribe({
        next: () => {
          this.feedback.set(`Usuario ${payload.name} ${payload.lastName} creado correctamente.`);
          this.newNickname.set('');
          this.newName.set('');
          this.newLastName.set('');
          this.newEmail.set('');
          this.showCreateForm.set(false);
          this.load();
        },
        error: (error) => {
          this.feedback.set(error?.error?.Message ?? 'No se pudo crear el usuario.');
        },
      });
  }

  toggleActivation(item: UserListItem) {
    const isActive = item.membershipStatus === 'Activa';
    const action = isActive ? 'suspender' : 'activar';
    if (!confirm(`¿Confirmas ${action} a ${item.name} ${item.lastName}?`)) {
      return;
    }

    this.savingId.set(item.id);
    const request$ = isActive ? this.usersService.deactivate(item.id) : this.usersService.activate(item.id);
    request$
      .pipe(
        finalize(() => this.savingId.set(null)),
        takeUntilDestroyed(this.destroyRef),
      )
      .subscribe({
        next: () => {
          this.items.update(list => list.map(u => u.id === item.id
            ? { ...u, membershipStatus: isActive ? 'Suspendida' : 'Activa' }
            : u));
          this.feedback.set(`Usuario ${isActive ? 'suspendido' : 'activado'} correctamente.`);
        },
        error: () => this.feedback.set('No se pudo actualizar el estado del usuario.'),
      });
  }

  limpiarFeedback() {
    this.feedback.set(null);
  }
}
