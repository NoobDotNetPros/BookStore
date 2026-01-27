import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
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
  constructor(private http: HttpClient) { }

  getCart(): Observable<ApiResponse<Cart>> {
    return this.http.get<ApiResponse<Cart>>(API_ENDPOINTS.CART.GET);
  }

  addItem(bookId: number, quantity: number): Observable<ApiResponse<Cart>> {
    return this.http.post<ApiResponse<Cart>>(API_ENDPOINTS.CART.ADD_ITEM, { bookId, quantity });
  }

  removeItem(itemId: number): Observable<ApiResponse<Cart>> {
    return this.http.delete<ApiResponse<Cart>>(API_ENDPOINTS.CART.REMOVE_ITEM(itemId));
  }

  updateItem(itemId: number, quantity: number): Observable<ApiResponse<Cart>> {
    return this.http.put<ApiResponse<Cart>>(API_ENDPOINTS.CART.UPDATE_ITEM(itemId), { quantity });
  }
}
