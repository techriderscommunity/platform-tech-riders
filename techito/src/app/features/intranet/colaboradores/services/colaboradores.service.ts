import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { environment } from '@env/environment';
import { map } from 'rxjs/operators';
import { ColaboradorItem, CreateColaboradorPayload, StaffUserResponse } from '../models/colaboradores.models';

@Injectable({ providedIn: 'root' })
export class ColaboradoresService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = environment.apiUrl;

  getColaboradores() {
    return this.http.get<StaffUserResponse[]>(`${this.baseUrl}/admin/staff/colaboradores`).pipe(
      map((items) => (items ?? []).map((item): ColaboradorItem => ({
        id: item.id,
        nombre: item.name,
        especialidad: item.roles.filter(role => role !== 'colaborador').join(', ') || 'Colaborador',
        proyectos: 0,
        pagos_pendientes: 0,
        estado: item.active ? 'activo' : 'inactivo',
        fecha_inicio: '',
        email: item.email,
        roles: item.roles,
      }))),
    );
  }

  createColaborador(payload: CreateColaboradorPayload) {
    return this.http.post(`${this.baseUrl}/admin/staff/colaboradores`, payload);
  }

  updateEstado(id: string, activo: boolean) {
    return this.http.put(`${this.baseUrl}/admin/staff/usuarios/${id}/estado`, { activo });
  }
}
