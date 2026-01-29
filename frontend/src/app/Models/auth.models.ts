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

// LoginResponse matches backend LoginResponseDto (flat structure)
export interface LoginResponse {
  token: string;
  userId: number;
  email: string;
  fullName: string;
  role: string;
}

export interface UserDto {
  id: string;
  fullName: string;
  email: string;
  mobileNumber: string;
}

// Backend returns { message, data } format (no success field)
export interface ApiResponse<T> {
  success?: boolean;
  message: string;
  data?: T;
  errors?: string[];
}
