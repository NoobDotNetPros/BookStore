import { Routes } from '@angular/router';
import { BookList } from './Components/book-list/book-list';
import { OrderSuccess } from './Components/order-success/order-success';
import { Login } from './login/login';
import { Signup } from './signup/signup';
import { ForgotPasswordComponent } from './forgot-password/forgot-password';
import { MyCartComponent } from './mycart/mycart';

export const routes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: 'login', component: Login },
  { path: 'signup', component: Signup },
  { path: 'forgot-password', component: ForgotPasswordComponent },
  { path: 'home', component: BookList },
  { path: 'my-cart', component: MyCartComponent },
  { path: 'order-success', component: OrderSuccess }
];
