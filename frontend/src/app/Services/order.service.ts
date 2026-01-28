import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../Models/api-constants';

export interface OrderItemHistory {
  bookId: number;
  bookName: string;
  author: string;
  quantity: number;
  price: number;
  coverImage: string;
}

export interface OrderHistory {
  orderId: number;
  orderDate: string;
  totalAmount: number;
  status: string;
  items: OrderItemHistory[];
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

  getOrderHistory(): Observable<ApiResponse<OrderHistory[]>> {
    return this.http.get<ApiResponse<OrderHistory[]>>(`${API_BASE_URL}/order-history`);
  }

  getUserOrders(): Observable<ApiResponse<any[]>> {
    return this.http.get<ApiResponse<any[]>>(`${this.apiUrl}`);
  }

  getOrderById(orderId: number): Observable<ApiResponse<any>> {
    return this.http.get<ApiResponse<any>>(`${this.apiUrl}/${orderId}`);
  }

  createOrder(orderData: any): Observable<ApiResponse<any>> {
    return this.http.post<ApiResponse<any>>(this.apiUrl, orderData);
  }

  cancelOrder(orderId: number): Observable<ApiResponse<void>> {
    return this.http.delete<ApiResponse<void>>(`${this.apiUrl}/${orderId}`);
  }
}
