import { ChangeDetectionStrategy, Component, signal, computed, inject, DestroyRef } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { HttpClient } from '@angular/common/http';
import { environment } from '\.\./\.\./\.\./\.\./environments/environment';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { tap, catchError } from 'rxjs/operators';
import { of } from 'rxjs';
import { Embajador, PagedResult } from './models/embajadores.models';
import { UiSelect, UiSelectOption  } from '@shared/ui/select/select';

interface AmbassadorApi {
  Id: string;
  Name: string;
  LastName: string;
  CategoryName?: string | null;
  OtherCategory?: string | null;
  UpdatedAt?: string | null;
  CreatedAt: string;
}

interface TalkItem {
  topic: string;
  speaker: string;
  date: string;
  rating: number;
}

@Component({
  selector: 'app-staff',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [FormsModule, NgxChartsModule, UiSelect],
  templateUrl: './staff.html',
  styleUrl: './staff.scss'
})
export class Staff {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = environment.apiUrl;
  private readonly destroyRef = inject(DestroyRef);

  readonly periodOptions = [
    { label: 'Este mes', value: 'month' },
    { label: 'Este año', value: 'year' },
    { label: 'Todo', value: 'all' }
  ];
  readonly periodSelectOptions: UiSelectOption[] = this.periodOptions.map(option => ({
    label: option.label,
    value: option.value
  }));
  readonly selectedPeriod = signal('month');
  readonly loading = signal(true);
  readonly talks = signal<TalkItem[]>([]);

  readonly filteredTalks = computed(() => {
    const now = new Date();
    const period = this.selectedPeriod();
    if (period === 'month') {
      return this.talks().filter(t => {
        const d = new Date(t.date);
        return d.getMonth() === now.getMonth() && d.getFullYear() === now.getFullYear();
      });
    }
    if (period === 'year') {
      return this.talks().filter(t => new Date(t.date).getFullYear() === now.getFullYear());
    }
    return this.talks();
  });

  readonly totalTalks = computed(() => this.filteredTalks().length);
  readonly uniqueTopics = computed(() => Array.from(new Set(this.filteredTalks().map(t => t.topic))).length);
  readonly uniqueSpeakers = computed(() => Array.from(new Set(this.filteredTalks().map(t => t.speaker))).length);
  readonly avgRating = computed(() => {
    const ft = this.filteredTalks();
    if (!ft.length) return '0';
    return (ft.reduce((sum, t) => sum + t.rating, 0) / ft.length).toFixed(2);
  });

  readonly topicChartData = computed(() => {
    const topicCounts: { [key: string]: number } = {};
    this.filteredTalks().forEach(t => { topicCounts[t.topic] = (topicCounts[t.topic] || 0) + 1; });
    return Object.keys(topicCounts).map(topic => ({ name: topic, value: topicCounts[topic] }));
  });

  readonly speakerChartData = computed(() => {
    const speakerCounts: { [key: string]: number } = {};
    this.filteredTalks().forEach(t => { speakerCounts[t.speaker] = (speakerCounts[t.speaker] || 0) + 1; });
    return Object.keys(speakerCounts).map(speaker => ({ name: speaker, value: speakerCounts[speaker] }));
  });

  readonly colorScheme = 'vivid';

  constructor() {
    this.loadTalks();
  }

  private loadTalks() {
    this.http.get<AmbassadorApi[]>(`${this.baseUrl}/ambassadors`)
      .pipe(
        tap((result) => {
          const talks = (result ?? []).map((embajador): TalkItem => ({
            topic: embajador.CategoryName ?? embajador.OtherCategory ?? 'General',
            speaker: [embajador.Name, embajador.LastName].filter(Boolean).join(' ').trim(),
            date: embajador.UpdatedAt ?? embajador.CreatedAt,
            rating: 3,
          }));
          this.talks.set(talks);
          this.loading.set(false);
        }),
        catchError(() => {
          this.loading.set(false);
          return of([] as AmbassadorApi[]);
        }),
        takeUntilDestroyed(this.destroyRef)
      )
      .subscribe();
  }

  formatPercentLabel(data: { value: number }): string {
    const chartData = this.topicChartData();
    const total = chartData.reduce((sum, d) => sum + d.value, 0);
    const percent = total ? ((data.value / total) * 100).toFixed(1) : '0';
    return `${percent}%`;
  }
}



