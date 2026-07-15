import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';
import { environment } from '../../environments/environment';
import { ApiResult } from '../models/auth.model';
import {
  InterviewStartRequest,
  InterviewAnswerRequest,
  InterviewTurnResult,
} from '../models/interview.model';

@Injectable({ providedIn: 'root' })
export class InterviewService {
  private http = inject(HttpClient);
  private apiUrl = environment.apiUrl;

  async start(request: InterviewStartRequest): Promise<InterviewTurnResult | null> {
    try {
      const res = await firstValueFrom(
        this.http.post<ApiResult<InterviewTurnResult>>(
          `${this.apiUrl}/interview/start`,
          request
        )
      );
      return res.data ?? null;
    } catch (err) {
      console.error('Error starting interview:', err);
      return null;
    }
  }

  async answer(request: InterviewAnswerRequest): Promise<InterviewTurnResult | null> {
    try {
      const res = await firstValueFrom(
        this.http.post<ApiResult<InterviewTurnResult>>(
          `${this.apiUrl}/interview/answer`,
          request
        )
      );
      return res.data ?? null;
    } catch (err) {
      console.error('Error sending answer:', err);
      return null;
    }
  }
}