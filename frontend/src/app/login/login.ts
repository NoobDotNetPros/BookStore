import { Component, inject, OnInit } from '@angular/core';
import { RouterLink, Router, ActivatedRoute } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AuthService } from '../Services/auth.service';

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
          if (this.authService.isAdmin()) {
            this.router.navigate(['/admin']);
          } else {
            this.router.navigate(['/home']);
          }
        } else {
          this.errorMessage = response.message || 'Login failed';
        }
        this.loading = false;
      },
      error: (err) => {
        this.errorMessage = err.error?.message || 'Login failed. Please try again.';
        this.loading = false;
      }
    });
  }

  get emailError() {
    return this.submitted && !this.email.trim();
  }

  get passwordError() {
    return this.submitted && !this.password.trim();
  }
}
