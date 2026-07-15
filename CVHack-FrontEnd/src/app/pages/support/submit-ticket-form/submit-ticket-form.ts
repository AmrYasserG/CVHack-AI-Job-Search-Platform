import { Component, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SupportService, TicketPayload } from '../../../services/support';

@Component({
  selector: 'app-submit-ticket-form',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './submit-ticket-form.html',
  styleUrl: './submit-ticket-form.css'
})
export class SubmitTicketForm {
  @Output() ticketSubmitted = new EventEmitter<void>();

  subject = '';
  category = 'Technical issue';
  description = '';

  categories = ['Technical issue', 'Billing', 'Feature request', 'Account','bug', 'Other'];

  isSubmitting = false;
  successMessage = '';
  errorMessage = '';

  constructor(private supportService: SupportService) {}

  onSubmit() {
    if (!this.subject.trim() || !this.description.trim()) return;

    this.isSubmitting = true;
    this.successMessage = '';
    this.errorMessage = '';

    const payload: TicketPayload = {
      subject: this.subject,
      category: this.category,
      description: this.description
    };

    this.supportService.submitTicket(payload).subscribe({
      next: (res) => {
        console.log('Ticket submitted:', res);
        this.successMessage = 'Ticket submitted successfully!';
        this.subject = '';
        this.description = '';
        this.category = 'Technical issue';
        this.isSubmitting = false;
        this.ticketSubmitted.emit(); 
      },
      error: (err) => {
        console.error('Error submitting ticket:', err);
        this.errorMessage = 'Failed to submit ticket. Please try again.';
        this.isSubmitting = false;
      }
    });
  }
}