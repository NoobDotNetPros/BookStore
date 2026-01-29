import { Component, OnInit, signal, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { BookService } from '../../Services/book.service';
import { CartService } from '../../Services/cart.service';
import { WishlistService } from '../../Services/wishlist.service';
import { AuthService } from '../../Services/auth.service';
import { ToastService } from '../../Services/toast.service';
import { BookDto } from '../../Models/book.models';

interface BookDetailsView {
  id: number;
  title: string;
  author: string;
  description: string;
  price: number;
  originalPrice: number;
  discount: number;
  rating: number;
  reviewsCount: number;
  image: string;
  isbn: string;
  stock: number;
  isOutOfStock: boolean;
}

interface CustomerReview {
  initial: string;
  name: string;
  rating: number;
  comment: string;
}

@Component({
  selector: 'app-book-details',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './book-details.html',
  styleUrl: './book-details.scss'
})
export class BookDetails implements OnInit {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private bookService = inject(BookService);
  private cartService = inject(CartService);
  private wishlistService = inject(WishlistService);
  private authService = inject(AuthService);

  book = signal<BookDetailsView | null>(null);
  loading = signal<boolean>(true);
  errorMessage = signal<string>('');
  isLoggedIn = signal<boolean>(false);

  selectedQuantity = signal<number>(1);

  // Customer reviews (mock data for now)
  customerReviews = signal<CustomerReview[]>([
    { initial: 'S', name: 'Sarah Johnson', rating: 5, comment: 'Excellent book! Highly recommended.' },
    { initial: 'M', name: 'Michael Smith', rating: 4, comment: 'Great read, very informative.' },
    { initial: 'E', name: 'Emily Davis', rating: 5, comment: 'One of the best books I\'ve read this year!' }
  ]);

  ngOnInit() {
    console.log('BookDetails component initialized');
    this.isLoggedIn.set(this.authService.isLoggedIn());

    const bookId = this.route.snapshot.paramMap.get('id');
    console.log('Book ID from route:', bookId);

    if (bookId) {
      this.loadBookDetails(+bookId);
    } else {
      console.error('No book ID found in route');
      this.router.navigate(['/home']);
    }
  }

  goBack() {
    this.router.navigate(['/home']);
  }

  loadBookDetails(bookId: number) {
    this.loading.set(true);
    this.errorMessage.set('');

    this.bookService.getBookById(bookId).subscribe({
      next: (response) => {
        if (response.success && response.data) {
          const bookDto = response.data;
          const discount = Math.round(((bookDto.price - bookDto.discountPrice) / bookDto.price) * 100);

          this.book.set({
            id: bookDto.id,
            title: bookDto.title || bookDto.bookName,
            author: bookDto.author,
            description: bookDto.description,
            price: bookDto.discountPrice,
            originalPrice: bookDto.price,
            discount: discount,
            rating: 4.5,
            reviewsCount: 120,
            image: bookDto.coverImage || 'https://via.placeholder.com/400x600',
            isbn: bookDto.isbn || 'N/A',
            stock: bookDto.stock || bookDto.quantity,
            isOutOfStock: (bookDto.stock || bookDto.quantity) <= 0
          });
        } else {
          this.errorMessage.set('Book not found');
        }
        this.loading.set(false);
      },
      error: (err: any) => {
        this.errorMessage.set('Failed to load book details');
        console.error('Error loading book details:', err);
        this.loading.set(false);
      }
    });
  }

  increaseQuantity() {
    const currentBook = this.book();
    if (currentBook && this.selectedQuantity() < currentBook.stock) {
      this.selectedQuantity.update(q => q + 1);
    }
  }

  decreaseQuantity() {
    if (this.selectedQuantity() > 1) {
      this.selectedQuantity.update(q => q - 1);
    }
  }

  private toastService = inject(ToastService);

  addToCart() {
    if (!this.isLoggedIn()) {
      this.router.navigate(['/login']);
      return;
    }

    const currentBook = this.book();
    if (currentBook) {
      // Using CartService method - adjust based on your actual service
      this.cartService.addItem(currentBook.id, this.selectedQuantity()).subscribe({
        next: () => {
          this.toastService.success('Book added to cart successfully!');
        },
        error: (err: any) => {
          console.error('Error adding to cart:', err);
          this.toastService.error('Failed to add book to cart');
        }
      });
    }
  }

  addToWishlist() {
    if (!this.isLoggedIn()) {
      this.router.navigate(['/login']);
      return;
    }

    const currentBook = this.book();
    if (currentBook) {
      this.wishlistService.addToWishlist(currentBook.id).subscribe({
        next: () => {
          this.toastService.success('Book added to wishlist successfully!');
        },
        error: (err: any) => {
          console.error('Error adding to wishlist:', err);
          this.toastService.error('Failed to add book to wishlist');
        }
      });
    }
  }

  notifyMe() {
    this.toastService.info('You will be notified when this book is back in stock!');
  }

  buyNow() {
    if (!this.isLoggedIn()) {
      this.router.navigate(['/login']);
      return;
    }

    this.addToCart();
    this.router.navigate(['/cart']);
  }
}
