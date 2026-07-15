import { Component, Input } from '@angular/core';

export interface CompanyBriefingData {
  staffRange: string;
  founded: string;
  facts: string[];
}

@Component({
  selector: 'app-company-briefing',
  imports: [],
  templateUrl: './company-briefing.html',
  styleUrl: './company-briefing.css',
})
export class CompanyBriefing {
  @Input() data!: CompanyBriefingData;
}
