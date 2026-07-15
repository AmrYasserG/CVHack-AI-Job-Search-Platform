import { Component, inject } from '@angular/core';
import { Logo } from "../../assets/logo/logo";
import { TicketIcon } from '../../assets/ticket-icon/ticket-icon';
import { UsersIcon } from '../../assets/users-icon/users-icon';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { BackIcon } from "../../assets/back-icon/back-icon";
import { DashboardIcon } from '../../assets/dashboard-icon/dashboard-icon';
import { AuthService } from '../../services/auth.service';
import { LogoutIcon } from "../../assets/logout-icon/logout-icon";
import { AdminThemeToggle } from '../admin-theme-toggle/admin-theme-toggle';

@Component({
  selector: 'app-admin-sidebar',
  imports: [AdminThemeToggle, Logo, TicketIcon, UsersIcon, RouterLink, BackIcon, RouterLinkActive, DashboardIcon, LogoutIcon],
  templateUrl: './admin-sidebar.html',
  styleUrl: './admin-sidebar.css',
})
export class AdminSidebar {
  private authService = inject(AuthService);
  private router = inject(Router);

  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
