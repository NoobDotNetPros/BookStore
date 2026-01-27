import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_ENDPOINTS } from '../Models/api-constants';
import { ApiResponse } from '../Models/auth.models';

export interface Order {
  id: number;
  userId: string;
  status: string;
  totalAmount: number;
  shippingAddress: string;
  createdDate: string;
  items: OrderItem[];
}

export interface OrderItem {
  id: number;
  orderId: number;
  bookId: number;
  bookTitle: string;
  quantity: number;
  price: number;
}

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  constructor(private http: HttpClient) { }

  getUserOrders(): Observable<ApiResponse<Order[]>> {
    return this.http.get<ApiResponse<Order[]>>(API_ENDPOINTS.ORDERS.GET_ALL);
  }

  getOrderById(id: number): Observable<ApiResponse<Order>> {
    return this.http.get<ApiResponse<Order>>(API_ENDPOINTS.ORDERS.GET_BY_ID(id));
  }

  createOrder(order: any): Observable<ApiResponse<Order>> {
    return this.http.post<ApiResponse<Order>>(API_ENDPOINTS.ORDERS.CREATE, order);
  }
}
