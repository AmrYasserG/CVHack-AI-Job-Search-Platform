import { Component, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TicketsList } from './tickets-list/tickets-list';
import { TicketDetail } from './ticket-detail/ticket-detail';
import { AdminTicket } from '../../services/admin-tickets';
import { AdminSidebar } from '../../components/admin-sidebar/admin-sidebar';

@Component({
  selector: 'app-admin-tickets',
  standalone: true,
  imports: [CommonModule, TicketsList, TicketDetail, AdminSidebar],
  templateUrl: './admin-tickets.html',
  styleUrl: './admin-tickets.css'
})
export class AdminTickets {
  @ViewChild(TicketsList) ticketsListComponent!: TicketsList;

  selectedTicket: AdminTicket | null = null;
  activeFilter: string = 'All';

  onFilterChange(filter: string) {
  this.activeFilter = filter;
  this.selectedTicket = null; 
}

onTicketSelect(ticket: AdminTicket) {
 
  if (this.activeFilter === 'All' || ticket.status === this.activeFilter) {
    this.selectedTicket = ticket;
  }
}

  onStatusUpdated() {
    if (this.ticketsListComponent) {
      this.ticketsListComponent.loadTickets();
    }
  }
}