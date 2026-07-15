import { Component } from '@angular/core';
import { Logo } from '../../assets/logo/logo';
import { ThemeToggle } from '../theme-toggle/theme-toggle';

@Component({
  selector: 'app-auth-card',
  imports: [Logo,ThemeToggle],
  templateUrl: './auth-card.html',
  styleUrl: './auth-card.css',
})
export class AuthCard {}
