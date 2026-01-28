import { Component, output, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink, Router } from '@angular/router';
import { BookService } from '../../../Services/book.service';
import { AuthService } from '../../../Services/auth.service';
import { CartService } from '../../../Services/cart.service';

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
  private cartService = inject(CartService);
  private router = inject(Router);

  isLoggedIn = false;
  userName = 'Profile';
  cartCount = 0;

  // Output events for parent components to handle navigation
  searchSubmit = output<string>();
  profileClick = output<void>();
  cartClick = output<void>();

  ngOnInit() {
    this.authService.currentUser$.subscribe(user => {
      this.isLoggedIn = !!user;
      this.userName = user?.user.fullName || 'Profile';

      if (this.isLoggedIn) {
        // Fetch cart to initialize count
        this.cartService.getCart().subscribe();
      } else {
        this.cartService.updateCartCount(0);
      }
    });

    this.cartService.cartCount$.subscribe(count => {
      this.cartCount = count;
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
    }
  }

  onCartClick(): void {
    this.cartClick.emit(); // Keep emitting just in case
    this.router.navigate(['/my-cart']);
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/']);
  }
}

