import { Component, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SubmitTicketForm } from './submit-ticket-form/submit-ticket-form';
import { MyTickets } from './my-tickets/my-tickets';
import { Navbar } from '../../components/navbar/navbar';

@Component({
  selector: 'app-support',
  standalone: true,
  imports: [CommonModule, SubmitTicketForm, MyTickets, Navbar],
  templateUrl: './support.html',
  styleUrl: './support.css'
})
export class Support {
  @ViewChild(MyTickets) myTicketsComponent!: MyTickets;

  onTicketSubmitted() {
    if (this.myTicketsComponent) {
      this.myTicketsComponent.refresh();
    }
  }
}