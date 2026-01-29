import { Component, output, inject, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink, Router, NavigationEnd } from '@angular/router';
import { BookService } from '../../../Services/book.service';
import { AuthService } from '../../../Services/auth.service';
import { CartService } from '../../../Services/cart.service';
import { Subject, debounceTime, distinctUntilChanged, takeUntil, filter } from 'rxjs';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss',
})
export class HeaderComponent implements OnInit, OnDestroy {
  searchQuery = '';
  private bookService = inject(BookService);
  private authService = inject(AuthService);
  private cartService = inject(CartService);
  private router = inject(Router);

  isLoggedIn = false;
  userName = 'Profile';
  cartCount$ = this.cartService.cartCount$;

  // Search
  private searchSubject = new Subject<string>();
  private destroy$ = new Subject<void>();

  // Output events for parent components to handle navigation
  searchSubmit = output<string>();
  profileClick = output<void>();
  cartClick = output<void>();

  ngOnInit() {
    // Preload books for instant search
    this.bookService.getAllBooks().subscribe();

    this.authService.currentUser$.subscribe(user => {
      this.isLoggedIn = !!user;
      this.userName = user?.fullName || 'Profile';

      if (this.isLoggedIn) {
        // Fetch cart from server to initialize count
        this.cartService.getCart().subscribe();
      }
      // Cart count for guest users is already initialized in CartService
    });

    // Setup debounced search - navigate to search page while typing
    this.searchSubject.pipe(
      debounceTime(150),
      distinctUntilChanged(),
      takeUntil(this.destroy$)
    ).subscribe(query => {
      this.navigateToSearch(query);
    });

    // Sync search query from URL when navigating
    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd),
      takeUntil(this.destroy$)
    ).subscribe(() => {
      const url = this.router.url;
      if (!url.startsWith('/search')) {
        // Clear search when leaving search page
        this.searchQuery = '';
      }
    });

    // Subscribe to search query changes from service
    this.bookService.searchQuery$.pipe(
      takeUntil(this.destroy$)
    ).subscribe(query => {
      if (this.searchQuery !== query) {
        this.searchQuery = query;
      }
    });
  }

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }

  onSearchInput(): void {
    this.searchSubject.next(this.searchQuery);
  }

  private navigateToSearch(query: string): void {
    const trimmedQuery = query.trim();

    if (trimmedQuery.length >= 1) {
      // Navigate to search page with query
      this.router.navigate(['/search'], {
        queryParams: { q: trimmedQuery }
      });
      this.bookService.updateSearchQuery(trimmedQuery);
    } else if (this.router.url.startsWith('/search')) {
      // If on search page and query is empty, go back to home
      this.router.navigate(['/home']);
    }
  }

  onSearch(): void {
    const trimmedQuery = this.searchQuery.trim();
    if (!trimmedQuery) {
      return;
    }

    // Navigate to search results page
    this.router.navigate(['/search'], {
      queryParams: { q: trimmedQuery }
    });
    this.bookService.updateSearchQuery(trimmedQuery);
    this.searchSubmit.emit(trimmedQuery);
  }

  clearSearch(): void {
    this.searchQuery = '';
    this.bookService.updateSearchQuery('');
    this.router.navigate(['/home']);
  }

  onProfileClick(): void {
    if (this.isLoggedIn) {
      this.profileClick.emit();
    }
  }

  onCartClick(): void {
    this.cartClick.emit();
    this.router.navigate(['/my-cart']);
  }

  logout(): void {
    this.authService.logout();
    this.cartService.updateCartCount(this.cartService.getLocalCart().length);
    this.router.navigate(['/home']);
  }
}
