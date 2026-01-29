import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_ENDPOINTS } from '../Models/api-constants';
import { ApiResponse } from '../Models/auth.models';

// Extended user profile type to match backend response
export interface UserProfileData {
  id?: number | string;
  fullName: string;
  email: string;
  phone?: string;
  mobileNumber?: string;
  addresses?: AddressData[];
}

export interface AddressData {
  id?: number;
  addressType?: string;
  type?: string;
  fullAddress?: string;
  address?: string;
  city?: string;
  state?: string;
}

export interface UpdateProfileRequest {
  fullName: string;
  email: string;
  phone: string;
}

@Injectable({
  providedIn: 'root'
})
export class UserService {
  constructor(private http: HttpClient) { }

  getUserProfile(): Observable<ApiResponse<UserProfileData>> {
    return this.http.get<ApiResponse<UserProfileData>>(API_ENDPOINTS.USERS.GET_PROFILE);
  }

  updateUserProfile(user: UpdateProfileRequest | any): Observable<ApiResponse<UserProfileData>> {
    return this.http.patch<ApiResponse<UserProfileData>>(API_ENDPOINTS.USERS.UPDATE_PROFILE, user);
  }
}
