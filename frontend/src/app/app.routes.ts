import { Routes } from '@angular/router';
import { Login } from './login/login';
import { Signup } from './signup/signup';
import { ForgotPasswordComponent } from './forgot-password/forgot-password';
import { MyCartComponent } from './mycart/mycart';

export const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'login', component: Login },
  { path: 'signup', component: Signup },
  { path: 'forgot-password', component: ForgotPasswordComponent },
  { path: 'my-cart', component: MyCartComponent }
];
