import { Component, Input, OnChanges, SimpleChanges, ChangeDetectorRef, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AdminTicket, AdminTicketsService } from '../../../services/admin-tickets';

@Component({
  selector: 'app-ticket-detail',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './ticket-detail.html',
  styleUrl: './ticket-detail.css'
})
export class TicketDetail implements OnChanges {
  @Input() ticket: AdminTicket | null = null;
  @Output() statusUpdated = new EventEmitter<void>();

  replyText = '';
  selectedStatus = '';
  isUpdating = false;
  successMessage = '';

  constructor(
    private adminTicketsService: AdminTicketsService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnChanges(changes: SimpleChanges) {
    if (changes['ticket'] && this.ticket) {
      this.selectedStatus = this.ticket.status;
      this.replyText = '';
      this.successMessage = '';
    }
  }

  

  onSendReply() {
    if (!this.replyText.trim()) return;
    this.updateStatus();
    setTimeout(() => {
      this.replyText = '';
      this.cdr.detectChanges();
    }, 500);
  }

  updateStatus() {
    if (!this.ticket || !this.selectedStatus || !this.replyText.trim()) return;
    this.isUpdating = true;

    this.adminTicketsService.updateTicketStatus(
      this.ticket.id,
      this.selectedStatus,
      this.replyText
    ).subscribe({
      next: (res) => {
        console.log('Status updated:', res);
        this.successMessage = 'Status updated successfully!';
        if (this.ticket) {
          this.ticket = { ...this.ticket, status: this.selectedStatus as any };
        }
        this.isUpdating = false;
        this.statusUpdated.emit();
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('Error details:', err.error);
        this.isUpdating = false;
        this.cdr.detectChanges();
      }
    });
  }

  statusClass(status: string): string {
    return 'status-badge status-badge--' + status.toLowerCase().replace(' ', '-');
  }
}