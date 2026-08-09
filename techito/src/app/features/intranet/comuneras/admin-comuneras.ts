import { ChangeDetectionStrategy, Component, computed, signal, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { UiButton } from '@shared/ui/button/button';
import { UiModal } from '@shared/ui/modal/modal';
import { UiSelect, UiSelectOption } from '@shared/ui/select/select';
import { UiTextField } from '@shared/ui/text-field/text-field';
import { UiTextarea } from '@shared/ui/textarea/textarea';
import {
  CommunityPartner,
  CommunityPartnerStatus,
} from '../../comuneras/models/community-partner.models';
import { CommunityPartnersStore } from '../../comuneras/services/community-partners.store';

@Component({
  selector: 'app-admin-comuneras',
  standalone: true,
  imports: [RouterLink, UiButton, UiModal, UiSelect, UiTextField, UiTextarea],
  templateUrl: './admin-comuneras.html',
  styleUrl: './admin-comuneras.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AdminComuneras {
  private readonly store = inject(CommunityPartnersStore);

  readonly feedback = signal<string | null>(null);
  readonly showEditModal = signal(false);
  readonly selected = signal<CommunityPartner | null>(null);
  readonly editShortDescription = signal('');
  readonly editMission = signal('');
  readonly searchTerm = signal('');
  readonly statusFilter = signal<'all' | CommunityPartnerStatus>('all');

  readonly items = this.store.allPartners;
  readonly filteredItems = computed(() => {
    const term = this.searchTerm().trim().toLowerCase();
    const status = this.statusFilter();

    return this.items().filter(item => {
      const matchesStatus = status === 'all' || item.status === status;
      const matchesTerm = !term
        || item.name.toLowerCase().includes(term)
        || item.contactName.toLowerCase().includes(term)
        || item.contactEmail.toLowerCase().includes(term)
        || item.topics.some(topic => topic.toLowerCase().includes(term));

      return matchesStatus && matchesTerm;
    });
  });

  readonly statusOptions: UiSelectOption[] = [
    { label: 'Todos los estados', value: 'all' },
    { label: 'Pendiente', value: 'pending' },
    { label: 'En revisión', value: 'review' },
    { label: 'Información solicitada', value: 'more-info' },
    { label: 'Aprobada', value: 'approved' },
    { label: 'Rechazada', value: 'rejected' },
    { label: 'Suspendida', value: 'suspended' },
  ];

  readonly pendingCount = computed(() =>
    this.items().filter(item => item.status === 'pending' || item.status === 'review').length,
  );

  openEdit(item: CommunityPartner): void {
    this.selected.set(item);
    this.editShortDescription.set(item.shortDescription);
    this.editMission.set(item.mission);
    this.showEditModal.set(true);
  }

  closeEdit(): void {
    this.showEditModal.set(false);
    this.selected.set(null);
  }

  applyStatus(item: CommunityPartner, status: CommunityPartnerStatus): void {
    this.store.updateStatus(item.id, status);
    const label = this.toStatusLabel(status);
    this.feedback.set(`Estado actualizado a ${label} para ${item.name}.`);
  }

  saveEdit(): void {
    const selected = this.selected();
    if (!selected) {
      return;
    }

    const nextDescription = this.editShortDescription().trim();
    const nextMission = this.editMission().trim();

    if (!nextDescription || !nextMission) {
      this.feedback.set('Completa descripción corta y misión para guardar cambios.');
      return;
    }

    this.store.updatePartner(selected.id, {
      shortDescription: nextDescription,
      mission: nextMission,
    });

    this.feedback.set(`Comuñera ${selected.name} actualizada.`);
    this.closeEdit();
  }

  statusClass(status: CommunityPartnerStatus): string {
    return `estado-${status}`;
  }

  toStatusLabel(status: CommunityPartnerStatus): string {
    const labels: Record<CommunityPartnerStatus, string> = {
      pending: 'Pendiente',
      review: 'En revisión',
      'more-info': 'Información solicitada',
      approved: 'Aprobada',
      rejected: 'Rechazada',
      suspended: 'Suspendida',
    };

    return labels[status];
  }

  formatDate(date?: Date): string {
    if (!date) {
      return 'N/D';
    }

    return new Date(date).toLocaleDateString('es-ES');
  }

  onStatusFilterChange(value: string): void {
    if (
      value === 'all'
      || value === 'pending'
      || value === 'review'
      || value === 'more-info'
      || value === 'approved'
      || value === 'rejected'
      || value === 'suspended'
    ) {
      this.statusFilter.set(value);
    }
  }
}
