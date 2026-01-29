import { Component, signal, inject, OnInit, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { WishlistService, WishlistItem } from '../../Services/wishlist.service';
import { CartService } from '../../Services/cart.service';
import { AuthService } from '../../Services/auth.service';
import { ToastService } from '../../Services/toast.service';

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
    private cartService = inject(CartService);
    private authService = inject(AuthService);
    private toastService = inject(ToastService);
    private router = inject(Router);

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
            this.loadLocalWishlist();
        }
    }

    loadLocalWishlist() {
        const localWishlist = this.wishlistService.getLocalWishlist();
        this.wishlistItems.set(
            localWishlist.map((item, index) => ({
                id: index + 1,
                bookId: item.bookId,
                title: item.bookTitle,
                author: item.author,
                image: item.coverImage || 'https://via.placeholder.com/200',
                price: item.price,
                originalPrice: item.originalPrice
            }))
        );
        this.loading.set(false);
    }

    loadWishlist() {
        this.loading.set(true);
        this.errorMessage.set('');

        this.wishlistService.getWishlist().subscribe({
            next: (response) => {
                if (response.success && response.data) {
                    this.wishlistItems.set(
                        response.data.map((item: any) => ({
                            id: item.id,
                            bookId: item.bookId,
                            title: item.bookTitle || 'Unknown Title',
                            author: item.author || 'Unknown Author',
                            image: item.coverImage || 'https://via.placeholder.com/200',
                            price: item.price,
                            originalPrice: item.originalPrice || item.price * 1.2
                        }))
                    );
                } else {
                    this.wishlistItems.set([]);
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
        if (this.isLoggedIn()) {
            this.wishlistService.removeFromWishlist(bookId).subscribe({
                next: () => {
                    this.wishlistItems.update(items => items.filter(item => item.bookId !== bookId));
                    this.toastService.success('Item removed from wishlist');
                },
                error: (err) => {
                    console.error('Error removing from wishlist:', err);
                    this.toastService.error('Failed to remove item from wishlist');
                }
            });
        } else {
            // Guest user - remove from local storage
            this.wishlistService.removeFromLocalWishlist(bookId);
            this.wishlistItems.update(items => items.filter(item => item.bookId !== bookId));
            this.toastService.success('Item removed from wishlist');
        }
    }

    addToCart(item: WishlistDisplayItem) {
        if (this.isLoggedIn()) {
            this.cartService.addItem(item.bookId, 1).subscribe({
                next: () => {
                    this.toastService.success('Book added to cart!');
                },
                error: (err) => {
                    console.error('Error adding to cart:', err);
                    this.toastService.error('Failed to add book to cart');
                }
            });
        } else {
            // Guest user - add to local cart
            this.cartService.addToLocalCart({
                bookId: item.bookId,
                bookTitle: item.title,
                bookAuthor: item.author,
                bookCoverImage: item.image,
                price: item.price,
                quantity: 1
            });
            this.toastService.success('Book added to cart!');
        }
    }
}
