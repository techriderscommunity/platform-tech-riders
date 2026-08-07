import { TestBed } from '@angular/core/testing';
import { provideRouter, Router, UrlTree } from '@angular/router';
import { authGuard, roleGuard } from './auth.guard';
import { AuthService } from './auth.service';

describe('auth guards', () => {
  let router: Router;

  const authMock = {
    isAuthenticated: jasmine.createSpy('isAuthenticated'),
    user: jasmine.createSpy('user'),
  };

  beforeEach(() => {
    authMock.isAuthenticated.calls.reset();
    authMock.user.calls.reset();

    TestBed.configureTestingModule({
      providers: [
        provideRouter([]),
        { provide: AuthService, useValue: authMock },
      ],
    });

    router = TestBed.inject(Router);
  });

  it('authGuard should allow authenticated users', () => {
    authMock.isAuthenticated.and.returnValue(true);

    const result = TestBed.runInInjectionContext(() => authGuard({} as any, { url: '/intranet/admin' } as any));

    expect(result).toBeTrue();
  });

  it('authGuard should redirect anonymous users to login', () => {
    authMock.isAuthenticated.and.returnValue(false);

    const result = TestBed.runInInjectionContext(() => authGuard({} as any, { url: '/intranet/admin' } as any)) as UrlTree;

    expect(router.serializeUrl(result)).toBe('/?login=1&returnUrl=%2Fintranet%2Fadmin');
  });

  it('roleGuard should allow matching role', () => {
    authMock.isAuthenticated.and.returnValue(true);
    authMock.user.and.returnValue({ role: 'admin' });

    const result = TestBed.runInInjectionContext(() => roleGuard('admin')({} as any, {} as any));

    expect(result).toBeTrue();
  });

  it('roleGuard should redirect to role area on role mismatch', () => {
    authMock.isAuthenticated.and.returnValue(true);
    authMock.user.and.returnValue({ role: 'empresa' });

    const result = TestBed.runInInjectionContext(() => roleGuard('admin')({} as any, {} as any)) as UrlTree;

    expect(router.serializeUrl(result)).toBe('/intranet/empresa');
  });

  it('roleGuard should redirect anonymous users to login', () => {
    authMock.isAuthenticated.and.returnValue(false);
    authMock.user.and.returnValue(null);

    const result = TestBed.runInInjectionContext(() => roleGuard('junior')({} as any, { url: '/intranet/junior' } as any)) as UrlTree;

    expect(router.serializeUrl(result)).toBe('/?login=1&returnUrl=%2Fintranet%2Fjunior');
  });
});


