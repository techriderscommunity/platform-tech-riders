import { ChangeDetectionStrategy, Component, DestroyRef, computed, inject, signal } from '@angular/core';
import { DatePipe } from '@angular/common';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { RouterLink } from '@angular/router';
import { UiButton } from '@shared/ui/button/button';
import { UiSelect, UiSelectOption } from '@shared/ui/select/select';
import { UiTextField } from '@shared/ui/text-field/text-field';
import { catchError, finalize } from 'rxjs/operators';
import { of } from 'rxjs';
import { StaffItem } from './models/staff-governance.models';
import { StaffGovernanceService } from './services/staff-governance.service';
import { CapabilityRequestService } from '@core/capability/capability-request.service';
import { CapabilityRequestItem } from '@core/capability/capability-request.models';
import { GpfLinkService, OrganizationRelationsService } from '@core/gpf/gpf.service';
import { GpfPersonLinkItem, PersonOrganizationItem } from '@core/gpf/gpf.models';
import { OrganizationsService } from '@core/organizations/organizations.service';

@Component({
  selector: 'app-admin-staff',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [RouterLink, UiButton, UiSelect, UiTextField, DatePipe],
  templateUrl: './admin-staff.html',
  styleUrl: './admin-staff.scss'
})
export class AdminStaff {
  private readonly staffGovernanceService = inject(StaffGovernanceService);
  private readonly capabilityRequestService = inject(CapabilityRequestService);
  private readonly gpfLinkService = inject(GpfLinkService);
  private readonly organizationRelationsService = inject(OrganizationRelationsService);
  private readonly organizationsService = inject(OrganizationsService);
  private readonly destroyRef = inject(DestroyRef);

  readonly feedback = signal<string | null>(null);
  readonly loading = signal(false);
  readonly savingUserId = signal<string | null>(null);
  readonly roleCatalog = signal<string[]>([]);

  readonly staff = signal<StaffItem[]>([]);
  readonly pendingRequests = signal<CapabilityRequestItem[]>([]);
  readonly resolvingRequestId = signal<string | null>(null);

  readonly capabilityHistory = signal<CapabilityRequestItem[]>([]);
  readonly capabilityHistoryFilter = signal('Ambassador');
  readonly revokingId = signal<string | null>(null);
  readonly capabilityFilterOptions: UiSelectOption[] = [
    { label: 'Ambassador', value: 'Ambassador' },
    { label: 'Staff', value: 'Staff' },
    { label: 'Community Leader', value: 'Community Leader' },
    { label: 'Center', value: 'Center' },
    { label: 'Community Partner', value: 'Community Partner' },
  ];


  readonly gpfLinks = signal<GpfPersonLinkItem[]>([]);
  readonly gpfLinkUserId = signal('');
  readonly gpfLinkCodUnico = signal('');
  readonly gpfLinkSaving = signal(false);
  readonly pendingOrgRelations = signal<PersonOrganizationItem[]>([]);
  readonly resolvingOrgRelationId = signal<string | null>(null);
  readonly organizationType = signal('Empresa');
  readonly organizationName = signal('');
  readonly organizationSaving = signal(false);

  readonly activeCount = computed(() => this.staff().filter(s => s.estado === 'activo').length);
  readonly adminCount = computed(() => this.staff().filter(s => s.roles.includes('admin') && s.estado === 'activo').length);
  readonly roleOptions = computed<UiSelectOption[]>(() =>
    this.roleCatalog().map(role => ({ label: role, value: role })),
  );
  readonly staffUserOptions = computed<UiSelectOption[]>(() =>
    this.staff().map(s => ({ label: `${s.nombre} (${s.email})`, value: s.id })),
  );

  constructor() {
    this.loadGovernanceData();
    this.loadPendingRequests();
    this.loadGpfLinks();
    this.loadPendingOrgRelations();
    this.loadCapabilityHistory();
  }

  loadGovernanceData() {
    this.loading.set(true);
    this.staffGovernanceService.getGovernanceData()
      .pipe(
        catchError(() => {
          this.feedback.set('No se pudo cargar el panel de gobierno. Revisa permisos de administrador.');
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

  loadPendingRequests() {
    this.capabilityRequestService.getPending()
      .pipe(
        catchError(() => of([] as CapabilityRequestItem[])),
        takeUntilDestroyed(this.destroyRef),
      )
      .subscribe(items => this.pendingRequests.set(items));
  }

  resolveRequest(id: string, action: 'approve' | 'reject') {
    this.resolvingRequestId.set(id);
    const request$ = action === 'approve'
      ? this.capabilityRequestService.approve(id)
      : this.capabilityRequestService.reject(id);

    request$
      .pipe(
        finalize(() => this.resolvingRequestId.set(null)),
        takeUntilDestroyed(this.destroyRef),
      )
      .subscribe({
        next: () => {
          this.pendingRequests.update(items => items.filter(item => item.id !== id));
          this.feedback.set(action === 'approve' ? 'Rol aprobado y otorgado.' : 'Solicitud rechazada.');
          this.loadGovernanceData();
        },
        error: (error) => {
          const message = error?.error?.Message ?? 'No se pudo procesar la solicitud.';
          this.feedback.set(message);
        },
      });
  }

  updateGpfLinkUserId(value: string) {
    this.gpfLinkUserId.set(value);
  }

  updateGpfLinkCodUnico(value: string) {
    this.gpfLinkCodUnico.set(value);
  }

  updateOrganizationType(value: string) {
    this.organizationType.set(value);
  }

  updateOrganizationName(value: string) {
    this.organizationName.set(value);
  }

  crearOrganizacion() {
    const name = this.organizationName().trim();
    if (!name) {
      this.feedback.set('Indica el nombre de la organización.');
      return;
    }

    this.organizationSaving.set(true);
    this.organizationsService.create({ organizationType: this.organizationType(), name })
      .pipe(
        finalize(() => this.organizationSaving.set(false)),
        takeUntilDestroyed(this.destroyRef),
      )
      .subscribe({
        next: () => {
          this.organizationName.set('');
          this.feedback.set('Organización creada correctamente.');
        },
        error: (error) => {
          this.feedback.set(error?.error?.Message ?? 'No se pudo crear la organización.');
        },
      });
  }

  loadGpfLinks() {
    this.gpfLinkService.getAll()
      .pipe(
        catchError(() => of([] as GpfPersonLinkItem[])),
        takeUntilDestroyed(this.destroyRef),
      )
      .subscribe(items => this.gpfLinks.set(items));
  }

  crearVinculoGpf() {
    if (!this.gpfLinkUserId() || !this.gpfLinkCodUnico()) {
      this.feedback.set('Selecciona una persona e indica el CodUnico.');
      return;
    }

    this.gpfLinkSaving.set(true);
    this.gpfLinkService.link(this.gpfLinkUserId(), this.gpfLinkCodUnico())
      .pipe(
        finalize(() => this.gpfLinkSaving.set(false)),
        takeUntilDestroyed(this.destroyRef),
      )
      .subscribe({
        next: () => {
          this.feedback.set('Vínculo GPF creado.');
          this.gpfLinkCodUnico.set('');
          this.loadGpfLinks();
        },
        error: (error) => {
          this.feedback.set(error?.error?.Message ?? 'No se pudo crear el vínculo.');
        },
      });
  }

  desvincularGpf(id: string) {
    if (!confirm('¿Desvincular este CodUnico de la persona?')) {
      return;
    }

    this.gpfLinkService.unlink(id)
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: () => {
          this.feedback.set('Vínculo GPF desactivado.');
          this.loadGpfLinks();
        },
        error: (error) => {
          this.feedback.set(error?.error?.Message ?? 'No se pudo desvincular.');
        },
      });
  }

  loadPendingOrgRelations() {
    this.organizationRelationsService.getPendingRelations()
      .pipe(
        catchError(() => of([] as PersonOrganizationItem[])),
        takeUntilDestroyed(this.destroyRef),
      )
      .subscribe(items => this.pendingOrgRelations.set(items));
  }

  resolveOrgRelation(id: string, action: 'approve' | 'reject') {
    this.resolvingOrgRelationId.set(id);
    const request$ = action === 'approve'
      ? this.organizationRelationsService.approve(id)
      : this.organizationRelationsService.reject(id);

    request$
      .pipe(
        finalize(() => this.resolvingOrgRelationId.set(null)),
        takeUntilDestroyed(this.destroyRef),
      )
      .subscribe({
        next: () => {
          this.pendingOrgRelations.update(items => items.filter(item => item.id !== id));
          this.feedback.set(action === 'approve' ? 'Relación con organización aprobada.' : 'Relación rechazada.');
        },
        error: (error) => {
          this.feedback.set(error?.error?.Message ?? 'No se pudo procesar la relación.');
        },
      });
  }

  updateCapabilityHistoryFilter(value: string) {
    this.capabilityHistoryFilter.set(value);
    this.loadCapabilityHistory();
  }

  loadCapabilityHistory() {
    this.capabilityRequestService.getHistory(this.capabilityHistoryFilter())
      .pipe(
        catchError(() => of([] as CapabilityRequestItem[])),
        takeUntilDestroyed(this.destroyRef),
      )
      .subscribe(items => this.capabilityHistory.set(items));
  }

  revokeCapability(id: string) {
    if (!confirm('¿Confirmas revocar esta capacidad? El rol de sistema asociado se retirará.')) {
      return;
    }

    this.revokingId.set(id);
    this.capabilityRequestService.revoke(id)
      .pipe(
        finalize(() => this.revokingId.set(null)),
        takeUntilDestroyed(this.destroyRef),
      )
      .subscribe({
        next: () => {
          this.feedback.set('Capacidad revocada correctamente.');
          this.loadCapabilityHistory();
        },
        error: (error) => {
          this.feedback.set(error?.error?.Message ?? 'No se pudo revocar la capacidad.');
        },
      });
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


