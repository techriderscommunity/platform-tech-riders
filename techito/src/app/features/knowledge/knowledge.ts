import { ChangeDetectionStrategy, Component, DestroyRef, computed, inject, signal, PLATFORM_ID, WritableSignal } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { takeUntilDestroyed, toObservable } from '@angular/core/rxjs-interop';
import { catchError, of, switchMap, tap } from 'rxjs';
import { KnowledgeService } from './services/knowledge.service';
import { PagedResult, KnowledgeModel } from './models/knowledge.models';
import { UiTextField  } from '@shared/ui/text-field/text-field';
import { UiMetricsStrip } from '@shared/ui/metrics-strip/metrics-strip';
import { UiCarouselItem, UiMediaCarousel } from '@shared/ui/media-carousel/media-carousel';
import { UiResourceCardItem, UiResourceCards } from '@shared/ui/resource-cards/resource-cards';
import { KnowledgePlaylistKey, YoutubePlaylistSection } from './models/knowledge-playlists.models';
import { KnowledgePlaylistsService } from './services/knowledge-playlists.service';

function playlistVideoItem(title: string, videoId: string, listId: string): UiCarouselItem {
  return {
    kind: 'video',
    title,
    src: `https://www.youtube-nocookie.com/embed/${videoId}?list=${listId}`,
    link: `https://www.youtube.com/watch?v=${videoId}&list=${listId}`
  };
}

@Component({
  selector: 'app-knowledge',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [UiTextField, UiMetricsStrip, UiMediaCarousel, UiResourceCards],
  templateUrl: './knowledge.html',
  styleUrl: './knowledge.scss'
})

export class Knowledge {
  readonly destroyRef = inject(DestroyRef);
  readonly knowledgeService = inject(KnowledgeService);
  readonly knowledgePlaylistsService = inject(KnowledgePlaylistsService);
  private readonly platformId = inject(PLATFORM_ID);

  private readonly knownCategories = new Set<string>();
  featuredCategories: string[] = [];

  readonly pageSize = 12;
  readonly selectedCategoria = signal('');
  readonly searchText = signal('');
  readonly currentPage = signal(1);
  readonly loadingKnowledges = signal(false);
  readonly knowledgesError = signal<string | null>(null);
  readonly knowledgePaged  = signal<PagedResult<KnowledgeModel>>({
    items: [],
    totalCount: 0,
    page: 1,
    pageSize: this.pageSize,
    totalPages: 0,
    hasNextPage: false,
    hasPreviousPage: false
  });

  readonly knowledgeQuery = computed(() => ({
    page: this.currentPage(),
    pageSize: this.pageSize,
    categoria: this.selectedCategoria() || undefined,
    busqueda: this.searchText().trim() || undefined
  }));

  readonly knowledgeMetrics = computed(() => [
    { icon: '📚', value: String(this.knowledgePaged().totalCount), label: 'Recursos disponibles' },
    { icon: '🏷️', value: String(this.featuredCategories.length), label: 'Categorías destacadas' },
    { icon: '🔎', value: this.searchText().trim() ? 'Activa' : 'General', label: 'Búsqueda' },
  ]);

  readonly activeFilterLabel = computed(() => this.selectedCategoria() || 'Todos');

  readonly knowledgeCards = computed<UiResourceCardItem[]>(() => this.knowledgePaged().items.map(tutorial => ({
    mode: tutorial.categorias[0] || 'Digital',
    title: tutorial.titulo,
    summary: tutorial.extracto,
    tags: tutorial.categorias,
    meta: `${this.formatFecha(tutorial.fechaPublicacion)} · ${tutorial.autor}`,
    ctaLabel: 'Ver detalle',
    ctaLink: tutorial.url,
    ctaHref: tutorial.url
  })));

  readonly youtubePlaylistsUrl = 'https://www.youtube.com/@TechRidersMedia/playlists';
  readonly loadingProfiles = signal(false);
  readonly loadingSuccessStories = signal(false);
  readonly loadingInterviews = signal(false);

  private readonly profilesFallback: UiCarouselItem[] = [
    playlistVideoItem('Perfiles profesionales · Episodio 1', 'J25VQJ7Wx34', 'PLxVLmPZVJwGEdXBvyBphPA_YxYjL8QNKO'),
    playlistVideoItem('Perfiles profesionales · Episodio 2', 'X4mIfCx6XPU', 'PLxVLmPZVJwGEdXBvyBphPA_YxYjL8QNKO'),
    playlistVideoItem('Perfiles profesionales · Episodio 3', 'vncHQDNPjEw', 'PLxVLmPZVJwGEdXBvyBphPA_YxYjL8QNKO'),
    playlistVideoItem('Perfiles profesionales · Episodio 4', 'A856m8nAx6g', 'PLxVLmPZVJwGEdXBvyBphPA_YxYjL8QNKO'),
    playlistVideoItem('Perfiles profesionales · Episodio 5', '5zfaHALRmis', 'PLxVLmPZVJwGEdXBvyBphPA_YxYjL8QNKO'),
  ];
  private readonly successStoriesFallback: UiCarouselItem[] = [
    playlistVideoItem('Historias de éxito · Episodio 1', 'HKgt8H8o-nI', 'PLxVLmPZVJwGESQcMxlUozaHcXVDBkigUo'),
    playlistVideoItem('Historias de éxito · Episodio 2', 'RXRqB_Ul_oI', 'PLxVLmPZVJwGESQcMxlUozaHcXVDBkigUo'),
    playlistVideoItem('Historias de éxito · Episodio 3', 'zlZwB1VlY28', 'PLxVLmPZVJwGESQcMxlUozaHcXVDBkigUo'),
    playlistVideoItem('Historias de éxito · Episodio 4', 'TAxnDg0kyRI', 'PLxVLmPZVJwGESQcMxlUozaHcXVDBkigUo'),
    playlistVideoItem('Historias de éxito · Episodio 5', 'NwEhryRqSio', 'PLxVLmPZVJwGESQcMxlUozaHcXVDBkigUo'),
  ];
  private readonly interviewsFallback: UiCarouselItem[] = [
    playlistVideoItem('Entrevistas · IA, Copilot y el futuro del desarrollo', 'WQp9pZb8shU', 'PLxVLmPZVJwGFXrUIxJdfJ9fB5QYuHyI-q'),
    playlistVideoItem('Entrevistas · Agentes de IA en empresa', 'SvZ50wArtaM', 'PLxVLmPZVJwGFXrUIxJdfJ9fB5QYuHyI-q'),
    playlistVideoItem('Entrevistas · Estudiantes AcademyVerso', 'baKNCZUbvL8', 'PLxVLmPZVJwGFXrUIxJdfJ9fB5QYuHyI-q'),
    playlistVideoItem('Entrevistas · IA con imágenes', '-biqjBJN_cI', 'PLxVLmPZVJwGFXrUIxJdfJ9fB5QYuHyI-q'),
    playlistVideoItem('Entrevistas · Microsoft Student Ambassador', 'Gc2sLw3vcvM', 'PLxVLmPZVJwGFXrUIxJdfJ9fB5QYuHyI-q'),
  ];

  readonly profilesItems = signal<UiCarouselItem[]>(this.profilesFallback);
  readonly successStoriesItems = signal<UiCarouselItem[]>(this.successStoriesFallback);
  readonly interviewsItems = signal<UiCarouselItem[]>(this.interviewsFallback);

  readonly youtubePlaylistSections: YoutubePlaylistSection[] = [
    { key: 'profiles', title: 'Perfiles profesionales', url: 'https://www.youtube.com/watch?v=J25VQJ7Wx34&list=PLxVLmPZVJwGEdXBvyBphPA_YxYjL8QNKO' },
    { key: 'success-stories', title: 'Historias de éxito', url: 'https://www.youtube.com/watch?v=HKgt8H8o-nI&list=PLxVLmPZVJwGESQcMxlUozaHcXVDBkigUo' },
    { key: 'interviews', title: 'Entrevistas', url: 'https://www.youtube.com/watch?v=WQp9pZb8shU&list=PLxVLmPZVJwGFXrUIxJdfJ9fB5QYuHyI-q' },
  ];

  private readonly dateFormatter = new Intl.DateTimeFormat('es-ES', {
    day: '2-digit',
    month: 'short',
    year: 'numeric'
  });

  constructor() {
    if (isPlatformBrowser(this.platformId)) toObservable(this.knowledgeQuery)
      .pipe(
        tap(() => {
          this.loadingKnowledges.set(true);
          this.knowledgesError.set(null);
        }),
        switchMap(query => this.knowledgeService.getKnowledges(query).pipe(
          catchError(() => {
            this.knowledgesError.set('No se pudieron cargar los tutoriales.');
            return of({
              items: [],
              totalCount: 0,
              page: query.page,
              pageSize: query.pageSize,
              totalPages: 0,
              hasNextPage: false,
              hasPreviousPage: false
            } satisfies PagedResult<KnowledgeModel>);
          })
        )),
        takeUntilDestroyed(this.destroyRef)
      )
      .subscribe(result => {
        this.knowledgePaged.set(result);
        result.items.flatMap((item) => item.categorias).forEach((cat) => {
          if (cat && cat.trim()) this.knownCategories.add(cat.trim());
        });
        this.featuredCategories = Array.from(this.knownCategories).sort((a, b) => a.localeCompare(b, 'es'));
        this.loadingKnowledges.set(false);
      });

    this.loadPlaylist('profiles', this.profilesItems, this.profilesFallback, this.loadingProfiles);
    this.loadPlaylist('success-stories', this.successStoriesItems, this.successStoriesFallback, this.loadingSuccessStories);
    this.loadPlaylist('interviews', this.interviewsItems, this.interviewsFallback, this.loadingInterviews);
  }

  private loadPlaylist(
    playlist: KnowledgePlaylistKey,
    target: WritableSignal<UiCarouselItem[]>,
    fallback: UiCarouselItem[],
    loading: WritableSignal<boolean>
  ): void {
    loading.set(true);
    this.knowledgePlaylistsService.getVideosByPlaylist(playlist, 8).pipe(
      tap((items) => {
        if (items.length === 0) {
          target.set(fallback);
        } else if (items.length < 5) {
          const existingSrc = new Set(items.map((item) => item.src));
          target.set([...items, ...fallback.filter((item) => !existingSrc.has(item.src))].slice(0, 8));
        } else {
          target.set(items);
        }
        loading.set(false);
      }),
      catchError(() => {
        target.set(fallback);
        loading.set(false);
        return of(null);
      }),
      takeUntilDestroyed(this.destroyRef)
    ).subscribe();
  }

  sectionItems(section: YoutubePlaylistSection): UiCarouselItem[] {
    if (section.key === 'profiles') return this.profilesItems();
    if (section.key === 'success-stories') return this.successStoriesItems();
    return this.interviewsItems();
  }

  sectionLoading(section: YoutubePlaylistSection): boolean {
    if (section.key === 'profiles') return this.loadingProfiles();
    if (section.key === 'success-stories') return this.loadingSuccessStories();
    return this.loadingInterviews();
  }

  setCategoria(categoria: string): void {
    this.selectedCategoria.set(categoria);
    this.currentPage.set(1);
  }

  updateSearch(term: string): void {
    this.searchText.set(term);
    this.currentPage.set(1);
  }

  nextKnowledgePage(): void {
    if (this.knowledgePaged().hasNextPage) {
      this.currentPage.update(page => page + 1);
    }
  }

  prevKnowledgePage(): void {
    if (this.knowledgePaged().hasPreviousPage) {
      this.currentPage.update(page => page - 1);
    }
  }

  formatFecha(fechaPublicacion: string): string {
    const date = new Date(fechaPublicacion);
    return Number.isNaN(date.getTime()) ? fechaPublicacion : this.dateFormatter.format(date);
  }
}


