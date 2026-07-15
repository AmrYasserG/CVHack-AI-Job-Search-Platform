import { Component, OnInit, inject, ChangeDetectorRef } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';
import { environment } from '../../../environments/environment';
import { AdminSidebar } from '../../components/admin-sidebar/admin-sidebar';
import { StatsCards, StatCard } from './components/stats-cards/stats-cards';
import { JobsByCategoryChart, CategoryBar } from './components/jobs-by-category-chart/jobs-by-category-chart';
import { RouterLink } from '@angular/router';
// اتعمله كومنت بناءً على طلب المستخدمة، ممكن نرجعهم تاني لو احتجناهم
// import { DailySearchesChart, DailyPoint } from './components/daily-searches-chart/daily-searches-chart';
// import { MatchScoreChart, ScoreBucket } from './components/match-score-chart/match-score-chart';

@Component({
  selector: 'app-admin-dashboard',
  imports: [AdminSidebar, StatsCards, JobsByCategoryChart],
  templateUrl: './admin-dashboard.html',
  styleUrl: './admin-dashboard.css',
})
export class AdminDashboard implements OnInit {
  private http = inject(HttpClient);
  private cdr = inject(ChangeDetectorRef);

  dateRange = 'Jun 8 – Jun 14, 2026';

  stats: StatCard[] = [
    { icon: `<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M17 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2"/><circle cx="9" cy="7" r="4"/><path d="M23 21v-2a4 4 0 0 0-3-3.87"/><path d="M16 3.13a4 4 0 0 1 0 7.75"/></svg>`, value: '...', label: 'Total users', change: '' },
    { icon: `<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><rect x="2" y="7" width="20" height="14" rx="2"/><path d="M16 7V5a2 2 0 0 0-4 0v2"/></svg>`, value: '...', label: 'Active jobs', change: '' },
    { icon: `<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><circle cx="11" cy="11" r="8"/><line x1="21" y1="21" x2="16.65" y2="16.65"/></svg>`, value: '...', label: 'Support tickets', change: '' },
    // اتعمله كومنت بناءً على طلب المستخدمة، ممكن نرجعه تاني لو احتجناه
    // { icon: `<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><circle cx="12" cy="12" r="10"/><circle cx="12" cy="12" r="4"/></svg>`, value: '71%', label: 'Avg match score', change: '+3%' },
  ];

  async ngOnInit() {
    await this.loadRealStats();
  }

  async loadRealStats() {
    try {
      const usersRes = await firstValueFrom(
        this.http.get<any>(`${environment.apiUrl}/admin/users`)
      );
      const usersCount = usersRes.data?.length ?? 0;
      this.stats = this.stats.map((s, i) => i === 0 ? { ...s, value: usersCount.toLocaleString() } : s);
      this.cdr.detectChanges();
    } catch (err) {
      console.error('Users error:', err);
    }

    try {
      const jobsRes = await firstValueFrom(
        this.http.get<any>(`${environment.apiUrl}/jobs`)
      );
      const jobs = jobsRes.data ?? [];
      const jobsCount = jobs.length;
      this.stats = this.stats.map((s, i) => i === 1 ? { ...s, value: jobsCount.toLocaleString() } : s);

      // Count jobs by category from title
      const categories: { [key: string]: number } = {};
      jobs.forEach((job: any) => {
        const title = job.title.toLowerCase();
        let category = 'Other';
        if (title.includes('frontend') || title.includes('front-end') || title.includes('react') || title.includes('angular') || title.includes('ui')) category = 'Frontend';
        else if (title.includes('backend') || title.includes('back-end') || title.includes('node') || title.includes('.net')) category = 'Backend';
        else if (title.includes('fullstack') || title.includes('full stack') || title.includes('full-stack')) category = 'Full Stack';
        else if (title.includes('designer') || title.includes('ux') || title.includes('design')) category = 'Design';
        else if (title.includes('data') || title.includes('analyst') || title.includes('ml') || title.includes('ai')) category = 'Data & AI';
        else if (title.includes('devops') || title.includes('cloud') || title.includes('security')) category = 'DevOps';
        else if (title.includes('mobile') || title.includes('flutter') || title.includes('ios') || title.includes('android')) category = 'Mobile';
        else if (title.includes('manager') || title.includes('product')) category = 'Product';
        categories[category] = (categories[category] ?? 0) + 1;
      });

      this.jobsByCategory = Object.entries(categories)
        .map(([label, value]) => ({ label, value }))
        .sort((a, b) => b.value - a.value);

      this.cdr.detectChanges();
    } catch (err) {
      console.error('Jobs error:', err);
    }

    try {
      const ticketsRes = await firstValueFrom(
        this.http.get<any>(`${environment.apiUrl}/admin/tickets`)
      );
      const ticketsCount = ticketsRes.data?.length ?? 0;
      this.stats = this.stats.map((s, i) => i === 2 ? { ...s, value: ticketsCount.toLocaleString() } : s);
      this.cdr.detectChanges();
    } catch (err) {
      console.error('Tickets error:', err);
    }
  }

  // اتعمله كومنت بناءً على طلب المستخدمة، ممكن نرجعهم تاني لو احتجناهم
  // dailySearches: DailyPoint[] = [
  //   { day: 'Mon', value: 980 },
  //   { day: 'Tue', value: 870 },
  //   { day: 'Wed', value: 1050 },
  //   { day: 'Thu', value: 1120 },
  //   { day: 'Fri', value: 1300 },
  //   { day: 'Sat', value: 1250 },
  //   { day: 'Sun', value: 1400 },
  // ];

  jobsByCategory: CategoryBar[] = [];

  // اتعمله كومنت بناءً على طلب المستخدمة، ممكن نرجعه تاني لو احتجناه
  // matchScores: ScoreBucket[] = [
  //   { range: '0–20', value: 60, color: '#B24A3F' },
  //   { range: '20–40', value: 120, color: '#B87333' },
  //   { range: '40–60', value: 240, color: '#B87333' },
  //   { range: '60–80', value: 360, color: '#2E7D5B' },
  //   { range: '80–100', value: 180, color: '#2E7D5B' },
  // ];
}