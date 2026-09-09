import { ChangeDetectionStrategy, Component, DestroyRef, computed, inject, signal } from '@angular/core';
import { DatePipe, NgClass } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { takeUntilDestroyed, toObservable } from '@angular/core/rxjs-interop';
import { catchError, of, switchMap, tap } from 'rxjs';
import { AuthService } from '@core/auth/auth.service';
import { PublicContentService } from '@core/content/public-content.service';
import { EmbajadoresService } from './services/embajadores.service';
import { AmbassadorPortalApi, Embajador } from './models/embajadores.models';
import { UiTextField  } from '@shared/ui/text-field/text-field';
import { UiSelect, UiSelectOption  } from '@shared/ui/select/select';
import { UiButton  } from '@shared/ui/button/button';
import { UiTextarea } from '@shared/ui/textarea/textarea';

@Component({
  selector: 'app-embajador',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [FormsModule, NgClass, DatePipe, UiTextField, UiSelect, UiButton, UiTextarea],
  templateUrl: './embajador.html',
  styleUrl: './embajador.scss'
})
export class EmbajadorComponent {
  private readonly authService = inject(AuthService);
  private readonly publicContentService = inject(PublicContentService);
  private readonly embajadoresService = inject(EmbajadoresService);
  private readonly destroyRef = inject(DestroyRef);

  readonly searchName = signal('');
  readonly searchStatus = signal('pendiente');
  readonly loading = signal(false);
  readonly error = signal<string | null>(null);
  readonly embajadores = signal<Embajador[]>([]);
  readonly success = signal<string | null>(null);
  readonly selectedEmbajadorId = signal<string>('');
  readonly bio = signal('Me interesa aportar sesiones prácticas, orientación y comunidad alrededor de tecnología real.');
  readonly especialidades = signal('Cloud, desarrollo web, mentoring, empleabilidad');
  readonly disponibilidad = signal('Martes y jueves por la tarde; viernes por la mañana con aviso previo.');

  estados: Array<{ label: string; value: string }> = [];
  estadoOptions: UiSelectOption[] = [];
  availabilityOptions: UiSelectOption[] = [];

  readonly query = computed(() => ({
    estado: this.searchStatus()
  }));

  constructor() {
    this.publicContentService
      .getPublicContent()
      .pipe(
        tap((content) => {
          this.estados = content.intranet.ambassadorStatusOptions.map((option) => ({
            label: option.label,
            value: option.value,
          }));
          this.estadoOptions = content.intranet.ambassadorStatusOptions;
          this.availabilityOptions = content.intranet.ambassadorAvailabilityOptions;
        }),
        catchError(() => of(null)),
        takeUntilDestroyed(this.destroyRef)
      )
      .subscribe();

    this.hydratePortalFromBackend();

    toObservable(this.query)
      .pipe(
        tap(() => {
          this.loading.set(true);
          this.error.set(null);
        }),
        switchMap(({ estado }) => this.embajadoresService.getEmbajadores(1, 100, estado || undefined).pipe(
          catchError(() => {
            this.error.set('No se pudieron cargar los embajadores.');
            return of({ items: [] as Embajador[] });
          })
        )),
        takeUntilDestroyed(this.destroyRef)
      )
      .subscribe(result => {
        this.embajadores.set(result.items);
        if (!this.selectedEmbajadorId() && result.items.length > 0) {
          this.selectedEmbajadorId.set(result.items[0].id);
        }
        this.loading.set(false);
      });
  }

  readonly embajadoresFiltrados = computed(() => {
    const name = this.searchName().toLowerCase();

    return this.embajadores().filter(e => {
      const matchName = e.nombre.toLowerCase().includes(name);
      return matchName;
    });
  });

  readonly ambassadorOptions = computed<UiSelectOption[]>(() => this.embajadoresFiltrados().map(embajador => ({
    label: embajador.nombre,
    value: embajador.id
  })));

  readonly ambassadorActual = computed(() => {
    const embajadorId = this.selectedEmbajadorId();
    return this.embajadores().find(item => item.id === embajadorId) ?? this.embajadores()[0] ?? null;
  });

  readonly sesionesPendientes = computed(() => {
    const actual = this.ambassadorActual();
    if (!actual) {
      return [] as Array<{ titulo: string; origen: string; fecha: string }>;
    }

    return [
      {
        titulo: 'Sesión de orientación sobre primeros pasos en tech',
        origen: 'IES colaborador · nivel básico',
        fecha: '2026-09-18'
      },
      {
        titulo: 'Talk de empleabilidad junior y portfolio',
        origen: 'Comunidad Tech Riders · evento abierto',
        fecha: '2026-09-25'
      }
    ];
  });

  readonly sesionesAsignadas = computed(() => {
    const actual = this.ambassadorActual();
    if (!actual) {
      return [] as Array<{ titulo: string; fecha: string; estado: string }>;
    }

    return [
      {
        titulo: 'Workshop de Git y trabajo en equipo',
        fecha: '2026-10-03',
        estado: 'Confirmada'
      },
      {
        titulo: 'Charla de salidas profesionales en cloud',
        fecha: '2026-10-15',
        estado: 'Planificada'
      }
    ];
  });

  readonly historicoParticipacion = computed(() => {
    const actual = this.ambassadorActual();
    if (!actual) {
      return [] as Array<{ titulo: string; fecha: string; impacto: string }>;
    }

    return [
      {
        titulo: 'Panel Tech Riders Summit 2026',
        fecha: '2026-05-20',
        impacto: '120 asistentes'
      },
      {
        titulo: 'Sesión FP Tour sobre APIs modernas',
        fecha: '2026-04-11',
        impacto: '3 centros participantes'
      }
    ];
  });

  normalizarEstado(estado: string): 'activo' | 'desactivado' | 'pendiente' {
    const normalized = estado.toLowerCase();
    if (normalized === 'activo') return 'activo';
    if (normalized === 'desactivado') return 'desactivado';
    return 'pendiente';
  }

  formatUltimaActividad(value: string | null): string {
    if (!value) return '-';
    return new Intl.DateTimeFormat('es-ES', {
      day: '2-digit',
      month: '2-digit',
      year: 'numeric'
    }).format(new Date(value));
  }

  seleccionarEmbajador(value: string) {
    this.selectedEmbajadorId.set(value);
    this.success.set(null);
  }

  updateBio(value: string) {
    this.bio.set(value);
  }

  updateEspecialidades(value: string) {
    this.especialidades.set(value);
  }

  updateDisponibilidad(value: string) {
    this.disponibilidad.set(value);
  }

  guardarPortalAmbassador() {
    if (!this.authService.user() && !this.ambassadorActual()?.email) {
      this.success.set('Debes iniciar sesión para guardar el portal ambassador.');
      return;
    }

    const payload = {
      userKey: this.resolveUserKey(),
      email: this.resolveCurrentEmail(),
      bio: this.bio(),
      specialties: this.especialidades(),
      availability: this.disponibilidad(),
    };

    this.embajadoresService.updateAmbassadorPortalProfile(payload)
      .pipe(
        tap(() => {
          this.success.set('Cambios guardados en backend.');
        }),
        catchError(() => {
          this.success.set('No se pudieron guardar los cambios. Intenta de nuevo.');
          return of(null);
        })
      )
      .subscribe();
  }

  private hydratePortalFromBackend() {
    this.embajadoresService.getAmbassadorPortalProfile(this.resolveUserKey(), this.resolveCurrentEmail())
      .pipe(
        tap(profile => {
          if (profile.Bio) this.bio.set(profile.Bio);
          if (profile.Specialties) this.especialidades.set(profile.Specialties);
          if (profile.Availability) this.disponibilidad.set(profile.Availability);
        }),
        catchError(() => of(null))
      )
      .subscribe();
  }

  private resolveUserKey(): string {
    return this.authService.user()?.email || this.resolveCurrentEmail();
  }

  private resolveCurrentEmail(): string {
    const email = this.ambassadorActual()?.email || this.authService.user()?.email;
    if (!email) {
      throw new Error('Authenticated user email is required.');
    }

    return email;
  }
}


