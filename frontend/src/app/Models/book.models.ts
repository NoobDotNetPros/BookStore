// Book Interface
export interface Book {
  id: number;
  bookName: string;
  title?: string; // Alias for bookName
  author: string;
  description: string;
  isbn?: string;
  quantity: number;
  stock?: number; // Alias for quantity
  price: number;
  discountPrice: number;
  coverImage?: string;
  createdAt?: string;
  updatedAt?: string;
}

// BookDto Interface
export interface BookDto {
  id: number;
  bookName: string;
  title?: string; // Alias for bookName
  author: string;
  description: string;
  isbn?: string;
  quantity: number;
  stock?: number; // Alias for quantity
  price: number;
  discountPrice: number;
  coverImage?: string;
  createdAt?: string;
  updatedAt?: string;
}

// Cart Item Interface
export interface CartItem {
  id: number;
  userId: number;
  bookId: number;
  book?: Book;
  bookName?: string;
  bookTitle?: string;
  bookAuthor?: string;
  bookCoverImage?: string;
  bookPrice?: number;
  price?: number;
  quantity: number;
  isWishlist: boolean;
  createdAt?: string;
  updatedAt?: string;
}

// Wishlist Item Interface
export interface WishlistItem {
  id: number;
  userId: number;
  bookId: number;
  bookTitle: string;
  coverImage: string;
  price: number;
  quantity: number;
  isWishlist: boolean;
  createdAt?: string;
  updatedAt?: string;
}

// Order Item Interface
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

// Order Interface
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
