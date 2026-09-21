import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter, Router } from '@angular/router';
import { of, throwError } from 'rxjs';
import { Login } from './login';
import { AuthService } from '@core/auth/auth.service';

describe('Login', () => {
  let fixture: ComponentFixture<Login>;
  let component: Login;
  let router: Router;
  let authService: jasmine.SpyObj<AuthService>;

  beforeEach(async () => {
    authService = jasmine.createSpyObj('AuthService', ['login', 'getDefaultRoute']);
    authService.login.and.returnValue(of({ token: 'jwt', user: { id: '1', email: 'a@b.com', name: 'Admin', role: 'admin', roles: ['admin'] } }));
    authService.getDefaultRoute.and.returnValue('/intranet');

    await TestBed.configureTestingModule({
      imports: [Login],
      providers: [
        provideRouter([]),
        { provide: AuthService, useValue: authService },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(Login);
    component = fixture.componentInstance;
    router = TestBed.inject(Router);
    spyOn(router, 'navigateByUrl').and.stub();
    fixture.detectChanges();
  });

  it('should redirect immediately to the intranet after a successful login without a refresh', () => {
    component.email.set('a@b.com');
    component.password.set('secret');

    component.onSubmit(new Event('submit'));

    expect(authService.login).toHaveBeenCalledWith('a@b.com', 'secret');
    expect(router.navigateByUrl).toHaveBeenCalledWith('/intranet', { replaceUrl: true });
  });

  it('should keep the login error state when the backend rejects the credentials', () => {
    authService.login.and.returnValue(throwError(() => ({ status: 401 })));

    component.email.set('a@b.com');
    component.password.set('bad');

    component.onSubmit(new Event('submit'));

    expect(component.error()).toBe('Usuario o contraseña incorrectos');
    expect(router.navigateByUrl).not.toHaveBeenCalled();
  });
});
