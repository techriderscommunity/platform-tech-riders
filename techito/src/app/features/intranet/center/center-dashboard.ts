import { ChangeDetectionStrategy, Component } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-center-dashboard',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './center-dashboard.html',
  styleUrl: './center-dashboard.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CenterDashboard {}
