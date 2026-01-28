
# 📚 Bookstore Backend API

**Clean Architecture | .NET 8 | CQRS | MediatR**

A scalable **Bookstore Backend API** built using **N-Tier Architecture**, **CQRS**, **MediatR**, and **Entity Framework Core 8**.
Designed for real-world production use with clear separation of concerns and extensibility.

---

## 📂 Detailed Project Structure


```
Bookstore.slnx
├── Bookstore.Web/                    (Presentation Layer)
│   ├── Controllers/
│   │   ├── BookController.cs
│   │   ├── UserAuthController.cs
│   │   ├── CartController.cs
│   │   ├── WishlistController.cs
│   │   ├── OrderController.cs
│   │   ├── AddressController.cs
│   │   ├── FeedbackController.cs
│   │   └── Admin/
│   │       ├── AdminBookController.cs
│   │       ├── AdminOrderController.cs
│   │       └── AdminAuthController.cs
│   ├── Middleware/
│   │   └── GlobalExceptionMiddleware.cs
│   ├── Program.cs
│   ├── appsettings.json
│   └── appsettings.Development.json
│
├── Bookstore.Business/               (Business Logic Layer)
│   ├── Interfaces/
│   │   ├── IBookRepository.cs
│   │   ├── IUserRepository.cs
│   │   ├── IOrderRepository.cs
│   │   ├── ICartRepository.cs
│   │   ├── IUnitOfWork.cs
│   │   ├── IEmailService.cs
│   │   ├── IJwtService.cs
│   │   └── IPasswordHasher.cs
│   ├── Services/
│   │   ├── PasswordHasher.cs
│   │   ├── JwtService.cs
│   │   ├── EmailService.cs
│   │   ├── ValidationBehavior.cs
│   │   ├── Books/
│   │   │   ├── Commands/
│   │   │   └── Queries/
│   │   ├── Users/
│   │   │   └── Commands/
│   │   └── MappingProfiles/
│   ├── Models/
│   │   ├── SmtpSettings.cs
│   │   ├── Result.cs
│   │   ├── ValidationException.cs
│   │   └── NotFoundException.cs
│   └── DependencyInjection.cs
│
├── Bookstore.DataAccess/             (Data Access Layer)
│   ├── Context/
│   │   ├── ApplicationDbContext.cs
│   │   ├── DbInitializer.cs
│   │   └── Configurations/
│   ├── Repositories/
│   │   ├── BookRepository.cs
│   │   ├── UserRepository.cs
│   │   ├── OrderRepository.cs
│   │   ├── CartRepository.cs
│   │   └── UnitOfWork.cs
│   └── Migrations/
│
└── Bookstore.Models/                 (Shared Models/DTOs)
    ├── Entities/
    │   ├── BaseAuditableEntity.cs
    │   ├── Book.cs
    │   ├── User.cs
    │   ├── Order.cs
    │   ├── OrderItem.cs
    │   ├── CartItem.cs
    │   ├── Address.cs
    │   ├── Feedback.cs
    │   ├── UserRole.cs
    │   └── OrderStatus.cs
    └── DTOs/
        ├── BookDto.cs
        ├── UserDto.cs
        └── LoginResponseDto.cs
```

### 🔹 Layer Responsibilities

| Layer              | Responsibility                              |
| ------------------ | ------------------------------------------- |
| **Domain**         | Pure business rules, entities, enums        |
| **Application**    | CQRS, MediatR handlers, validation, mapping |
| **Infrastructure** | Database, repositories, external services   |
| **API**            | HTTP endpoints, auth, middleware, Swagger   |

---

## 📦 NuGet Packages

### ✅ Domain

* No external dependencies (pure domain)

---

### ✅ Application

```bash
dotnet add package MediatR --version 12.2.0
dotnet add package FluentValidation --version 11.9.0
dotnet add package FluentValidation.DependencyInjectionExtensions --version 11.9.0
dotnet add package AutoMapper --version 12.0.1
dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection --version 12.0.1
```

---

### ✅ Infrastructure

```bash
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 8.0.0
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.0
dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.0
dotnet add package Microsoft.Extensions.Options.ConfigurationExtensions --version 8.0.0
dotnet add package MailKit --version 4.3.0
```

---

### ✅ API

```bash
dotnet add package Swashbuckle.AspNetCore --version 6.5.0
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version 8.0.0
dotnet add package System.IdentityModel.Tokens.Jwt --version 7.1.2
```

---

## ⚙️ Setup Instructions

### 1️⃣ Prerequisites

* .NET 8 SDK
* SQL Server / LocalDB
* Visual Studio 2022 or VS Code

---

### 2️⃣ Install EF Core Tools

```bash
dotnet tool install --global dotnet-ef --version 8.0.0
```

---

### 3️⃣ Clone & Build

```bash
git clone <your-repo-url>
cd Bookstore/backend
dotnet restore
dotnet build
```

---

### 4️⃣ Configure Database

`Bookstore.Api/appsettings.json`

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=BookstoreDb;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

---

### 5️⃣ Run Migrations

```bash
dotnet ef migrations add InitialCreate \
  --project Bookstore.Infrastructure \
  --startup-project Bookstore.Api

dotnet ef database update \
  --project Bookstore.Infrastructure \
  --startup-project Bookstore.Api
```

---

### 6️⃣ Run API

```bash
dotnet run --project Bookstore.Api
```

Swagger UI:

```
https://localhost:<port>/swagger
```

---

## 🔌 API Endpoints (Summary)

### 🔐 Admin

* Register / Login
* Add / Update / Delete Books
* View Orders

### 👤 User

* Register / Login / Verify Email
* Browse Books
* Cart & Wishlist
* Place Orders
* Feedback

---

## 🗄 Database Tables

* Users
* Books
* CartItems
* Orders
* OrderItems
* Addresses
* Feedbacks

---

## 🔐 Security Best Practices

* ❌ Never commit secrets
* ✅ Use **User Secrets** locally
* ✅ Use **Key Vault / Secrets Manager** in production

```bash
dotnet user-secrets init
dotnet user-secrets set "SmtpSettings:Password" "your-password"
```

---

## 🧪 Testing 

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


---

## 🛠 Useful Commands

```bash
dotnet clean
dotnet restore
dotnet build
dotnet run --project Bookstore.Api
dotnet ef migrations list --project Bookstore.Infrastructure --startup-project Bookstore.Api
```

---

## 🚀 Roadmap

* [ ] JWT Authentication & Authorization
* [ ] Role-based access control
* [ ] Serilog logging
* [ ] Redis caching
* [ ] Unit & Integration Tests
* [ ] Docker support
* [ ] Cloud deployment
* [ ] Frontend (Angular / React)

---

## 👨‍💻 Author

**Kotipalli Srikesh**
📧 Email: [srikesh2017@gmail.com](mailto:srikesh2017@gmail.com)
🐙 GitHub: *Add your GitHub URL*

---

## 📄 License

MIT License


