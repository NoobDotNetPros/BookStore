import { Component, output, inject, OnInit, OnDestroy, HostListener } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink, Router } from '@angular/router';
import { BookService } from '../../../Services/book.service';
import { AuthService } from '../../../Services/auth.service';
import { CartService } from '../../../Services/cart.service';
import { BookDto } from '../../../Models/book.models';
import { Subject, debounceTime, distinctUntilChanged, takeUntil } from 'rxjs';

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
  cartCount = 0;

  // Search autocomplete
  allBooks: BookDto[] = [];
  searchResults: BookDto[] = [];
  showSearchDropdown = false;
  isSearching = false;
  private searchSubject = new Subject<string>();
  private destroy$ = new Subject<void>();

  // Output events for parent components to handle navigation
  searchSubmit = output<string>();
  profileClick = output<void>();
  cartClick = output<void>();

  ngOnInit() {
    // Load books for search (available for all users)
    this.loadBooks();

    this.authService.currentUser$.subscribe(user => {
      this.isLoggedIn = !!user;
      this.userName = user?.fullName || 'Profile';

      if (this.isLoggedIn) {
        // Fetch cart from server to initialize count
        this.cartService.getCart().subscribe();
      }
      // Cart count for guest users is already initialized in CartService
    });

    this.cartService.cartCount$.subscribe(count => {
      this.cartCount = count;
    });

    // Setup debounced search for dropdown preview
    this.searchSubject.pipe(
      debounceTime(300),
      distinctUntilChanged(),
      takeUntil(this.destroy$)
    ).subscribe(query => {
      this.performSearch(query);
    });
  }

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private loadBooks(): void {
    this.bookService.getAllBooks().subscribe({
      next: (response) => {
        this.allBooks = response.data || [];
      },
      error: () => {
        this.allBooks = [];
      }
    });
  }

  onSearchInput(): void {
    this.searchSubject.next(this.searchQuery);
  }

  private performSearch(query: string): void {
    if (!query || query.trim().length < 2) {
      this.searchResults = [];
      this.showSearchDropdown = false;
      return;
    }

    this.isSearching = true;
    const searchTerm = query.toLowerCase().trim();

    this.searchResults = this.allBooks.filter(book =>
      book.bookName?.toLowerCase().includes(searchTerm) ||
      book.author?.toLowerCase().includes(searchTerm)
    ).slice(0, 6); // Limit to 6 results

    this.showSearchDropdown = this.searchResults.length > 0;
    this.isSearching = false;
  }

  onSearch(): void {
    if (!this.searchQuery.trim()) {
      return;
    }

    this.showSearchDropdown = false;
    // Navigate to search results page
    this.router.navigate(['/search'], {
      queryParams: { q: this.searchQuery.trim() }
    });
    // Update service for other components
    this.bookService.updateSearchQuery(this.searchQuery.trim());
    this.searchSubmit.emit(this.searchQuery.trim());
  }

  selectBook(book: BookDto): void {
    this.showSearchDropdown = false;
    this.searchQuery = '';
    this.router.navigate(['/book', book.id]);
  }

  onSearchFocus(): void {
    if (this.searchResults.length > 0) {
      this.showSearchDropdown = true;
    }
  }

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: Event): void {
    const target = event.target as HTMLElement;
    if (!target.closest('.header__search')) {
      this.showSearchDropdown = false;
    }
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
