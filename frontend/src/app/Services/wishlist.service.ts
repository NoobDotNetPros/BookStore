import { Injectable, PLATFORM_ID, inject } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Observable, of, BehaviorSubject } from 'rxjs';
import { tap } from 'rxjs/operators';
import { API_ENDPOINTS } from '../Models/api-constants';
import { ApiResponse } from '../Models/auth.models';

export interface WishlistItem {
  id: number;
  bookId: number;
  bookTitle: string;
  author?: string;
  coverImage: string;
  price: number;
  originalPrice?: number;
}

// Local wishlist item for guest users
export interface LocalWishlistItem {
  bookId: number;
  bookTitle: string;
  author: string;
  coverImage: string;
  price: number;
  originalPrice: number;
}

@Injectable({
  providedIn: 'root'
})
export class WishlistService {
  private localWishlistKey = 'guestWishlist';
  private platformId = inject(PLATFORM_ID);

  private wishlistCountSubject = new BehaviorSubject<number>(0);
  wishlistCount$ = this.wishlistCountSubject.asObservable();

  constructor(private http: HttpClient) {
    this.initializeWishlistCount();
  }

  private initializeWishlistCount(): void {
    if (isPlatformBrowser(this.platformId)) {
      const localWishlist = this.getLocalWishlist();
      this.wishlistCountSubject.next(localWishlist.length);
    }
  }

  updateWishlistCount(count: number): void {
    this.wishlistCountSubject.next(count);
  }

  // Local storage methods for guest wishlist
  getLocalWishlist(): LocalWishlistItem[] {
    if (!isPlatformBrowser(this.platformId)) return [];
    const wishlist = localStorage.getItem(this.localWishlistKey);
    return wishlist ? JSON.parse(wishlist) : [];
  }

  private saveLocalWishlist(wishlist: LocalWishlistItem[]): void {
    if (isPlatformBrowser(this.platformId)) {
      localStorage.setItem(this.localWishlistKey, JSON.stringify(wishlist));
      this.wishlistCountSubject.next(wishlist.length);
    }
  }

  clearLocalWishlist(): void {
    if (isPlatformBrowser(this.platformId)) {
      localStorage.removeItem(this.localWishlistKey);
      this.wishlistCountSubject.next(0);
    }
  }

  addToLocalWishlist(item: LocalWishlistItem): boolean {
    const wishlist = this.getLocalWishlist();
    const exists = wishlist.some(i => i.bookId === item.bookId);

    if (!exists) {
      wishlist.push(item);
      this.saveLocalWishlist(wishlist);
      return true;
    }
    return false;
  }

  removeFromLocalWishlist(bookId: number): void {
    const wishlist = this.getLocalWishlist().filter(item => item.bookId !== bookId);
    this.saveLocalWishlist(wishlist);
  }

  isInLocalWishlist(bookId: number): boolean {
    return this.getLocalWishlist().some(item => item.bookId === bookId);
  }

  // Sync local wishlist with server after login
  syncLocalWishlistWithServer(): Observable<ApiResponse<WishlistItem> | null> {
    const localWishlist = this.getLocalWishlist();
    if (localWishlist.length === 0) {
      return of(null);
    }

    // Add each local wishlist item to server
    const addRequests = localWishlist.map(item =>
      this.http.post<ApiResponse<WishlistItem>>(API_ENDPOINTS.WISHLIST.ADD_ITEM(item.bookId), {})
    );

    if (addRequests.length > 0) {
      return addRequests[addRequests.length - 1].pipe(
        tap(() => this.clearLocalWishlist())
      );
    }

    return of(null);
  }

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
