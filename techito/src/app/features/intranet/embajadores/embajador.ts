import { ChangeDetectionStrategy, Component, DestroyRef, computed, inject, signal } from '@angular/core';
import { DatePipe, NgClass } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { takeUntilDestroyed, toObservable } from '@angular/core/rxjs-interop';
import { catchError, of, switchMap, tap } from 'rxjs';
import { AuthService } from '@core/auth/auth.service';
import { environment } from '@env/environment';
import { EmbajadoresService } from './services/embajadores.service';
import { Embajador } from './models/embajadores.models';
import { UiTextField  } from '@shared/ui/text-field/text-field';
import { UiSelect, UiSelectOption  } from '@shared/ui/select/select';
import { UiButton  } from '@shared/ui/button/button';
import { UiTextarea } from '@shared/ui/textarea/textarea';

interface AmbassadorPortalApi {
  Email: string;
  Bio: string;
  Specialties: string;
  Availability: string;
}

@Component({
  selector: 'app-embajador',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [FormsModule, NgClass, DatePipe, UiTextField, UiSelect, UiButton, UiTextarea],
  templateUrl: './embajador.html',
  styleUrl: './embajador.scss'
})
export class EmbajadorComponent {
  private readonly http = inject(HttpClient);
  private readonly authService = inject(AuthService);
  private readonly embajadoresService = inject(EmbajadoresService);
  private readonly destroyRef = inject(DestroyRef);
  private readonly baseUrl = environment.apiUrl;

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

  readonly estados = [
    { label: 'Todos', value: '' },
    { label: 'Activos', value: 'activo' },
    { label: 'Desactivados', value: 'desactivado' },
    { label: 'Pendientes', value: 'pendiente' }
  ];

  readonly estadoOptions: UiSelectOption[] = this.estados.map(estado => ({
    label: estado.label,
    value: estado.value
  }));

  readonly availabilityOptions: UiSelectOption[] = [
    { label: 'Baja disponibilidad', value: '1 bloque semanal' },
    { label: 'Disponibilidad media', value: '2 o 3 bloques semanales' },
    { label: 'Alta disponibilidad', value: '4 o más bloques semanales' }
  ];

  readonly query = computed(() => ({
    estado: this.searchStatus()
  }));

  constructor() {
    this.hydrateDraftFromLocalStorage();

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
          const matchedByEmail = this.tryFindEmbajadorByDraftEmail(result.items);
          this.selectedEmbajadorId.set(matchedByEmail?.id ?? result.items[0].id);
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
    const payload = {
      userKey: this.resolveUserKey(),
      email: this.resolveCurrentEmail(),
      bio: this.bio(),
      specialties: this.especialidades(),
      availability: this.disponibilidad(),
    };

    this.http.put<AmbassadorPortalApi>(`${this.baseUrl}/intranet/ambassador-profile`, payload)
      .pipe(
        tap(() => {
          this.persistPortalLocally();
          this.success.set('Cambios guardados en backend y en caché local.');
        }),
        catchError(() => {
          this.persistPortalLocally();
          this.success.set('Cambios guardados solo en caché local; el backend no respondió.');
          return of(null);
        })
      )
      .subscribe();
  }

  private hydrateDraftFromLocalStorage() {
    if (typeof localStorage === 'undefined') {
      return;
    }

    const draft = localStorage.getItem('techriders.mvp.ambassadorDraft');
    const portal = localStorage.getItem('techriders.mvp.ambassadorPortal');

    if (draft) {
      try {
        const parsed = JSON.parse(draft) as { motivation?: string; audience?: string; organization?: string | null };
        if (parsed.motivation) {
          this.bio.set(parsed.motivation);
        }
        if (parsed.audience || parsed.organization) {
          const especialidades = [parsed.audience, parsed.organization].filter(Boolean).join(' · ');
          if (especialidades) {
            this.especialidades.set(especialidades);
          }
        }
      }
      catch {
        // Ignore malformed local MVP draft data.
      }
    }

    if (portal) {
      try {
        const parsed = JSON.parse(portal) as { bio?: string; especialidades?: string; disponibilidad?: string };
        if (parsed.bio) this.bio.set(parsed.bio);
        if (parsed.especialidades) this.especialidades.set(parsed.especialidades);
        if (parsed.disponibilidad) this.disponibilidad.set(parsed.disponibilidad);
      }
      catch {
        // Ignore malformed local MVP portal data.
      }
    }

    this.http.get<AmbassadorPortalApi>(`${this.baseUrl}/intranet/ambassador-profile`, {
      params: {
        userKey: this.resolveUserKey(),
        email: this.resolveCurrentEmail(),
      },
    })
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

  private tryFindEmbajadorByDraftEmail(items: Embajador[]): Embajador | undefined {
    if (typeof localStorage === 'undefined') {
      return undefined;
    }

    const draft = localStorage.getItem('techriders.mvp.ambassadorDraft');
    if (!draft) {
      return undefined;
    }

    try {
      const parsed = JSON.parse(draft) as { email?: string };
      if (!parsed.email) {
        return undefined;
      }
      return items.find(item => item.email.toLowerCase() === parsed.email?.toLowerCase());
    }
    catch {
      return undefined;
    }
  }


  private persistPortalLocally() {
    if (typeof localStorage === 'undefined') {
      return;
    }

    localStorage.setItem('techriders.mvp.ambassadorPortal', JSON.stringify({
      bio: this.bio(),
      especialidades: this.especialidades(),
      disponibilidad: this.disponibilidad(),
    }));
  }

  private resolveUserKey(): string {
    return this.authService.user()?.email || this.resolveCurrentEmail();
  }

  private resolveCurrentEmail(): string {
    return this.ambassadorActual()?.email || this.authService.user()?.email || 'local-user@techriders.local';
  }
}


