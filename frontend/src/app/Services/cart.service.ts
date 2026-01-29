import { Injectable, PLATFORM_ID, inject } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject, of } from 'rxjs';
import { tap, switchMap } from 'rxjs/operators';
import { API_ENDPOINTS } from '../Models/api-constants';
import { ApiResponse } from '../Models/auth.models';

export interface CartItem {
  id: number;
  bookId: number;
  bookTitle: string;
  price: number;
  quantity: number;
  bookCoverImage?: string;
  bookAuthor?: string;
}

export interface Cart {
  id: number;
  userId: string;
  items: CartItem[];
  totalPrice: number;
}

// Local cart item for guest users
export interface LocalCartItem {
  bookId: number;
  bookTitle: string;
  bookAuthor: string;
  bookCoverImage: string;
  price: number;
  quantity: number;
}

@Injectable({
  providedIn: 'root'
})
export class CartService {
  private cartCountSubject = new BehaviorSubject<number>(0);
  cartCount$ = this.cartCountSubject.asObservable();

  private localCartKey = 'guestCart';
  private platformId = inject(PLATFORM_ID);

  constructor(private http: HttpClient) {
    // Initialize cart count from local storage for guest users
    this.initializeCartCount();
  }

  private initializeCartCount(): void {
    if (isPlatformBrowser(this.platformId)) {
      const localCart = this.getLocalCart();
      this.updateCartCount(localCart.length);
    }
  }

  updateCartCount(count: number) {
    this.cartCountSubject.next(count);
  }

  // Local storage methods for guest cart
  getLocalCart(): LocalCartItem[] {
    if (!isPlatformBrowser(this.platformId)) return [];
    const cart = localStorage.getItem(this.localCartKey);
    return cart ? JSON.parse(cart) : [];
  }

  private saveLocalCart(cart: LocalCartItem[]): void {
    if (isPlatformBrowser(this.platformId)) {
      localStorage.setItem(this.localCartKey, JSON.stringify(cart));
      this.updateCartCount(cart.length);
    }
  }

  clearLocalCart(): void {
    if (isPlatformBrowser(this.platformId)) {
      localStorage.removeItem(this.localCartKey);
      this.updateCartCount(0);
    }
  }

  addToLocalCart(item: LocalCartItem): void {
    const cart = this.getLocalCart();
    const existingIndex = cart.findIndex(i => i.bookId === item.bookId);

    if (existingIndex >= 0) {
      cart[existingIndex].quantity += item.quantity;
    } else {
      cart.push(item);
    }

    this.saveLocalCart(cart);
  }

  removeFromLocalCart(bookId: number): void {
    const cart = this.getLocalCart().filter(item => item.bookId !== bookId);
    this.saveLocalCart(cart);
  }

  updateLocalCartItem(bookId: number, quantity: number): void {
    const cart = this.getLocalCart();
    const index = cart.findIndex(item => item.bookId === bookId);
    if (index >= 0) {
      cart[index].quantity = quantity;
      this.saveLocalCart(cart);
    }
  }

  // Sync local cart with server after login
  syncLocalCartWithServer(): Observable<ApiResponse<Cart> | null> {
    const localCart = this.getLocalCart();
    if (localCart.length === 0) {
      return of(null);
    }

    // Add each local cart item to server cart
    const addRequests = localCart.map(item =>
      this.http.post<ApiResponse<Cart>>(API_ENDPOINTS.CART.ADD_ITEM, {
        bookId: item.bookId,
        quantity: item.quantity
      })
    );

    // Execute all requests and clear local cart
    if (addRequests.length > 0) {
      return addRequests[addRequests.length - 1].pipe(
        tap(() => this.clearLocalCart())
      );
    }

    return of(null);
  }

  getCart(): Observable<ApiResponse<Cart>> {
    return this.http.get<ApiResponse<Cart>>(API_ENDPOINTS.CART.GET).pipe(
      tap(response => {
        if (response.success && response.data) {
          this.updateCartCount(response.data.items.length);
        }
      })
    );
  }

  addItem(bookId: number, quantity: number): Observable<ApiResponse<Cart>> {
    return this.http.post<ApiResponse<Cart>>(API_ENDPOINTS.CART.ADD_ITEM, { bookId, quantity }).pipe(
      tap(response => {
        if (response.success && response.data) {
          this.updateCartCount(response.data.items.length);
        }
      })
    );
  }

  removeItem(itemId: number): Observable<ApiResponse<Cart>> {
    return this.http.delete<ApiResponse<Cart>>(API_ENDPOINTS.CART.REMOVE_ITEM(itemId)).pipe(
      tap(response => {
        if (response.success && response.data) {
          this.updateCartCount(response.data.items.length);
        }
      })
    );
  }

  updateItem(itemId: number, quantity: number): Observable<ApiResponse<Cart>> {
    return this.http.put<ApiResponse<Cart>>(API_ENDPOINTS.CART.UPDATE_ITEM(itemId), { quantity }).pipe(
      tap(response => {
        if (response.success && response.data) {
          this.updateCartCount(response.data.items.length);
        }
      })
    );
  }
}
