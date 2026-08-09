export interface Tutorial {
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

export interface TutorialesQueryParams {
  page: number;
  pageSize: number;
  categoria?: string;
  busqueda?: string;
}

export interface TutorialApi {
  Id: string;
  Slug: string;
  Titulo: string;
  Extracto: string;
  Autor: string;
  FechaPublicacion: string;
  CategoriasJson: string;
  Url: string;
}

export interface TutorialPagedApi {
  items: TutorialApi[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
}


