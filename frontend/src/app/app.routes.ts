import { Routes } from '@angular/router';
import { BookList } from './Components/book-list/book-list';
import { BookDetails } from './Components/book-details/book-details';
import { Wishlist } from './Components/wishlist/wishlist';
import { MyOrders } from './Components/my-orders/my-orders';
import { Profile } from './Components/profile/profile';
import { OrderSuccess } from './Components/order-success/order-success';
import { AdminPanelComponent } from './Components/admin-panel/admin-panel';
import { Login } from './login/login';
import { Signup } from './signup/signup';
import { ForgotPasswordComponent } from './forgot-password/forgot-password';
import { VerifyEmailComponent } from './verify-email/verify-email';
import { MyCartComponent } from './mycart/mycart';
import { AuthGuard } from './shared/guards/auth.guard';
import { AdminAuthGuard } from './shared/guards/admin.guard';

export const routes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: 'login', component: Login },
  { path: 'signup', component: Signup },
  { path: 'forgot-password', component: ForgotPasswordComponent },
  { path: 'verify-email', component: VerifyEmailComponent },
  { path: 'home', component: BookList, canActivate: [AuthGuard] },
  { path: 'book/:id', component: BookDetails, canActivate: [AuthGuard] },
  { path: 'wishlist', component: Wishlist, canActivate: [AuthGuard] },
  { path: 'orders', component: MyOrders, canActivate: [AuthGuard] },
  { path: 'profile', component: Profile, canActivate: [AuthGuard] },
  { path: 'my-cart', component: MyCartComponent, canActivate: [AuthGuard] },
  { path: 'order-success', component: OrderSuccess, canActivate: [AuthGuard] },
  { path: 'admin', component: AdminPanelComponent, canActivate: [AdminAuthGuard] }
];
