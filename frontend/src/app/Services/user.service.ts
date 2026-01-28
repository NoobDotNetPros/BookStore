import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_ENDPOINTS } from '../Models/api-constants';
import { ApiResponse, UserDto } from '../Models/auth.models';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  constructor(private http: HttpClient) { }

  getUserProfile(): Observable<ApiResponse<UserDto>> {
    return this.http.get<ApiResponse<UserDto>>(API_ENDPOINTS.USERS.GET_PROFILE);
  }

  updateUserProfile(user: UserDto): Observable<ApiResponse<UserDto>> {
    return this.http.put<ApiResponse<UserDto>>(API_ENDPOINTS.USERS.UPDATE_PROFILE, user);
  }
}
