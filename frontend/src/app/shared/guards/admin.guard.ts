import { Injectable, PLATFORM_ID, inject } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { CanActivate, Router, UrlTree } from '@angular/router';
import { AuthService } from '../../Services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AdminAuthGuard implements CanActivate {
  private platformId = inject(PLATFORM_ID);

  constructor(private authService: AuthService, private router: Router) {}

  canActivate(): boolean | UrlTree {
    // Avoid redirect during SSR/pre-render; validate on browser
    if (!isPlatformBrowser(this.platformId)) {
      return true;
    }

    if (this.authService.isLoggedIn() && this.authService.isAdmin()) {
      return true;
    }
    // If logged in but not admin, maybe redirect to home?
    // If not logged in, redirect to login.
    return this.router.createUrlTree(['/login']);
  }
}
