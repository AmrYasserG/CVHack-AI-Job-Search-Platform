import { Component, inject, signal } from '@angular/core';
import { AuthCard } from '../../components/auth-card/auth-card';
import { RegisterPayload } from '../../models/auth.model';
import { Router } from '@angular/router';
import { RegisterForm } from '../../components/register-form/register-form';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-register',
  imports: [AuthCard, RegisterForm],
  templateUrl: './register.html',
  styleUrl: './register.css',
})
export class Register {
  private router = inject(Router);
  private authService = inject(AuthService);

  loading = signal(false);
  errorMessage = signal('');

  onRegisterSubmitted(data: RegisterPayload) {
    this.errorMessage.set('');
    this.loading.set(true);

    this.authService.register(data).subscribe({
      next: (res) => {
        this.loading.set(false);
        if (res.isSuccess) {
          this.router.navigateByUrl(this.authService.getLandingUrl());
        } else {
          this.errorMessage.set(this.friendlyError(res.statusCode, res.errors, res.message));
        }
      },
      error: (err) => {
        this.loading.set(false);
        this.errorMessage.set(this.friendlyError(err.status, err.error?.errors, err.error?.message));
      },
    });
  }

  private friendlyError(status: number, errors?: string[], message?: string | null): string {
    if (status === 0) return 'Cannot reach the server. Please try again.';
    // backend returns specific reasons (e.g. "Email already exists", password rules)
    return errors?.[0] || message || 'Registration failed. Please try again.';
  }
}