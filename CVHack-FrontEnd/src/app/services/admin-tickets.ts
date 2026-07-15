import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface AdminTicket {
  id: number;
  userName?: string;
  email?: string;
  subject: string;
  description?: string;
  category?: string;
  status: 'Open' | 'In Progress' | 'Resolved';
  priority?: string;
  createdAt?: string;
}

export interface ApiResponse<T> {
  isSuccess: boolean;
  statusCode: number;
  message: string | null;
  data: T;
  errors: any[];
}

@Injectable({ providedIn: 'root' })
export class AdminTicketsService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getAllTickets(): Observable<ApiResponse<AdminTicket[]>> {
    return this.http.get<ApiResponse<AdminTicket[]>>(
      `${this.apiUrl}/admin/tickets`
    );
  }

  getTicketById(id: number): Observable<ApiResponse<AdminTicket>> {
    return this.http.get<ApiResponse<AdminTicket>>(
      `${this.apiUrl}/admin/tickets/${id}`
    );
  }

  updateTicketStatus(id: number, status: string, reply?: string): Observable<ApiResponse<any>> {
  return this.http.put<ApiResponse<any>>(
    `${this.apiUrl}/admin/tickets/${id}/status`,
    { status, reply: reply || null }
  );
}
}