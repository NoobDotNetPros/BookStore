import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';

@Component({
  imports: [FormsModule, RouterLink],
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.html',
  styleUrls: ['./forgot-password.css']
})
export class ForgotPasswordComponent {
  email: string = '';

  onSubmit() {
    // TODO: Implement password reset logic (API call)
    alert('Password reset link sent to: ' + this.email);
  }

  onCreateAccount() {
    // TODO: Implement navigation to create account page
    alert('Navigate to create account page');
  }
}