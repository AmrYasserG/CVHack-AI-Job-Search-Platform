import { Component, OnInit, inject, signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Navbar } from '../../components/navbar/navbar';
import { InterviewHeader, InterviewHeaderData } from './components/interview-header/interview-header';
import { ChatWindow, ChatMessage } from './components/chat-window/chat-window';
import { ChatInput } from './components/chat-input/chat-input';
import { JobsService } from '../../services/jobs.service';
import { InterviewService } from '../../services/interview.service';
import { InterviewTurnResult } from '../../models/interview.model';

@Component({
  selector: 'app-mock-interview',
  imports: [Navbar, InterviewHeader, ChatWindow, ChatInput],
  templateUrl: './mock-interview.html',
  styleUrl: './mock-interview.css',
})
export class MockInterview implements OnInit {
  private route = inject(ActivatedRoute);
  private jobsService = inject(JobsService);
  private interviewService = inject(InterviewService);

  headerData = signal<InterviewHeaderData>({
    jobTitle: '',
    company: '',
    currentQuestion: 1,
    totalQuestions: 6,
  });

  messages = signal<ChatMessage[]>([]);
  isLoading = signal(false);
  errorMessage = signal<string | null>(null);

  private sessionId: string | null = null;

  async ngOnInit() {
    this.isLoading.set(true);

    const jobId = Number(this.route.snapshot.paramMap.get('id'));
    if (!jobId) {
      this.errorMessage.set('No job selected. Please start the interview from a job page.');
      this.isLoading.set(false);
      return;
    }

    const job = await this.jobsService.getById(jobId);
    if (!job) {
      this.errorMessage.set('Could not load this job. Please try again.');
      this.isLoading.set(false);
      return;
    }

    this.headerData.set({
      jobTitle: job.title,
      company: job.company,
      currentQuestion: 1,
      totalQuestions: 6,
    });

    const result = await this.interviewService.start({
      jobTitle: job.title,
      seniority: job.seniority,
      companyName: job.company,
    });

    if (!result) {
      this.errorMessage.set('Could not start the interview right now. Please try again.');
      this.isLoading.set(false);
      return;
    }

    this.applyResult(result);
    this.isLoading.set(false);
  }

  async onSend(text: string) {
    if (!this.sessionId) return;

    this.messages.update((msgs) => [...msgs, { type: 'user-answer', text }]);
    this.isLoading.set(true);

    const result = await this.interviewService.answer({
      sessionId: this.sessionId,
      answer: text,
    });

    if (!result) {
      this.errorMessage.set('Could not send your answer right now. Please try again.');
      this.isLoading.set(false);
      return;
    }

    this.applyResult(result);
    this.isLoading.set(false);
  }

  private applyResult(result: InterviewTurnResult) {
    this.sessionId = result.sessionId;

    if (result.feedback) {
      this.messages.update((msgs) => [...msgs, { type: 'ai-feedback', text: result.feedback! }]);
    }

    if (result.isComplete) {
      if (result.closingMessage) {
        this.messages.update((msgs) => [...msgs, { type: 'ai-feedback', text: result.closingMessage! }]);
      }
      return;
    }

    if (result.question) {
      this.messages.update((msgs) => [...msgs, { type: 'ai-question', text: result.question! }]);
    }

    this.headerData.update((h) => ({
      ...h,
      currentQuestion: result.questionNumber,
      totalQuestions: result.totalQuestions,
    }));
  }
}