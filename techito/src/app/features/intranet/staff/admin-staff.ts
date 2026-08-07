import { HttpClient } from '@angular/common/http';
import { ChangeDetectionStrategy, Component, DestroyRef, computed, inject, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { RouterLink } from '@angular/router';
import { environment } from '@env/environment';
import { UiButton  } from '@shared/ui/button/button';
import { catchError, finalize } from 'rxjs/operators';
import { forkJoin, of } from 'rxjs';

interface StaffItem {
  id: string;
  nombre: string;
  email: string;
  rolPrincipal: string;
  roles: string[];
  estado: 'activo' | 'inactivo';
}

interface GovernanceUserResponse {
  id: string;
  email: string;
  name: string;
  primaryRole: string;
  active: boolean;
  roles: string[];
}

interface GovernanceRoleResponse {
  id: string;
  name: string;
  description?: string | null;
  active: boolean;
}

@Component({
  selector: 'app-admin-staff',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [RouterLink, UiButton],
  templateUrl: './admin-staff.html',
  styleUrl: './admin-staff.scss'
})
export class AdminStaff {
  private readonly http = inject(HttpClient);
  private readonly destroyRef = inject(DestroyRef);
  private readonly baseUrl = environment.apiUrl;

  readonly feedback = signal<string | null>(null);
  readonly loading = signal(false);
  readonly savingUserId = signal<string | null>(null);
  readonly roleCatalog = signal<string[]>([]);

  readonly staff = signal<StaffItem[]>([]);

  readonly activeCount = computed(() => this.staff().filter(s => s.estado === 'activo').length);
  readonly superAdminCount = computed(() => this.staff().filter(s => s.roles.includes('superadmin') && s.estado === 'activo').length);

  constructor() {
    this.loadGovernanceData();
  }

  loadGovernanceData() {
    this.loading.set(true);
    forkJoin({
      users: this.http.get<GovernanceUserResponse[]>(`${this.baseUrl}/admin/staff/usuarios`),
      roles: this.http.get<GovernanceRoleResponse[]>(`${this.baseUrl}/admin/staff/roles`),
    })
      .pipe(
        catchError(() => {
          this.feedback.set('No se pudo cargar el panel de gobierno. Revisa permisos superadmin.');
          return of({ users: [], roles: [] as GovernanceRoleResponse[] });
        }),
        finalize(() => this.loading.set(false)),
        takeUntilDestroyed(this.destroyRef),
      )
      .subscribe(({ users, roles }) => {
        this.roleCatalog.set(roles.map(role => role.name));
        this.staff.set(users.map(user => ({
          id: user.id,
          nombre: user.name,
          email: user.email,
          rolPrincipal: user.primaryRole,
          roles: user.roles ?? [user.primaryRole],
          estado: user.active ? 'activo' : 'inactivo',
        })));
      });
  }

  agregarStaff() {
    this.feedback.set('Alta de usuario en iteración siguiente. En esta fase se habilita gobierno de roles y estado.');
  }

  onPrimaryRoleChange(id: string, event: Event) {
    const selected = (event.target as HTMLSelectElement).value;
    const member = this.staff().find(item => item.id === id);
    if (!member || !selected) return;

    const nextRoles = member.roles.includes(selected)
      ? member.roles
      : [...member.roles, selected];

    this.saveRoles(member.id, selected, nextRoles);
  }

  toggleRole(id: string, role: string, checked: boolean) {
    const member = this.staff().find(item => item.id === id);
    if (!member) return;

    let nextRoles = checked
      ? Array.from(new Set([...member.roles, role]))
      : member.roles.filter(current => current !== role);

    if (!nextRoles.length) {
      this.feedback.set('Un usuario debe mantener al menos un rol activo.');
      return;
    }

    let nextPrimaryRole = member.rolPrincipal;
    if (!nextRoles.includes(nextPrimaryRole)) {
      nextPrimaryRole = nextRoles[0];
    }

    this.saveRoles(member.id, nextPrimaryRole, nextRoles);
  }

  desactivarStaff(id: string) {
    const member = this.staff().find(item => item.id === id);
    if (!member) return;

    const actionLabel = member.estado === 'activo' ? 'desactivar' : 'reactivar';
    if (!confirm(`Vas a ${actionLabel} a ${member.nombre}. ¿Confirmas?`)) {
      return;
    }

    const active = member.estado !== 'activo';
    this.savingUserId.set(id);
    this.http.put(`${this.baseUrl}/admin/staff/usuarios/${id}/estado`, { activo: active })
      .pipe(
        finalize(() => this.savingUserId.set(null)),
        takeUntilDestroyed(this.destroyRef),
      )
      .subscribe({
        next: () => {
          this.staff.update(items =>
            items.map(item => item.id === id ? { ...item, estado: active ? 'activo' : 'inactivo' } : item),
          );
          this.feedback.set(`Usuario ${active ? 'reactivado' : 'desactivado'} correctamente.`);
        },
        error: (error) => {
          const message = error?.error?.error ?? 'No se pudo actualizar el estado del usuario.';
          this.feedback.set(message);
        },
      });
  }

  isRoleAssigned(member: StaffItem, role: string): boolean {
    return member.roles.includes(role);
  }

  private saveRoles(id: string, primaryRole: string, roles: string[]) {
    this.savingUserId.set(id);
    this.http.put(`${this.baseUrl}/admin/staff/usuarios/${id}/roles`, {
      primaryRole,
      roles,
    })
      .pipe(
        finalize(() => this.savingUserId.set(null)),
        takeUntilDestroyed(this.destroyRef),
      )
      .subscribe({
        next: () => {
          this.staff.update(items =>
            items.map(item => item.id === id ? { ...item, rolPrincipal: primaryRole, roles: [...roles] } : item),
          );
          this.feedback.set('Roles actualizados correctamente.');
        },
        error: (error) => {
          const message = error?.error?.error ?? 'No se pudieron actualizar los roles.';
          this.feedback.set(message);
          this.loadGovernanceData();
        },
      });
  }

  limpiarFeedback() {
    this.feedback.set(null);
  }
}


