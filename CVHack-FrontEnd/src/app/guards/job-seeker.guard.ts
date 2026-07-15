import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

// Allows logged-in non-admins (job seekers). Blocks admins.
export const jobSeekerGuard: CanActivateFn = () => {
    const authService = inject(AuthService);
    const router = inject(Router);

    if (!authService.isLoggedIn()) {
        return router.createUrlTree(['/login']);
    }

    if (authService.hasRole('Admin')) {
        return router.createUrlTree(['/admin/dashboard']);
    }

    return true;
};