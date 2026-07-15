import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface TicketPayload {
  subject: string;
  category: string;
  description: string;
}

export interface SupportTicket {
  id: number;
  subject: string;
  category?: string;
  description?: string;
  status: 'Open' | 'In Progress' | 'Resolved';
  createdAt: string;
}

export interface ApiResponse<T> {
  isSuccess: boolean;
  statusCode: number;
  message: string | null;
  data: T;
  errors: any[];
}

@Injectable({ providedIn: 'root' })
export class SupportService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  submitTicket(payload: TicketPayload): Observable<ApiResponse<SupportTicket>> {
    return this.http.post<ApiResponse<SupportTicket>>(
      `${this.apiUrl}/tickets`, payload
    );
  }

  getMyTickets(): Observable<ApiResponse<SupportTicket[]>> {
    return this.http.get<ApiResponse<SupportTicket[]>>(
      `${this.apiUrl}/tickets`
    );
  }

  getTicketById(id: number): Observable<ApiResponse<SupportTicket>> {
    return this.http.get<ApiResponse<SupportTicket>>(
      `${this.apiUrl}/tickets/${id}`
    );
  }
}