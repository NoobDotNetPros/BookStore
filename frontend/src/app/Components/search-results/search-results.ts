import { Component, OnInit, inject, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { BookService } from '../../Services/book.service';
import { BookDto } from '../../Models/book.models';
import { Subject, takeUntil, combineLatest } from 'rxjs';

@Component({
  selector: 'app-search-results',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './search-results.html',
  styleUrl: './search-results.scss'
})
export class SearchResultsComponent implements OnInit, OnDestroy {
  private bookService = inject(BookService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  searchQuery = '';
  allBooks: BookDto[] = [];
  searchResults: BookDto[] = [];
  isLoading = false;
  noResults = false;
  private destroy$ = new Subject<void>();

  // Filter options
  sortBy: 'relevance' | 'price-low' | 'price-high' | 'name' = 'relevance';
  priceRange: 'all' | 'under-500' | '500-1000' | 'above-1000' = 'all';

  ngOnInit(): void {
    // Try to use cached books first for instant results
    if (this.bookService.hasCachedBooks()) {
      this.allBooks = this.bookService.getCachedBooks();
    }

    // Load books (will use cache if available)
    this.loadBooks();

    // Subscribe to both URL params and service query changes
    combineLatest([
      this.route.queryParamMap,
      this.bookService.searchQuery$
    ]).pipe(
      takeUntil(this.destroy$)
    ).subscribe(([params, serviceQuery]) => {
      const urlQuery = params.get('q') || '';
      // Prefer URL query, fallback to service query
      this.searchQuery = urlQuery || serviceQuery;

      // Perform search immediately if we have books
      if (this.allBooks.length > 0) {
        this.performSearch();
      }
    });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private loadBooks(): void {
    // Only show loading if we don't have cached books
    if (!this.bookService.hasCachedBooks()) {
      this.isLoading = true;
    }

    this.bookService.getAllBooks().subscribe({
      next: (response) => {
        this.allBooks = response.data || [];
        this.performSearch();
        this.isLoading = false;
      },
      error: () => {
        this.allBooks = [];
        this.isLoading = false;
      }
    });
  }

  performSearch(): void {
    if (!this.searchQuery || this.searchQuery.trim().length < 1) {
      this.searchResults = [];
      this.noResults = false;
      return;
    }

    const searchTerm = this.searchQuery.toLowerCase().trim();

    let results = this.allBooks.filter(book =>
      book.bookName?.toLowerCase().includes(searchTerm) ||
      book.author?.toLowerCase().includes(searchTerm) ||
      book.description?.toLowerCase().includes(searchTerm)
    );

    // Apply price filter
    results = this.applyPriceFilter(results);

    // Apply sorting
    results = this.applySorting(results, searchTerm);

    this.searchResults = results;
    this.noResults = this.searchResults.length === 0;
  }

  private applyPriceFilter(books: BookDto[]): BookDto[] {
    switch (this.priceRange) {
      case 'under-500':
        return books.filter(b => (b.discountPrice || b.price) < 500);
      case '500-1000':
        return books.filter(b => {
          const price = b.discountPrice || b.price;
          return price >= 500 && price <= 1000;
        });
      case 'above-1000':
        return books.filter(b => (b.discountPrice || b.price) > 1000);
      default:
        return books;
    }
  }

  private applySorting(books: BookDto[], searchTerm: string): BookDto[] {
    switch (this.sortBy) {
      case 'price-low':
        return [...books].sort((a, b) =>
          (a.discountPrice || a.price) - (b.discountPrice || b.price)
        );
      case 'price-high':
        return [...books].sort((a, b) =>
          (b.discountPrice || b.price) - (a.discountPrice || a.price)
        );
      case 'name':
        return [...books].sort((a, b) =>
          (a.bookName || '').localeCompare(b.bookName || '')
        );
      case 'relevance':
      default:
        // Sort by relevance - exact title matches first, then author, then description
        return [...books].sort((a, b) => {
          const aTitle = a.bookName?.toLowerCase() || '';
          const bTitle = b.bookName?.toLowerCase() || '';
          const aAuthor = a.author?.toLowerCase() || '';
          const bAuthor = b.author?.toLowerCase() || '';

          const aExactTitle = aTitle === searchTerm;
          const bExactTitle = bTitle === searchTerm;
          if (aExactTitle && !bExactTitle) return -1;
          if (bExactTitle && !aExactTitle) return 1;

          const aTitleStarts = aTitle.startsWith(searchTerm);
          const bTitleStarts = bTitle.startsWith(searchTerm);
          if (aTitleStarts && !bTitleStarts) return -1;
          if (bTitleStarts && !aTitleStarts) return 1;

          const aTitleContains = aTitle.includes(searchTerm);
          const bTitleContains = bTitle.includes(searchTerm);
          if (aTitleContains && !bTitleContains) return -1;
          if (bTitleContains && !aTitleContains) return 1;

          const aAuthorContains = aAuthor.includes(searchTerm);
          const bAuthorContains = bAuthor.includes(searchTerm);
          if (aAuthorContains && !bAuthorContains) return -1;
          if (bAuthorContains && !aAuthorContains) return 1;

          return 0;
        });
    }
  }

  onFilterChange(): void {
    this.performSearch();
  }

  onBookClick(bookId: number): void {
    this.router.navigate(['/book', bookId]);
  }

  clearSearch(): void {
    this.bookService.updateSearchQuery('');
    this.router.navigate(['/home']);
  }

  // Highlight matching text in results
  highlightMatch(text: string): string {
    if (!this.searchQuery || !text) return text;
    const searchTerm = this.searchQuery.trim();
    const regex = new RegExp(`(${this.escapeRegex(searchTerm)})`, 'gi');
    return text.replace(regex, '<mark>$1</mark>');
  }

  private escapeRegex(str: string): string {
    return str.replace(/[.*+?^${}()|[\]\\]/g, '\\$&');
  }
}
