# 🧪 Unit Test Report – BookStore Application

## ✅ Summary

| Layer | Test Files | Tests | Status |
|------|-----------|-------|--------|
| Backend (.NET / NUnit) | 6 | 45 | ✅ |
| Frontend (Angular / Jest) | 7 | 26 | ✅ |
| **Total** | **13** | **71** | ✅ All Passing |

---

## 🔧 Backend Tests (NUnit)

### PasswordHasherTests
- Validates password hashing & verification
- Ensures salt randomness, Unicode & special character support
- Handles null, empty & malformed inputs  
✅ **10 tests passed**

### JwtServiceTests
- JWT generation & validation
- Verifies claims (UserId, Email, Role)
- Issuer, audience & expiration validation
- Detects invalid & tampered tokens  
✅ **18 tests passed**

### EmailServiceTests
- Service initialization
- Configuration validation  
✅ **2 tests passed**

### CreateBookCommandValidatorTests
- Book name, author, price & quantity validation
- Discount price rule enforcement  
✅ **6 tests passed**

### RegisterUserCommandValidatorTests
- Full name, email, password & phone validation  
✅ **6 tests passed**

### ValidationBehaviorTests
- MediatR pipeline validation behavior
- Ensures invalid requests throw exceptions  
✅ **3 tests passed**

---

## 🎨 Frontend Tests (Angular – Jest)

### Services
- **AuthService**: login state, token handling, logout
- **BookService**: book retrieval methods
- **CartService**: cart CRUD operations
- **WishlistService**: wishlist operations  
✅ **15 tests passed**

### Components
- **LoginComponent**: initialization & password toggle
- **SignupComponent**: field validation & password visibility
- **AppComponent**: application bootstrap  
✅ **11 tests passed**

---

## 📈 Test Coverage Overview

| Area | Coverage |
|-----|----------|
| Security | Password hashing, JWT, authentication |
| Validation | Command & input validation |
| Services | Business logic & HTTP services |
| Components | UI behavior & interactions |
| Infrastructure | Pipeline & service configuration |

---

### ✅ Result
All **71 unit tests** passed successfully, ensuring **security, correctness, and stability** across backend and frontend layers.
