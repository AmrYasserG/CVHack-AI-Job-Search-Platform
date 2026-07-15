import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { environment } from '../../environments/environment';
import { ApiResult, AuthResult, LoginPayload, RegisterPayload } from '../models/auth.model';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private http = inject(HttpClient);
  private apiUrl = environment.apiUrl;

  login(payload: LoginPayload): Observable<ApiResult<AuthResult>> {
    return this.http
      .post<ApiResult<AuthResult>>(`${this.apiUrl}/auth/login`, payload)
      .pipe(
        tap((res) => {
          if (res.isSuccess && res.data) {
            localStorage.setItem('token', res.data.token);
            localStorage.setItem('email', res.data.email);
            localStorage.setItem('firstName', res.data.firstName);
            localStorage.setItem('lastName', res.data.lastName);
            localStorage.setItem('roles', JSON.stringify(res.data.roles));
          }
        })
      );
  }
  register(payload: RegisterPayload): Observable<ApiResult<AuthResult>> {
    return this.http
      .post<ApiResult<AuthResult>>(`${this.apiUrl}/auth/register/jobseeker`, payload)
      .pipe(
        tap((res) => {
          // Backend logs the user straight in and returns a token
          if (res.isSuccess && res.data?.token) {
            localStorage.setItem('token', res.data.token);
            localStorage.setItem('email', res.data.email);
            localStorage.setItem('firstName', res.data.firstName);
            localStorage.setItem('lastName', res.data.lastName);
            localStorage.setItem('roles', JSON.stringify(res.data.roles));
          }
        })
      );
  }

  logout(): void {
    localStorage.removeItem('token');
    localStorage.removeItem('email');
    localStorage.removeItem('firstName');
    localStorage.removeItem('lastName');
    localStorage.removeItem('roles');
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }
  getRoles(): string[] {
    const raw = localStorage.getItem('roles');
    return raw ? JSON.parse(raw) : [];
  }

  hasRole(role: string): boolean {
    return this.getRoles().includes(role);
  }
  getLandingUrl(): string {
    return this.hasRole('Admin') ? '/admin/dashboard' : '/jobs';
  }
  getFirstName(): string {
    return localStorage.getItem('firstName') ?? '';
  }

  getLastName(): string {
    return localStorage.getItem('lastName') ?? '';
  }

  getFullName(): string {
    return `${this.getFirstName()} ${this.getLastName()}`.trim();
  }
  getInitials(): string {
    const first = this.getFirstName().charAt(0);
    const last = this.getLastName().charAt(0);
    const initials = (first + last).toUpperCase();
    return initials || '?';
  }
}