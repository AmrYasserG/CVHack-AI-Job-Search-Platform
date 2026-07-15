import { Component, Input } from '@angular/core';

export interface InterviewHeaderData {
  jobTitle: string;
  company: string;
  currentQuestion: number;
  totalQuestions: number;
}

@Component({
  selector: 'app-interview-header',
  imports: [],
  templateUrl: './interview-header.html',
  styleUrl: './interview-header.css',
})
export class InterviewHeader {
  @Input() data!: InterviewHeaderData;

  get dots(): number[] {
    return Array.from({ length: this.data.totalQuestions }, (_, i) => i);
  }
}
