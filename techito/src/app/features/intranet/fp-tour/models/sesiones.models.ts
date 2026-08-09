export interface Sesion {
  id: string;
  titulo: string;
  centro: string;
  fecha: string;
  categoria: string;
  estado: string;
  numAlumnos: number;
  embajadorAsignadoId: string | null;
}

export interface SessionApi {
  Id: string;
  Title: string;
  Level?: string | null;
  MaxCapacity?: number | null;
  IsActive: boolean;
  Event?: {
    Name: string;
    StartDate: string;
  } | null;
}

export interface SessionWorkflowApi {
  SessionId: string;
  Status?: string;
  AmbassadorAssignedId?: string | null;
  UpdatedAt: string;
}

export interface UpdateSessionWorkflowRequest {
  status?: string;
  ambassadorAssignedId?: string | null;
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


