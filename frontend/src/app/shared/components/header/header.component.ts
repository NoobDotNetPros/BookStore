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
    this.authService.currentUser$.subscribe(user => {
      this.isLoggedIn = !!user;
      this.userName = user?.fullName || 'Profile';

      if (this.isLoggedIn) {
        // Fetch cart to initialize count
        this.cartService.getCart().subscribe();
        // Load all books for search
        this.loadBooks();
      } else {
        this.cartService.updateCartCount(0);
      }
    });

    this.cartService.cartCount$.subscribe(count => {
      this.cartCount = count;
    });

    // Setup debounced search
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
    this.showSearchDropdown = false;
    // Always emit update, even if empty, to clear search
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
    this.cartClick.emit(); // Keep emitting just in case
    this.router.navigate(['/my-cart']);
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/']);
  }
}

