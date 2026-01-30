import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiResponse } from '../Models/auth.models';
import { environment } from '../../environments/environment';

export interface BookFormData {
  id?: number;
  bookName: string;
  author: string;
  description: string;
  isbn: string;
  quantity: number;
  price: number;
  discountPrice: number;
  coverImage: string;
}

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  private apiUrl = `${environment.apiBaseUrl}/admin`;

  constructor(private http: HttpClient) { }

  getAllBooks(): Observable<ApiResponse<BookFormData[]>> {
    return this.http.get<ApiResponse<BookFormData[]>>(`${this.apiUrl}/books`);
  }

  getBookById(id: number): Observable<ApiResponse<BookFormData>> {
    return this.http.get<ApiResponse<BookFormData>>(`${this.apiUrl}/books/${id}`);
  }

  createBook(book: BookFormData): Observable<ApiResponse<BookFormData>> {
    return this.http.post<ApiResponse<BookFormData>>(`${this.apiUrl}/books`, book);
  }

  updateBook(id: number, book: BookFormData): Observable<ApiResponse<BookFormData>> {
    return this.http.put<ApiResponse<BookFormData>>(`${this.apiUrl}/books/${id}`, { ...book, id });
  }

  deleteBook(id: number): Observable<ApiResponse<boolean>> {
    return this.http.delete<ApiResponse<boolean>>(`${this.apiUrl}/books/${id}`);
  }
}
