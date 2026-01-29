import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../Models/api-constants';

export interface OrderItem {
  id: number;
  orderId: number;
  bookId: number;
  productName: string;
  bookCoverImage: string;
  quantity: number;
  price: number;
  createdAt?: string;
  updatedAt?: string;
}

export interface Order {
  id: number;
  userId: number;
  status: string;
  totalAmount: number;
  shippingAddress: string;
  items: OrderItem[];
  createdDate: string;
  createdAt?: string;
  updatedAt?: string;
}

export interface ApiResponse<T> {
  success: boolean;
  message?: string;
  data?: T;
}

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  private http = inject(HttpClient);
  private apiUrl = `${API_BASE_URL}/orders`;

  getUserOrders(): Observable<ApiResponse<Order[]>> {
    return this.http.get<ApiResponse<Order[]>>(this.apiUrl);
  }

  getOrderById(orderId: number): Observable<ApiResponse<Order>> {
    return this.http.get<ApiResponse<Order>>(`${this.apiUrl}/${orderId}`);
  }

  createOrder(orderData: any): Observable<ApiResponse<Order>> {
    return this.http.post<ApiResponse<Order>>(this.apiUrl, orderData);
  }

  cancelOrder(orderId: number): Observable<ApiResponse<void>> {
    return this.http.delete<ApiResponse<void>>(`${this.apiUrl}/${orderId}`);
  }
}
