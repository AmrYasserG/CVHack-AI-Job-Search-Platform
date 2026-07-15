import { Component, EventEmitter, input, Output } from '@angular/core';
import { RegisterPayload } from '../../models/auth.model';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { MailIcon } from "../../assets/mail-icon/mail-icon";
import { LockIcon } from '../../assets/lock-icon/lock-icon';
import { EyeIcon } from "../../assets/eye-icon/eye-icon";
import { PassStrengthIcon } from '../../assets/pass-strength-icon/pass-strength-icon';

@Component({
  selector: 'app-register-form',
  imports: [FormsModule, RouterLink, MailIcon, LockIcon, EyeIcon, PassStrengthIcon],
  templateUrl: './register-form.html',
  styleUrl: './register-form.css',
})
export class RegisterForm {
  firstName = '';
  lastName = '';
  email = '';
  password = '';
  confirmPassword = '';

  showPassword = false;
  showConfirmPassword = false;

  @Output() submitted = new EventEmitter<RegisterPayload>();

  loading = input(false);
  serverError = input('');

  get passwordsMatch(): boolean {
    return this.confirmPassword.length === 0 || this.password === this.confirmPassword;
  }

  get passwordStrength(): number {
    const pw = this.password;
    if (pw.length === 0) return 0;
    let score = 0;
    if (pw.length >= 8) score++;
    if (/[A-Z]/.test(pw)) score++;
    if (/[0-9]/.test(pw)) score++;
    if (/[^A-Za-z0-9]/.test(pw)) score++;
    if (pw.length < 8) return Math.min(score, 1) || 1;
    return Math.max(score, 1);
  }

  get passwordStrengthLabel(): string {
    switch (this.passwordStrength) {
      case 1: return 'Weak password';
      case 2: return 'Fair password';
      case 3: return 'Good password';
      case 4: return 'Strong password';
      default: return '';
    }
  }

  get passwordStrengthColor(): string {
    switch (this.passwordStrength) {
      case 1: return 'var(--red)';
      case 2: return 'var(--copper)';
      case 3: return 'var(--amber)';
      case 4: return 'var(--green)';
      default: return 'var(--border2)';
    }
  }

  get passwordSuggestions(): string[] {
    const pw = this.password;
    if (pw.length === 0) return [];
    const suggestions: string[] = [];
    if (pw.length < 8) suggestions.push('At least 8 characters');
    if (!/[A-Z]/.test(pw)) suggestions.push('One uppercase letter');
    if (!/[a-z]/.test(pw)) suggestions.push('One lowercase letter');
    if (!/[0-9]/.test(pw)) suggestions.push('One number');
    if (!/[^A-Za-z0-9]/.test(pw)) suggestions.push('One special character');
    return suggestions;
  }

  get isEmailValid(): boolean {
    if (this.email.length === 0) return true; // don't show error before they've typed
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(this.email);
  }

  get isPasswordValid(): boolean {
    const pw = this.password;
    return (
      pw.length >= 8 &&
      /[A-Z]/.test(pw) &&
      /[a-z]/.test(pw) &&
      /[0-9]/.test(pw) &&
      /[^A-Za-z0-9]/.test(pw)
    );
  }

  get canSubmit(): boolean {
    return (
      this.firstName.trim().length > 0 &&
      this.lastName.trim().length > 0 &&
      this.email.trim().length > 0 &&
      this.isEmailValid &&
      this.isPasswordValid &&
      this.password === this.confirmPassword
    );
  }

  togglePassword() {
    this.showPassword = !this.showPassword;
  }

  toggleConfirmPassword() {
    this.showConfirmPassword = !this.showConfirmPassword;
  }

  onSubmit() {
    if (!this.canSubmit) return;
    this.submitted.emit({
      firstName: this.firstName,
      lastName: this.lastName,
      email: this.email,
      password: this.password,
    });
  }
}