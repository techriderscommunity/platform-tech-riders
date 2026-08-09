import { ChangeDetectionStrategy, Component, Input } from '@angular/core';

export interface UiMetricItem {
  value: string;
  label: string;
  icon?: string;
}

@Component({
  selector: 'app-ui-metrics-strip',
  standalone: true,
  templateUrl: './metrics-strip.html',
  styleUrl: './metrics-strip.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class UiMetricsStrip {
  @Input() items: UiMetricItem[] = [];
}
