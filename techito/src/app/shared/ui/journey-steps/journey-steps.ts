import { ChangeDetectionStrategy, Component, Input } from '@angular/core';

export interface UiJourneyStep {
  step: string;
  title: string;
  text: string;
}

@Component({
  selector: 'app-ui-journey-steps',
  standalone: true,
  templateUrl: './journey-steps.html',
  styleUrl: './journey-steps.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class UiJourneySteps {
  @Input() title = '';
  @Input() kicker = '';
  @Input() steps: UiJourneyStep[] = [];
}
