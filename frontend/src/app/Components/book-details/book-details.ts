import { Component, signal, inject, OnInit, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { BookService } from '../../Services/book.service';
import { FeedbackService, CreateFeedbackRequest } from '../../Services/feedback.service';
import { CartService } from '../../Services/cart.service';
import { WishlistService } from '../../Services/wishlist.service';
import { BookDto } from '../../Models/book.models';

interface Review {
  id: number;
  name: string;
  rating: number;
  comment: string;
  initial: string;
  createdDate: string;
}

interface BookDetailsView {
  id: number;
  title: string;
  author: string;
  rating: number;
  reviewsCount: number;
  price: number;
  originalPrice: number;
  description: string;
  image: string;
  isbn: string;
  stock: number;
  isOutOfStock?: boolean;
}

@Component({
  selector: 'app-book-details',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './book-details.html',
  styleUrl: './book-details.scss',
})
export class BookDetails implements OnInit {
  private route = inject(ActivatedRoute);
  private bookService = inject(BookService);
  private feedbackService = inject(FeedbackService);
  private cartService = inject(CartService);
  private wishlistService = inject(WishlistService);

  bookId = signal<number>(0);
  book = signal<BookDetailsView>({
    id: 0,
    title: '',
    author: '',
    rating: 0,
    reviewsCount: 0,
    price: 0,
    originalPrice: 0,
    description: '',
    image: '',
    isbn: '',
    stock: 0,
    isOutOfStock: false
  });
  customerReviews = signal<Review[]>([]);
  loading = signal<boolean>(true);
  errorMessage = signal<string>('');
  showReviewForm = signal<boolean>(false);

  // Form data
  rating = signal<string>('5');
  comment = signal<string>('');
  submittingReview = signal<boolean>(false);

  // Computed
  customerReviewsArray = computed(() => this.customerReviews());

  ngOnInit() {
    this.route.params.subscribe(params => {
      const id = parseInt(params['id']);
      this.bookId.set(id);
      this.loadBookDetails(id);
      this.loadReviews(id);
    });
  }

  loadBookDetails(id: number) {
    this.loading.set(true);
    this.bookService.getBookById(id).subscribe({
      next: (response) => {
        if (response.success && response.data) {
          const bookDto = response.data;
          const reviewCount = this.customerReviews().length;
          this.book.set({
            id: bookDto.id,
            title: bookDto.title,
            author: bookDto.author,
            rating: 4,
            reviewsCount: reviewCount,
            price: bookDto.price,
            originalPrice: bookDto.price * 1.2,
            description: bookDto.description,
            image: bookDto.coverImage,
            isbn: bookDto.isbn,
            stock: bookDto.stock,
            isOutOfStock: bookDto.stock <= 0
          });
        }
        this.loading.set(false);
      },
      error: (err) => {
        this.errorMessage.set('Failed to load book details');
        this.loading.set(false);
      }
    });
  }

  loadReviews(bookId: number) {
    this.feedbackService.getFeedbackByBook(bookId).subscribe({
      next: (response) => {
        if (response.success && response.data) {
          this.customerReviews.set(
            response.data.map(fb => ({
              id: fb.id,
              name: fb.userName,
              rating: fb.rating,
              comment: fb.comment,
              initial: fb.userName.substring(0, 2).toUpperCase(),
              createdDate: fb.createdDate
            }))
          );
        }
      },
      error: (err) => {
        console.error('Error loading reviews:', err);
      }
    });
  }

  addToBag() {
    const bookData = this.book();
    if (!bookData) return;

    this.cartService.addItem(bookData.id, 1).subscribe({
      next: () => {
        alert('Added to cart successfully');
      },
      error: (err) => {
        console.error('Error adding to cart:', err);
        alert('Failed to add to cart');
      }
    });
  }

  addToWishlist() {
    const bookData = this.book();
    if (!bookData) return;

    this.wishlistService.addToWishlist(bookData.id).subscribe({
      next: () => {
        alert('Added to wishlist successfully');
      },
      error: (err) => {
        console.error('Error adding to wishlist:', err);
        alert('Failed to add to wishlist');
      }
    });
  }

  notifyMe() {
    alert('You will be notified when this book is back in stock');
  }

  submitReview() {
    if (!this.comment().trim()) {
      alert('Please enter a comment');
      return;
    }

    this.submittingReview.set(true);
    const feedback: CreateFeedbackRequest = {
      rating: this.rating(),
      comment: this.comment()
    };

    this.feedbackService.addFeedback(this.bookId(), feedback).subscribe({
      next: () => {
        alert('Review submitted successfully');
        this.rating.set('5');
        this.comment.set('');
        this.showReviewForm.set(false);
        this.loadReviews(this.bookId());
        this.submittingReview.set(false);
      },
      error: (err) => {
        console.error('Error submitting review:', err);
        alert('Failed to submit review');
        this.submittingReview.set(false);
      }
    });
  }

  toggleReviewForm() {
    this.showReviewForm.update(v => !v);
  }

  getRatingArray(count: number): number[] {
    return Array(count).fill(0).map((_, i) => i + 1);
  }
}
