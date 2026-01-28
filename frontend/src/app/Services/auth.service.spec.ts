import { TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { AuthService } from './auth.service';

describe('AuthService', () => {
  let service: AuthService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        AuthService,
        provideHttpClient(),
        provideHttpClientTesting()
      ]
    });

    service = TestBed.inject(AuthService);
    localStorage.clear();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should return false when not logged in', () => {
    expect(service.isLoggedIn()).toBe(false);
  });

  it('should return true when token exists', () => {
    localStorage.setItem('authToken', 'test-token');
    expect(service.isLoggedIn()).toBe(true);
  });

  it('should clear token on logout', () => {
    localStorage.setItem('authToken', 'test-token');
    service.logout();
    expect(localStorage.getItem('authToken')).toBeNull();
  });
});
