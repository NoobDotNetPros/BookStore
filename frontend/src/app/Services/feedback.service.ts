import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_ENDPOINTS } from '../Models/api-constants';
import { ApiResponse } from '../Models/auth.models';

export interface Feedback {
  id: number;
  bookId: number;
  userId: string;
  rating: number;
  comment: string;
  userName: string;
  createdDate: string;
}

export interface CreateFeedbackRequest {
  rating: string;
  comment: string;
}

@Injectable({
  providedIn: 'root'
})
export class FeedbackService {
  constructor(private http: HttpClient) { }

  getFeedbackByBook(bookId: number): Observable<ApiResponse<Feedback[]>> {
    return this.http.get<ApiResponse<Feedback[]>>(API_ENDPOINTS.FEEDBACK.GET_BY_BOOK(bookId));
  }

  addFeedback(bookId: number, feedback: CreateFeedbackRequest): Observable<ApiResponse<Feedback>> {
    return this.http.post<ApiResponse<Feedback>>(API_ENDPOINTS.FEEDBACK.ADD(bookId), feedback);
  }
}
