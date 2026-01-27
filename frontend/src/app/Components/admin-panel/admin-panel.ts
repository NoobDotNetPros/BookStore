import { Component, signal, computed, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AdminService, BookFormData } from '../../Services/admin.service';

@Component({
  selector: 'app-admin-panel',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './admin-panel.html',
  styleUrl: './admin-panel.scss'
})
export class AdminPanelComponent implements OnInit {
  private adminService = inject(AdminService);

  books = signal<BookFormData[]>([]);
  loading = signal<boolean>(true);
  errorMessage = signal<string>('');
  showForm = signal<boolean>(false);
  editingId = signal<number | null>(null);

  formData = signal<BookFormData>({
    bookName: '',
    author: '',
    description: '',
    isbn: '',
    quantity: 0,
    price: 0,
    discountPrice: 0,
    coverImage: ''
  });

  totalBooks = computed(() => this.books().length);
  totalValue = computed(() =>
    this.books().reduce((sum, book) => sum + (book.price * book.quantity), 0)
  );

  ngOnInit() {
    this.loadBooks();
  }

  loadBooks() {
    this.loading.set(true);
    this.errorMessage.set('');

    this.adminService.getAllBooks().subscribe({
      next: (response) => {
        if (response.success && response.data) {
          this.books.set(response.data);
        } else {
          this.errorMessage.set('Failed to load books');
        }
        this.loading.set(false);
      },
      error: (err) => {
        this.errorMessage.set('Error loading books');
        console.error('Error:', err);
        this.loading.set(false);
      }
    });
  }

  openNewForm() {
    this.editingId.set(null);
    this.formData.set({
      bookName: '',
      author: '',
      description: '',
      isbn: '',
      quantity: 0,
      price: 0,
      discountPrice: 0,
      coverImage: ''
    });
    this.showForm.set(true);
  }

  editBook(book: BookFormData) {
    this.editingId.set(book.id!);
    this.formData.set({ ...book });
    this.showForm.set(true);
  }

  closeForm() {
    this.showForm.set(false);
  }

  saveBook() {
    const form = this.formData();

    if (!form.bookName || !form.author || !form.isbn) {
      this.errorMessage.set('Please fill in all required fields');
      return;
    }

    if (this.editingId()) {
      this.adminService.updateBook(this.editingId()!, form).subscribe({
        next: () => {
          this.loadBooks();
          this.closeForm();
          alert('Book updated successfully');
        },
        error: (err) => {
          this.errorMessage.set('Failed to update book');
          console.error('Error:', err);
        }
      });
    } else {
      this.adminService.createBook(form).subscribe({
        next: () => {
          this.loadBooks();
          this.closeForm();
          alert('Book created successfully');
        },
        error: (err) => {
          this.errorMessage.set('Failed to create book');
          console.error('Error:', err);
        }
      });
    }
  }

  deleteBook(book: BookFormData) {
    if (confirm(`Are you sure you want to delete "${book.bookName}"?`)) {
      this.adminService.deleteBook(book.id!).subscribe({
        next: () => {
          this.loadBooks();
          alert('Book deleted successfully');
        },
        error: (err) => {
          this.errorMessage.set('Failed to delete book');
          console.error('Error:', err);
        }
      });
    }
  }

  updateFormField(field: string, value: any) {
    const current = this.formData();
    this.formData.set({ ...current, [field]: value });
  }
}
