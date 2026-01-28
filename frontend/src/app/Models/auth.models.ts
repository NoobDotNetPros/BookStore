export interface LoginRequest {
  email: string;
  password: string;
}

export interface SignupRequest {
  fullName: string;
  email: string;
  password: string;
  phone: string;
}

export interface LoginResponse {
  token: string;
  user: UserDto;
}

export interface UserDto {
  id: string;
  fullName: string;
  email: string;
  mobileNumber: string;
}

export interface ApiResponse<T> {
  success: boolean;
  message: string;
  data?: T;
  errors?: string[];
}
