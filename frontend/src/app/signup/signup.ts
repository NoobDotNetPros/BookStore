import { Component, inject } from '@angular/core';
import { RouterLink, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AuthService } from '../Services/auth.service';

@Component({
  selector: 'app-signup',
  imports: [RouterLink, FormsModule, CommonModule],
  templateUrl: './signup.html',
  styleUrl: './signup.scss',
})
export class Signup {
  showPassword = false;
  fullName = '';
  email = '';
  password = '';
  mobileNumber = '';
  submitted = false;
  loading = false;
  errorMessage = '';

  private authService = inject(AuthService);
  private router = inject(Router);

  togglePassword() {
    this.showPassword = !this.showPassword;
  }

  onSubmit() {
    this.submitted = true;
    this.errorMessage = '';

    if (!this.fullName.trim() || !this.email.trim() || !this.password.trim() || !this.mobileNumber.trim()) {
      return;
    }

    this.loading = true;
    this.authService.signup(this.fullName, this.email, this.password, this.mobileNumber).subscribe({
      next: (response) => {
        if (response.success && response.data) {
          this.authService.setUser(response.data);
          this.router.navigate(['/']);
        } else {
          this.errorMessage = response.message || 'Signup failed';
        }
        this.loading = false;
      },
      error: (err) => {
        this.errorMessage = err.error?.message || 'Signup failed. Please try again.';
        this.loading = false;
      }
    });
  }

  get fullNameError() {
    return this.submitted && !this.fullName.trim();
  }

  get emailError() {
    return this.submitted && !this.email.trim();
  }

  get passwordError() {
    return this.submitted && !this.password.trim();
  }

  get mobileError() {
    return this.submitted && !this.mobileNumber.trim();
  }
}
