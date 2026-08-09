export interface ColaboradorItem {
  readonly id: string;
  readonly nombre: string;
  readonly especialidad: string;
  readonly proyectos: number;
  readonly pagos_pendientes: number;
  readonly estado: 'activo' | 'inactivo';
  readonly fecha_inicio: string;
  readonly email: string;
  readonly roles: string[];
}

export interface StaffUserResponse {
  readonly id: string;
  readonly email: string;
  readonly name: string;
  readonly primaryRole: string;
  readonly active: boolean;
  readonly roles: string[];
}

export interface CreateColaboradorPayload {
  readonly email: string;
  readonly nombre: string;
  readonly password: string;
  readonly primaryRole: 'colaborador';
  readonly roles: ['colaborador'];
}
