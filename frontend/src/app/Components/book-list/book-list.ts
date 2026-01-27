import { Component, signal, inject, OnInit } from '@angular/core';
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

  books = signal<DisplayBook[]>([]);
  loading = signal<boolean>(true);
  errorMessage = signal<string>('');

  sortOptions = signal<string[]>([
    'Relevance',
    'Price: Low to High',
    'Price: High to Low',
    'Newest First',
    'Rating: High to Low'
  ]);

  ngOnInit() {
    this.loadBooks();
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
            rating: 4.5,
            reviewsCount: 120,
            price: book.discountPrice,
            originalPrice: book.price,
            image: book.coverImage || 'https://via.placeholder.com/200x300',
            isOutOfStock: (book.stock || book.quantity) === 0
          }));
          this.books.set(displayBooks);
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

  openDetails(bookId: number) {
    this.router.navigate(['/book', bookId]);
  }

  viewBookDetails(bookId: number) {
    this.router.navigate(['/book', bookId]);
  }

  formatPrice(price: number): string {
    return price.toLocaleString('en-IN');
  }

  trackByBookId(index: number, book: DisplayBook): number {
    return book.id;
  }
}
