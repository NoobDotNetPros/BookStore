import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthService } from '../Services/auth.service';

@Component({
  selector: 'app-verify-email',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './verify-email.html',
  styleUrl: './verify-email.scss'
})
export class VerifyEmailComponent implements OnInit {
  status: 'loading' | 'success' | 'error' = 'loading';
  message = 'Verifying your email...';

  private authService = inject(AuthService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);

  ngOnInit(): void {
    const token = this.route.snapshot.queryParamMap.get('token');

    if (!token) {
      this.status = 'error';
      this.message = 'Invalid verification link. No token provided.';
      return;
    }

    this.verifyEmail(token);
  }

  private verifyEmail(token: string): void {
    this.authService.verifyEmail(token).subscribe({
      next: () => {
        this.status = 'success';
        this.message = 'Email successfully verified!';

        // Redirect to login immediately after 1.5 seconds
        setTimeout(() => {
          this.router.navigate(['/login'], {
            queryParams: { verified: 'true' }
          });
        }, 1500);
      },
      error: (err) => {
        this.status = 'error';
        this.message = err.error?.message || 'Verification failed. The link may be invalid or expired.';
      }
    });
  }

  goToLogin(): void {
    this.router.navigate(['/login']);
  }
}
