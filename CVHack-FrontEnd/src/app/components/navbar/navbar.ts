import { Component, EventEmitter, inject, Output, signal } from '@angular/core';
import { Logo } from '../../assets/logo/logo';
import { MagnifyingGlassIcon } from '../../assets/magnifying-glass-icon/magnifying-glass-icon';
import { ThemeToggle } from '../theme-toggle/theme-toggle';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from '../../services/auth.service';


@Component({
  selector: 'app-navbar',
  imports: [Logo, MagnifyingGlassIcon, ThemeToggle, RouterLink, RouterLinkActive],
  templateUrl: './navbar.html',
  styleUrl: './navbar.css',
})
export class Navbar {
  private authService = inject(AuthService);
  private router = inject(Router);

  menuOpen = signal(false);
  @Output() search = new EventEmitter<string>();

  isAdmin = this.authService.hasRole('Admin');
  initials = this.authService.getInitials();


  toggleMenu() {
    this.menuOpen.update((v) => !v);
  }

  onSearch(event: Event) {
    this.search.emit((event.target as HTMLInputElement).value);
  }
  
  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
  toggleTheme() {
    throw new Error('Method not implemented.');
  }
}
