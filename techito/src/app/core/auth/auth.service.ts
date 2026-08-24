import { Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map, tap } from 'rxjs';
import { environment } from '@env/environment';

export type AppRole =
  | 'member'
  | 'admin'
  | 'superadmin'
  | 'staff'
  | 'coordinador'
  | 'empresa'
  | 'junior'
  | 'colaborador'
  | 'embajador'
  | 'young-riders'
  | 'centro';

export interface UserProfile {
  id: string;
  email: string;
  name: string;
  role: AppRole;
  roles: AppRole[];
}

export interface LoginResponse {
  token: string;
  user: UserProfile;
}

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly baseUrl = environment.apiUrl;
  private currentUser = signal<UserProfile | null>(null);
  readonly userType = signal<AppRole | null>(null);

  readonly user = this.currentUser.asReadonly();
  readonly isAuthenticated = () => this.currentUser() !== null;

  constructor(private http: HttpClient) {
    this.loadUser();
  }

  login(email: string, password: string): Observable<LoginResponse> {
    return this.http.post<unknown>(`${this.baseUrl}/auth/login`, { email, password })
      .pipe(
        map((response) => this.normalizeLoginResponse(response)),
        tap((response) => this.persistSession(response)),
      );
  }

  hasRole(required: AppRole | AppRole[]): boolean {
    const user = this.currentUser();
    if (!user) return false;

    const requiredRoles = Array.isArray(required) ? required : [required];
    const userRoles = user.roles?.length ? user.roles : [user.role];

    return requiredRoles.some(requiredRole =>
      userRoles.some(role => role === requiredRole)
    );
  }

  getDefaultRoute(): string {
    if (this.hasRole('superadmin')) return '/intranet/staff';
    if (this.hasRole('staff') || this.hasRole('coordinador')) return '/intranet/staff';
    if (this.hasRole('admin')) return '/intranet/admin';
    if (this.hasRole('empresa')) return '/intranet/company';
    if (this.hasRole(['embajador', 'colaborador'])) return '/intranet/ambassador/portal';
    return '/intranet/junior';
  }

  logout(): void {
    this.currentUser.set(null);
    this.userType.set(null);
    if (this.isBrowser()) {
      localStorage.removeItem('token');
      localStorage.removeItem('user');
    }
  }

  private loadUser(): void {
    if (!this.isBrowser()) return;
    const userJson = localStorage.getItem('user');
    if (userJson) {
      const parsed = JSON.parse(userJson) as Partial<UserProfile>;
      const normalized = this.normalizeUser(parsed);
      this.currentUser.set(normalized);
      this.userType.set(normalized.role);
    }
  }

  private normalizeUser(user: Partial<UserProfile>): UserProfile {
    const fallbackRole = (user.role ?? 'junior') as AppRole;
    const normalizedRoles = (user.roles ?? [])
      .filter((role): role is AppRole => !!role)
      .map(role => role as AppRole);

    if (!normalizedRoles.includes(fallbackRole)) {
      normalizedRoles.push(fallbackRole);
    }

    return {
      id: user.id ?? '',
      email: user.email ?? '',
      name: user.name ?? '',
      role: this.resolvePrimaryRole(normalizedRoles),
      roles: normalizedRoles,
    };
  }

  private normalizeLoginResponse(response: unknown): LoginResponse {
    const payload = (response ?? {}) as Partial<LoginResponse> & {
      Token?: string;
      User?: Partial<UserProfile> & {
        Id?: string;
        Email?: string;
        Name?: string;
        Role?: string;
        Roles?: Array<string | AppRole>;
      };
    };

    const token = (payload.token ?? payload.Token ?? '').toString();
    const user = {
      id: payload.user?.id ?? payload.User?.Id ?? '',
      email: payload.user?.email ?? payload.User?.Email ?? '',
      name: payload.user?.name ?? payload.User?.Name ?? '',
      role: (payload.user?.role ?? payload.User?.Role ?? 'junior') as AppRole,
      roles: (payload.user?.roles ?? payload.User?.Roles ?? []) as AppRole[],
    };

    return { token, user: this.normalizeUser(user) };
  }

  private persistSession(response: LoginResponse): void {
    const normalized = this.normalizeUser(response.user);
    this.currentUser.set(normalized);
    this.userType.set(normalized.role);

    if (this.isBrowser()) {
      localStorage.setItem('token', response.token);
      localStorage.setItem('user', JSON.stringify(normalized));
    }
  }

  private resolvePrimaryRole(roles: AppRole[]): AppRole {
    const priority: AppRole[] = ['superadmin', 'staff', 'coordinador', 'admin', 'empresa', 'junior', 'colaborador', 'embajador', 'member', 'young-riders', 'centro'];
    const matched = priority.find(role => roles.includes(role));
    return matched ?? 'junior';
  }

  private isBrowser(): boolean {
    return typeof localStorage !== 'undefined';
  }
}


