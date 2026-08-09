import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { environment } from '@env/environment';
import { forkJoin } from 'rxjs';
import { map } from 'rxjs/operators';
import { GovernanceData, GovernanceRoleResponse, GovernanceUserResponse, StaffItem } from '../models/staff-governance.models';

@Injectable({ providedIn: 'root' })
export class StaffGovernanceService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = environment.apiUrl;

  getGovernanceData() {
    return forkJoin({
      users: this.http.get<GovernanceUserResponse[]>(`${this.baseUrl}/admin/staff/usuarios`),
      roles: this.http.get<GovernanceRoleResponse[]>(`${this.baseUrl}/admin/staff/roles`),
    }).pipe(
      map(({ users, roles }) => ({
        staff: (users ?? []).map((user): StaffItem => ({
          id: user.id,
          nombre: user.name,
          email: user.email,
          rolPrincipal: user.primaryRole,
          roles: user.roles ?? [user.primaryRole],
          estado: user.active ? 'activo' : 'inactivo',
        })),
        roles: (roles ?? []).map(role => role.name),
      } satisfies GovernanceData)),
    );
  }

  updateEstado(id: string, active: boolean) {
    return this.http.put(`${this.baseUrl}/admin/staff/usuarios/${id}/estado`, { activo: active });
  }

  updateRoles(id: string, primaryRole: string, roles: string[]) {
    return this.http.put(`${this.baseUrl}/admin/staff/usuarios/${id}/roles`, {
      primaryRole,
      roles,
    });
  }
}
