import { Component, inject, OnInit } from '@angular/core';
import { RouterLink, Router, ActivatedRoute } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AuthService } from '../Services/auth.service';
import { CartService } from '../Services/cart.service';
import { WishlistService } from '../Services/wishlist.service';
import { forkJoin, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Component({
  selector: 'app-login',
  imports: [RouterLink, FormsModule, CommonModule],
  templateUrl: './login.html',
  styleUrl: './login.scss',
})
export class Login implements OnInit {
  showPassword = false;
  email = '';
  password = '';
  submitted = false;
  loading = false;
  errorMessage = '';
  successMessage = '';

  private authService = inject(AuthService);
  private cartService = inject(CartService);
  private wishlistService = inject(WishlistService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);

  ngOnInit(): void {
    // Check if user was redirected after email verification
    const verified = this.route.snapshot.queryParamMap.get('verified');
    if (verified === 'true') {
      this.successMessage = 'Email verified successfully! You can now login.';
    }
  }

  togglePassword() {
    this.showPassword = !this.showPassword;
  }

  onSubmit() {
    this.submitted = true;
    this.errorMessage = '';

    if (!this.email.trim() || !this.password.trim()) {
      return;
    }

    this.loading = true;
    this.authService.login(this.email, this.password).subscribe({
      next: (response) => {
        // Backend returns { message, data } - if data exists, login was successful
        if (response.data) {
          this.authService.setUser(response.data);

          // Sync local cart and wishlist with server
          this.syncGuestDataAndRedirect();
        } else {
          this.errorMessage = response.message || 'Login failed';
          this.loading = false;
        }
      },
      error: (err) => {
        this.errorMessage = err.error?.message || 'Login failed. Please try again.';
        this.loading = false;
      }
    });
  }

  private syncGuestDataAndRedirect(): void {
    // Sync cart and wishlist in parallel
    forkJoin([
      this.cartService.syncLocalCartWithServer().pipe(catchError(() => of(null))),
      this.wishlistService.syncLocalWishlistWithServer().pipe(catchError(() => of(null)))
    ]).subscribe({
      next: () => {
        this.redirectAfterLogin();
      },
      error: () => {
        // Even if sync fails, redirect the user
        this.redirectAfterLogin();
      }
    });
  }

  private redirectAfterLogin(): void {
    this.loading = false;

    // Check for saved redirect URL
    let redirectUrl = '/home';
    if (typeof localStorage !== 'undefined') {
      const savedUrl = localStorage.getItem('redirectUrl');
      if (savedUrl) {
        redirectUrl = savedUrl;
        localStorage.removeItem('redirectUrl');
      }
    }

    if (this.authService.isAdmin()) {
      this.router.navigate(['/admin']);
    } else {
      this.router.navigate([redirectUrl]);
    }
  }

  get emailError() {
    return this.submitted && !this.email.trim();
  }

  get passwordError() {
    return this.submitted && !this.password.trim();
  }
}
