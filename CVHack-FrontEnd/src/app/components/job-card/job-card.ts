import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import type { Job } from '../../models/job.model';
import { LocationIcon } from "../../assets/location-icon/location-icon";
import { ClockIcon } from '../../assets/clock-icon/clock-icon';
import { MatchScore } from '../match-score/match-score';
import { RouterLink } from '@angular/router';


@Component({
  selector: 'app-job-card',
  imports: [CommonModule, LocationIcon, ClockIcon, RouterLink],
  templateUrl: './job-card.html',
  styleUrl: './job-card.css',
})
export class JobCard {
  @Input() job!: Job;

  get isHighMatch() {
    return this.job.matchScore >= 80;
  }

}
