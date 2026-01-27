import { Component, signal, inject, OnInit, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { WishlistService, WishlistItem } from '../../Services/wishlist.service';
import { AuthService } from '../../Services/auth.service';

interface WishlistDisplayItem {
  id: number;
  bookId: number;
  title: string;
  author: string;
  image: string;
  price: number;
  originalPrice: number;
}

@Component({
    selector: 'app-wishlist',
    standalone: true,
    imports: [CommonModule],
    templateUrl: './wishlist.html',
    styleUrl: './wishlist.scss',
})
export class Wishlist implements OnInit {
    private wishlistService = inject(WishlistService);
    private authService = inject(AuthService);

    isLoggedIn = signal<boolean>(false);
    wishlistItems = signal<WishlistDisplayItem[]>([]);
    loading = signal<boolean>(true);
    errorMessage = signal<string>('');

    itemsCount = computed(() => this.wishlistItems().length);

    ngOnInit() {
        this.isLoggedIn.set(this.authService.isLoggedIn());
        if (this.isLoggedIn()) {
            this.loadWishlist();
        } else {
            this.loading.set(false);
        }
    }

    loadWishlist() {
        this.loading.set(true);
        this.errorMessage.set('');

        this.wishlistService.getWishlist().subscribe({
            next: (response) => {
                if (response.success && response.data) {
                    this.wishlistItems.set(
                        response.data.map(item => ({
                            id: item.id,
                            bookId: item.bookId,
                            title: item.bookTitle,
                            author: item.bookAuthor || 'Unknown Author',
                            image: item.coverImage || 'https://via.placeholder.com/200',
                            price: item.price,
                            originalPrice: item.price * 1.2
                        }))
                    );
                } else {
                    this.errorMessage.set(response.message || 'Failed to load wishlist');
                }
                this.loading.set(false);
            },
            error: (err) => {
                this.errorMessage.set('Failed to load wishlist. Please try again.');
                console.error('Error loading wishlist:', err);
                this.loading.set(false);
            }
        });
    }

    removeFromWishlist(bookId: number) {
        this.wishlistService.removeFromWishlist(bookId).subscribe({
            next: () => {
                this.wishlistItems.update(items => items.filter(item => item.bookId !== bookId));
            },
            error: (err) => {
                console.error('Error removing from wishlist:', err);
                alert('Failed to remove item from wishlist');
            }
        });
    }
}
