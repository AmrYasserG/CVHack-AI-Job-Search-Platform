import { Component, Input } from '@angular/core';

export interface ScoreBucket { range: string; value: number; color: string; }

@Component({
  selector: 'app-match-score-chart',
  imports: [],
  templateUrl: './match-score-chart.html',
  styleUrl: './match-score-chart.css',
})
export class MatchScoreChart {
  @Input() data: ScoreBucket[] = [];

  get maxVal(): number {
    return Math.max(...this.data.map(d => d.value));
  }

  barHeight(val: number): number {
    return (val / this.maxVal) * 100;
  }
}
