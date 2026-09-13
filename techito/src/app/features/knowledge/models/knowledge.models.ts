export interface KnowledgeModel {
  id: string;
  slug: string;
  titulo: string;
  extracto: string;
  autor: string;
  fechaPublicacion: string;
  categorias: string[];
  url: string;
}

export interface PagedResult<T> {
  items: T[];
  totalCount: number;
  page: number;
  pageSize: number;
  totalPages: number;
  hasNextPage: boolean;
  hasPreviousPage: boolean;
}

export interface KnowledgeQueryParams {
  page: number;
  pageSize: number;
  categoria?: string;
  busqueda?: string;
}

export interface KnowledgeArticleApiSummary {
  id: string;
  slug: string;
  title: string;
  publishedAt: string | null;
  authorName: string;
  statusName: string | null;
  categories: string[];
  skills: string[];
}

export interface KnowledgeArticleDetailApi {
  id: string;
  slug: string;
  title: string;
  contentMd: string;
  publishedAt: string | null;
  authorName: string;
  statusName: string | null;
  categories: string[];
  skills: string[];
}

// El backend serializa en PascalCase (ver Program.cs: PropertyNamingPolicy = null),
// por lo que el payload real trae ambas variantes segun el endpoint/entorno.
export interface KnowledgeArticleApiSummaryRaw {
  Id?: string; id?: string;
  Slug?: string; slug?: string;
  Title?: string; title?: string;
  PublishedAt?: string | null; publishedAt?: string | null;
  AuthorName?: string; authorName?: string;
  StatusName?: string | null; statusName?: string | null;
  Categories?: string[]; categories?: string[];
  Skills?: string[]; skills?: string[];
}

export interface KnowledgeArticleDetailApiRaw {
  Id?: string; id?: string;
  Slug?: string; slug?: string;
  Title?: string; title?: string;
  ContentMd?: string; contentMd?: string;
  PublishedAt?: string | null; publishedAt?: string | null;
  AuthorName?: string; authorName?: string;
  StatusName?: string | null; statusName?: string | null;
  Categories?: string[]; categories?: string[];
  Skills?: string[]; skills?: string[];
}

export interface KnowledgePagedApiRaw {
  Items?: KnowledgeArticleApiSummaryRaw[]; items?: KnowledgeArticleApiSummaryRaw[];
  TotalCount?: number; totalCount?: number;
  Page?: number; page?: number;
  PageSize?: number; pageSize?: number;
}

export interface KnowledgeArticleDetail extends KnowledgeArticleDetailApi {}

export interface KnowledgePagedApi {
  items: KnowledgeArticleApiSummary[];
  totalCount: number;
  page: number;
  pageSize: number;
}


