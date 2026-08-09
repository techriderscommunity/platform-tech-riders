import { ChangeDetectionStrategy, Component, OnInit, signal } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-footer',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [RouterLink],
  templateUrl: './footer.html',
  styleUrl: './footer.scss'
})
export class Footer {
  readonly year = new Date().getFullYear();
  readonly isDarkMode = signal(true);

  ngOnInit(): void {
    if (typeof window === 'undefined') {
      return;
    }

    const savedTheme = localStorage.getItem('tr-theme') || 'dark';
    this.isDarkMode.set(savedTheme === 'dark');
  }
}


