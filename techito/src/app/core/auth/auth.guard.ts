import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AppRole, AuthService } from './auth.service';

function toLoginModal(router: Router, returnUrl: string) {
  return router.createUrlTree(['/'], {
    queryParams: {
      login: '1',
      returnUrl,
    },
  });
}

export const authGuard: CanActivateFn = (_, state) => {
  const auth = inject(AuthService);
  const router = inject(Router);

  if (auth.isAuthenticated()) {
    return true;
  }

  return toLoginModal(router, state.url || '/');
};

export function roleGuard(requiredRole: AppRole | AppRole[]): CanActivateFn {
  return (_, state) => {
    const auth = inject(AuthService);
    const router = inject(Router);

    if (!auth.isAuthenticated()) {
      return toLoginModal(router, state.url || '/');
    }

    if (auth.hasRole(requiredRole)) {
      return true;
    }

    // Autenticado pero rol incorrecto → redirigir a su área
    return router.parseUrl(auth.getDefaultRoute());
  };
}


