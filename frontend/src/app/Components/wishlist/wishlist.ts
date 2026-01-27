import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
    selector: 'app-wishlist',
    standalone: true,
    imports: [CommonModule],
    templateUrl: './wishlist.html',
    styleUrl: './wishlist.scss',
})
export class Wishlist {
    isLoggedIn = signal(false);

    wishlistItems = signal([
        {
            id: '1',
            title: "Don't Make Me Think",
            author: 'by Steve Krug',
            price: 1500,
            originalPrice: 2000,
            image: 'https://images-na.ssl-images-amazon.com/images/I/41SH-SvWPxL._SX430_BO1,204,203,200_.jpg'
        },
        {
            id: '2',
            title: "React Material-UI",
            author: 'by Cookbook',
            price: 780,
            originalPrice: 1000,
            image: 'https://m.media-amazon.com/images/I/71UBc1jVb-L._AC_UF1000,1000_QL80_.jpg'
        }
    ]);

    itemsCount = signal(2);

    removeFromWishlist(id: string) {
        this.wishlistItems.update(items => items.filter(item => item.id !== id));
        this.itemsCount.set(this.wishlistItems().length);
        console.log(`Removed item ${id} from wishlist`);
    }
}
