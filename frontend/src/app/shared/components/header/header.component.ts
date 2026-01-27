import { Component, output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss',
})
export class HeaderComponent {
  searchQuery = '';

  // Output events for parent components to handle navigation
  searchSubmit = output<string>();
  profileClick = output<void>();
  cartClick = output<void>();

  onSearch(): void {
    if (this.searchQuery.trim()) {
      this.searchSubmit.emit(this.searchQuery.trim());
    }
  }

  onProfileClick(): void {
    this.profileClick.emit();
  }

  onCartClick(): void {
    this.cartClick.emit();
  }
}
