import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from './shared';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, HeaderComponent],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  protected readonly title = signal('frontend');

  onSearch(query: string): void {
    console.log('Search:', query);
    // TODO: Implement search navigation
  }

  onProfileClick(): void {
    console.log('Profile clicked');
    // TODO: Navigate to profile
  }

  onCartClick(): void {
    console.log('Cart clicked');
    // TODO: Navigate to cart
  }
}
