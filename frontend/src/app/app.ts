import { Component, inject } from '@angular/core';
import { RouterOutlet, Router, NavigationEnd } from '@angular/router';
import { FooterComponent } from './shared/components/footer';
import { HeaderComponent } from './shared/components/header';
import { ToastComponent } from './Components/toast/toast.component';
import { CommonModule } from '@angular/common';
import { filter } from 'rxjs/operators';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, FooterComponent, HeaderComponent, ToastComponent, CommonModule],
  templateUrl: './app.html',
  styleUrls: ['./app.scss']
})
export class App {
  title = 'Bookstore';
  showLayout = true;

  private router = inject(Router);

  // Routes that should not show header/footer
  private noLayoutRoutes = ['/login', '/signup', '/forgot-password', '/verify-email'];

  constructor() {
    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe((event: NavigationEnd) => {
      this.showLayout = !this.noLayoutRoutes.some(route => event.urlAfterRedirects.startsWith(route));
    });

    // Check initial route
    this.showLayout = !this.noLayoutRoutes.some(route => this.router.url.startsWith(route));
  }
}
