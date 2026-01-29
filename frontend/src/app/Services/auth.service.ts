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

  verifyEmail(token: string): Observable<{ message: string }> {
    return this.http.post<{ message: string }>(API_ENDPOINTS.AUTH.VERIFY_EMAIL(token), {});
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
    // Only evaluate login status in the browser (localStorage + JWT validation)
    if (!isPlatformBrowser(this.platformId)) {
      return false;
    }

    const token = this.currentUserSubject.value?.token || this.authToken;
    if (!token) {
      return false;
    }

    if (!this.isTokenValid(token)) {
      this.logout();
      return false;
    }

    return true;
  }

  isAdmin(): boolean {
    const role = this.getUserRole();
    return role === 'Admin';
  }

  getUserRole(): string | null {
    // First try to get role directly from in-memory user (immediate availability after login)
    const currentUser = this.currentUserSubject.value;
    if (currentUser?.role) {
      return currentUser.role;
    }

    // Fallback to decoding token
    const token = currentUser?.token || this.authToken;
    if (!token) return null;

    const decoded = this.decodeToken(token);
    return decoded ? decoded.role || decoded.Role || decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] : null;
  }

  private decodeToken(token: string): any {
    if (!isPlatformBrowser(this.platformId)) {
      return null;
    }
    try {
      const payload = token.split('.')[1];
      if (!payload) return null;
      return JSON.parse(atob(payload));
    } catch (e) {
      console.error('Error decoding token', e);
      return null;
    }
  }

  private isTokenValid(token: string): boolean {
    const decoded = this.decodeToken(token);
    if (!decoded) return false;

    const exp = decoded.exp;
    if (!exp || typeof exp !== 'number') return false;

    const nowSeconds = Math.floor(Date.now() / 1000);
    return exp > nowSeconds;
  }
}
