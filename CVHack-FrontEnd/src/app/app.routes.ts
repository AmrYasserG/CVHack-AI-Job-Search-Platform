import { Routes } from '@angular/router';
import { authGuard } from './guards/auth.guard';
import { adminGuard } from './guards/admin.guard';
import { guestGuard } from './guards/guest.guard';
import { jobSeekerGuard } from './guards/job-seeker.guard';

export const routes: Routes = [
  {
  path: 'home',
  loadComponent: () =>
    import('./pages/landing/landing').then((c) => c.Landing),
},

{
    path: '',
    loadComponent: () =>
      import('./pages/landing/landing').then((c) => c.Landing),
  },

  // Auth (public)
  {
    path: 'login',
    canActivate: [guestGuard],
    loadComponent: () => import('./pages/login/login').then((c) => c.Login),
  },
  {
    path: 'register',
    canActivate: [guestGuard],
    loadComponent: () => import('./pages/register/register').then((c) => c.Register),
  },

  // User pages (require login)
  {
    path: 'jobs',
    canActivate: [authGuard],
    loadComponent: () => import('./pages/job-search/job-search').then((c) => c.JobSearch),
  },
  {
    path: 'jobs/:id',
    canActivate: [authGuard],
    loadComponent: () => import('./pages/job-detail/job-detail').then((c) => c.JobDetail),
  },
 {
  path: 'mock-interview/:id',
  canActivate: [authGuard],
  loadComponent: () => import('./pages/mock-interview/mock-interview').then((c) => c.MockInterview),
},
  {
    path: 'profile',
    canActivate: [jobSeekerGuard],
    loadComponent: () => import('./pages/profile/profile/profile').then((c) => c.Profile),
  },
  {
    path: 'support',
    canActivate: [jobSeekerGuard],
    loadComponent: () => import('./pages/support/support').then((c) => c.Support),
  },

  // Admin pages (require login + Admin role)
  {
    path: 'admin/dashboard',
    canActivate: [adminGuard],
    loadComponent: () => import('./pages/admin-dashboard/admin-dashboard').then((c) => c.AdminDashboard),
  },
  {
    path: 'admin/tickets',
    canActivate: [adminGuard],
    loadComponent: () => import('./pages/admin-tickets/admin-tickets').then((c) => c.AdminTickets),
  },
  {
    path: 'admin/users',
    canActivate: [adminGuard],
    loadComponent: () => import('./pages/admin-users/admin-users').then((c) => c.AdminUsers),
  },

  // Fallback
  {
    path: '**',
    loadComponent: () => import('./pages/not-found/not-found').then((c) => c.NotFound),
  },
];

