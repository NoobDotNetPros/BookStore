import { Component, signal, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OrderService, Order, OrderItem } from '../../Services/order.service';
import { AuthService } from '../../Services/auth.service';

interface OrderItemDisplay {
    bookId: number;
    title: string;
    image: string;
    quantity: number;
    price: number;
}

interface OrderDisplay {
    id: number;
    orderDate: string;
    status: string;
    totalAmount: number;
    items: OrderItemDisplay[];
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

    getImageUrl(imageUrl: string): string {
        if (!imageUrl) {
            return 'https://via.placeholder.com/200';
        }
        if (!imageUrl.startsWith('http')) {
            return `http://localhost:5000${imageUrl.startsWith('/') ? '' : '/'}${imageUrl}`;
        }
        return imageUrl;
    }

    loadOrders() {
        this.loading.set(true);
        this.errorMessage.set('');

        this.orderService.getUserOrders().subscribe({
            next: (response) => {
                if (response.success && response.data) {
                    this.orders.set(
                        response.data.map(order => {
                            // Parse date safely
                            let orderDate = 'Unknown Date';
                            if (order.createdDate) {
                                const date = new Date(order.createdDate);
                                if (!isNaN(date.getTime())) {
                                    orderDate = date.toLocaleDateString('en-US', { month: 'long', day: 'numeric' });
                                }
                            }

                            return {
                                id: order.id,
                                orderDate: orderDate,
                                status: order.status,
                                totalAmount: order.totalAmount,
                                items: order.items.map(item => ({
                                    bookId: item.bookId,
                                    title: item.productName || 'Unknown Product',
                                    image: this.getImageUrl(item.bookCoverImage),
                                    quantity: item.quantity,
                                    price: item.price
                                }))
                            };
                        })
                    );
                }
                this.loading.set(false);
            },
            error: (err: any) => {
                this.errorMessage.set('Failed to load orders. Please try again.');
                console.error('Error loading orders:', err);
                this.loading.set(false);
            }
        });
    }
}
