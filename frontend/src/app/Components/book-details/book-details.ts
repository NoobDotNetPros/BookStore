import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-book-details',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './book-details.html',
  styleUrl: './book-details.scss',
})
export class BookDetails {
  bookId = signal<string>('');

  // Mock book data - in a real app this would come from a service
  book = signal({
    id: '1',
    title: "UX For Dummies",
    author: 'by Donald Chesnut',
    rating: 4.5,
    reviewsCount: 20,
    price: 1500,
    originalPrice: 2000,
    description: "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut",
    image: 'https://m.media-amazon.com/images/I/51p1kQ0C1AL.jpg',
    isOutOfStock: false // Toggle this to test "Notify Me" vs "Add to Bag"
  });

  customerReviews = [
    {
      name: 'Aniket Chile',
      rating: 3,
      comment: 'Good product. Even though the translation could have been better, Chanakya\'s neeti are thought provoking. Chanakya has written on many different topics and his writings are succinct.',
      initial: 'AC'
    },
    {
      name: 'Shweta Bodkar',
      rating: 4,
      comment: 'Good product. Even though the translation could have been better, Chanakya\'s neeti are thought provoking. Chanakya has written on many different topics and his writings are succinct.',
      initial: 'SB'
    }
  ];

  constructor(private route: ActivatedRoute) {
    this.route.params.subscribe(params => {
      this.bookId.set(params['id']);
      // Should fetch book details here
    });
  }

  addToBag() {
    console.log('Added to bag');
  }

  addToWishlist() {
    console.log('Added to wishlist');
  }

  notifyMe() {
    console.log('User will be notified');
  }
}
