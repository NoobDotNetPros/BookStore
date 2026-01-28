import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { API_ENDPOINTS } from '../Models/api-constants';
import { BookDto } from '../Models/book.models';
import { ApiResponse } from '../Models/auth.models';

@Injectable({
  providedIn: 'root'
})
export class BookService {
  private searchQuerySubject = new BehaviorSubject<string>('');
  searchQuery$ = this.searchQuerySubject.asObservable();

  constructor(private http: HttpClient) { }

  updateSearchQuery(query: string) {
    this.searchQuerySubject.next(query);
  }

  getAllBooks(): Observable<ApiResponse<BookDto[]>> {
    return this.http.get<ApiResponse<BookDto[]>>(API_ENDPOINTS.BOOKS.GET_ALL);
  }

  getBookById(id: number): Observable<ApiResponse<BookDto>> {
    return this.http.get<ApiResponse<BookDto>>(API_ENDPOINTS.BOOKS.GET_BY_ID(id));
  }
}
