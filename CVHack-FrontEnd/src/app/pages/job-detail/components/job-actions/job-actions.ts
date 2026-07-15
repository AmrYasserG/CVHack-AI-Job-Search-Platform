import { Component, Input, inject, signal } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';
import { environment } from '../../../../../environments/environment';

@Component({
  selector: 'app-job-actions',
  imports: [],
  templateUrl: './job-actions.html',
  styleUrl: './job-actions.css',
})
export class JobActions {
  @Input() jobId!: number;
  @Input() jobUrl!: string;
  @Input() overallScore: number | null = null;

  private router = inject(Router);
  private http = inject(HttpClient);

  downloading = signal(false);
  downloadError = signal('');

  // CV generation requires a match score of at least 50% (enforced in the backend too)
  get canDownloadCv(): boolean {
    return this.overallScore != null && this.overallScore >= 50;
  }

  startMockInterview() {
    this.router.navigate(['/mock-interview', this.jobId]);
  }

  async onDownloadCV() {
    if (!this.canDownloadCv || this.downloading()) return;
    this.downloading.set(true);
    this.downloadError.set('');
    try {
      const res = await firstValueFrom(
        this.http.get<{ cvText: string }>(`${environment.apiUrl}/cv/generate/${this.jobId}`)
      );
      const text = res?.cvText ?? '';
      if (!text.trim()) throw new Error('empty');

      // download the generated CV as a text file
      const blob = new Blob([text], { type: 'text/plain;charset=utf-8' });
      const url = URL.createObjectURL(blob);
      const a = document.createElement('a');
      a.href = url;
      a.download = `CV-job-${this.jobId}.txt`;
      a.click();
      URL.revokeObjectURL(url);
    } catch {
      this.downloadError.set('Could not generate the CV. Please try again.');
    } finally {
      this.downloading.set(false);
    }
  }

  applyNow() {
  if (this.jobUrl) {
    window.open(this.jobUrl, '_blank', 'noopener,noreferrer');
  }
}

}