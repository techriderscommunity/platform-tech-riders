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

export interface AmbassadorApi {
  Id: string;
  Name: string;
  LastName: string;
  Email: string;
  Phone?: string | null;
  CategoryName?: string | null;
  OtherCategory?: string | null;
  UpdatedAt?: string | null;
  CreatedAt: string;
  IsActive: boolean;
}

export interface TalkItem {
  topic: string;
  speaker: string;
  date: string;
  rating: number;
}

export interface AmbassadorPortalApi {
  Email: string;
  Bio: string;
  Specialties: string;
  Availability: string;
}

export interface UpdateAmbassadorPortalPayload {
  userKey: string;
  email: string;
  bio: string;
  specialties: string;
  availability: string;
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


