export interface PagedResult<T> {
  readonly items: T[];
  readonly totalCount: number;
  readonly page: number;
  readonly pageSize: number;
  readonly totalPages: number;
  readonly hasNextPage: boolean;
  readonly hasPreviousPage: boolean;
}

export interface EventoResumen {
  readonly id: string;
  readonly titulo: string;
  readonly ubicacion: string;
  readonly categoria: string;
  readonly estado: string;
  readonly url: string;
  readonly esPasado: boolean;
}

export type EventoListApi = {
  readonly Id: string;
  readonly Name: string;
  readonly Location?: string;
  readonly IsActive: boolean;
};
