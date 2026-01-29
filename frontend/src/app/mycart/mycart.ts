import { Component, signal, computed, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { CartService } from '../Services/cart.service';
import { UserService } from '../Services/user.service';
import { OrderService } from '../Services/order.service';
import { AuthService } from '../Services/auth.service';

type ActiveSection = 'cart' | 'address' | 'summary';

interface CartItem {
  id: number;
  bookId: number;
  title: string;
  author: string;
  price: number;
  originalPrice: number;
  cover: string;
  quantity: number;
}

interface Address {
  fullName: string;
  mobileNumber: string;
  address: string;
  city: string;
  state: string;
  type: 'Home' | 'Work' | 'Other';
}

@Component({
  selector: 'app-my-cart',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './mycart.html',
  styleUrls: ['./mycart.scss']
})
export class MyCartComponent implements OnInit {
  private cartService = inject(CartService);
  private userService = inject(UserService);
  private orderService = inject(OrderService);
  private authService = inject(AuthService);
  private router = inject(Router);

  activeSection = signal<ActiveSection>('cart');
  cartItems = signal<CartItem[]>([]);
  loading = signal<boolean>(true);
  errorMessage = signal<string>('');
  isLoggedIn = signal<boolean>(false);

  // Address data
  addressData: Address = {
    fullName: '',
    mobileNumber: '',
    address: '',
    city: '',
    state: '',
    type: 'Home'
  };

  savedAddresses: Address[] = [];
  selectedAddressIndex = signal(0);

  // Computed values
  cartCount = computed(() => this.cartItems().reduce((sum, item) => sum + item.quantity, 0));

  cartTotal = computed(() =>
    this.cartItems().reduce((sum, item) => sum + (item.price * item.quantity), 0)
  );

  cartSavings = computed(() =>
    this.cartItems().reduce((sum, item) => sum + ((item.originalPrice - item.price) * item.quantity), 0)
  );

  ngOnInit() {
    this.isLoggedIn.set(this.authService.isLoggedIn());
    if (this.isLoggedIn()) {
      this.loadCart();
      this.loadUserProfile();
    } else {
      this.loading.set(false);
    }
  }

  loadCart() {
    this.loading.set(true);
    this.cartService.getCart().subscribe({
      next: (response) => {
        if (response.success && response.data) {
          const cart = response.data;
          this.cartItems.set(
            cart.items.map((item: any) => ({
              id: item.id,
              bookId: item.bookId,
              title: item.bookTitle || item.bookName || item.book?.bookName || 'Unknown',
              author: item.bookAuthor || item.book?.author || 'Unknown Author',
              price: item.price || item.bookPrice || 0,
              originalPrice: (item.price || item.bookPrice || 0) * 1.2,
              cover: item.bookCoverImage || item.book?.coverImage || 'https://via.placeholder.com/200',
              quantity: item.quantity
            }))
          );
        }
        this.loading.set(false);
      },
      error: (err) => {
        this.errorMessage.set('Failed to load cart');
        console.error('Error loading cart:', err);
        this.loading.set(false);
      }
    });
  }

  loadUserProfile() {
    this.userService.getUserProfile().subscribe({
      next: (response) => {
        if (response.data) {
          const user = response.data;
          this.addressData = {
            fullName: user.fullName || '',
            mobileNumber: user.phone || user.mobileNumber || '',
            address: '',
            city: '',
            state: '',
            type: 'Home'
          };

          // Load saved addresses
          if (user.addresses && Array.isArray(user.addresses)) {
            this.savedAddresses = user.addresses.map((addr: any) => ({
              fullName: user.fullName || '',
              mobileNumber: user.phone || user.mobileNumber || '',
              address: addr.fullAddress || addr.address || '',
              city: addr.city || '',
              state: addr.state || '',
              type: (addr.addressType || addr.type || 'Home') as 'Home' | 'Work' | 'Other'
            }));
          }

          // Set address data from first saved address if available
          if (this.savedAddresses.length > 0) {
            this.addressData = { ...this.savedAddresses[0] };
          }
        }
      },
      error: (err) => {
        console.error('Error loading user profile:', err);
      }
    });
  }

  // Section toggle
  toggleSection(section: ActiveSection): void {
    if (section === 'cart') this.activeSection.set('cart');
    if (section === 'address' && this.cartItems().length > 0) this.activeSection.set('address');
    if (section === 'summary' && this.activeSection() === 'address') this.activeSection.set('summary');
  }

  // Cart operations
  increaseQty(itemId: number): void {
    const items = this.cartItems();
    const index = items.findIndex(item => item.id === itemId);
    if (index !== -1 && items[index].quantity < 10) {
      const newQty = items[index].quantity + 1;
      this.cartService.updateItem(itemId, newQty).subscribe({
        next: () => {
          items[index].quantity = newQty;
          this.cartItems.set([...items]);
        },
        error: (err) => console.error('Error updating quantity:', err)
      });
    }
  }

  decreaseQty(itemId: number): void {
    const items = this.cartItems();
    const index = items.findIndex(item => item.id === itemId);
    if (index !== -1 && items[index].quantity > 1) {
      const newQty = items[index].quantity - 1;
      this.cartService.updateItem(itemId, newQty).subscribe({
        next: () => {
          items[index].quantity = newQty;
          this.cartItems.set([...items]);
        },
        error: (err) => console.error('Error updating quantity:', err)
      });
    }
  }

  removeItem(itemId: number): void {
    this.cartService.removeItem(itemId).subscribe({
      next: () => {
        const items = this.cartItems().filter(item => item.id !== itemId);
        this.cartItems.set(items);
      },
      error: (err) => console.error('Error removing item:', err)
    });
  }

  // Navigation
  placeOrder(): void {
    if (this.cartItems().length > 0) {
      this.activeSection.set('address');
    }
  }

  continueToSummary(): void {
    this.activeSection.set('summary');
  }

  checkout(): void {
    // Create order
    const orderItems = this.cartItems().map(item => ({
      Product_Id: item.bookId.toString(),
      Product_Name: item.title,
      Product_Quantity: item.quantity,
      Product_Price: item.price
    }));

    this.orderService.createOrder({ Orders: orderItems }).subscribe({
      next: () => {
        this.router.navigate(['/order-success']);
      },
      error: (err) => {
        console.error('Error creating order:', err);
        alert('Failed to create order');
      }
    });
  }

  // Select address
  selectAddress(index: number): void {
    this.selectedAddressIndex.set(index);
    this.addressData = { ...this.savedAddresses[index] };
  }

  // Get selected address
  selectedAddress(): Address {
    if (this.savedAddresses.length > 0) {
      return this.savedAddresses[this.selectedAddressIndex()] || this.addressData;
    }
    return this.addressData;
  }

  hasAddresses(): boolean {
    return this.savedAddresses.length > 0;
  }

  // Format price
  formatPrice(price: number): string {
    return price.toLocaleString('en-IN');
  }

  trackByItemId(index: number, item: CartItem): number {
    return item.id;
  }
}
