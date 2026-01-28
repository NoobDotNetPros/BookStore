import { Component, output, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink, Router } from '@angular/router';
import { BookService } from '../../../Services/book.service';
import { AuthService } from '../../../Services/auth.service';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss',
})
export class HeaderComponent implements OnInit {
  searchQuery = '';
  private bookService = inject(BookService);
  private authService = inject(AuthService);
  private router = inject(Router);

  isLoggedIn = false;
  userName = 'Profile';

  // Output events for parent components to handle navigation
  searchSubmit = output<string>();
  profileClick = output<void>();
  cartClick = output<void>();

  ngOnInit() {
    this.authService.currentUser$.subscribe(user => {
      this.isLoggedIn = !!user;
      this.userName = user?.user.fullName || 'Profile';
    });
  }

  onSearch(): void {
    // Always emit update, even if empty, to clear search
    this.bookService.updateSearchQuery(this.searchQuery.trim());
    this.searchSubmit.emit(this.searchQuery.trim());
  }

  onProfileClick(): void {
    if (this.isLoggedIn) {
      this.profileClick.emit();
    } else {
      // If not logged in, maybe clicking profile icon should also go to login or just toggle dropdown?
      // For now, let's just let the hover work, but click can redirect to login
      // this.router.navigate(['/login']);
    }
  }

  onCartClick(): void {
    this.cartClick.emit();
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/']);
  }
}

