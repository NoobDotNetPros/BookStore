import { Component, signal, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OrderService, Order, OrderItem } from '../../Services/order.service';
import { AuthService } from '../../Services/auth.service';

interface OrderDisplay {
    id: number;
    title: string;
    author: string;
    price: number;
    originalPrice: number;
    image: string;
    orderDate: string;
    status: string;
    items: OrderItem[];
}

@Component({
    selector: 'app-my-orders',
    standalone: true,
    imports: [CommonModule],
    templateUrl: './my-orders.html',
    styleUrl: './my-orders.scss',
})
export class MyOrders implements OnInit {
    private orderService = inject(OrderService);
    private authService = inject(AuthService);

    orders = signal<OrderDisplay[]>([]);
    loading = signal<boolean>(true);
    errorMessage = signal<string>('');
    isLoggedIn = signal<boolean>(false);

    ngOnInit() {
        this.isLoggedIn.set(this.authService.isLoggedIn());
        if (this.isLoggedIn()) {
            this.loadOrders();
        } else {
            this.loading.set(false);
        }
    }

    loadOrders() {
        this.loading.set(true);
        this.errorMessage.set('');

        this.orderService.getUserOrders().subscribe({
            next: (response) => {
                if (response.success && response.data) {
                    this.orders.set(
                        response.data.map(order => ({
                            id: order.id,
                            title: order.items && order.items.length > 0 ? order.items[0].productName : 'Unknown',
                            author: 'Order #' + order.id,
                            price: order.totalAmount,
                            originalPrice: order.totalAmount * 1.2,
                            image: order.items && order.items.length > 0 ? order.items[0].bookCoverImage || 'https://via.placeholder.com/200' : 'https://via.placeholder.com/200',
                            orderDate: new Date(order.createdDate).toLocaleDateString('en-US', { month: 'long', day: 'numeric' }),
                            status: order.status,
                            items: order.items
                        }))
                    );
                }
                this.loading.set(false);
            },
            error: (err) => {
                this.errorMessage.set('Failed to load orders. Please try again.');
                console.error('Error loading orders:', err);
                this.loading.set(false);
            }
        });
    }
}
