import { HttpInterceptorFn } from '@angular/common/http';
import { inject, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const platformId = inject(PLATFORM_ID);

  // Skip adding token for auth endpoints
  if (req.url.includes('/auth/login') ||
      req.url.includes('/auth/signup') ||
      req.url.includes('/auth/verification') ||
      req.url.includes('/auth/forgot-password') ||
      req.url.includes('/auth/verify-otp') ||
      req.url.includes('/auth/resend-otp') ||
      req.url.includes('/auth/reset-password')) {
    return next(req);
  }

  // Get token from localStorage (only in browser)
  let token: string | null = null;
  if (isPlatformBrowser(platformId)) {
    token = localStorage.getItem('authToken');
  }

  // Add authorization header if token exists
  if (token) {
    req = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
  }

  return next(req);
};
