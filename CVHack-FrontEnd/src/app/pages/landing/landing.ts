import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Logo } from '../../assets/logo/logo';
import { ThemeToggle } from '../../components/theme-toggle/theme-toggle';

@Component({
  selector: 'app-landing',
  standalone: true,
  imports: [Logo, ThemeToggle],
  templateUrl: './landing.html',
  styleUrl: './landing.css',
})
export class Landing {
  constructor(private router: Router) {}

  goToLogin(): void {
    this.router.navigate(['/login']);
  }
}