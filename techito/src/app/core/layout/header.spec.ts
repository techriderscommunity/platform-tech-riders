import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { Header } from './header';
import { AuthService } from '../auth/auth.service';

describe('Header', () => {
  let fixture: ComponentFixture<Header>;
  let component: Header;

  const authMock = {
    isAuthenticated: jasmine.createSpy('isAuthenticated').and.returnValue(false),
    logout: jasmine.createSpy('logout'),
  };

  beforeEach(async () => {
    localStorage.clear();

    await TestBed.configureTestingModule({
      imports: [Header],
      providers: [
        provideRouter([]),
        { provide: AuthService, useValue: authMock },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(Header);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  afterEach(() => {
    localStorage.clear();
  });

  it('should toggle mobile menu state', () => {
    expect(component.mobileMenuOpen()).toBeFalse();

    component.toggleMobileMenu();
    expect(component.mobileMenuOpen()).toBeTrue();

    component.closeMobileMenu();
    expect(component.mobileMenuOpen()).toBeFalse();
  });

  it('should restore theme from localStorage on init', () => {
    localStorage.setItem('tr-theme', 'light');

    component.ngOnInit();

    expect(component.isDarkMode()).toBeFalse();
    expect(document.documentElement.getAttribute('data-theme')).toBe('light');
  });

  it('should toggle theme and persist preference', () => {
    const previous = component.isDarkMode();

    component.toggleTheme();

    expect(component.isDarkMode()).toBe(!previous);
    expect(localStorage.getItem('tr-theme')).toBe(component.isDarkMode() ? 'dark' : 'light');
  });

  it('should logout through AuthService', () => {
    component.logout();

    expect(authMock.logout).toHaveBeenCalled();
  });
});


