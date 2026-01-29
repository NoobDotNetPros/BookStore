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
  showHeader = true;
  showFooter = true;

  private router = inject(Router);

  // Routes that should not show footer (but header is shown)
  private noFooterRoutes = ['/login', '/signup', '/forgot-password', '/verify-email'];

  constructor() {
    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe((event: NavigationEnd) => {
      this.updateLayoutVisibility(event.urlAfterRedirects);
    });

    // Check initial route
    this.updateLayoutVisibility(this.router.url);
  }

  private updateLayoutVisibility(url: string): void {
    const isNoFooterRoute = this.noFooterRoutes.some(route => url.startsWith(route));
    this.showHeader = true; // Always show header
    this.showFooter = !isNoFooterRoute;
  }
}
