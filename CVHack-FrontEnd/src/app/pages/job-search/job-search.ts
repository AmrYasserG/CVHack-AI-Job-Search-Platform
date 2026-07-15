import { Component, OnInit, computed, inject, signal } from '@angular/core';
import { Navbar } from '../../components/navbar/navbar';
import { Sidebar } from '../../components/sidebar/sidebar';
import { JobCardsContainer } from '../../components/job-cards-container/job-cards-container';
import { JobFilters } from '../../models/job-filters.model';
import { Job } from '../../models/job.model';
import { JobsService } from '../../services/jobs.service';
import { buildJobFilterOptions } from '../../models/job-filter-options.builder';

@Component({
  selector: 'app-job-search',
  imports: [Navbar, Sidebar, JobCardsContainer],
  templateUrl: './job-search.html',
  styleUrl: './job-search.css',
})
export class JobSearch implements OnInit {
  private jobsService = inject(JobsService);

  jobs = signal<Job[]>([]);
  filterOptions = computed(() => buildJobFilterOptions(this.jobs()));
  isLoading = signal(true);
  errorMessage = signal('');
  currentFilters: JobFilters | null = null;
  showFilters = signal(false);
  searchTerm = signal('');

  onSearchChange(term: string) {
    this.searchTerm.set(term);
  }
  
  ngOnInit() {
    this.jobsService.getJobs().subscribe({
      next: (jobs) => { this.jobs.set(jobs); this.isLoading.set(false); },
      error: () => { this.errorMessage.set('Failed to load jobs.'); this.isLoading.set(false); },
    });
  }

  onFiltersChanged(filters: JobFilters) {
    this.currentFilters = filters;
  }
}