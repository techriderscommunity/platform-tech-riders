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

export interface KnowledgeApi {
  Id: string;
  Slug: string;
  Titulo: string;
  Extracto: string;
  Autor: string;
  FechaPublicacion: string;
  CategoriasJson: string;
  Url: string;
}

export interface KnowledgePagedApi {
  items: KnowledgeApi[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
}


