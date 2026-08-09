export interface SesionEnEventoApi {
  Id: string;
  Title: string;
  StartTime: string;
  EndTime: string;
  Description?: string;
  Speaker?: string;
  Room?: string;
}

export interface EventoConSesionesApi {
  Id: string;
  Name: string;
  Location?: string;
  StartDate: string;
  EndDate: string;
  Description?: string;
  IsActive: boolean;
  Sessions?: SesionEnEventoApi[];
}

export interface EventoResumen {
  id: string;
  titulo: string;
  ubicacion: string;
  categoria: string;
  estado: string;
  url: string;
  esPasado: boolean;
}

export interface EventoListApi {
  Id: string;
  Name: string;
  Location?: string;
  IsActive: boolean;
}
