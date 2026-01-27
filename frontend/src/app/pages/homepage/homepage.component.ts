import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HeaderComponent } from '../../components/header/header.component';
import { BookCardComponent } from '../../components/book-card/book-card.component';
import { PaginationComponent } from '../../components/pagination/pagination.component';
import { Book } from '../../models/book.model';

@Component({
  selector: 'app-homepage',
  standalone: true,
  imports: [CommonModule, FormsModule, HeaderComponent, BookCardComponent, PaginationComponent],
  templateUrl: './homepage.component.html',
  styleUrl: './homepage.component.scss'
})
export class HomepageComponent {
  books = signal<Book[]>([]);
  totalItems = signal(128);
  currentPage = signal(1);
  totalPages = signal(18);
  sortBy = signal('relevance');
  itemsPerPage = 8;

  sortOptions = [
    { value: 'relevance', label: 'Sort by relevance' },
    { value: 'price-low', label: 'Price: Low to High' },
    { value: 'price-high', label: 'Price: High to Low' },
    { value: 'rating', label: 'Rating' },
    { value: 'newest', label: 'Newest First' }
  ];

  constructor() {
    this.loadBooks();
  }

  loadBooks(): void {
    // Mock data based on the images
    const mockBooks: Book[] = [
      {
        id: 1,
        title: "Don't Make Me Think",
        author: 'Steve Krug',
        coverImage: 'https://m.media-amazon.com/images/I/51WS36aA2BL._SY445_SX342_.jpg',
        rating: 4.5,
        reviewCount: 20,
        price: 1500,
        originalPrice: 2000,
        isOutOfStock: false
      },
      {
        id: 2,
        title: "Don't Make Me Think",
        author: 'Steve Krug',
        coverImage: 'https://m.media-amazon.com/images/I/61ZKhZCk0iL._SY466_.jpg',
        rating: 4.5,
        reviewCount: 20,
        price: 1500,
        originalPrice: 2000,
        isOutOfStock: false
      },
      {
        id: 3,
        title: "Don't Make Me Think",
        author: 'Steve Krug',
        coverImage: 'https://m.media-amazon.com/images/I/61mIq2iJUXL._SY466_.jpg',
        rating: 4.5,
        reviewCount: 20,
        price: 1500,
        originalPrice: 2000,
        isOutOfStock: false
      },
      {
        id: 4,
        title: 'UX For DUMMIES',
        author: 'Steve Krug',
        coverImage: 'https://m.media-amazon.com/images/I/51cYWSHBfvL._SY466_.jpg',
        rating: 4.5,
        reviewCount: 20,
        price: 1500,
        originalPrice: 2000,
        isOutOfStock: true
      },
      {
        id: 5,
        title: "Don't Make Me Think",
        author: 'Steve Krug',
        coverImage: 'https://m.media-amazon.com/images/I/41bimzkvtlL._SY466_.jpg',
        rating: 4.5,
        reviewCount: 20,
        price: 1500,
        originalPrice: 2000,
        isOutOfStock: false
      },
      {
        id: 6,
        title: "Don't Make Me Think",
        author: 'Steve Krug',
        coverImage: 'https://m.media-amazon.com/images/I/51RFFZ9RQEL._SY466_.jpg',
        rating: 4.5,
        reviewCount: 20,
        price: 1500,
        originalPrice: 2000,
        isOutOfStock: false
      },
      {
        id: 7,
        title: "Don't Make Me Think",
        author: 'Steve Krug',
        coverImage: 'https://m.media-amazon.com/images/I/41k3nCy4fPL._SY466_.jpg',
        rating: 4.5,
        reviewCount: 20,
        price: 1500,
        originalPrice: 2000,
        isOutOfStock: false
      },
      {
        id: 8,
        title: "Don't Make Me Think",
        author: 'Steve Krug',
        coverImage: 'https://m.media-amazon.com/images/I/410RTdGSdgL._SY466_.jpg',
        rating: 4.5,
        reviewCount: 20,
        price: 1500,
        originalPrice: 2000,
        isOutOfStock: false
      }
    ];

    this.books.set(mockBooks);
  }

  onSortChange(): void {
    console.log('Sorting by:', this.sortBy());
    // Implement sorting logic here
  }

  onPageChange(page: number): void {
    this.currentPage.set(page);
    // Implement page change logic here
    console.log('Changed to page:', page);
  }
}
