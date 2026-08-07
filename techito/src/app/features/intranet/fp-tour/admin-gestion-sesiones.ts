import { ChangeDetectionStrategy, Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { CalendarEventos } from './calendar-eventos';

@Component({
  selector: 'app-admin-gestion-sesiones',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [RouterLink, CalendarEventos],
  templateUrl: './admin-gestion-sesiones.html',
  styleUrl: './admin-gestion-sesiones.scss'
})
export class AdminGestionSesiones {}


