
# 📚 Bookstore Backend API

**Clean Architecture | .NET 8 | CQRS | MediatR**

A scalable **Bookstore Backend API** built using **Clean Architecture**, **CQRS**, **MediatR**, and **Entity Framework Core 8**.
Designed for real-world production use with clear separation of concerns and extensibility.

---

## 🏗 Architecture Overview

This project strictly follows **Clean Architecture** principles:

```
Bookstore/
└── backend/
    ├── Bookstore.Domain/          # Core business logic (Entities, Enums)
    ├── Bookstore.Application/     # Use cases, CQRS, DTOs, Validators
    ├── Bookstore.Infrastructure/  # EF Core, Repositories, External services
    └── Bookstore.Api/             # Controllers, Middleware, Swagger
```

### 🔹 Layer Responsibilities

| Layer              | Responsibility                              |
| ------------------ | ------------------------------------------- |
| **Domain**         | Pure business rules, entities, enums        |
| **Application**    | CQRS, MediatR handlers, validation, mapping |
| **Infrastructure** | Database, repositories, external services   |
| **API**            | HTTP endpoints, auth, middleware, Swagger   |

---

## 📂 Detailed Project Structure

```
backend/
├── Bookstore.Domain/
│   ├── Common/
│   │   ├── BaseAuditableEntity.cs
│   │   └── DomainException.cs
│   ├── Entities/
│   │   ├── Book.cs
│   │   ├── User.cs
│   │   ├── CartItem.cs
│   │   ├── Order.cs
│   │   ├── OrderItem.cs
│   │   ├── Address.cs
│   │   └── Feedback.cs
│   └── Enums/
│       ├── UserRole.cs
│       └── OrderStatus.cs
│
├── Bookstore.Application/
│   ├── Common/
│   │   ├── Exceptions/
│   │   └── Models/
│   ├── Contracts/
│   │   ├── Repositories/
│   │   └── Services/
│   ├── Features/
│   │   ├── Books/
│   │   ├── Users/
│   │   └── Orders/
│   ├── Behaviors/
│   │   └── ValidationBehavior.cs
│   ├── MappingProfiles/
│   └── DependencyInjection.cs
│
├── Bookstore.Infrastructure/
│   ├── Data/
│   │   ├── ApplicationDbContext.cs
│   │   ├── Configurations/
│   │   ├── Migrations/
│   │   └── DbInitializer.cs
│   ├── Repositories/
│   ├── Services/
│   ├── Settings/
│   └── DependencyInjection.cs
│
└── Bookstore.Api/
    ├── Controllers/
    │   ├── Admin/
    │   └── User/
    ├── Middleware/
    │   └── GlobalExceptionMiddleware.cs
    ├── appsettings.json
    ├── appsettings.Development.json
    └── Program.cs
```

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

## 🧪 Testing (Planned)

```bash
dotnet test
```

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


