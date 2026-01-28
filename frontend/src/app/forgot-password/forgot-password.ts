import { Component, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../Services/auth.service';

type ForgotPasswordStep = 'email' | 'otp' | 'reset';

@Component({
  imports: [CommonModule, FormsModule, RouterLink],
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.html',
  styleUrls: ['./forgot-password.css']
})
export class ForgotPasswordComponent implements OnDestroy {
  // Current step in the password reset flow
  currentStep: ForgotPasswordStep = 'email';

  // Form fields
  email: string = '';
  otp: string = '';
  newPassword: string = '';
  confirmPassword: string = '';

  // State
  isLoading: boolean = false;
  errorMessage: string = '';
  successMessage: string = '';
  resetToken: string = '';

  // Resend OTP timer
  resendCooldown: number = 0;
  private resendTimer: any = null;

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnDestroy(): void {
    if (this.resendTimer) {
      clearInterval(this.resendTimer);
    }
  }

  onSubmitEmail(): void {
    if (!this.email) return;

    this.isLoading = true;
    this.errorMessage = '';
    this.successMessage = '';

    this.authService.forgotPassword(this.email).subscribe({
      next: (response) => {
        this.isLoading = false;
        this.successMessage = response.message;
        this.currentStep = 'otp';
        this.startResendCooldown();
      },
      error: (error) => {
        this.isLoading = false;
        this.errorMessage = error.error?.message || 'Failed to send OTP. Please try again.';
      }
    });
  }

  onSubmitOtp(): void {
    if (!this.otp || this.otp.length !== 6) {
      this.errorMessage = 'Please enter a valid 6-digit OTP';
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';
    this.successMessage = '';

    this.authService.verifyOtp(this.email, this.otp).subscribe({
      next: (response) => {
        this.isLoading = false;
        this.successMessage = response.message;
        this.resetToken = response.resetToken || '';
        this.currentStep = 'reset';
      },
      error: (error) => {
        this.isLoading = false;
        this.errorMessage = error.error?.message || 'Invalid OTP. Please try again.';
      }
    });
  }

  onResendOtp(): void {
    if (this.resendCooldown > 0) return;

    this.isLoading = true;
    this.errorMessage = '';
    this.successMessage = '';

    this.authService.resendOtp(this.email).subscribe({
      next: (response) => {
        this.isLoading = false;
        this.successMessage = response.message;
        this.startResendCooldown();
      },
      error: (error) => {
        this.isLoading = false;
        if (error.error?.waitTimeSeconds) {
          this.resendCooldown = error.error.waitTimeSeconds;
          this.startResendCooldown();
        }
        this.errorMessage = error.error?.message || 'Failed to resend OTP. Please try again.';
      }
    });
  }

  onSubmitNewPassword(): void {
    if (!this.newPassword || !this.confirmPassword) {
      this.errorMessage = 'Please fill in all fields';
      return;
    }

    if (this.newPassword !== this.confirmPassword) {
      this.errorMessage = 'Passwords do not match';
      return;
    }

    if (this.newPassword.length < 8) {
      this.errorMessage = 'Password must be at least 8 characters long';
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';
    this.successMessage = '';

    this.authService.resetPassword({
      email: this.email,
      resetToken: this.resetToken,
      newPassword: this.newPassword,
      confirmPassword: this.confirmPassword
    }).subscribe({
      next: (response) => {
        this.isLoading = false;
        this.successMessage = response.message;
        // Redirect to login after 2 seconds
        setTimeout(() => {
          this.router.navigate(['/login']);
        }, 2000);
      },
      error: (error) => {
        this.isLoading = false;
        this.errorMessage = error.error?.message || 'Failed to reset password. Please try again.';
      }
    });
  }

  private startResendCooldown(): void {
    this.resendCooldown = 180; // 3 minutes

    if (this.resendTimer) {
      clearInterval(this.resendTimer);
    }

    this.resendTimer = setInterval(() => {
      this.resendCooldown--;
      if (this.resendCooldown <= 0) {
        clearInterval(this.resendTimer);
        this.resendTimer = null;
      }
    }, 1000);
  }

  formatCooldownTime(): string {
    const minutes = Math.floor(this.resendCooldown / 60);
    const seconds = this.resendCooldown % 60;
    return `${minutes}:${seconds.toString().padStart(2, '0')}`;
  }

  goBackToEmail(): void {
    this.currentStep = 'email';
    this.otp = '';
    this.errorMessage = '';
    this.successMessage = '';
  }
}
