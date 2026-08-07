import { Component, computed, inject, signal } from '@angular/core';
import { Router, RouterOutlet, NavigationEnd, ActivatedRoute } from '@angular/router';
import { toSignal } from '@angular/core/rxjs-interop';
import { filter, startWith } from 'rxjs/operators';
import { Header } from './core/layout/header';
import { Footer } from './core/layout/footer';
import { Login } from './features/login/login';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, Header, Footer, Login],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);
  private readonly navigationDone = toSignal(
    this.router.events.pipe(
      filter((event) => event instanceof NavigationEnd),
      startWith(null)
    )
  );

  protected readonly title = signal('techito');
  protected readonly showLoginModal = computed(() => {
    this.navigationDone();
    return this.route.snapshot.queryParamMap.get('login') === '1';
  });
}


