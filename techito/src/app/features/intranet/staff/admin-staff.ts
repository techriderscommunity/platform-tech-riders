import { ChangeDetectionStrategy, Component, DestroyRef, computed, inject, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { RouterLink } from '@angular/router';
import { UiButton } from '@shared/ui/button/button';
import { UiSelect, UiSelectOption } from '@shared/ui/select/select';
import { catchError, finalize } from 'rxjs/operators';
import { of } from 'rxjs';
import { StaffItem } from './models/staff-governance.models';
import { StaffGovernanceService } from './services/staff-governance.service';

@Component({
  selector: 'app-admin-staff',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [RouterLink, UiButton, UiSelect],
  templateUrl: './admin-staff.html',
  styleUrl: './admin-staff.scss'
})
export class AdminStaff {
  private readonly staffGovernanceService = inject(StaffGovernanceService);
  private readonly destroyRef = inject(DestroyRef);

  readonly feedback = signal<string | null>(null);
  readonly loading = signal(false);
  readonly savingUserId = signal<string | null>(null);
  readonly roleCatalog = signal<string[]>([]);

  readonly staff = signal<StaffItem[]>([]);

  readonly activeCount = computed(() => this.staff().filter(s => s.estado === 'activo').length);
  readonly superAdminCount = computed(() => this.staff().filter(s => s.roles.includes('superadmin') && s.estado === 'activo').length);
  readonly roleOptions = computed<UiSelectOption[]>(() =>
    this.roleCatalog().map(role => ({ label: role, value: role })),
  );

  constructor() {
    this.loadGovernanceData();
  }

  loadGovernanceData() {
    this.loading.set(true);
    this.staffGovernanceService.getGovernanceData()
      .pipe(
        catchError(() => {
          this.feedback.set('No se pudo cargar el panel de gobierno. Revisa permisos superadmin.');
          return of({ staff: [], roles: [] as string[] });
        }),
        finalize(() => this.loading.set(false)),
        takeUntilDestroyed(this.destroyRef),
      )
      .subscribe(({ staff, roles }) => {
        this.roleCatalog.set(roles);
        this.staff.set(staff);
      });
  }

  agregarStaff() {
    this.feedback.set('Alta de usuario en iteración siguiente. En esta fase se habilita gobierno de roles y estado.');
  }

  onPrimaryRoleChange(id: string, event: Event) {
    const selected = (event.target as HTMLSelectElement).value;
    this.onPrimaryRoleValueChange(id, selected);
  }

  onPrimaryRoleValueChange(id: string, selected: string) {
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
    this.staffGovernanceService.updateEstado(id, active)
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
    this.staffGovernanceService.updateRoles(id, primaryRole, roles)
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


