import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map, firstValueFrom } from 'rxjs';
import { environment } from '../../environments/environment';
import { ApiResult } from '../models/auth.model';
import { Job } from '../models/job.model';
import { mapJobDtoToJob } from '../models/job.mapper';
import { JobDto } from '../models/job.dto.model';

@Injectable({ providedIn: 'root' })
export class JobsService {
  private http = inject(HttpClient);
  private apiUrl = environment.apiUrl;

  // ✅ زيادة بس
  async getAll(): Promise<Job[]> {
    try {
      const res = await firstValueFrom(
        this.http.get<ApiResult<JobDto[]>>(`${this.apiUrl}/jobs`)
      );
      return (res.data ?? []).map(mapJobDtoToJob);
    } catch {
      return [];
    }
  }

  // ✅ زيادة بس
  async getById(id: number): Promise<Job | null> {
    try {
      const res = await firstValueFrom(
        this.http.get<ApiResult<JobDto>>(`${this.apiUrl}/jobs/${id}`)
      );
      return res.data ? mapJobDtoToJob(res.data) : null;
    } catch (err) {
      console.error('Error:', err);
      return null;
    }
  }

  getJobs(): Observable<Job[]> {
    return this.http
      .get<ApiResult<JobDto[]>>(`${this.apiUrl}/jobs`)
      .pipe(map((res) => (res.data ?? []).map(mapJobDtoToJob)));
  }

  getJob(id: number): Observable<Job> {
    return this.http
      .get<ApiResult<JobDto>>(`${this.apiUrl}/jobs/${id}`)
      .pipe(map((res) => mapJobDtoToJob(res.data!)));
  }
}