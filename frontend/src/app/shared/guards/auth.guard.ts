import { Injectable, PLATFORM_ID, inject } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { CanActivate, Router, UrlTree } from '@angular/router';
import { AuthService } from '../../Services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  private platformId = inject(PLATFORM_ID);

  constructor(private authService: AuthService, private router: Router) {}

  canActivate(): boolean | UrlTree {
    // Avoid redirect during SSR/pre-render; validate on browser
    if (!isPlatformBrowser(this.platformId)) {
      return true;
    }

    if (this.authService.isLoggedIn()) {
      return true;
    }
    return this.router.createUrlTree(['/login']);
  }
}
