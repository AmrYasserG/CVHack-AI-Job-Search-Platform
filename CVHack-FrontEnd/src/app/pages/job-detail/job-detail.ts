import { Component, OnInit, inject, ChangeDetectorRef } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Navbar } from '../../components/navbar/navbar';
import { JobHeader, JobHeaderData } from './components/job-header/job-header';
import { JobAbout, JobAboutData } from './components/job-about/job-about';
import { AiMatchCard, AiMatchData } from './components/ai-match-card/ai-match-card';
import { CompanyBriefing, CompanyBriefingData } from './components/company-briefing/company-briefing';
import { JobActions } from './components/job-actions/job-actions';
import { JobsService } from '../../services/jobs.service';
import { Job } from '../../models/job.model';

@Component({
  selector: 'app-job-detail',
  imports: [Navbar, JobHeader, JobAbout, AiMatchCard, CompanyBriefing, JobActions],
  templateUrl: './job-detail.html',
  styleUrl: './job-detail.css',
})
export class JobDetail implements OnInit {
  private route = inject(ActivatedRoute);
  private jobsService = inject(JobsService);
  private http = inject(HttpClient);
  private cdr = inject(ChangeDetectorRef);

  loading = true;
  error = '';
  job: Job | null = null;
  jobId = 0;
  header!: JobHeaderData;
  about!: JobAboutData;

  // الـ AI match analysis بياخد وقت لإنه AI agent، فبنخليه له loading منفصل
  // عشان متستناش كل الصفحة لحد ما يرجع
  aiMatch: AiMatchData | null = null;
  aiMatchLoading = true;
  aiMatchError = '';

  companyBriefing: CompanyBriefingData = {
    staffRange: '120–250',
    founded: '2016',
    facts: [
      'Series B — raised $12M in early 2024',
      'Engineering-led culture, ships to production weekly',
      'Top-rated employer on Wuzzuf · strong work–life balance',
      'Remote-first across Egypt & MENA time zones',
    ],
  };

  async ngOnInit() {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.jobId = id;
    this.loading = true;

    // مستقل عن باقي الصفحة، عشان الـ AI agent بياخد وقت ومتستنيش عليه كل الصفحة
    this.fetchSkillAnalysis(id);

    try {
      this.job = await this.jobsService.getById(id);
      console.log('Job data:', this.job);
      if (this.job) {
        this.header = {
          companyInitials: this.job.company.substring(0, 2).toUpperCase(),
          title: this.job.title,
          companyName: this.job.company,
          location: this.job.location,
          postedAt: this.job.postedAgo,
          tags: this.job.tags,
          salary: `EGP ${(this.job.salaryMin / 1000).toFixed(0)}k – ${(this.job.salaryMax / 1000).toFixed(0)}k/mo`,
        };
        this.about = {
          description: this.job.description,
          responsibilities: this.job.responsibilities,
          requirements: this.job.requirements,
        };

        try {
          const briefingRes = await firstValueFrom(
            this.http.get<any>(`${environment.apiUrl}/jobs/${id}/briefing`)
          );
          const b = briefingRes.data;
          console.log('Company briefing data:', b);
          if (b) {
            this.companyBriefing = {
              staffRange: `${b.staffMin ?? ''}–${b.staffMax ?? ''}`,
              founded: b.founded?.toString() ?? '',
              facts: b.content ?? [],
            };
          }
        } catch {}

      } else {
        this.error = 'Job not found.';
      }
    } catch (err) {
      console.error('Component error:', err);
      this.error = 'Something went wrong.';
    } finally {
      this.loading = false;
      this.cdr.detectChanges();
    }
  }

  get salaryDisplay(): string {
    if (!this.job || (this.job.salaryMin === 0 && this.job.salaryMax === 0)) return 'Not specified';
    const fmt = (n: number) => (n >= 1000 ? `${Math.round(n / 1000)}k` : `${n}`);
    return `${fmt(this.job.salaryMin)} – ${fmt(this.job.salaryMax)}`;
  }

  get facts(): { label: string; value: string }[] {
    if (!this.job) return [];
    return [
      { label: 'Seniority', value: this.job.seniority || '—' },
      { label: 'Work type', value: this.job.workType || '—' },
      { label: 'Employment', value: this.job.workTime || '—' },
      { label: 'Location', value: this.job.location || '—' },
      { label: 'Salary', value: this.salaryDisplay },
      { label: 'Posted', value: this.job.postedAgo || '—' },
      { label: 'Source', value: this.job.sourcePlatform || '—' },
    ];
  }

  getPostedAt(dateStr: string): string {
    const diff = Date.now() - new Date(dateStr).getTime();
    const hours = Math.floor(diff / 3600000);
    if (hours < 24) return `${hours}h ago`;
    const days = Math.floor(hours / 24);
    return `${days}d ago`;
  }

  private async fetchSkillAnalysis(id: number) {
    this.aiMatchLoading = true;
    this.aiMatchError = '';
    try {
      const res = await firstValueFrom(
        this.http.get<any>(`${environment.apiUrl}/jobs/${id}/skill-analysis`)
      );
      const d = res?.data;
      console.log('Skill analysis data:', d);
      if (d) {
        const items = d.items ?? [];
        this.aiMatch = {
          score: d.overallScore,
          summary: d.overallSummary,
          skills: items.map((i: any) => ({
            name: i.skillName,
            percent: i.matchPercent,
            status: i.matchPercent < 50 ? 'gap' : 'good',
            severity: i.severity,
            category: i.category,
          })),
          gapSkills: items
            .filter((i: any) => i.matchPercent < 50)
            .map((i: any) => ({
              name: i.skillName,
              whyItMatters: i.whyItMatters,
              suggestedStep: i.suggestedStep,
              actionType: i.actionType,
            })),
        };
      }
    } catch (err) {
      console.error('Skill analysis error:', err);
      this.aiMatchError = 'Could not load AI match analysis.';
    } finally {
      this.aiMatchLoading = false;
      this.cdr.detectChanges();
    }
  }
}