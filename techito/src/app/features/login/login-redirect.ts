import { Component, inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-login-redirect',
  standalone: true,
  template: ''
})
export class LoginRedirect {
  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);

  constructor() {
    const returnUrl = this.route.snapshot.queryParamMap.get('returnUrl') || '/';
    this.router.navigate(['/'], {
      queryParams: {
        login: '1',
        returnUrl,
      },
      replaceUrl: true,
    });
  }
}


