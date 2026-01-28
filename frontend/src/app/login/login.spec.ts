import { TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { provideRouter } from '@angular/router';
import { Login } from './login';
import { AuthService } from '../Services/auth.service';

describe('Login', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        AuthService,
        provideHttpClient(),
        provideHttpClientTesting(),
        provideRouter([])
      ]
    });
  });

  it('should create', () => {
    const fixture = TestBed.createComponent(Login);
    const component = fixture.componentInstance;
    expect(component).toBeTruthy();
  });

  it('should initialize with empty credentials', () => {
    const fixture = TestBed.createComponent(Login);
    const component = fixture.componentInstance;
    expect(component.email).toBe('');
    expect(component.password).toBe('');
  });

  it('should toggle password visibility', () => {
    const fixture = TestBed.createComponent(Login);
    const component = fixture.componentInstance;
    expect(component.showPassword).toBe(false);
    component.togglePassword();
    expect(component.showPassword).toBe(true);
  });
});
