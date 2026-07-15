import { Component, Input } from '@angular/core';

export interface SkillBreakdownItem {
  name: string;
  percent: number;
  status: 'good' | 'gap';
  severity?: string;   // "critical" | "important" | "nice-to-have"
  category?: string;   // "skill" | "experience" | "education" | "certification"
}

export interface GapSkillDetail {
  name: string;
  whyItMatters: string;
  suggestedStep: string;
  actionType?: string; // "course" | "project" | "certification" | "reframe"
}

export interface AiMatchData {
  score: number;
  summary: string;
  skills: SkillBreakdownItem[];
  gapSkills: GapSkillDetail[];
}

@Component({
  selector: 'app-ai-match-card',
  imports: [],
  templateUrl: './ai-match-card.html',
  styleUrl: './ai-match-card.css',
})
export class AiMatchCard {
  @Input() data: AiMatchData | null = null;
  @Input() loading = false;
  @Input() error = '';

  get circumference(): number {
    return 2 * Math.PI * 36;
  }

  get dashOffset(): number {
    const score = this.data?.score ?? 0;
    return this.circumference * (1 - score / 100);
  }

  get matchLabel(): string {
    const score = this.data?.score ?? 0;
    if (score >= 80) return 'Strong match';
    if (score >= 60) return 'Good match';
    return 'Weak match';
  }

  get ringColor(): string {
    const score = this.data?.score ?? 0;
    if (score >= 80) return 'var(--green)';
    if (score >= 60) return '#f97316';
    return 'var(--red)';
  }

  severityColor(severity?: string): string {
    switch ((severity ?? '').toLowerCase()) {
      case 'critical': return 'var(--red)';
      case 'important': return 'var(--amber)';
      default: return 'var(--faint)';
    }
  }

  actionIcon(type?: string): string {
    switch ((type ?? '').toLowerCase()) {
      case 'course': return '📚';
      case 'project': return '🛠️';
      case 'certification': return '📜';
      case 'reframe': return '✏️';
      default: return '💡';
    }
  }
}