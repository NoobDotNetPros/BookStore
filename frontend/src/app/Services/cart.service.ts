import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { tap } from 'rxjs/operators';
import { API_ENDPOINTS } from '../Models/api-constants';
import { ApiResponse } from '../Models/auth.models';

export interface CartItem {
  id: number;
  bookId: number;
  bookTitle: string;
  price: number;
  quantity: number;
}

export interface Cart {
  id: number;
  userId: string;
  items: CartItem[];
  totalPrice: number;
}

@Injectable({
  providedIn: 'root'
})
export class CartService {
  private cartCountSubject = new BehaviorSubject<number>(0);
  cartCount$ = this.cartCountSubject.asObservable();

  constructor(private http: HttpClient) { }

  updateCartCount(count: number) {
    this.cartCountSubject.next(count);
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
