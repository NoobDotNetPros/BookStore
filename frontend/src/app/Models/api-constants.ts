// API Configuration Constants
export const API_BASE_URL = 'http://localhost:5000/api';

export const API_ENDPOINTS = {
  // Auth endpoints
  AUTH: {
    LOGIN: `${API_BASE_URL}/auth/login`,
    SIGNUP: `${API_BASE_URL}/auth/signup`,
    LOGOUT: `${API_BASE_URL}/auth/logout`,
  },
  // Books endpoints
  BOOKS: {
    GET_ALL: `${API_BASE_URL}/books`,
    GET_BY_ID: (id: number) => `${API_BASE_URL}/books/${id}`,
  },
  // User endpoints
  USERS: {
    GET_PROFILE: `${API_BASE_URL}/users/profile`,
    UPDATE_PROFILE: `${API_BASE_URL}/users/profile`,
  },
  // Cart endpoints
  CART: {
    GET: `${API_BASE_URL}/cart`,
    ADD_ITEM: `${API_BASE_URL}/cart/items`,
    REMOVE_ITEM: (id: number) => `${API_BASE_URL}/cart/items/${id}`,
    UPDATE_ITEM: (id: number) => `${API_BASE_URL}/cart/items/${id}`,
  },
  // Orders endpoints
  ORDERS: {
    GET_ALL: `${API_BASE_URL}/orders`,
    GET_BY_ID: (id: number) => `${API_BASE_URL}/orders/${id}`,
    CREATE: `${API_BASE_URL}/orders`,
  },
  // Wishlist endpoints
  WISHLIST: {
    GET_ALL: `${API_BASE_URL}/wishlist`,
    ADD_ITEM: (id: number) => `${API_BASE_URL}/wishlist/items/${id}`,
    REMOVE_ITEM: (id: number) => `${API_BASE_URL}/wishlist/items/${id}`,
  },
  // Feedback endpoints
  FEEDBACK: {
    GET_BY_BOOK: (bookId: number) => `${API_BASE_URL}/feedback/books/${bookId}`,
    ADD: (bookId: number) => `${API_BASE_URL}/feedback/books/${bookId}`,
  },
};
