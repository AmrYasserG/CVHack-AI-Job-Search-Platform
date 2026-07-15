import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-match-score',
  imports: [],
  templateUrl: './match-score.html',
  styleUrl: './match-score.css',
})
export class MatchScore {

  @Input() score: number = 0;

  get scoreColor() {
    if (this.score >= 80) return 'var(--green)';
    if (this.score >= 60) return 'var(--amber)';
    return 'var(--red)';
  }
}
