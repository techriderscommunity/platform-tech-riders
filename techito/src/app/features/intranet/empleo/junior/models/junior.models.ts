export interface Curso {
  id: string;
  titulo: string;
  descripcion: string;
  duracion: string;
  nivel: 'basico' | 'intermedio' | 'avanzado';
  url: string;
}

export interface OfertaJunior {
  id: string;
  titulo: string;
  empresa: string;
  salario: string;
  ubicacion: string;
  modalidad: string;
  estado: string;
  fechaPublicacion: string;
}

export interface Evento {
  id: string;
  titulo: string;
  descripcion: string;
  fecha: string;
  lugar: string;
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


