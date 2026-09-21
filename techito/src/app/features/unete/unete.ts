import { ChangeDetectionStrategy, Component, DestroyRef, OnInit, inject } from '@angular/core';
import { Router } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { catchError, EMPTY, tap } from 'rxjs';
import { JoinStatsService } from './join-stats.service';
import { JOIN_METRICS_META } from './unete.content';
import { MetricItem } from '@shared/ui/public-content.types';
import { UiButton  } from '@shared/ui/button/button';

@Component({
  selector: 'app-unete',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [UiButton],
  templateUrl: './unete.html',
  styleUrl: './unete.scss'
})
export class Unete implements OnInit {
  private readonly joinStatsService = inject(JoinStatsService);
  private readonly destroyRef = inject(DestroyRef);
  private readonly router = inject(Router);

  joinMetrics: MetricItem[] = [];

  ngOnInit(): void {
    this.joinStatsService
      .getStats()
      .pipe(
        tap((stats) => {
          this.joinMetrics = JOIN_METRICS_META.map((meta) => ({
            icon: meta.icon,
            label: meta.label,
            value: meta.staticValue ?? String(stats.activeAmbassadors),
          }));
        }),
        catchError(() => EMPTY),
        takeUntilDestroyed(this.destroyRef)
      )
      .subscribe();
  }

  abrirAcceso() {
    this.router.navigate([], {
      queryParams: {
        login: '1',
        authMode: 'register',
        returnUrl: '/intranet',
      },
      queryParamsHandling: 'merge',
    });
  }
}


