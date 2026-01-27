import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HeaderComponent } from '../shared/components/header/header.component';
import { FooterComponent } from '../shared/components/footer/footer.component';

interface WishlistItem {
    id: number;
    title: string;
    author: string;
    price: number;
    originalPrice: number;
    cover: string;
}

@Component({
    selector: 'app-wishlist',
    standalone: true,
    imports: [CommonModule, HeaderComponent, FooterComponent],
    templateUrl: './wishlist.html',
    styleUrls: ['./wishlist.scss']
})
export class WishlistComponent {
    // Dummy data for 2 books
    wishlistItems = signal<WishlistItem[]>([
        {
            id: 1,
            title: "Don't Make Me Think",
            author: 'by Steve Krug',
            price: 1500,
            originalPrice: 2000,
            cover: 'assets/images/DontMakeMeThink_Large.png'
        },
        {
            id: 2,
            title: 'React Material-UI',
            author: 'by Cookbook',
            price: 780,
            originalPrice: 1000,
            cover: 'assets/images/React_Large.png'
        }
    ]);

    removeFromWishlist(id: number): void {
        this.wishlistItems.update(items => items.filter(item => item.id !== id));
    }

    formatPrice(price: number): string {
        return price.toString();
    }
}
