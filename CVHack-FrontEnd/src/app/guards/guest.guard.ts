import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

// Blocks login/register for users who are already logged in
export const guestGuard: CanActivateFn = () => {
  const authService = inject(AuthService);
  const router = inject(Router);

  if (authService.isLoggedIn()) {
    // Already logged in → send them to their home page based on role
    return router.createUrlTree([authService.getLandingUrl()]);
  }

  return true; // not logged in → allow access to login/register
};