import { Component, Input } from '@angular/core';

export interface CategoryBar { label: string; value: number; }

@Component({
  selector: 'app-jobs-by-category-chart',
  imports: [],
  templateUrl: './jobs-by-category-chart.html',
  styleUrl: './jobs-by-category-chart.css',
})
export class JobsByCategoryChart {
  @Input() data: CategoryBar[] = [];

  get maxVal(): number {
    return Math.max(...this.data.map(d => d.value));
  }

  barHeight(val: number): number {
    return (val / this.maxVal) * 100;
  }

  isFirst(i: number): boolean {
    return i === 0;
  }
}
