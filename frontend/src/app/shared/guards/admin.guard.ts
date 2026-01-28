import { Injectable } from '@angular/core';
import { CanActivate, Router, UrlTree } from '@angular/router';
import { AuthService } from '../../Services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AdminAuthGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router) {}

  canActivate(): boolean | UrlTree {
    if (this.authService.isLoggedIn() && this.authService.isAdmin()) {
      return true;
    }
    // If logged in but not admin, maybe redirect to home? 
    // If not logged in, redirect to login.
    return this.router.createUrlTree(['/login']);
  }
}
