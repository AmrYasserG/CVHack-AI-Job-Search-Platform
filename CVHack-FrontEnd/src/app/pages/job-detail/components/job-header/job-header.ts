import { Component, Input } from '@angular/core';

export interface JobHeaderData {
  companyInitials: string;
  title: string;
  companyName: string;
  location: string;
  postedAt: string;
  tags: string[];
  salary: string;
}

@Component({
  selector: 'app-job-header',
  imports: [],
  templateUrl: './job-header.html',
  styleUrl: './job-header.css',
})
export class JobHeader {
  @Input() data!: JobHeaderData;
}
