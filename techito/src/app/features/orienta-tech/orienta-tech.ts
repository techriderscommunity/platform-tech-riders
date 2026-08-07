import { ChangeDetectionStrategy, Component } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-orienta-tech',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [RouterLink],
  templateUrl: './orienta-tech.html',
  styleUrl: './orienta-tech.scss'
})
export class OrientaTech {

}


