import { Component, computed, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { MailIcon } from "../../assets/mail-icon/mail-icon";
import { LockIcon } from "../../assets/lock-icon/lock-icon";
import { EyeIcon } from "../../assets/eye-icon/eye-icon";
import { AuthService } from '../../services/auth.service';

const EMAIL_REGEX = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

@Component({
  selector: 'app-login-form',
  imports: [FormsModule, RouterLink, MailIcon, LockIcon, EyeIcon],
  templateUrl: './login-form.html',
  styleUrl: './login-form.css',
})
export class LoginForm {
  email = signal('');
  password = signal('');
  showPassword = signal(false);

  loading = signal(false);
  errorMessage = signal('');

  emailValid = computed(() => EMAIL_REGEX.test(this.email().trim()));

  showEmailError = computed(() => this.email().length > 0 && !this.emailValid());

  formValid = computed(
    () => this.email().trim().length > 0 && this.password().length > 0 && this.emailValid()
  );

  private router = inject(Router);
  private authService = inject(AuthService);

  private route = inject(ActivatedRoute);

  constructor() {
    if (this.route.snapshot.queryParams['sessionExpired']) {
      this.errorMessage.set('Your session expired. Please log in again.');
    }
  }

  togglePassword() {
    this.showPassword.set(!this.showPassword());
  }

  onSubmit() {
    if (!this.formValid()) return; // guard against Enter key bypassing the disabled button

    this.errorMessage.set('');
    this.loading.set(true);

    this.authService.login({ email: this.email().trim(), password: this.password() }).subscribe({
      next: (res) => {
        this.loading.set(false);
        if (res.isSuccess) {
          this.router.navigateByUrl(this.authService.getLandingUrl());
        } else {
          this.errorMessage.set(this.friendlyError(res.statusCode, res.message));
        }
      },
      error: (err) => {
        this.loading.set(false);
        this.errorMessage.set(this.friendlyError(err.status, err.error?.message));
        password: this.password.set('');
      },
    });
  }

  // Turn raw status codes / backend messages into clear, user-friendly text
  private friendlyError(status: number, backendMessage?: string | null): string {
    switch (status) {
      case 0:
        return 'Cannot reach the server. Please check your connection and try again.';
      case 400:
      case 401:
        return 'Incorrect email or password. Please try again.';
      case 403:
        return backendMessage || 'Your account has been suspended. Please contact support.';
      case 500:
        return 'Something went wrong on our end. Please try again later.';
      default:
        return backendMessage || 'Unable to log in. Please try again.';
    }
  }
}