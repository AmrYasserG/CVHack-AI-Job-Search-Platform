import { Component, inject } from '@angular/core';
import { Navbar } from '../../components/navbar/navbar';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-not-found',
  imports: [Navbar],
  templateUrl: './not-found.html',
  styleUrl: './not-found.css',
})
export class NotFound {
  private router = inject(Router);
  private authService = inject(AuthService);

  isLoggedIn = this.authService.isLoggedIn();
  isAdmin = this.authService.hasRole('Admin');

  buttonLabel = !this.isLoggedIn
    ? 'Go to login'
    : this.isAdmin
      ? 'Go to dashboard'
      : 'Go to job search';

  goHome() {
    if (!this.isLoggedIn) {
      this.router.navigate(['/login']);
      return;
    }
    this.router.navigateByUrl(this.authService.getLandingUrl());
  }
}
