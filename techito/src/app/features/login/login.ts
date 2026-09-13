import { ChangeDetectionStrategy, Component, signal, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
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

  readonly email = signal('');
  readonly password = signal('');
  readonly error = signal('');
  readonly loading = signal(false);

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
      queryParams: { login: null, returnUrl: null },
      queryParamsHandling: 'merge',
    });
  }

  onSubmit(event?: Event) {
    if (event) event.preventDefault();
    this.loading.set(true);
    this.error.set('');

    this.authService.login(this.email(), this.password()).subscribe({
      next: () => {
        this.loading.set(false);
        const targetUrl = this.authService.getDefaultRoute();

        this.router.navigateByUrl(targetUrl, {
          replaceUrl: true,
        });
      },
      error: () => {
        this.loading.set(false);
        this.error.set('Usuario o contraseña incorrectos');
      }
    });
  }
}


