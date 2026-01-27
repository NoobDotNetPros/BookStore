import { CommonModule } from '@angular/common';
import { Component, signal, computed } from '@angular/core';
import { FormsModule } from '@angular/forms';

type ActiveSection = 'cart' | 'address' | 'summary';

interface CartItem {
  id: string;
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
  imports: [CommonModule, FormsModule],
  templateUrl: './mycart.html',
  styleUrls: ['./mycart.scss']
})
export class MyCartComponent {
  activeSection = signal<ActiveSection>('cart');

  // Cart items
  cartItems = signal<CartItem[]>([
    {
      id: 'book-1',
      title: "Don't Make Me Think",
      author: 'by Steve Krug',
      price: 1500,
      originalPrice: 2000,
      cover: 'https://images-na.ssl-images-amazon.com/images/I/41SH-SvWPxL._SX430_BO1,204,203,200_.jpg',
      quantity: 1
    }
  ]);

  // Address data
  addressData: Address = {
    fullName: 'Poonam Yadav',
    mobileNumber: '81678954778',
    address: 'BridgeLabz Solutions LLP, No. 42, 14th Main, 15th Cross, Sector 4, HSR Layout, Bangalore',
    city: 'Bengaluru',
    state: 'Karnataka',
    type: 'Work'
  };

  // Saved addresses list
  savedAddresses: Address[] = [
    {
      fullName: 'Poonam Yadav',
      mobileNumber: '81678954778',
      address: 'BridgeLabz Solutions LLP, No. 42, 14th Main, 15th Cross, Sector 4, Opp to BDA complex, near Kumarakom restaurant, HSR Layout, Bangalore',
      city: 'Bengaluru',
      state: 'Karnataka',
      type: 'Work'
    },
    {
      fullName: 'Poonam Yadav',
      mobileNumber: '81678954778',
      address: 'BridgeLabz Solutions LLP, No. 42, 14th Main, 15th Cross, Sector 4, Opp to BDA complex, near Kumarakom restaurant, HSR Layout, Bangalore',
      city: 'Bengaluru',
      state: 'Karnataka',
      type: 'Home'
    }
  ];

  selectedAddressIndex = signal(0);

  // Computed values
  cartCount = computed(() => this.cartItems().reduce((sum, item) => sum + item.quantity, 0));
  
  cartTotal = computed(() => 
    this.cartItems().reduce((sum, item) => sum + (item.price * item.quantity), 0)
  );

  cartSavings = computed(() => 
    this.cartItems().reduce((sum, item) => sum + ((item.originalPrice - item.price) * item.quantity), 0)
  );

  // Section toggle - clicking section header opens that section
  toggleSection(section: ActiveSection): void {
    this.activeSection.set(section);
  }

  // Cart operations
  increaseQty(itemId: string): void {
    const items = this.cartItems();
    const index = items.findIndex(item => item.id === itemId);
    if (index !== -1 && items[index].quantity < 10) {
      items[index].quantity++;
      this.cartItems.set([...items]);
    }
  }

  decreaseQty(itemId: string): void {
    const items = this.cartItems();
    const index = items.findIndex(item => item.id === itemId);
    if (index !== -1 && items[index].quantity > 1) {
      items[index].quantity--;
      this.cartItems.set([...items]);
    }
  }

  removeItem(itemId: string): void {
    const items = this.cartItems().filter(item => item.id !== itemId);
    this.cartItems.set(items);
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
    alert('Order placed successfully!');
  }

  // Select address
  selectAddress(index: number): void {
    this.selectedAddressIndex.set(index);
    this.addressData = { ...this.savedAddresses[index] };
  }

  // Get selected address
  selectedAddress(): Address {
    return this.savedAddresses[this.selectedAddressIndex()];
  }

  // Format price
  formatPrice(price: number): string {
    return price.toLocaleString('en-IN');
  }

  trackByItemId(index: number, item: CartItem): string {
    return item.id;
  }
}
