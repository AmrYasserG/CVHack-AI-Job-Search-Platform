import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';
import { AuthService } from '../services/auth.service';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const router = inject(Router);
  const token = authService.getToken();

  // Attach the token if we have one
  const authReq = token
    ? req.clone({ setHeaders: { Authorization: `Bearer ${token}` } })
    : req;

  return next(authReq).pipe(
    catchError((err: HttpErrorResponse) => {
      // Token expired / invalid / not authenticated
      if (err.status === 401) {
        authService.logout();              // clear the dead token
        router.navigate(['/login'], {
          queryParams: { sessionExpired: true },
        });
      }
      // re-throw so the calling code can still handle its own errors
      return throwError(() => err);
    })
  );
};