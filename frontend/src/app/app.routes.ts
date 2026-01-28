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
import { MyCartComponent } from './mycart/mycart';
import { WishlistComponent } from './wishlist/wishlist';
import { authGuard } from './shared/guards/auth.guard';
import { HomepageComponent } from './pages/homepage/homepage.component';

export const routes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: 'login', component: Login },
  { path: 'signup', component: Signup },
  { path: 'forgot-password', component: ForgotPasswordComponent },
  { path: 'home', component: BookList },
  { path: 'book/:id', component: BookDetails },
  { path: 'wishlist', component: Wishlist },
  { path: 'orders', component: MyOrders },
  { path: 'profile', component: Profile },
  { path: 'my-cart', component: MyCartComponent, canActivate: [authGuard] },
  { path: 'order-success', component: OrderSuccess },
  { path: 'admin', component: AdminPanelComponent },
  { path: '**', redirectTo: '/home' }
];
