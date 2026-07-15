import { Component, Input } from '@angular/core';

export interface StatCard {
  icon: string;
  value: string;
  label: string;
  change: string;
}

@Component({
  selector: 'app-stats-cards',
  imports: [],
  templateUrl: './stats-cards.html',
  styleUrl: './stats-cards.css',
})
export class StatsCards {
  @Input() cards: StatCard[] = [];
}
