import { Component, inject } from '@angular/core';
import { RouterLink, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AuthService } from '../Services/auth.service';
import { ToastService } from '../Services/toast.service';

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
  successMessage = '';

  private authService = inject(AuthService);
  private router = inject(Router);
  private toastService = inject(ToastService);

  togglePassword() {
    this.showPassword = !this.showPassword;
  }

  onSubmit() {
    this.submitted = true;
    this.errorMessage = '';
    this.successMessage = '';

    if (!this.fullName.trim() || !this.email.trim() || !this.password.trim() || !this.mobileNumber.trim()) {
      return;
    }

    this.loading = true;
    this.authService.signup(this.fullName, this.email, this.password, this.mobileNumber).subscribe({
      next: (response: any) => {
        this.loading = false;
        this.successMessage = response.message || 'Registration successful! A verification email has been sent to your email address.';
        this.toastService.success(this.successMessage, 5000);

        // Redirect to login after 3 seconds
        setTimeout(() => {
          this.router.navigate(['/login']);
        }, 3000);
      },
      error: (err) => {
        this.errorMessage = err.error?.message || 'Signup failed. Please try again.';
        this.toastService.error(this.errorMessage);
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
