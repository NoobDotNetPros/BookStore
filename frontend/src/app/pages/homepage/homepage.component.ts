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
    // Mock data with reliable image URLs from picsum.photos
    const mockBooks: Book[] = [
      {
        id: 1,
        title: "Don't Make Me Think",
        author: 'Steve Krug',
        coverImage: 'https://picsum.photos/seed/book1/150/200',
        rating: 4.5,
        reviewCount: 20,
        price: 1500,
        originalPrice: 2000,
        isOutOfStock: false
      },
      {
        id: 2,
        title: 'React Material-UI Cookbook',
        author: 'Adam Boduch',
        coverImage: 'https://picsum.photos/seed/book2/150/200',
        rating: 4.5,
        reviewCount: 20,
        price: 1500,
        originalPrice: 2000,
        isOutOfStock: false
      },
      {
        id: 3,
        title: 'Cracking the Coding Interview',
        author: 'Gayle McDowell',
        coverImage: 'https://picsum.photos/seed/book3/150/200',
        rating: 4.5,
        reviewCount: 20,
        price: 1500,
        originalPrice: 2000,
        isOutOfStock: false
      },
      {
        id: 4,
        title: 'UX For Dummies',
        author: 'Kevin Nichols',
        coverImage: 'https://picsum.photos/seed/book4/150/200',
        rating: 4.5,
        reviewCount: 20,
        price: 1500,
        originalPrice: 2000,
        isOutOfStock: true
      },
      {
        id: 5,
        title: 'A Project Guide to UX Design',
        author: 'Russ Unger',
        coverImage: 'https://picsum.photos/seed/book5/150/200',
        rating: 4.5,
        reviewCount: 20,
        price: 1500,
        originalPrice: 2000,
        isOutOfStock: false
      },
      {
        id: 6,
        title: 'Group Discussion',
        author: 'M.B. Lal',
        coverImage: 'https://picsum.photos/seed/book6/150/200',
        rating: 4.5,
        reviewCount: 20,
        price: 1500,
        originalPrice: 2000,
        isOutOfStock: false
      },
      {
        id: 7,
        title: 'Lean UX',
        author: 'Jeff Gothelf',
        coverImage: 'https://picsum.photos/seed/book7/150/200',
        rating: 4.5,
        reviewCount: 20,
        price: 1500,
        originalPrice: 2000,
        isOutOfStock: false
      },
      {
        id: 8,
        title: 'The Design of Everyday Things',
        author: 'Don Norman',
        coverImage: 'https://picsum.photos/seed/book8/150/200',
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
