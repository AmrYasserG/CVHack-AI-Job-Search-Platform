import { Component, EventEmitter, Output, signal, effect, input, untracked, computed } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { FilterIcon } from '../../assets/filter-icon/filter-icon';
import { JobFilters } from '../../models/job-filters.model';
import { JobFilterOptions } from '../../models/job-filter-options.model';

@Component({
  selector: 'app-sidebar',
  imports: [FormsModule, FilterIcon],
  templateUrl: './sidebar.html',
  styleUrl: './sidebar.css',
})
export class Sidebar {
  options = input<JobFilterOptions>({
    countries: [], citiesByCountry: {}, workTypes: [], workTimes: [], seniorities: [],
    salaryMin: 0, salaryMax: 200,
  });

  dateOptions = ['Any time', 'Past 24h', 'Past week', 'Past month'];


  salarySliderStep = computed(() => {
    const range = this.options().salaryMax - this.options().salaryMin;
    if (range <= 0) return 1;
    for (const step of [3, 2]) {
      if (range % step === 0) return step;
    }
    return 1;
  });

  filters = signal<JobFilters>({
    country: '', city: '',
    workType: [], seniority: [], employment: [],
    minSalary: 10, datePosted: 'Any time',
  });

  @Output() filtersChanged = new EventEmitter<JobFilters>();

  constructor() {
    effect(() => this.filtersChanged.emit(this.filters()));

    effect(() => {
      const opts = this.options();
      untracked(() => {
        this.filters.update((f) => ({
          ...f,
          minSalary: Math.min(Math.max(f.minSalary, opts.salaryMin), opts.salaryMax),
        }));
      });
    });
  }

  get cities() {
    return this.options().citiesByCountry[this.filters().country] ?? [];
  }

  onCountryChange(country: string) { this.filters.update(f => ({ ...f, country, city: '' })); }
  onCityChange(city: string) { this.filters.update(f => ({ ...f, city })); }
  onSalaryChange(minSalary: number) { this.filters.update(f => ({ ...f, minSalary })); }

  toggle(key: 'workType' | 'seniority' | 'employment', value: string) {
    this.filters.update(f => {
      const arr = f[key];
      const updated = arr.includes(value) ? arr.filter(v => v !== value) : [...arr, value];
      return { ...f, [key]: updated };
    });
  }

  isSelected(key: 'workType' | 'seniority' | 'employment', value: string) {
    return this.filters()[key].includes(value);
  }

  setDatePosted(opt: string) { this.filters.update(f => ({ ...f, datePosted: opt })); }

  clearAll() {
    this.filters.set({
      country: '', city: '',
      workType: [], seniority: [], employment: [],
      minSalary: 10, datePosted: 'Any time',
    });
  }
}