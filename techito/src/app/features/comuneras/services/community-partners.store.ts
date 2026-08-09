import { Injectable, computed, effect, signal } from '@angular/core';
import {
  CommunityPartner,
  CommunityPartnerApplication,
  CommunityPartnerStatus,
} from '../models/community-partner.models';
import { CommunityPartnerAnalyticsService } from './community-partner-analytics.service';

const INITIAL_PARTNERS: CommunityPartner[] = [
  {
    id: 'github-community-spain',
    name: 'GitHub Community Spain',
    logoUrl: 'assets/logo-dark.png',
    shortDescription: 'Comunidad para compartir prácticas, tooling y cultura de desarrollo colaborativo.',
    description:
      'Espacio de referencia para aprender sobre GitHub, flujos modernos de ingeniería y colaboración entre comunidades técnicas.',
    mission: 'Conectar personas y comunidades para construir software con mejores prácticas y aprendizaje continuo.',
    cityOrScope: 'Nacional · España',
    website: 'https://example.org/github-community-spain',
    linkedin: 'https://www.linkedin.com',
    x: 'https://x.com',
    youtube: 'https://www.youtube.com',
    topics: ['Open Source', 'Comunidad', 'Formación'],
    scope: 'national',
    contactName: 'Equipo GitHub Community Spain',
    contactEmail: 'hola@githubcommunityspain.org',
    requestedAt: new Date('2025-03-15'),
    memberCount: 1500,
    joinedAt: new Date('2025-04-10'),
    collaborations: ['Sesiones sobre colaboración y repositorios', 'Eventos con comunidades tech locales'],
    status: 'approved',
  },
  {
    id: 'commit-conf',
    name: 'Commit conf',
    logoUrl: 'assets/google.svg',
    shortDescription: 'Comunidad y conferencia orientada a ingeniería de software, arquitectura y cultura técnica.',
    description:
      'Espacio para compartir experiencias reales de desarrollo, liderazgo técnico y construcción de equipos sólidos.',
    mission: 'Crear un punto de encuentro de alto impacto para la comunidad de desarrollo.',
    cityOrScope: 'Nacional · España',
    website: 'https://example.org/commit-conf',
    linkedin: 'https://www.linkedin.com',
    x: 'https://x.com',
    topics: ['Comunidad', 'Empleo', 'Formación'],
    scope: 'national',
    contactName: 'Equipo Commit conf',
    contactEmail: 'hola@commitconf.org',
    requestedAt: new Date('2025-05-03'),
    memberCount: 900,
    joinedAt: new Date('2025-06-18'),
    collaborations: ['Evento colaborativo anual', 'Mentoring cruzado entre comunidades'],
    status: 'approved',
  },
  {
    id: 'global-azure-spain',
    name: 'Global Azure Spain',
    logoUrl: 'assets/microsoft.svg',
    shortDescription: 'Capítulo local de la jornada global Azure con enfoque en aprendizaje práctico y networking.',
    description:
      'Comunidad que conecta especialistas cloud para compartir conocimiento técnico sobre Azure y modernización.',
    mission: 'Acercar cloud y arquitectura moderna a más profesionales de forma abierta.',
    cityOrScope: 'Nacional · España',
    website: 'https://example.org/global-azure-spain',
    linkedin: 'https://www.linkedin.com',
    x: 'https://x.com',
    topics: ['Cloud', 'Formación', 'Comunidad'],
    scope: 'national',
    contactName: 'Equipo Global Azure Spain',
    contactEmail: 'hola@globalazurespain.org',
    requestedAt: new Date('2025-04-22'),
    memberCount: 1300,
    joinedAt: new Date('2025-05-28'),
    collaborations: ['Sesiones conjuntas de cloud', 'Participación en jornadas técnicas Tech Riders'],
    status: 'approved',
  },
  {
    id: 'sirviendo-codigo',
    name: 'SirviendoCódigo',
    logoUrl: 'assets/logo-light.png',
    shortDescription: 'Comunidad enfocada en desarrollo, calidad de código y crecimiento profesional.',
    description:
      'Comunidad abierta para compartir prácticas de ingeniería, testing y evolución de carrera tecnológica.',
    mission: 'Impulsar carreras técnicas sostenibles con código de calidad y colaboración entre pares.',
    cityOrScope: 'Nacional · España',
    website: 'https://example.org/sirviendo-codigo',
    linkedin: 'https://www.linkedin.com',
    instagram: 'https://www.instagram.com',
    topics: ['Comunidad', 'Open Source', 'Empleo'],
    scope: 'national',
    contactName: 'Equipo SirviendoCódigo',
    contactEmail: 'hola@sirviendocodigo.org',
    requestedAt: new Date('2025-06-10'),
    memberCount: 740,
    joinedAt: new Date('2025-07-20'),
    collaborations: ['Charlas técnicas colaborativas', 'Iniciativas de mentoring junior'],
    status: 'approved',
  },
  {
    id: 'women4tt',
    name: 'women4tt',
    logoUrl: 'assets/Staff_Azure.png',
    shortDescription: 'Comunidad para potenciar liderazgo, visibilidad y carrera tecnológica de mujeres.',
    description:
      'Red de apoyo y crecimiento con foco en liderazgo técnico, mentoring y oportunidades en el sector.',
    mission: 'Acelerar la presencia y liderazgo femenino en tecnología desde la comunidad.',
    cityOrScope: 'Nacional · España',
    website: 'https://example.org/women4tt',
    linkedin: 'https://www.linkedin.com',
    instagram: 'https://www.instagram.com',
    topics: ['Comunidad', 'Formación', 'Empleo'],
    scope: 'national',
    contactName: 'Equipo women4tt',
    contactEmail: 'hola@women4tt.org',
    requestedAt: new Date('2025-05-30'),
    memberCount: 820,
    joinedAt: new Date('2025-06-26'),
    collaborations: ['Sesiones de liderazgo y carrera', 'Acciones de visibilidad en eventos'],
    status: 'approved',
  },
  {
    id: 'adopta-un-jr',
    name: 'adopta un jr',
    logoUrl: 'assets/user.svg',
    shortDescription: 'Comunidad orientada a acompañar perfiles junior en su entrada al mercado laboral.',
    description:
      'Conecta mentores y talento emergente para mejorar empleabilidad y transición al mundo profesional.',
    mission: 'Reducir la brecha de acceso al primer empleo tecnológico con apoyo comunitario.',
    cityOrScope: 'Nacional · España',
    website: 'https://example.org/adopta-un-jr',
    linkedin: 'https://www.linkedin.com',
    x: 'https://x.com',
    topics: ['Empleo', 'Formación', 'Comunidad'],
    scope: 'national',
    contactName: 'Equipo adopta un jr',
    contactEmail: 'hola@adoptaunjr.org',
    requestedAt: new Date('2025-07-02'),
    memberCount: 610,
    joinedAt: new Date('2025-08-04'),
    collaborations: ['Mentoring de empleabilidad', 'Sesiones de CV y entrevistas'],
    status: 'approved',
  },
  {
    id: 'guarandinga-tech',
    name: 'Guarandinga TECH',
    logoUrl: 'assets/techito_piscineo.jpg',
    shortDescription: 'Comunidad iberoamericana que conecta desarrollo, cultura tech y proyectos colaborativos.',
    description:
      'Espacio comunitario con foco en compartir conocimiento técnico y construir iniciativas entre comunidades.',
    mission: 'Tejer puentes entre comunidades hispanohablantes para amplificar impacto tecnológico.',
    cityOrScope: 'Internacional · Iberoamérica',
    website: 'https://example.org/guarandinga-tech',
    linkedin: 'https://www.linkedin.com',
    youtube: 'https://www.youtube.com',
    topics: ['Comunidad', 'Open Source', 'Formación'],
    scope: 'international',
    contactName: 'Equipo Guarandinga TECH',
    contactEmail: 'hola@guarandingatech.org',
    requestedAt: new Date('2025-08-01'),
    memberCount: 980,
    joinedAt: new Date('2025-09-12'),
    collaborations: ['Paneles internacionales', 'Ciclos de formación técnica conjunta'],
    status: 'approved',
  },
  {
    id: 'sql-server-espanol',
    name: 'SQL Server Español',
    logoUrl: 'assets/techito_salero_ming.jpg',
    shortDescription: 'Comunidad de base de datos y data platform centrada en SQL Server y ecosistema Microsoft.',
    description:
      'Comparten contenido técnico, sesiones y recursos para profesionales de datos en español.',
    mission: 'Fortalecer la comunidad de datos en español con aprendizaje práctico y colaboración.',
    cityOrScope: 'Internacional · Comunidad hispanohablante',
    website: 'https://example.org/sql-server-espanol',
    linkedin: 'https://www.linkedin.com',
    x: 'https://x.com',
    topics: ['Data', 'Cloud', 'Formación'],
    scope: 'international',
    contactName: 'Equipo SQL Server Español',
    contactEmail: 'hola@sqlserverespanol.org',
    requestedAt: new Date('2025-06-14'),
    memberCount: 1400,
    joinedAt: new Date('2025-07-08'),
    collaborations: ['Sesiones de datos con Tech Riders', 'Ruta formativa SQL y carrera profesional'],
    status: 'approved',
  },
];

const COMMUNITY_PARTNERS_STORAGE_KEY = 'techriders.community-partners.v2';

@Injectable({ providedIn: 'root' })
export class CommunityPartnersStore {
  private readonly partnersState = signal<CommunityPartner[]>(this.loadInitialState());

  readonly allPartners = computed(() => this.partnersState());
  readonly approvedPartners = computed(() =>
    this.partnersState().filter(partner => partner.status === 'approved'),
  );

  constructor(private readonly analytics: CommunityPartnerAnalyticsService) {
    effect(() => {
      this.persistState(this.partnersState());
    });
  }

  findById(id: string): CommunityPartner | undefined {
    const partner = this.partnersState().find(item => item.id === id);
    if (partner) {
      this.analytics.track('community_partner_viewed', { id: partner.id, name: partner.name });
    }
    return partner;
  }

  submitApplication(payload: CommunityPartnerApplication): CommunityPartner {
    if (this.hasDuplicate(payload)) {
      throw new Error('Ya existe una Comuñera o solicitud con ese nombre, web o email de contacto.');
    }

    const created: CommunityPartner = {
      id: this.toId(payload.name),
      name: payload.name,
      logoUrl: payload.logoUrl || 'assets/logo.png',
      shortDescription: payload.whatYouDo,
      description: payload.whoYouAre,
      mission: payload.mission,
      cityOrScope: this.toScopeLabel(payload.scope),
      website: payload.website,
      linkedin: payload.linkedin,
      instagram: payload.instagram,
      x: payload.x,
      youtube: payload.youtube,
      discord: payload.discord,
      telegram: payload.telegram,
      topics: payload.topics,
      scope: payload.scope,
      contactName: payload.contactName,
      contactEmail: payload.contactEmail,
      requestedAt: new Date(),
      collaborations: [payload.collaborationIdeas],
      status: 'pending',
    };

    this.partnersState.update(list => [created, ...list]);
    this.analytics.track('community_partner_application_submitted', {
      id: created.id,
      name: created.name,
      scope: created.scope,
    });
    return created;
  }

  updateStatus(id: string, status: CommunityPartnerStatus): void {
    this.partnersState.update(list =>
      list.map(item => {
        if (item.id !== id) {
          return item;
        }

        const joinedAt = status === 'approved' ? item.joinedAt ?? new Date() : item.joinedAt;
        return { ...item, status, joinedAt };
      }),
    );

    if (status === 'approved') {
      this.analytics.track('community_partner_approved', { id });
    }
  }

  updatePartner(id: string, patch: Pick<CommunityPartner, 'shortDescription' | 'mission'>): void {
    this.partnersState.update(list =>
      list.map(item => {
        if (item.id !== id) {
          return item;
        }

        return {
          ...item,
          shortDescription: patch.shortDescription,
          mission: patch.mission,
        };
      }),
    );
  }

  trackCardClick(id: string, name: string): void {
    this.analytics.track('community_partner_card_clicked', { id, name });
  }

  trackApplyClick(origin: string): void {
    this.analytics.track('community_partner_apply_clicked', { origin });
  }

  private toId(name: string): string {
    const base = name
      .normalize('NFD')
      .replace(/[\u0300-\u036f]/g, '')
      .toLowerCase()
      .replace(/[^a-z0-9]+/g, '-')
      .replace(/(^-|-$)/g, '');

    return `${base || 'comunera'}-${Date.now()}`;
  }

  private toScopeLabel(scope: CommunityPartner['scope']): string {
    if (scope === 'local') {
      return 'Local';
    }
    if (scope === 'national') {
      return 'Nacional';
    }
    return 'Internacional';
  }

  private hasDuplicate(payload: CommunityPartnerApplication): boolean {
    const normalizedName = payload.name.trim().toLowerCase();
    const normalizedWebsite = payload.website.trim().toLowerCase();
    const normalizedEmail = payload.contactEmail.trim().toLowerCase();

    return this.partnersState().some(item =>
      item.name.trim().toLowerCase() === normalizedName
      || (item.website?.trim().toLowerCase() ?? '') === normalizedWebsite
      || item.contactEmail.trim().toLowerCase() === normalizedEmail,
    );
  }

  private loadInitialState(): CommunityPartner[] {
    if (typeof localStorage === 'undefined') {
      return INITIAL_PARTNERS;
    }

    const raw = localStorage.getItem(COMMUNITY_PARTNERS_STORAGE_KEY);
    if (!raw) {
      return INITIAL_PARTNERS;
    }

    try {
      const parsed = JSON.parse(raw) as Array<CommunityPartner & { requestedAt?: string; joinedAt?: string }>;
      return parsed.map(item => this.hydratePartner(item));
    } catch {
      return INITIAL_PARTNERS;
    }
  }

  private persistState(partners: CommunityPartner[]): void {
    if (typeof localStorage === 'undefined') {
      return;
    }

    localStorage.setItem(COMMUNITY_PARTNERS_STORAGE_KEY, JSON.stringify(partners));
  }

  private hydratePartner(
    item: CommunityPartner & { requestedAt?: string | Date; joinedAt?: string | Date },
  ): CommunityPartner {
    return {
      ...item,
      requestedAt: item.requestedAt ? new Date(item.requestedAt) : undefined,
      joinedAt: item.joinedAt ? new Date(item.joinedAt) : undefined,
    };
  }
}
