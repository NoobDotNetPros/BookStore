import { TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { provideRouter } from '@angular/router';
import { Signup } from './signup';

describe('Signup', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        provideHttpClient(),
        provideHttpClientTesting(),
        provideRouter([])
      ]
    });
  });

  it('should create', () => {
    const fixture = TestBed.createComponent(Signup);
    const component = fixture.componentInstance;
    expect(component).toBeTruthy();
  });

  it('should initialize with empty fields', () => {
    const fixture = TestBed.createComponent(Signup);
    const component = fixture.componentInstance;
    expect(component.fullName).toBe('');
    expect(component.email).toBe('');
    expect(component.password).toBe('');
    expect(component.mobileNumber).toBe('');
  });

  it('should initialize with showPassword as false', () => {
    const fixture = TestBed.createComponent(Signup);
    const component = fixture.componentInstance;
    expect(component.showPassword).toBe(false);
  });

  it('should toggle password visibility', () => {
    const fixture = TestBed.createComponent(Signup);
    const component = fixture.componentInstance;
    expect(component.showPassword).toBe(false);
    component.togglePassword();
    expect(component.showPassword).toBe(true);
    component.togglePassword();
    expect(component.showPassword).toBe(false);
  });

  it('should have fullNameError as false when not submitted', () => {
    const fixture = TestBed.createComponent(Signup);
    const component = fixture.componentInstance;
    expect(component.fullNameError).toBe(false);
  });

  it('should have emailError as false when not submitted', () => {
    const fixture = TestBed.createComponent(Signup);
    const component = fixture.componentInstance;
    expect(component.emailError).toBe(false);
  });
});
