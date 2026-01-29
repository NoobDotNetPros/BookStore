import { Injectable, PLATFORM_ID, inject } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { API_ENDPOINTS } from '../Models/api-constants';
import { LoginRequest, SignupRequest, LoginResponse, ApiResponse } from '../Models/auth.models';

export interface ForgotPasswordResponse {
  message: string;
  email?: string;
}

export interface VerifyOtpResponse {
  message: string;
  resetToken?: string;
}

export interface ResendOtpResponse {
  message: string;
  waitTimeSeconds?: number;
}

export interface ResetPasswordRequest {
  email: string;
  resetToken: string;
  newPassword: string;
  confirmPassword: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private currentUserSubject: BehaviorSubject<LoginResponse | null>;
  public currentUser$: Observable<LoginResponse | null>;
  private tokenKey = 'authToken';
  private platformId = inject(PLATFORM_ID);

  constructor(private http: HttpClient) {
    const storedUser = this.getFromLocalStorage('currentUser');
    this.currentUserSubject = new BehaviorSubject<LoginResponse | null>(
      storedUser ? JSON.parse(storedUser) : null
    );
    this.currentUser$ = this.currentUserSubject.asObservable();
  }

  public get currentUserValue(): LoginResponse | null {
    return this.currentUserSubject.value;
  }

  public get authToken(): string | null {
    return this.getFromLocalStorage(this.tokenKey);
  }

  private getFromLocalStorage(key: string): string | null {
    if (isPlatformBrowser(this.platformId)) {
      return localStorage.getItem(key);
    }
    return null;
  }

  private setInLocalStorage(key: string, value: string): void {
    if (isPlatformBrowser(this.platformId)) {
      localStorage.setItem(key, value);
    }
  }

  private removeFromLocalStorage(key: string): void {
    if (isPlatformBrowser(this.platformId)) {
      localStorage.removeItem(key);
    }
  }

  login(email: string, password: string): Observable<ApiResponse<LoginResponse>> {
    return this.http.post<ApiResponse<LoginResponse>>(API_ENDPOINTS.AUTH.LOGIN, { email, password });
  }

  signup(fullName: string, email: string, password: string, phone: string): Observable<ApiResponse<LoginResponse>> {
    const request: SignupRequest = { fullName, email, password, phone };
    return this.http.post<ApiResponse<LoginResponse>>(API_ENDPOINTS.AUTH.SIGNUP, request);
  }

  forgotPassword(email: string): Observable<ForgotPasswordResponse> {
    return this.http.post<ForgotPasswordResponse>(API_ENDPOINTS.AUTH.FORGOT_PASSWORD, { email });
  }

  verifyOtp(email: string, otp: string): Observable<VerifyOtpResponse> {
    return this.http.post<VerifyOtpResponse>(API_ENDPOINTS.AUTH.VERIFY_OTP, { email, otp });
  }

  resendOtp(email: string): Observable<ResendOtpResponse> {
    return this.http.post<ResendOtpResponse>(API_ENDPOINTS.AUTH.RESEND_OTP, { email });
  }

  resetPassword(request: ResetPasswordRequest): Observable<{ message: string }> {
    return this.http.post<{ message: string }>(API_ENDPOINTS.AUTH.RESET_PASSWORD, request);
  }

  logout(): void {
    this.removeFromLocalStorage(this.tokenKey);
    this.removeFromLocalStorage('currentUser');
    this.currentUserSubject.next(null);
  }

  setUser(response: LoginResponse): void {
    this.setInLocalStorage(this.tokenKey, response.token);
    this.setInLocalStorage('currentUser', JSON.stringify(response));
    this.currentUserSubject.next(response);
  }

  isLoggedIn(): boolean {
    return !!this.authToken;
  }

  isAdmin(): boolean {
    const role = this.getUserRole();
    return role === 'Admin';
  }

  getUserRole(): string | null {
    const token = this.authToken;
    if (!token) return null;

    const decoded = this.decodeToken(token);
    return decoded ? decoded.role || decoded.Role || decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] : null;
  }

  private decodeToken(token: string): any {
    try {
      const payload = token.split('.')[1];
      if (!payload) return null;
      return JSON.parse(atob(payload));
    } catch (e) {
      console.error('Error decoding token', e);
      return null;
    }
  }
}
