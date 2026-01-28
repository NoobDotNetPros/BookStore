import { TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { CartService } from './cart.service';

describe('CartService', () => {
  let service: CartService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        CartService,
        provideHttpClient(),
        provideHttpClientTesting()
      ]
    });

    service = TestBed.inject(CartService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should have getCart method', () => {
    expect(service.getCart).toBeDefined();
  });

  it('should have addItem method', () => {
    expect(service.addItem).toBeDefined();
  });

  it('should have removeItem method', () => {
    expect(service.removeItem).toBeDefined();
  });

  it('should have updateItem method', () => {
    expect(service.updateItem).toBeDefined();
  });
});
