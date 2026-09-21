import { signal } from '@angular/core';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { of } from 'rxjs';
import { AuthService, type UserProfile } from '@core/auth/auth.service';
import { IntranetLayout } from './intranet-layout';
import { IntranetLayoutTraceService } from './services/intranet-layout-trace.service';

describe('IntranetLayout', () => {
  let fixture: ComponentFixture<IntranetLayout>;

  beforeEach(() => {
    const user = signal<UserProfile | null>({
      id: 'admin-1',
      email: 'admin@techriders.es',
      name: 'Administradora',
      role: 'admin',
      roles: ['admin', 'ambassador'],
    });

    TestBed.configureTestingModule({
      imports: [IntranetLayout],
      providers: [
        provideRouter([]),
        {
          provide: AuthService,
          useValue: {
            user: user.asReadonly(),
            getDefaultRoute: () => '/intranet',
          },
        },
        {
          provide: IntranetLayoutTraceService,
          useValue: { emitHeartbeatTrace: () => of(null) },
        },
      ],
    });

    fixture = TestBed.createComponent(IntranetLayout);
    fixture.detectChanges();
  });

  it('should show the resolved role instead of the user name', () => {
    const role = fixture.nativeElement.querySelector('.role-pill')?.textContent;
    const userName = fixture.nativeElement.querySelector('.role-user')?.textContent;

    expect(role).toContain('Administrador');
    expect(userName).toContain('Administradora');
  });

  it('should render the administrator workspace without ambassador modules', () => {
    const menu = fixture.nativeElement.querySelector('.sidebar-nav')?.textContent;

    expect(menu).toContain('Usuarios y roles');
    expect(menu).not.toContain('Portal de embajadores');
    expect(menu).not.toContain('Mis eventos');
  });
});
