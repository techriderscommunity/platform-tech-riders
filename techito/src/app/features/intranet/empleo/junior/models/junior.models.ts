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

export interface OfferApi {
  Id: string;
  Titulo: string;
  Empresa: string;
  Salario: number;
  Ubicacion: string;
  Modalidad: number;
  Estado: number;
  FechaPublicacion: string;
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

export interface PerfilPublico {
  nombre: string;
  titulo: string;
  ubicacion: string;
  resumen: string;
  habilidades: string[];
  experiencia: string;
  foto: string;
}

export interface PerfilPrivado {
  email: string;
  telefono: string;
  edad: number;
  gradoAcademico: string;
  universidad: string;
  disponibilidad: string;
  pretensionSalarial: string;
}


