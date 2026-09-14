import { Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map, tap } from 'rxjs';
import { environment } from '@env/environment';
import { resolveIntranetWorkspace } from '../../features/intranet/empleo/intranet-nav.config';

export type AppRole =
  | 'member'
  | 'admin'
  | 'staff'
  | 'community-leader'
  | 'ambassador'
  | 'center'
  | 'community-partner';

export type AppPermission =
  | 'profile.manage'
  | 'preferences.manage'
  | 'favorites.manage'
  | 'events.register'
  | 'sessions.request'
  | 'communities.follow'
  | 'ambassador.profile.manage'
  | 'ambassador.availability.manage'
  | 'sessions.assigned.view'
  | 'sessions.assigned.respond'
  | 'sessions.history.view'
  | 'sessions.propose'
  | 'events.participate'
  | 'community.profile.manage'
  | 'community.info.manage'
  | 'events.create'
  | 'activities.create'
  | 'collaborations.propose'
  | 'center.info.manage'
  | 'center.requests.view'
  | 'center.sessions.view'
  | 'center.sessions.request'
  | 'center.sessions.history.view'
  | 'center.contacts.manage'
  | 'community.events.approve'
  | 'community.activities.manage'
  | 'community.requests.validate'
  | 'community.initiatives.coordinate'
  | 'community.manage'
  | 'role-requests.approve'
  | 'sessions.fptour.manage'
  | 'events.official.create'
  | 'taxonomy.manage'
  | 'content.manage'
  | 'validations.manage'
  | 'functional-config.manage'
  | 'platform.manage'
  | 'security.manage'
  | 'audit.manage'
  | 'users.manage';

const PERMISSIONS_BY_ROLE: Record<AppRole, readonly AppPermission[]> = {
  member: [
    'profile.manage', 'preferences.manage', 'favorites.manage', 'events.register',
    'sessions.request', 'communities.follow',
  ],
  ambassador: [
    'ambassador.profile.manage', 'ambassador.availability.manage', 'sessions.assigned.view',
    'sessions.assigned.respond', 'sessions.history.view', 'sessions.propose', 'events.participate',
  ],
  'community-partner': [
    'community.profile.manage', 'community.info.manage', 'events.create', 'activities.create',
    'collaborations.propose',
  ],
  center: [
    'center.info.manage', 'center.requests.view', 'center.sessions.view', 'center.sessions.request',
    'center.sessions.history.view', 'center.contacts.manage',
  ],
  'community-leader': [
    'community.events.approve', 'community.activities.manage', 'community.requests.validate',
    'community.initiatives.coordinate', 'events.create', 'community.manage',
  ],
  staff: [
    'role-requests.approve', 'sessions.fptour.manage', 'events.official.create', 'community.manage',
    'taxonomy.manage', 'content.manage', 'validations.manage', 'functional-config.manage',
  ],
  admin: [
    'platform.manage', 'security.manage', 'audit.manage', 'users.manage',
  ],
};

export interface UserProfile {
  id: string;
  email: string;
  name: string;
  role: AppRole;
  roles: AppRole[];
  linkedIn?: string | null;
  instagram?: string | null;
  x?: string | null;
  youTube?: string | null;
  github?: string | null;
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

  hasPermission(required: AppPermission | AppPermission[]): boolean {
    const user = this.currentUser();
    if (!user) return false;

    const requiredPermissions = Array.isArray(required) ? required : [required];
    const roles = user.roles?.length ? user.roles : [user.role];
    const permissions = new Set<AppPermission>([
      ...roles.flatMap(role => PERMISSIONS_BY_ROLE[role] ?? []),
      ...(roles.includes('admin') ? Object.values(PERMISSIONS_BY_ROLE).flat() : []),
    ]);

    return requiredPermissions.some(permission => permissions.has(permission));
  }

  getDefaultRoute(): string {
    const user = this.currentUser();
    if (!user) return '/';

    return resolveIntranetWorkspace(this.getUserRoles(user)).homeRoute;
  }

  getRoleHomeRoute(): string {
    const user = this.currentUser();
    if (!user) return '/intranet';

    return resolveIntranetWorkspace(this.getUserRoles(user)).homeRoute;
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
      try {
        const parsed = JSON.parse(userJson) as Partial<UserProfile>;
        const normalized = this.normalizeUser(parsed);
        this.currentUser.set(normalized);
        this.userType.set(normalized.role);
      }
      catch {
        this.logout();
      }
    }
  }

  private normalizeUser(user: Partial<UserProfile>): UserProfile {
    const primaryRole = user.role as AppRole | undefined;
    const normalizedRoles = (user.roles ?? [])
      .filter((role): role is AppRole => !!role)
      .map(role => role as AppRole);

    if (primaryRole && !normalizedRoles.includes(primaryRole)) {
      normalizedRoles.push(primaryRole);
    }

    if (!user.id || !user.email || !user.name || !normalizedRoles.length) {
      throw new Error('Invalid authentication profile.');
    }

    return {
      id: user.id,
      email: user.email,
      name: user.name,
      role: this.resolvePrimaryRole(normalizedRoles),
      roles: normalizedRoles,
      linkedIn: user.linkedIn ?? null,
      instagram: user.instagram ?? null,
      x: user.x ?? null,
      youTube: user.youTube ?? null,
      github: user.github ?? null,
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
        LinkedIn?: string | null;
        Instagram?: string | null;
        X?: string | null;
        YouTube?: string | null;
        Github?: string | null;
      };
    };

    const token = (payload.token ?? payload.Token ?? '').toString();
    const user = {
      id: payload.user?.id ?? payload.User?.Id ?? '',
      email: payload.user?.email ?? payload.User?.Email ?? '',
      name: payload.user?.name ?? payload.User?.Name ?? '',
      role: (payload.user?.role ?? payload.User?.Role) as AppRole | undefined,
      roles: (payload.user?.roles ?? payload.User?.Roles ?? []) as AppRole[],
      linkedIn: payload.user?.linkedIn ?? payload.User?.LinkedIn ?? null,
      instagram: payload.user?.instagram ?? payload.User?.Instagram ?? null,
      x: payload.user?.x ?? payload.User?.X ?? null,
      youTube: payload.user?.youTube ?? payload.User?.YouTube ?? null,
      github: payload.user?.github ?? payload.User?.Github ?? null,
    };

    if (!token) {
      throw new Error('Authentication token was not returned by the backend.');
    }

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
    return resolveIntranetWorkspace(roles).role;
  }

  private getUserRoles(user: UserProfile): AppRole[] {
    return user.roles?.length ? user.roles : [user.role];
  }

  private isBrowser(): boolean {
    return typeof localStorage !== 'undefined';
  }
}


