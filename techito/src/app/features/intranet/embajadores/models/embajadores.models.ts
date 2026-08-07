export interface Embajador {
  id: string;
  nombre: string;
  email: string;
  telefono: string;
  categoria: string;
  estado: string;
  totalSesiones: number;
  ultimaActividad: string | null;
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


