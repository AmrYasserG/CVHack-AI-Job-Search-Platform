import { Component, Input } from '@angular/core';

export interface JobAboutData {
  description: string;
  responsibilities?: string[];
  requirements?: string[];
}

@Component({
  selector: 'app-job-about',
  imports: [],
  templateUrl: './job-about.html',
  styleUrl: './job-about.css',
})
export class JobAbout {
  @Input() data!: JobAboutData;
}
