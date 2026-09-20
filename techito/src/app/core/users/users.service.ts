import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { environment } from '@env/environment';
import { map } from 'rxjs/operators';
import {
  CreateUserPayload,
  UpdateUserPayload,
  UserActivityApi,
  UserDetailApi,
  UserListItem,
  UserListItemApi,
  UserListResponseApi,
} from './users.models';

@Injectable({ providedIn: 'root' })
export class UsersService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/users`;

  list(filters: { search?: string; role?: string; membershipStatus?: string; page?: number; pageSize?: number } = {}) {
    let params = new HttpParams();
    if (filters.search) params = params.set('search', filters.search);
    if (filters.role) params = params.set('role', filters.role);
    if (filters.membershipStatus) params = params.set('membershipStatus', filters.membershipStatus);
    params = params.set('page', String(filters.page ?? 1));
    params = params.set('pageSize', String(filters.pageSize ?? 50));

    return this.http.get<UserListResponseApi>(this.baseUrl, { params }).pipe(
      map((response) => ({
        items: (response.Items ?? []).map(toListItem),
        totalCount: response.TotalCount,
      })),
    );
  }

  getById(id: string) {
    return this.http.get<UserDetailApi>(`${this.baseUrl}/${id}`);
  }

  getActivity(id: string) {
    return this.http.get<UserActivityApi>(`${this.baseUrl}/${id}/activity`);
  }

  create(payload: CreateUserPayload) {
    return this.http.post(this.baseUrl, {
      Nickname: payload.nickname,
      Name: payload.name,
      LastName: payload.lastName,
      Email: payload.email,
      Phone: payload.phone,
      Locality: payload.locality,
      About: payload.about,
    });
  }

  update(id: string, payload: UpdateUserPayload) {
    return this.http.put(`${this.baseUrl}/${id}`, {
      Name: payload.name,
      LastName: payload.lastName,
      Email: payload.email,
      Phone: payload.phone,
      Locality: payload.locality,
      About: payload.about,
    });
  }

  activate(id: string) {
    return this.http.post(`${this.baseUrl}/${id}/activate`, {});
  }

  deactivate(id: string) {
    return this.http.post(`${this.baseUrl}/${id}/deactivate`, {});
  }

  revokeRole(id: string, roleName: string) {
    return this.http.post(`${this.baseUrl}/${id}/roles/${encodeURIComponent(roleName)}/revoke`, {});
  }
}

function toListItem(api: UserListItemApi): UserListItem {
  return {
    id: api.Id,
    nickname: api.Nickname,
    name: api.Name,
    lastName: api.LastName,
    email: api.Email,
    roles: api.Roles ?? [],
    membershipStatus: api.MembershipStatus ?? 'Pendiente',
    isWorking: api.IsWorking,
    lastActivityDate: api.LastActivityDate ?? null,
  };
}
