# Bookstore API Test Project

## Overview

This project contains comprehensive NUnit tests for the Bookstore Web API backend. The tests cover all controller endpoints ensuring proper functionality, error handling, and authorization.

## Test Statistics

| Metric | Value |
|--------|-------|
| **Total Tests** | 87 |
| **Passed** | 87 |
| **Failed** | 0 |
| **Skipped** | 0 |
| **Test Duration** | ~6 seconds |

## Technology Stack

| Package | Version | Purpose |
|---------|---------|---------|
| NUnit | 3.14.0 | Testing framework |
| NUnit3TestAdapter | 4.5.0 | VS Test adapter |
| Moq | 4.20.70 | Mocking framework |
| Microsoft.NET.Test.Sdk | 17.8.0 | Test SDK |
| Microsoft.AspNetCore.Mvc.Testing | 8.0.0 | Integration testing |
| Microsoft.EntityFrameworkCore.InMemory | 8.0.0 | In-memory database |
| FluentAssertions | 6.12.0 | Assertion library |
| AutoFixture | 4.18.1 | Test data generation |
| AutoFixture.NUnit3 | 4.18.1 | AutoFixture NUnit integration |

## Project Structure

```
Bookstore.Tests/
├── Controllers/
│   ├── Admin/
│   │   └── AdminControllersTests.cs
│   ├── AddressControllerTests.cs
│   ├── AdminControllerTests.cs
│   ├── BookControllerTests.cs
│   ├── CartControllerTests.cs
│   ├── FeedbackControllerTests.cs
│   ├── HealthControllerTests.cs
│   ├── ImageUploadControllerTests.cs
│   ├── OrderControllerTests.cs
│   ├── UserAuthControllerTests.cs
│   └── WishlistControllerTests.cs
├── Fixtures/
│   └── TestFixtures.cs
├── Helpers/
│   └── MockDbSetHelper.cs
├── Bookstore.Tests.csproj
└── README.md
```

## Test Coverage by Controller

### 1. BookController Tests (4 tests)
**File:** `Controllers/BookControllerTests.cs`

| Test Method | Description | Expected Result |
|-------------|-------------|-----------------|
| `GetAllBooks_ShouldReturnOkWithBooks` | Tests retrieval of all books | Returns 200 OK with book list |
| `GetAllBooks_WithEmptyList_ShouldReturnOkWithEmptyList` | Tests empty book list scenario | Returns 200 OK with empty list |
| `GetBookById_WithValidId_ShouldReturnOkWithBook` | Tests retrieval of specific book | Returns 200 OK with book data |
| `GetBookById_WithInvalidId_ShouldReturnNotFound` | Tests non-existent book retrieval | Returns 404 Not Found |

### 2. CartController Tests (12 tests)
**File:** `Controllers/CartControllerTests.cs`

| Test Method | Description | Expected Result |
|-------------|-------------|-----------------|
| `AddToCart_WithValidBook_ShouldReturnOk` | Tests adding book to cart | Returns 200 OK |
| `AddToCart_WithNonExistentBook_ShouldReturnBadRequest` | Tests adding non-existent book | Returns 400 Bad Request |
| `AddToCart_WithoutAuth_ShouldReturnUnauthorized` | Tests unauthorized cart access | Returns 401 Unauthorized |
| `AddToCart_WithExistingItemInCart_ShouldUpdateQuantity` | Tests quantity update for existing item | Returns 200 OK |
| `UpdateQuantity_WithValidItem_ShouldReturnOk` | Tests updating cart item quantity | Returns 200 OK |
| `UpdateQuantity_WithNonExistentItem_ShouldReturnNotFound` | Tests updating non-existent item | Returns 404 Not Found |
| `UpdateQuantity_WithDifferentUserItem_ShouldReturnNotFound` | Tests updating another user's item | Returns 404 Not Found |
| `RemoveFromCart_WithValidItem_ShouldReturnOk` | Tests removing item from cart | Returns 200 OK |
| `RemoveFromCart_WithNonExistentItem_ShouldReturnNotFound` | Tests removing non-existent item | Returns 404 Not Found |
| `GetCartItems_ShouldReturnOkWithItems` | Tests retrieving cart items | Returns 200 OK with items |
| `GetCartItems_WithEmptyCart_ShouldReturnOkWithEmptyList` | Tests empty cart retrieval | Returns 200 OK with empty list |
| `GetCartItems_WithoutAuth_ShouldReturnUnauthorized` | Tests unauthorized cart retrieval | Returns 401 Unauthorized |

### 3. OrderController Tests (7 tests)
**File:** `Controllers/OrderControllerTests.cs`

| Test Method | Description | Expected Result |
|-------------|-------------|-----------------|
| `CreateOrder_WithValidRequest_ShouldReturnOk` | Tests order creation | Returns 200 OK |
| `CreateOrder_WithEmptyCart_ShouldReturnBadRequest` | Tests order with empty cart | Returns 400 Bad Request |
| `CreateOrder_WithoutAuth_ShouldReturnUnauthorized` | Tests unauthorized order creation | Returns 401 Unauthorized |
| `GetUserOrders_ShouldReturnOkWithOrders` | Tests retrieving user orders | Returns 200 OK with orders |
| `GetUserOrders_WithNoOrders_ShouldReturnOkWithEmptyList` | Tests no orders scenario | Returns 200 OK with empty list |
| `GetUserOrders_WithoutAuth_ShouldReturnUnauthorized` | Tests unauthorized orders retrieval | Returns 401 Unauthorized |
| `GetOrderById_WithValidOrder_ShouldReturnOk` | Tests retrieving specific order | Returns 200 OK |

### 4. AddressController Tests (12 tests)
**File:** `Controllers/AddressControllerTests.cs`

| Test Method | Description | Expected Result |
|-------------|-------------|-----------------|
| `GetUserProfile_WithValidUser_ShouldReturnOk` | Tests profile retrieval | Returns 200 OK |
| `GetUserProfile_WithNonExistentUser_ShouldReturnNotFound` | Tests non-existent user profile | Returns 404 Not Found |
| `GetUserProfile_WithoutAuth_ShouldReturnUnauthorized` | Tests unauthorized profile access | Returns 401 Unauthorized |
| `UpdateUserProfile_WithValidRequest_ShouldReturnOk` | Tests profile update | Returns 200 OK |
| `UpdateUserProfile_WithNonExistentUser_ShouldReturnNotFound` | Tests updating non-existent user | Returns 404 Not Found |
| `UpdateCustomerDetails_WithValidRequest_ShouldReturnOk` | Tests customer details update | Returns 200 OK |
| `UpdateCustomerDetails_WithNonExistentUser_ShouldReturnNotFound` | Tests updating non-existent customer | Returns 404 Not Found |
| `AddAddress_WithValidRequest_ShouldReturnCreated` | Tests adding new address | Returns 201 Created |
| `AddAddress_WithoutAuth_ShouldReturnUnauthorized` | Tests unauthorized address addition | Returns 401 Unauthorized |
| `UpdateAddress_WithValidRequest_ShouldReturnOk` | Tests address update | Returns 200 OK |
| `UpdateAddress_WithNonExistentAddress_ShouldReturnNotFound` | Tests updating non-existent address | Returns 404 Not Found |
| `DeleteAddress_WithValidAddress_ShouldReturnOk` | Tests address deletion | Returns 200 OK |
| `DeleteAddress_WithNonExistentAddress_ShouldReturnNotFound` | Tests deleting non-existent address | Returns 404 Not Found |

### 5. UserAuthController Tests (10 tests)
**File:** `Controllers/UserAuthControllerTests.cs`

| Test Method | Description | Expected Result |
|-------------|-------------|-----------------|
| `RegisterUser_WithValidRequest_ShouldReturnOk` | Tests user registration | Returns 200 OK |
| `RegisterUser_WithDuplicateEmail_ShouldReturnBadRequest` | Tests duplicate email registration | Returns 400 Bad Request |
| `LoginUser_WithValidCredentials_ShouldReturnOkWithToken` | Tests successful login | Returns 200 OK with JWT token |
| `LoginUser_WithInvalidCredentials_ShouldReturnUnauthorized` | Tests invalid login credentials | Returns 401 Unauthorized |
| `ForgotPassword_WithValidEmail_ShouldReturnOk` | Tests password reset request | Returns 200 OK |
| `ForgotPassword_WithInvalidEmail_ShouldReturnBadRequest` | Tests invalid email for password reset | Returns 400 Bad Request |
| `VerifyOtp_WithValidOtp_ShouldReturnOk` | Tests OTP verification | Returns 200 OK |
| `VerifyOtp_WithInvalidOtp_ShouldReturnBadRequest` | Tests invalid OTP | Returns 400 Bad Request |
| `ResendOtp_WithValidEmail_ShouldReturnOk` | Tests OTP resend | Returns 200 OK |
| `ResendOtp_WithRateLimitExceeded_ShouldReturnBadRequest` | Tests OTP rate limiting | Returns 400 Bad Request |

### 6. WishlistController Tests (10 tests)
**File:** `Controllers/WishlistControllerTests.cs`

| Test Method | Description | Expected Result |
|-------------|-------------|-----------------|
| `AddToWishlist_WithValidBook_ShouldReturnOk` | Tests adding to wishlist | Returns 200 OK |
| `AddToWishlist_WithNonExistentBook_ShouldReturnBadRequest` | Tests adding non-existent book | Returns 400 Bad Request |
| `AddToWishlist_WithExistingItemInWishlist_ShouldReturnBadRequest` | Tests duplicate wishlist item | Returns 400 Bad Request |
| `AddToWishlist_WithoutAuth_ShouldReturnUnauthorized` | Tests unauthorized wishlist access | Returns 401 Unauthorized |
| `RemoveFromWishlist_WithValidItem_ShouldReturnOk` | Tests removing from wishlist | Returns 200 OK |
| `RemoveFromWishlist_WithNonExistentItem_ShouldReturnNotFound` | Tests removing non-existent item | Returns 404 Not Found |
| `RemoveFromWishlist_WithoutAuth_ShouldReturnUnauthorized` | Tests unauthorized removal | Returns 401 Unauthorized |
| `GetWishlistItems_ShouldReturnOkWithItems` | Tests retrieving wishlist | Returns 200 OK with items |
| `GetWishlistItems_WithEmptyWishlist_ShouldReturnOkWithEmptyList` | Tests empty wishlist | Returns 200 OK with empty list |
| `GetWishlistItems_WithoutAuth_ShouldReturnUnauthorized` | Tests unauthorized wishlist retrieval | Returns 401 Unauthorized |

### 7. FeedbackController Tests (5 tests)
**File:** `Controllers/FeedbackControllerTests.cs`

| Test Method | Description | Expected Result |
|-------------|-------------|-----------------|
| `AddFeedback_WithValidRequest_ShouldReturnOk` | Tests adding feedback | Returns 200 OK |
| `AddFeedback_MultipleFeedbacks_ShouldAddAll` | Tests multiple feedbacks | All feedbacks added |
| `GetFeedbacks_WithExistingFeedbacks_ShouldReturnOkWithFeedbacks` | Tests retrieving feedbacks | Returns 200 OK with feedbacks |
| `GetFeedbacks_WithNoFeedbacks_ShouldReturnOkWithEmptyList` | Tests empty feedbacks | Returns 200 OK with empty list |
| `GetFeedbacks_WithNonExistentProduct_ShouldReturnOkWithEmptyList` | Tests non-existent product feedbacks | Returns 200 OK with empty list |

### 8. AdminController Tests (11 tests)
**File:** `Controllers/AdminControllerTests.cs`

| Test Method | Description | Expected Result |
|-------------|-------------|-----------------|
| `GetAllBooks_ShouldReturnOkWithBooks` | Tests admin book list retrieval | Returns 200 OK |
| `GetAllBooks_WithEmptyList_ShouldReturnOkWithEmptyList` | Tests empty book list | Returns 200 OK with empty list |
| `GetBookById_WithValidId_ShouldReturnOkWithBook` | Tests admin book retrieval | Returns 200 OK |
| `GetBookById_WithInvalidId_ShouldReturnNotFound` | Tests non-existent book | Returns 404 Not Found |
| `CreateBook_WithValidData_ShouldReturnCreated` | Tests book creation | Returns 201 Created |
| `CreateBook_ShouldCallRepositoryAddAsync` | Tests repository interaction | AddAsync called once |
| `UpdateBook_WithValidData_ShouldReturnOk` | Tests book update | Returns 200 OK |
| `UpdateBook_WithNonExistentBook_ShouldReturnNotFound` | Tests updating non-existent book | Returns 404 Not Found |
| `UpdateBook_WithMismatchedId_ShouldReturnBadRequest` | Tests ID mismatch validation | Returns 400 Bad Request |
| `DeleteBook_WithValidId_ShouldReturnOk` | Tests book deletion | Returns 200 OK |
| `DeleteBook_WithNonExistentBook_ShouldReturnNotFound` | Tests deleting non-existent book | Returns 404 Not Found |

### 9. ImageUploadController Tests (3 tests)
**File:** `Controllers/ImageUploadControllerTests.cs`

| Test Method | Description | Expected Result |
|-------------|-------------|-----------------|
| `UploadImage_WithNullFile_ShouldReturnBadRequest` | Tests null file upload | Returns 400 Bad Request |
| `UploadImage_WithEmptyFile_ShouldReturnBadRequest` | Tests empty file upload | Returns 400 Bad Request |
| `UploadImage_WithValidFile_ShouldCallImageKit` | Tests valid file upload flow | ImageKit service called |

### 10. HealthController Tests (4 tests)
**File:** `Controllers/HealthControllerTests.cs`

| Test Method | Description | Expected Result |
|-------------|-------------|-----------------|
| `CheckHealth_WithValidConfiguration_ShouldReturnOk` | Tests health check endpoint | Returns 200 OK |
| `CheckHealth_WithInvalidJwtKey_ShouldStillReturnOkButNotHealthy` | Tests invalid JWT config | Returns 200 OK (unhealthy status) |
| `CheckHealth_WithMissingSmtpConfig_ShouldStillReturnOk` | Tests missing SMTP config | Returns 200 OK |
| `CheckHealth_DatabaseCanConnect_ShouldShowDatabaseOnline` | Tests database connectivity | Database shows online |

### 11. Admin Subfolder Controllers Tests (9 tests)
**File:** `Controllers/Admin/AdminControllersTests.cs`

#### AdminAuthController (2 tests)
| Test Method | Description | Expected Result |
|-------------|-------------|-----------------|
| `RegisterAdmin_WithValidCommand_ShouldReturnOk` | Tests admin registration | Returns 200 OK |
| `LoginAdmin_WithValidCredentials_ShouldReturnOk` | Tests admin login | Returns 200 OK |

#### AdminBookController (4 tests)
| Test Method | Description | Expected Result |
|-------------|-------------|-----------------|
| `AddBook_WithValidCommand_ShouldReturnOk` | Tests admin book addition | Returns 200 OK |
| `UpdateBook_WithValidRequest_ShouldReturnOk` | Tests admin book update | Returns 200 OK |
| `DeleteBook_WithValidId_ShouldReturnOk` | Tests admin book deletion | Returns 200 OK |
| `AddBook_ShouldCallMediatorSend` | Tests MediatR interaction | Send called once |

#### AdminOrderController (2 tests)
| Test Method | Description | Expected Result |
|-------------|-------------|-----------------|
| `GetAllOrders_ShouldReturnOkWithOrders` | Tests admin orders retrieval | Returns 200 OK |
| `GetAllOrders_WithEmptyList_ShouldReturnOkWithEmptyList` | Tests empty orders list | Returns 200 OK with empty list |

## Testing Patterns Used

### 1. Unit Testing with Mocking
- **Moq** is used to mock repository interfaces (`IBookRepository`, `ICartRepository`, `IOrderRepository`, `IUserRepository`)
- **IUnitOfWork** is mocked for transaction handling
- **IMediator** is mocked for CQRS pattern testing

### 2. In-Memory Database Testing
- **Entity Framework Core InMemory** provider for integration-style tests
- Used in `FeedbackControllerTests` and `HealthControllerTests`
- Each test gets a fresh database instance using `Guid.NewGuid()` as database name

### 3. Authentication Mocking
- Claims-based identity is mocked for authorized endpoints
- `ClaimTypes.NameIdentifier` and `sub` claims are set up for user identification
- Tests verify both authorized and unauthorized scenarios

### 4. Test Lifecycle
- `[SetUp]` attribute for test initialization
- `[TearDown]` attribute for cleanup (database disposal)
- `[TestFixture]` for test class identification

## Running Tests

### Command Line
```bash
# Run all tests
dotnet test Bookstore.Tests/Bookstore.Tests.csproj

# Run with verbose output
dotnet test Bookstore.Tests/Bookstore.Tests.csproj --verbosity normal

# Run specific test class
dotnet test Bookstore.Tests/Bookstore.Tests.csproj --filter "FullyQualifiedName~CartControllerTests"

# Run with code coverage
dotnet test Bookstore.Tests/Bookstore.Tests.csproj --collect:"XPlat Code Coverage"
```

### Visual Studio
1. Open Test Explorer (Test → Test Explorer)
2. Click "Run All Tests" or right-click specific tests to run

### VS Code
1. Install C# Dev Kit extension
2. Use Testing panel to discover and run tests

## Test Naming Convention

Tests follow the pattern: `MethodName_Scenario_ExpectedResult`

Examples:
- `GetAllBooks_ShouldReturnOkWithBooks`
- `AddToCart_WithNonExistentBook_ShouldReturnBadRequest`
- `UpdateQuantity_WithDifferentUserItem_ShouldReturnNotFound`

## Helper Classes

### TestFixtures.cs
Provides factory methods for creating test dependencies:
- `CreateInMemoryDbContext()` - Creates EF Core in-memory context
- `CreateMockBookRepository()` - Creates mock book repository
- `CreateMockUserRepository()` - Creates mock user repository
- `CreateMockCartRepository()` - Creates mock cart repository
- `CreateMockOrderRepository()` - Creates mock order repository
- `CreateMockUnitOfWork()` - Creates mock unit of work

### MockDbSetHelper.cs
Provides utilities for mocking `DbSet<T>` with async operations:
- `TestAsyncQueryProvider<T>` - Async query provider implementation
- `TestAsyncEnumerable<T>` - Async enumerable implementation
- `TestAsyncEnumerator<T>` - Async enumerator implementation

## Best Practices Followed

1. **Arrange-Act-Assert (AAA) Pattern** - All tests follow this structure
2. **Single Responsibility** - Each test verifies one behavior
3. **Descriptive Names** - Test names clearly describe the scenario
4. **Independent Tests** - Tests don't depend on each other
5. **Mocking External Dependencies** - No actual database or external service calls
6. **Error Case Coverage** - Tests cover both success and failure scenarios
7. **Authorization Testing** - All secured endpoints test unauthorized access


