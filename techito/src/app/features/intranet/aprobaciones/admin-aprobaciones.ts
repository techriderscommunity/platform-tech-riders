import { ChangeDetectionStrategy, Component, DestroyRef, computed, inject, signal } from '@angular/core';
import { Observable } from 'rxjs';
import { RouterLink } from '@angular/router';
import { DatePipe } from '@angular/common';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { UiButton } from '@shared/ui/button/button';
import { UiSelect, UiSelectOption } from '@shared/ui/select/select';
import { catchError, finalize, of } from 'rxjs';
import { ApprovalsService } from '@core/approvals/approvals.service';
import { APPROVAL_TYPE_LABELS, ApprovalItem } from '@core/approvals/approvals.models';
import { CapabilityRequestService } from '@core/capability/capability-request.service';
import { OrganizationRelationsService } from '@core/gpf/gpf.service';
import { CommunityPartnerApplicationsService } from '@core/community-partners/community-partner-applications.service';
import { OrganizationsService } from '@core/organizations/organizations.service';

/**
 * Bandeja centralizada de aprobaciones: agrega los pendientes de todos los módulos y despacha
 * la acción aprobar/rechazar al servicio específico de cada tipo (no hay workflow nuevo aquí).
 */
@Component({
  selector: 'app-admin-aprobaciones',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [RouterLink, UiButton, UiSelect, DatePipe],
  templateUrl: './admin-aprobaciones.html',
  styleUrl: './admin-aprobaciones.scss',
})
export class AdminAprobaciones {
  private readonly approvalsService = inject(ApprovalsService);
  private readonly capabilityRequestService = inject(CapabilityRequestService);
  private readonly organizationRelationsService = inject(OrganizationRelationsService);
  private readonly communityPartnerApplicationsService = inject(CommunityPartnerApplicationsService);
  private readonly organizationsService = inject(OrganizationsService);
  private readonly destroyRef = inject(DestroyRef);

  readonly feedback = signal<string | null>(null);
  readonly loading = signal(false);
  readonly resolvingId = signal<string | null>(null);
  readonly items = signal<ApprovalItem[]>([]);
  readonly typeFilter = signal('');

  readonly typeLabels = APPROVAL_TYPE_LABELS;
  readonly typeOptions: UiSelectOption[] = [
    { label: 'Todos los tipos', value: '' },
    { label: 'Solicitudes de rol', value: 'CapabilityRequest' },
    { label: 'Relaciones con organización', value: 'OrganizationRelation' },
    { label: 'Comuneras', value: 'CommunityPartnerApplication' },
    { label: 'Centros', value: 'OrganizationRequest' },
  ];

  readonly filteredItems = computed(() => {
    const type = this.typeFilter();
    return type ? this.items().filter(i => i.type === type) : this.items();
  });

  constructor() {
    this.load();
  }

  updateTypeFilter(value: string) {
    this.typeFilter.set(value);
  }

  load() {
    this.loading.set(true);
    this.approvalsService.getPending()
      .pipe(
        catchError(() => {
          this.feedback.set('No se pudo cargar la bandeja de aprobaciones.');
          return of([] as ApprovalItem[]);
        }),
        finalize(() => this.loading.set(false)),
        takeUntilDestroyed(this.destroyRef),
      )
      .subscribe(items => this.items.set(items));
  }

  resolve(item: ApprovalItem, action: 'approve' | 'reject') {
    this.resolvingId.set(item.id);
    const request$ = this.dispatch(item, action);

    request$
      .pipe(
        finalize(() => this.resolvingId.set(null)),
        takeUntilDestroyed(this.destroyRef),
      )
      .subscribe({
        next: () => {
          this.items.update(list => list.filter(i => i.id !== item.id));
          this.feedback.set(action === 'approve' ? 'Elemento aprobado.' : 'Elemento rechazado.');
        },
        error: (error: unknown) => {
          const message = (error as { error?: { Message?: string } })?.error?.Message ?? 'No se pudo procesar la acción.';
          this.feedback.set(message);
        },
      });
  }

  private dispatch(item: ApprovalItem, action: 'approve' | 'reject'): Observable<unknown> {
    switch (item.type) {
      case 'CapabilityRequest':
        return action === 'approve' ? this.capabilityRequestService.approve(item.id) : this.capabilityRequestService.reject(item.id);
      case 'OrganizationRelation':
        return action === 'approve' ? this.organizationRelationsService.approve(item.id) : this.organizationRelationsService.reject(item.id);
      case 'CommunityPartnerApplication':
        return action === 'approve' ? this.communityPartnerApplicationsService.approve(item.id) : this.communityPartnerApplicationsService.reject(item.id);
      case 'OrganizationRequest':
        return action === 'approve' ? this.organizationsService.activate(item.id) : this.organizationsService.suspend(item.id);
      default:
        return of(null);
    }
  }

  limpiarFeedback() {
    this.feedback.set(null);
  }
}
