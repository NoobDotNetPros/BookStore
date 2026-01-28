import { Component, signal, inject, OnInit, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { BookService } from '../../Services/book.service';
import { BookDto } from '../../Models/book.models';

interface DisplayBook {
  id: number;
  title: string;
  author: string;
  rating: number;
  reviewsCount: number;
  price: number;
  originalPrice: number;
  image: string;
  isOutOfStock: boolean;
}

@Component({
  selector: 'app-book-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './book-list.html',
  styleUrl: './book-list.scss'
})
export class BookList implements OnInit {
  private bookService = inject(BookService);
  private router = inject(Router);

  rawBooks = signal<DisplayBook[]>([]); // Data from API

  // State
  searchQuery = signal<string>('');
  currentSort = signal<string>('Relevance');
  loading = signal<boolean>(true);
  errorMessage = signal<string>('');

  // Pagination
  currentPage = signal<number>(1);
  itemsPerPage = 12;

  sortOptions = signal<string[]>([
    'Relevance',
    'Price: Low to High',
    'Price: High to Low',
    'Newest First',
    'Rating: High to Low'
  ]);

  // Computed: Filter and Sort books
  filteredBooks = computed(() => {
    let books = [...this.rawBooks()];
    const query = this.searchQuery().toLowerCase();
    const sort = this.currentSort();

    // 1. Filter
    if (query) {
      books = books.filter(b =>
        b.title.toLowerCase().includes(query) ||
        b.author.toLowerCase().includes(query)
      );
    }

    // 2. Sort
    switch (sort) {
      case 'Price: Low to High':
        books.sort((a, b) => a.price - b.price);
        break;
      case 'Price: High to Low':
        books.sort((a, b) => b.price - a.price);
        break;
      case 'Newest First':
        books.sort((a, b) => b.id - a.id); // Assuming higher ID = newer
        break;
      case 'Rating: High to Low':
        books.sort((a, b) => b.rating - a.rating);
        break;
      default: // Relevance - Keep original order (or maybe by ID?)
        break;
    }

    return books;
  });

  // Computed: total pages
  totalPages = computed(() => Math.ceil(this.filteredBooks().length / this.itemsPerPage) || 1);

  // Computed: books for current page
  paginatedBooks = computed(() => {
    // Reset page if filtered results are less
    if (this.currentPage() > this.totalPages()) {
      // We cannot update signal inside computed directly, 
      // but logic handles index out of bounds by slice naturally
    }

    const start = (this.currentPage() - 1) * this.itemsPerPage;
    const end = start + this.itemsPerPage;
    return this.filteredBooks().slice(start, end);
  });

  // Computed: page numbers to display
  pageNumbers = computed(() => {
    const total = this.totalPages();
    const current = this.currentPage();
    const pages: (number | string)[] = [];

    if (total <= 7) {
      for (let i = 1; i <= total; i++) pages.push(i);
    } else {
      pages.push(1);
      if (current > 3) pages.push('...');

      const start = Math.max(2, current - 1);
      const end = Math.min(total - 1, current + 1);

      for (let i = start; i <= end; i++) pages.push(i);

      if (current < total - 2) pages.push('...');
      pages.push(total);
    }

    return pages;
  });

  ngOnInit() {
    this.loadBooks();

    // Subscribe to search query
    this.bookService.searchQuery$.subscribe(query => {
      this.searchQuery.set(query);
      this.currentPage.set(1); // Reset to first page on search
    });
  }

  loadBooks() {
    this.loading.set(true);
    this.errorMessage.set('');

    this.bookService.getAllBooks().subscribe({
      next: (response) => {
        if (response.success && response.data) {
          const displayBooks: DisplayBook[] = response.data.map((book: BookDto) => ({
            id: book.id,
            title: book.title || book.bookName,
            author: book.author,
            rating: 4.5, // Mock rating
            reviewsCount: 20,
            price: book.discountPrice,
            originalPrice: book.price,
            image: book.coverImage || 'https://via.placeholder.com/200x300',
            isOutOfStock: (book.stock || book.quantity) === 0
          }));
          this.rawBooks.set(displayBooks);
        } else {
          this.errorMessage.set(response.message || 'Failed to load books');
        }
        this.loading.set(false);
      },
      error: (err: any) => {
        this.errorMessage.set('Failed to load books. Please try again.');
        console.error('Error loading books:', err);
        this.loading.set(false);
      }
    });
  }

  onSortChange(event: Event) {
    const select = event.target as HTMLSelectElement;
    this.currentSort.set(select.value);
    this.currentPage.set(1);
  }

  goToPage(page: number | string) {
    if (typeof page === 'number' && page >= 1 && page <= this.totalPages()) {
      this.currentPage.set(page);
      window.scrollTo({ top: 0, behavior: 'smooth' });
    }
  }

  prevPage() {
    if (this.currentPage() > 1) {
      this.currentPage.update(p => p - 1);
      window.scrollTo({ top: 0, behavior: 'smooth' });
    }
  }

  nextPage() {
    if (this.currentPage() < this.totalPages()) {
      this.currentPage.update(p => p + 1);
      window.scrollTo({ top: 0, behavior: 'smooth' });
    }
  }

  openDetails(bookId: number) {
    this.router.navigate(['/book', bookId]);
  }

  formatPrice(price: number): string {
    return price.toLocaleString('en-IN');
  }

  trackByBookId(index: number, book: DisplayBook): number {
    return book.id;
  }
}
