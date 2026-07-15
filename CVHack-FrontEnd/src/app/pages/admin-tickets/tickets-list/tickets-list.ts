import { Component, OnInit, Output, EventEmitter, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminTicketsService, AdminTicket } from '../../../services/admin-tickets';

@Component({
  selector: 'app-tickets-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './tickets-list.html',
  styleUrl: './tickets-list.css'
})
export class TicketsList implements OnInit {
  @Output() ticketSelect = new EventEmitter<AdminTicket>();
  @Output() filterChange = new EventEmitter<string>();

  tickets: AdminTicket[] = [];
  filteredTickets: AdminTicket[] = [];
  selectedTicketId: number | null = null;

  filters = ['All', 'Open', 'In Progress', 'Closed'];
  activeFilter = 'All';

  isLoading = true;

  constructor(
    private adminTicketsService: AdminTicketsService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.loadTickets();
  }

  loadTickets() {
    this.isLoading = true;
    this.adminTicketsService.getAllTickets().subscribe({
      next: (res: any) => {
        console.log('Admin tickets:', res);
        this.tickets = res?.data || [];
        this.applyFilter();
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('Error loading tickets:', err);
        this.isLoading = false;
        this.cdr.detectChanges();
      }
    });
  }

  applyFilter() {
    if (this.activeFilter === 'All') {
      this.filteredTickets = this.tickets;
    } else {
      this.filteredTickets = this.tickets.filter(
        t => t.status === this.activeFilter
      );
    }
  }

  setFilter(filter: string) {
    this.activeFilter = filter;
    this.applyFilter();
    this.selectedTicketId = null;
    this.filterChange.emit(filter);
  }

  selectTicket(ticket: AdminTicket) {
    this.selectedTicketId = ticket.id;
    this.ticketSelect.emit(ticket);
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