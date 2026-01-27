export interface Book {
  id: number;
  title: string;
  author: string;
  description: string;
  price: number;
  coverImage: string;
  isbn: string;
  publishedDate: string;
  category: string;
  stock: number;
}

export interface BookDto {
  id: number;
  title: string;
  author: string;
  description: string;
  price: number;
  coverImage: string;
  isbn: string;
  publishedDate: string;
  category: string;
  stock: number;
}
