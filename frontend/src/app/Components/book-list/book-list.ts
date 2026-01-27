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
  isOutOfStock?: boolean;
}

@Component({
  selector: 'app-book-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './book-list.html',
  styleUrl: './book-list.scss',
})
export class BookList implements OnInit {
  private router = inject(Router);
  private bookService = inject(BookService);

  books = signal<DisplayBook[]>([]);
  loading = signal<boolean>(true);
  errorMessage = signal<string>('');

  ngOnInit() {
    this.loadBooks();
  }

  loadBooks() {
    this.loading.set(true);
    this.errorMessage.set('');

    this.bookService.getAllBooks().subscribe({
      next: (response) => {
        if (response.success && response.data) {
          const displayBooks = response.data.map(book => ({
            id: book.id,
            title: book.title,
            author: book.author,
            rating: 4,
            reviewsCount: 0,
            price: book.price,
            originalPrice: book.price * 1.2,
            image: book.coverImage,
            isOutOfStock: book.stock === 0
          }));
          this.books.set(displayBooks);
        }
        this.loading.set(false);
      },
      error: (err) => {
        this.errorMessage.set('Failed to load books. Please try again.');
        console.error('Error loading books:', err);
        this.loading.set(false);
        // Use mock data as fallback
        this.setMockBooks();
      }
    });
  }

  setMockBooks() {
    this.books.set([
      {
        id: 1,
        title: "Don't Make Me Think",
        author: 'by Steve Krug',
        rating: 4.5,
        reviewsCount: 20,
        price: 1500,
        originalPrice: 2000,
        image: 'https://images-na.ssl-images-amazon.com/images/I/41SH-SvWPxL._SX430_BO1,204,203,200_.jpg'
      },
      {
        id: 2,
        title: "React Material-UI Cookbook",
        author: 'by Adam Boduch',
        rating: 4.5,
        reviewsCount: 20,
        price: 1500,
        originalPrice: 2000,
        image: 'https://m.media-amazon.com/images/I/71UBc1jVb-L._AC_UF1000,1000_QL80_.jpg'
      },
      {
        id: 3,
        title: "Mastering SharePoint Framework",
        author: 'by Nanddeep Nachan',
        rating: 4.5,
        reviewsCount: 20,
        price: 1500,
        originalPrice: 2000,
        image: 'https://bpb-us-w2.wpmucdn.com/u.osu.edu/dist/5/33423/files/2016/09/sharepoint-1-248j85k.jpg'
      },
      {
        id: 4,
        title: "UX For Dummies",
        author: 'by Donald Chesnut',
        rating: 4.5,
        reviewsCount: 20,
        price: 1500,
        originalPrice: 3000,
        image: 'https://m.media-amazon.com/images/I/51p1kQ0C1AL.jpg',
        isOutOfStock: true
      },
      {
        id: 5,
        title: "UX Design",
        author: 'by Someone',
        rating: 4.5,
        reviewsCount: 20,
        price: 1500,
        originalPrice: 2000,
        image: 'https://m.media-amazon.com/images/I/41d-XzJgZLL._AC_SY200_QL15_.jpg'
      },
      {
        id: 6,
        title: "Don't Make Me Think",
        author: 'by Steve Krug',
        rating: 4.5,
        reviewsCount: 20,
        price: 1500,
        originalPrice: 2000,
        image: 'https://images-na.ssl-images-amazon.com/images/I/41SH-SvWPxL._SX430_BO1,204,203,200_.jpg'
      },
      {
        id: 7,
        title: "Lean UX",
        author: 'by Jeff Gothelf',
        rating: 4.5,
        reviewsCount: 20,
        price: 1500,
        originalPrice: 2000,
        image: 'https://m.media-amazon.com/images/I/51+u7l+C+rL._SX342_SY445_QL70_ML2_.jpg'
      },
      {
        id: 8,
        title: "The Design of Everyday Things",
        author: 'by Don Norman',
        rating: 4.5,
        reviewsCount: 20,
        price: 1500,
        originalPrice: 2000,
        image: 'https://m.media-amazon.com/images/I/410RTQezHYL._AC_SY200_QL15_.jpg'
      }
    ]);
  }

  openDetails(bookId: number) {
    this.router.navigate(['/book', bookId]);
  }

  sortOptions = ['Sort by relevance', 'Price: Low to High', 'Price: High to Low', 'Newest First'];
  itemCount = 128;
}
