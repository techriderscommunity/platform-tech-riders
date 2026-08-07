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

export interface PagedResult<T> {
  items: T[];
  totalCount: number;
  page: number;
  pageSize: number;
  totalPages: number;
  hasNextPage: boolean;
  hasPreviousPage: boolean;
}


