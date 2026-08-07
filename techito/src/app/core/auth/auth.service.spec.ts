import { TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting, HttpTestingController } from '@angular/common/http/testing';
import { AuthService, type LoginResponse, type UserProfile } from './auth.service';
import { environment } from '@env/environment';

describe('AuthService', () => {
  let service: AuthService;
  let httpMock: HttpTestingController;

  const mockUser: UserProfile = {
    id: 'u1',
    email: 'admin@techriders.es',
    name: 'Admin',
    role: 'admin',
    roles: ['admin'],
  };

  beforeEach(() => {
    localStorage.clear();

    TestBed.configureTestingModule({
      providers: [AuthService, provideHttpClient(), provideHttpClientTesting()],
    });

    service = TestBed.inject(AuthService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
    localStorage.clear();
  });

  it('should login and persist token + user', () => {
    const mockResponse: LoginResponse = {
      token: 'jwt-token',
      user: mockUser,
    };

    service.login('admin@techriders.es', 'secret').subscribe((response) => {
      expect(response.token).toBe('jwt-token');
    });

    const req = httpMock.expectOne(`${environment.apiUrl}/auth/login`);
    expect(req.request.method).toBe('POST');
    expect(req.request.body.email).toBe('admin@techriders.es');
    req.flush(mockResponse);

    expect(service.isAuthenticated()).toBeTrue();
    expect(service.user()?.role).toBe('admin');
    expect(service.userType()).toBe('admin');
    expect(localStorage.getItem('token')).toBe('jwt-token');
    expect(localStorage.getItem('user')).toContain('admin@techriders.es');
  });

  it('should load persisted user from localStorage', () => {
    localStorage.setItem('user', JSON.stringify(mockUser));

    const reloaded = new AuthService({} as any);

    expect(reloaded.isAuthenticated()).toBeTrue();
    expect(reloaded.user()?.id).toBe('u1');
    expect(reloaded.userType()).toBe('admin');
  });

  it('should logout and clear session data', () => {
    localStorage.setItem('token', 'jwt-token');
    localStorage.setItem('user', JSON.stringify(mockUser));

    const reloaded = new AuthService({} as any);
    expect(reloaded.isAuthenticated()).toBeTrue();

    reloaded.logout();

    expect(reloaded.isAuthenticated()).toBeFalse();
    expect(reloaded.user()).toBeNull();
    expect(reloaded.userType()).toBeNull();
    expect(localStorage.getItem('token')).toBeNull();
    expect(localStorage.getItem('user')).toBeNull();
  });
});


