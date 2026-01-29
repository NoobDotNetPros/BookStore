import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject, of, tap, shareReplay } from 'rxjs';
import { API_ENDPOINTS } from '../Models/api-constants';
import { BookDto } from '../Models/book.models';
import { ApiResponse } from '../Models/auth.models';

@Injectable({
  providedIn: 'root'
})
export class BookService {
  private searchQuerySubject = new BehaviorSubject<string>('');
  searchQuery$ = this.searchQuerySubject.asObservable();

  // Cache for all books
  private booksCache$: Observable<ApiResponse<BookDto[]>> | null = null;
  private cachedBooks: BookDto[] = [];

  constructor(private http: HttpClient) { }

  updateSearchQuery(query: string) {
    this.searchQuerySubject.next(query);
  }

  getAllBooks(): Observable<ApiResponse<BookDto[]>> {
    // Return cached observable if available
    if (this.booksCache$) {
      return this.booksCache$;
    }

    // Create cached observable with shareReplay
    this.booksCache$ = this.http.get<ApiResponse<BookDto[]>>(API_ENDPOINTS.BOOKS.GET_ALL).pipe(
      tap(response => {
        this.cachedBooks = response.data || [];
      }),
      shareReplay(1)
    );

    return this.booksCache$;
  }

  // Get cached books synchronously (for instant search)
  getCachedBooks(): BookDto[] {
    return this.cachedBooks;
  }

  // Check if books are already cached
  hasCachedBooks(): boolean {
    return this.cachedBooks.length > 0;
  }

  // Force refresh the cache
  refreshBooks(): Observable<ApiResponse<BookDto[]>> {
    this.booksCache$ = null;
    this.cachedBooks = [];
    return this.getAllBooks();
  }

  getBookById(id: number): Observable<ApiResponse<BookDto>> {
    return this.http.get<ApiResponse<BookDto>>(API_ENDPOINTS.BOOKS.GET_BY_ID(id));
  }
}
