import { Component, signal } from '@angular/core';

@Component({
  selector: 'app-book-details',
  imports: [],
  templateUrl: './book-details.html',
  styleUrl: './book-details.scss',
})
export class BookDetails {
  selectedRating = signal(0);

  setRating(rating: number) {
    this.selectedRating.set(rating);
  }
}
