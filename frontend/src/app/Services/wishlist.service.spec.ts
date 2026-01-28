import { TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { WishlistService } from './wishlist.service';

describe('WishlistService', () => {
  let service: WishlistService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        WishlistService,
        provideHttpClient(),
        provideHttpClientTesting()
      ]
    });

    service = TestBed.inject(WishlistService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should have getWishlist method', () => {
    expect(service.getWishlist).toBeDefined();
  });

  it('should have addToWishlist method', () => {
    expect(service.addToWishlist).toBeDefined();
  });

  it('should have removeFromWishlist method', () => {
    expect(service.removeFromWishlist).toBeDefined();
  });
});
