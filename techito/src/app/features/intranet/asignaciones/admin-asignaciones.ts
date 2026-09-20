import { ChangeDetectionStrategy, Component, DestroyRef, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { UiButton } from '@shared/ui/button/button';
import { UiSelect, UiSelectOption } from '@shared/ui/select/select';
import { catchError, finalize, of } from 'rxjs';
import { AssignmentsService } from '@core/assignments/assignments.service';
import { AssignmentCandidateApi } from '@core/assignments/assignments.models';
import { SkillsService } from '@core/skills/skills.service';
import { SkillApi } from '@core/skills/skills.models';
import { PreferencesService } from '@core/preferences/preferences.service';
import { PreferenceDimension } from '@core/preferences/preferences.models';

/**
 * Sugerencia de candidatos para asignar a una iniciativa (sesión/evento/FPTour), combinando
 * skills + disponibilidad + histórico de participación. La decisión final de asignar sigue
 * siendo manual: se realiza desde el módulo correspondiente (Sesiones, Eventos, FPTour).
 */
@Component({
  selector: 'app-admin-asignaciones',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [RouterLink, UiButton, UiSelect],
  templateUrl: './admin-asignaciones.html',
  styleUrl: './admin-asignaciones.scss',
})
export class AdminAsignaciones {
  private readonly assignmentsService = inject(AssignmentsService);
  private readonly skillsService = inject(SkillsService);
  private readonly preferencesService = inject(PreferencesService);
  private readonly destroyRef = inject(DestroyRef);

  readonly feedback = signal<string | null>(null);
  readonly loading = signal(false);
  readonly candidates = signal<AssignmentCandidateApi[]>([]);

  readonly skillOptions = signal<UiSelectOption[]>([{ label: 'Cualquier skill', value: '' }]);
  readonly availabilityOptions = signal<UiSelectOption[]>([{ label: 'Cualquier disponibilidad', value: '' }]);
  readonly selectedSkillId = signal('');
  readonly selectedAvailabilityValueId = signal('');

  constructor() {
    this.loadCatalogs();
    this.search();
  }

  private loadCatalogs() {
    this.skillsService.getCatalog()
      .pipe(catchError(() => of([] as SkillApi[])), takeUntilDestroyed(this.destroyRef))
      .subscribe(skills => this.skillOptions.set([
        { label: 'Cualquier skill', value: '' },
        ...skills.map(s => ({ label: s.Name, value: s.Id })),
      ]));

    this.preferencesService.getCatalog()
      .pipe(catchError(() => of([] as PreferenceDimension[])), takeUntilDestroyed(this.destroyRef))
      .subscribe(dimensions => {
        const availability = dimensions.find(d => d.code === 'availability');
        this.availabilityOptions.set([
          { label: 'Cualquier disponibilidad', value: '' },
          ...(availability?.values ?? []).map(v => ({ label: v.name, value: v.id })),
        ]);
      });
  }

  updateSelectedSkillId(value: string) {
    this.selectedSkillId.set(value);
  }

  updateSelectedAvailabilityValueId(value: string) {
    this.selectedAvailabilityValueId.set(value);
  }

  search() {
    this.loading.set(true);
    this.assignmentsService.getCandidates(this.selectedSkillId() || undefined, this.selectedAvailabilityValueId() || undefined)
      .pipe(
        catchError(() => {
          this.feedback.set('No se pudieron cargar los candidatos.');
          return of([] as AssignmentCandidateApi[]);
        }),
        finalize(() => this.loading.set(false)),
        takeUntilDestroyed(this.destroyRef),
      )
      .subscribe(items => this.candidates.set(items));
  }

  limpiarFeedback() {
    this.feedback.set(null);
  }
}
