import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

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

  togglePassword() {
    this.showPassword = !this.showPassword;
  }

  onSubmit() {
    this.submitted = true;
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
