import { Component, OnInit, inject, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { BookService } from '../../Services/book.service';
import { BookDto } from '../../Models/book.models';
import { Subject, takeUntil } from 'rxjs';

@Component({
  selector: 'app-search-results',
  standalone: true,
  imports: [CommonModule],
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

  ngOnInit(): void {
    // Get search query from URL
    this.route.queryParamMap.pipe(
      takeUntil(this.destroy$)
    ).subscribe(params => {
      this.searchQuery = params.get('q') || '';
      if (this.searchQuery) {
        this.performSearch();
      }
    });

    // Load all books for searching
    this.loadBooks();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private loadBooks(): void {
    this.isLoading = true;
    this.bookService.getAllBooks().subscribe({
      next: (response) => {
        this.allBooks = response.data || [];
        if (this.searchQuery) {
          this.performSearch();
        }
        this.isLoading = false;
      },
      error: () => {
        this.allBooks = [];
        this.isLoading = false;
      }
    });
  }

  private performSearch(): void {
    if (!this.searchQuery || this.searchQuery.trim().length < 1) {
      this.searchResults = [];
      this.noResults = false;
      return;
    }

    const searchTerm = this.searchQuery.toLowerCase().trim();

    this.searchResults = this.allBooks.filter(book =>
      book.bookName?.toLowerCase().includes(searchTerm) ||
      book.author?.toLowerCase().includes(searchTerm) ||
      book.description?.toLowerCase().includes(searchTerm)
    );

    this.noResults = this.searchResults.length === 0;
  }

  onBookClick(bookId: number): void {
    this.router.navigate(['/book', bookId]);
  }

  clearSearch(): void {
    this.router.navigate(['/home']);
  }
}
