import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SupportService, SupportTicket } from '../../../services/support';

@Component({
  selector: 'app-my-tickets',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './my-tickets.html',
  styleUrl: './my-tickets.css'
})
export class MyTickets implements OnInit {
  tickets: SupportTicket[] = [];
  isLoading = true;

  constructor(
    private supportService: SupportService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.loadTickets();
  }

  loadTickets() {
    this.isLoading = true;

    this.supportService.getMyTickets().subscribe({
      next: (res: any) => {
        console.log('Tickets response:', res);
        this.tickets = res?.data || res || [];
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('Error:', err);
        this.tickets = [];
        this.isLoading = false;
        this.cdr.detectChanges();
      }
    });
  }

  refresh() {
    this.loadTickets();
  }

  statusClass(status: string): string {
    return 'status-badge status-badge--' + status.toLowerCase().replace(' ', '-');
  }

  formatDate(dateStr: string): string {
    if (!dateStr) return '';
    return new Date(dateStr).toLocaleDateString('en-US', {
      month: 'short', day: 'numeric'
    });
  }
}