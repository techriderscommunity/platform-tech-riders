export interface StaffItem {
  readonly id: string;
  readonly nombre: string;
  readonly email: string;
  readonly rolPrincipal: string;
  readonly roles: string[];
  readonly estado: 'activo' | 'inactivo';
}

export interface GovernanceUserResponse {
  readonly id: string;
  readonly email: string;
  readonly name: string;
  readonly primaryRole: string;
  readonly active: boolean;
  readonly roles: string[];
}

export interface GovernanceRoleResponse {
  readonly id: string;
  readonly name: string;
  readonly description?: string | null;
  readonly active: boolean;
}

export interface GovernanceData {
  readonly staff: StaffItem[];
  readonly roles: string[];
}
