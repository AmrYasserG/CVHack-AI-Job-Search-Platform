import { Component, EventEmitter, Input, OnChanges, Output, SimpleChanges } from '@angular/core';
import { JobCard } from '../job-card/job-card';
import { FormsModule } from '@angular/forms';
import type { Job } from '../../models/job.model';
import { JobFilters } from '../../models/job-filters.model';
import { FilterIcon } from '../../assets/filter-icon/filter-icon';

@Component({
  selector: 'app-job-cards-container',
  imports: [JobCard, FormsModule, FilterIcon],
  templateUrl: './job-cards-container.html',
  styleUrl: './job-cards-container.css',
})
export class JobCardsContainer implements OnChanges {
  @Input() activeFilters: JobFilters | null = null;
  @Input() jobs: Job[] = [];
  @Input() searchTerm = '';
  @Input() isLoading = false;
  @Output() openFilters = new EventEmitter<void>();

  sortOptions = ['Most recent'];
  selectedSort = 'Most recent';


  readonly pageSize = 10;
  currentPage = 1;

  ngOnChanges(changes: SimpleChanges) {
    if (changes['jobs'] || changes['searchTerm'] || changes['activeFilters']) {
      this.currentPage = 1;
    }
  }

  private dateLimits: Record<string, number> = {
    'Past 24h': 24,
    'Past week': 24 * 7,
    'Past month': 24 * 30,
  };



  get filteredJobs(): Job[] {
    const term = this.searchTerm.trim().toLowerCase();
    const f = this.activeFilters;

    return this.jobs.filter((job) => {
      // text search
      const matchesSearch =
        !term ||
        job.title.toLowerCase().includes(term) ||
        job.company.toLowerCase().includes(term) ||
        job.location.toLowerCase().includes(term) ||
        job.tags.some((t) => t.toLowerCase().includes(term));
      if (!matchesSearch) return false;

      // sidebar filters
      if (!f) return true;
      return (
        (!f.country || job.country === f.country) &&
        (!f.city || job.city === f.city) &&
        (f.workType.length === 0 || f.workType.includes(job.workType)) &&
        (f.employment.length === 0 || f.employment.includes(job.workTime)) &&
        (f.seniority.length === 0 || f.seniority.includes(job.seniority)) &&
        (job.salaryMax === 0 || job.salaryMax >= f.minSalary) &&
        (f.datePosted === 'Any time' ||
          this.parsePostedAgo(job.postedAgo) <= this.dateLimits[f.datePosted])
      );
    });
  }

  get sortedJobs(): Job[] {
    const list = [...this.filteredJobs];

    switch (this.selectedSort) {
      case 'Best match':
        return list.sort((a, b) => b.matchScore - a.matchScore);

      case 'Highest salary':
        return list.sort((a, b) => b.salaryMax - a.salaryMax);

      case 'Most recent':
        return list.sort((a, b) => this.parsePostedAgo(a.postedAgo) - this.parsePostedAgo(b.postedAgo));

      default:
        return list;
    }
  }

  // ---- pagination (over the filtered + sorted list) ----
  get totalPages(): number {
    return Math.max(1, Math.ceil(this.sortedJobs.length / this.pageSize));
  }

  get pagedJobs(): Job[] {
    const page = Math.min(this.currentPage, this.totalPages);
    const start = (page - 1) * this.pageSize;
    return this.sortedJobs.slice(start, start + this.pageSize);
  }

  get pages(): number[] {
    return Array.from({ length: this.totalPages }, (_, i) => i + 1);
  }

  goToPage(p: number) {
    this.currentPage = Math.min(Math.max(1, p), this.totalPages);
  }
  nextPage() { this.goToPage(this.currentPage + 1); }
  prevPage() { this.goToPage(this.currentPage - 1); }

  onSortChange() { this.currentPage = 1; }

  // Converts "2h ago", "1d ago", "1w ago" into a comparable number of hours
  private parsePostedAgo(value: string): number {
    const match = value.match(/(\d+)([hdw])/);
    if (!match) return Infinity;

    const num = Number(match[1]);
    const unit = match[2];

    if (unit === 'h') return num;
    if (unit === 'd') return num * 24;
    if (unit === 'w') return num * 24 * 7;
    return Infinity;
  }
}