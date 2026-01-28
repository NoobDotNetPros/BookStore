import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_ENDPOINTS } from '../Models/api-constants';
import { ApiResponse } from '../Models/auth.models';

export interface WishlistItem {
  id: number;
  bookId: number;
  bookTitle: string;
  coverImage: string;
  price: number;
}

@Injectable({
  providedIn: 'root'
})
export class WishlistService {
  constructor(private http: HttpClient) { }

  getWishlist(): Observable<ApiResponse<WishlistItem[]>> {
    return this.http.get<ApiResponse<WishlistItem[]>>(API_ENDPOINTS.WISHLIST.GET_ALL);
  }

  addToWishlist(bookId: number): Observable<ApiResponse<WishlistItem>> {
    return this.http.post<ApiResponse<WishlistItem>>(API_ENDPOINTS.WISHLIST.ADD_ITEM(bookId), {});
  }

  removeFromWishlist(bookId: number): Observable<ApiResponse<boolean>> {
    return this.http.delete<ApiResponse<boolean>>(API_ENDPOINTS.WISHLIST.REMOVE_ITEM(bookId));
  }
}
