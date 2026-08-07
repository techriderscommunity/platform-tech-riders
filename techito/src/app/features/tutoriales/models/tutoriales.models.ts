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


