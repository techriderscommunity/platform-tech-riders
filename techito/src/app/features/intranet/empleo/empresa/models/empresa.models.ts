export interface Oferta {
  id: string;
  titulo: string;
  empresa: string;
  salario: string;
  ubicacion: string;
  modalidad: string;
  estado: string;
  fechaPublicacion: string;
  candidatos?: number;
}

export interface Candidato {
  id: string;
  ofertaId: string;
  juniorId: string;
  nombreJunior: string;
  nombre?: string;
  emailJunior: string;
  puesto?: string;
  estado: string;
  fechaSolicitud: string;
  fecha?: string;
}

export interface EmpresaDashboardStats {
  ofertasActivas: number;
  candidatosTotal: number;
  enProceso: number;
  contratados: number;
}

export interface EmpresaDashboard {
  nombreEmpresa: string;
  stats: EmpresaDashboardStats;
  ultimasOfertas: Oferta[];
  ultimosCandidatos: Candidato[];
}

export interface NuevaOfertaForm {
  titulo: string;
  descripcion: string;
  requisitos: string;
  ubicacion: string;
  salario: string;
  tipo: string;
  modalidad: string;
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

export interface ApplicationApi {
  Id: string;
  OfertaId: string;
  JuniorId: string;
  NombreJunior: string;
  EmailJunior: string;
  Estado: number;
  FechaSolicitud: string;
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


