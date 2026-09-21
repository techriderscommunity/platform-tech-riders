import { ChangeDetectionStrategy, Component, signal, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService  } from '@core/auth/auth.service';
import { UiModal  } from '@shared/ui/modal/modal';
import { UiTextField  } from '@shared/ui/text-field/text-field';
import { UiButton  } from '@shared/ui/button/button';

@Component({
  selector: 'app-login',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [FormsModule, UiModal, UiTextField, UiButton],
  templateUrl: './login.html',
  styleUrl: './login.scss'
})
export class Login {
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);

  readonly email = signal('');
  readonly password = signal('');
  readonly name = signal('');
  readonly lastName = signal('');
  readonly error = signal('');
  readonly loading = signal(false);
  readonly mode = signal<'login' | 'register'>('login');

  constructor() {
    if (this.getQueryParam('authMode') === 'register') {
      this.mode.set('register');
    }
  }

  closeLogin(event?: Event | void) {
    if (event instanceof Event) {
      event.preventDefault();
    }
    const currentPath = this.router.url.split('?')[0];
    if (currentPath === '/login') {
      this.router.navigate(['/']);
      return;
    }

    this.router.navigate([], {
      queryParams: { login: null, returnUrl: null, authMode: null },
      queryParamsHandling: 'merge',
    });
  }

  setMode(mode: 'login' | 'register'): void {
    this.mode.set(mode);
    this.error.set('');
  }

  onSubmit(event?: Event) {
    if (event) event.preventDefault();
    this.loading.set(true);
    this.error.set('');

    const request$ = this.mode() === 'register'
      ? this.authService.register({
          nickname: this.buildNickname(),
          name: this.name().trim(),
          lastName: this.lastName().trim(),
          email: this.email().trim(),
          password: this.password(),
        })
      : this.authService.login(this.email().trim(), this.password());

    request$.subscribe({
      next: () => {
        this.loading.set(false);
        const targetUrl = this.getQueryParam('returnUrl') || this.authService.getDefaultRoute();

        this.router.navigateByUrl(targetUrl, {
          replaceUrl: true,
        });
      },
      error: () => {
        this.loading.set(false);
        this.error.set(this.mode() === 'register'
          ? 'No se pudo crear la cuenta. Revisa los datos o usa otro email.'
          : 'Usuario o contraseña incorrectos');
      }
    });
  }

  private buildNickname(): string {
    const emailPrefix = this.email().split('@')[0]?.trim();
    const normalizedName = `${this.name()} ${this.lastName()}`
      .normalize('NFD')
      .replace(/[\u0300-\u036f]/g, '')
      .toLowerCase()
      .replace(/[^a-z0-9]+/g, '-')
      .replace(/(^-|-$)/g, '');

    return normalizedName || emailPrefix || `member-${Date.now()}`;
  }

  private getQueryParam(name: string): string | null {
    return this.router.parseUrl(this.router.url).queryParams[name] ?? null;
  }
}


