# 📚 BookStore - Full Stack Online Bookstore

**Modern E-commerce Platform | .NET 8 | Angular 19 | Clean Architecture | SQL Server**

Welcome to **BookStore**, a comprehensive full-stack e-commerce application designed for book enthusiasts. This project features a robust **.NET 8 Web API** backend following **N-Tier Architecture** and a dynamic **Angular 19** frontend.

- **Frontend:** [bookstore-production-d904.up.railway.app](https://bookstore-production-d904.up.railway.app)

- **Backend (Swagger Docs):** [bookstore-production-a27c.up.railway.app/swagger](https://bookstore-production-a27c.up.railway.app/swagger)

---

## 🚀 Key Features

### 👤 User Features
- **User Authentication**: Secure Signup, Login, and Forgot Password with JWT-based authentication
- **Browse Books**: Search and explore a wide collection of books with detailed descriptions
- **Shopping Cart**: Seamlessly add, remove, and manage quantities of books in your cart
- **Wishlist**: Save your favorite books for future purchases
- **Order Management**: Secure checkout process with order history and success tracking
- **User Profile**: Manage personal details and delivery addresses
- **Feedback & Reviews**: Share your thoughts on books with a built-in feedback system

### 🛡️ Admin Features
- **Admin Dashboard**: Overview of store performance and management
- **Book Management**: Full CRUD operations (Add, Update, Delete) for the book catalog
- **Order Monitoring**: View and manage customer orders
- **User Management**: Monitor registered users and their activities

---

## 🛠️ Tech Stack

### Backend
- **Framework**: .NET 8 Web API
- **Architecture**: N-Tier / Clean Architecture (Presentation, Business, DataAccess, Models)
- **Database**: Microsoft SQL Server
- **ORM**: Entity Framework Core
- **Authentication**: JWT (JSON Web Tokens)
- **Patterns**: Repository Pattern, Unit of Work, Dependency Injection
- **Documentation**: Swagger / OpenAPI
- **Other**: ImageKit (Image Storage), SMTP (Email Notifications)

### Frontend
- **Framework**: Angular 19+
- **Styling**: SCSS, Responsive UI
- **State Management**: RxJS & Services
- **Validation**: Reactive Forms
- **Testing**: Vitest
- **Features**: SSR (Server-Side Rendering) Support

---

## 📂 Project Structure

```text
noobdotnetpros-bookstore/
├── README.md
├── backend/
│   ├── Bookstore.slnx
│   ├── Bookstore.Business/
│   │   ├── Bookstore.Business.csproj
│   │   ├── DependencyInjection.cs
│   │   ├── Interfaces/
│   │   │   ├── IBookRepository.cs
│   │   │   ├── ICartRepository.cs
│   │   │   ├── IEmailService.cs
│   │   │   ├── IJwtService.cs
│   │   │   ├── IOrderRepository.cs
│   │   │   ├── IPasswordHasher.cs
│   │   │   ├── IUnitOfWork.cs
│   │   │   └── IUserRepository.cs
│   │   ├── Models/
│   │   │   ├── NotFoundException.cs
│   │   │   ├── Result.cs
│   │   │   ├── SmtpSettings.cs
│   │   │   └── ValidationException.cs
│   │   └── Services/
│   │       ├── EmailService.cs
│   │       ├── JwtService.cs
│   │       ├── PasswordHasher.cs
│   │       ├── ValidationBehavior.cs
│   │       ├── Books/
│   │       │   ├── Commands/
│   │       │   │   ├── CreateBookCommand.cs
│   │       │   │   ├── CreateBookCommandHandler.cs
│   │       │   │   ├── CreateBookCommandValidator.cs
│   │       │   │   ├── DeleteBookCommand.cs
│   │       │   │   ├── DeleteBookCommandHandler.cs
│   │       │   │   ├── UpdateBookCommand.cs
│   │       │   │   └── UpdateBookCommandHandler.cs
│   │       │   └── Queries/
│   │       │       ├── GetBookByIdQuery.cs
│   │       │       ├── GetBookByIdQueryHandler.cs
│   │       │       ├── GetBooksQuery.cs
│   │       │       └── GetBooksQueryHandler.cs
│   │       ├── MappingProfiles/
│   │       │   ├── BookProfile.cs
│   │       │   └── UserProfile.cs
│   │       └── Users/
│   │           └── Commands/
│   │               ├── ForgotPasswordCommand.cs
│   │               ├── ForgotPasswordCommandHandler.cs
│   │               ├── ForgotPasswordCommandValidator.cs
│   │               ├── LoginCommand.cs
│   │               ├── LoginCommandHandler.cs
│   │               ├── RegisterUserCommand.cs
│   │               ├── RegisterUserCommandHandler.cs
│   │               ├── RegisterUserCommandValidator.cs
│   │               ├── ResendOtpCommand.cs
│   │               ├── ResendOtpCommandHandler.cs
│   │               ├── ResendOtpCommandValidator.cs
│   │               ├── ResetPasswordCommand.cs
│   │               ├── ResetPasswordCommandHandler.cs
│   │               ├── ResetPasswordCommandValidator.cs
│   │               ├── VerifyEmailCommand.cs
│   │               ├── VerifyEmailCommandHandler.cs
│   │               ├── VerifyOtpCommand.cs
│   │               ├── VerifyOtpCommandHandler.cs
│   │               └── VerifyOtpCommandValidator.cs
│   ├── Bookstore.DataAccess/
│   │   ├── Bookstore.DataAccess.csproj
│   │   ├── Context/
│   │   │   ├── ApplicationDbContext.cs
│   │   │   ├── DbInitializer.cs
│   │   │   └── Configurations/
│   │   │       ├── AddressConfiguration.cs
│   │   │       ├── BookConfiguration.cs
│   │   │       ├── CartItemConfiguration.cs
│   │   │       ├── FeedbackConfiguration.cs
│   │   │       ├── OrderConfiguration.cs
│   │   │       ├── OrderItemConfiguration.cs
│   │   │       └── UserConfiguration.cs
│   │   ├── Migrations/
│   │   │   ├── 20260125125603_InitialCreate.cs
│   │   │   ├── 20260125125603_InitialCreate.Designer.cs
│   │   │   ├── 20260127092406_AddBookCreatedAtDefault.cs
│   │   │   ├── 20260127092406_AddBookCreatedAtDefault.Designer.cs
│   │   │   ├── 20260128000000_AddCoverImageToBooksTable.cs
│   │   │   ├── 20260128000000_AddCoverImageToBooksTable.Designer.cs
│   │   │   ├── 20260128100000_AddPasswordResetOtpFields.cs
│   │   │   ├── 20260128100000_AddPasswordResetOtpFields.Designer.cs
│   │   │   ├── 20260128_AddCoverImageToBooks.cs
│   │   │   └── ApplicationDbContextModelSnapshot.cs
│   │   └── Repositories/
│   │       ├── BookRepository.cs
│   │       ├── CartRepository.cs
│   │       ├── OrderRepository.cs
│   │       ├── UnitOfWork.cs
│   │       └── UserRepository.cs
│   ├── Bookstore.Models/
│   │   ├── ApiResponse.cs
│   │   ├── Bookstore.Models.csproj
│   │   ├── DTOs/
│   │   │   ├── BookDto.cs
│   │   │   ├── LoginResponseDto.cs
│   │   │   └── UserDto.cs
│   │   └── Entities/
│   │       ├── Address.cs
│   │       ├── BaseAuditableEntity.cs
│   │       ├── Book.cs
│   │       ├── CartItem.cs
│   │       ├── DomainException.cs
│   │       ├── Feedback.cs
│   │       ├── Order.cs
│   │       ├── OrderItem.cs
│   │       ├── OrderStatus.cs
│   │       ├── User.cs
│   │       └── UserRole.cs
│   └── Bookstore.Web/
│       ├── appsettings.Development.json
│       ├── appsettings.json
│       ├── Bookstore.Web.csproj
│       ├── migration.sql
│       ├── Program.cs
│       ├── Controllers/
│       │   ├── AddressController.cs
│       │   ├── AdminController.cs
│       │   ├── BookController.cs
│       │   ├── CartController.cs
│       │   ├── FeedbackController.cs
│       │   ├── HealthController.cs
│       │   ├── ImageUploadController.cs
│       │   ├── OrderController.cs
│       │   ├── UserAuthController.cs
│       │   ├── WishlistController.cs
│       │   └── Admin/
│       │       ├── AdminAuthController.cs
│       │       ├── AdminBookController.cs
│       │       └── AdminOrderController.cs
│       ├── Middleware/
│       │   └── GlobalExceptionMiddleware.cs
│       └── Properties/
│           └── launchSettings.json
└── frontend/
    ├── README.md
    ├── angular.json
    ├── build_log.txt
    ├── nginx.conf
    ├── package.json
    ├── tsconfig.app.json
    ├── tsconfig.json
    ├── tsconfig.spec.json
    ├── .editorconfig
    └── src/
        ├── index.html
        ├── main.server.ts
        ├── main.ts
        ├── server.ts
        ├── styles.scss
        ├── app/
        │   ├── app.config.server.ts
        │   ├── app.config.ts
        │   ├── app.html
        │   ├── app.routes.server.ts
        │   ├── app.routes.ts
        │   ├── app.scss
        │   ├── app.spec.ts
        │   ├── app.ts
        │   ├── Components/
        │   │   ├── admin-panel/
        │   │   │   ├── admin-panel.html
        │   │   │   ├── admin-panel.scss
        │   │   │   └── admin-panel.ts
        │   │   ├── book-details/
        │   │   │   ├── book-details.html
        │   │   │   ├── book-details.scss
        │   │   │   ├── book-details.spec.ts
        │   │   │   └── book-details.ts
        │   │   ├── book-list/
        │   │   │   ├── book-list.html
        │   │   │   ├── book-list.scss
        │   │   │   └── book-list.ts
        │   │   ├── my-orders/
        │   │   │   ├── my-orders.html
        │   │   │   ├── my-orders.scss
        │   │   │   └── my-orders.ts
        │   │   ├── order-success/
        │   │   │   ├── order-success.html
        │   │   │   ├── order-success.scss
        │   │   │   └── order-success.ts
        │   │   ├── profile/
        │   │   │   ├── profile.html
        │   │   │   ├── profile.scss
        │   │   │   └── profile.ts
        │   │   ├── search-results/
        │   │   │   ├── search-results.html
        │   │   │   ├── search-results.scss
        │   │   │   └── search-results.ts
        │   │   ├── toast/
        │   │   │   └── toast.component.ts
        │   │   └── wishlist/
        │   │       ├── wishlist.html
        │   │       ├── wishlist.scss
        │   │       └── wishlist.ts
        │   ├── forgot-password/
        │   │   ├── forgot-password.css
        │   │   ├── forgot-password.html
        │   │   └── forgot-password.ts
        │   ├── login/
        │   │   ├── login.html
        │   │   ├── login.scss
        │   │   ├── login.spec.ts
        │   │   └── login.ts
        │   ├── Models/
        │   │   ├── api-constants.ts
        │   │   ├── auth.models.ts
        │   │   └── book.models.ts
        │   ├── mycart/
        │   │   ├── mycart.html
        │   │   ├── mycart.scss
        │   │   └── mycart.ts
        │   ├── Services/
        │   │   ├── admin.service.ts
        │   │   ├── auth.interceptor.ts
        │   │   ├── auth.service.ts
        │   │   ├── book.service.ts
        │   │   ├── cart.service.ts
        │   │   ├── feedback.service.ts
        │   │   ├── order.service.ts
        │   │   ├── toast.service.ts
        │   │   ├── user.service.ts
        │   │   └── wishlist.service.ts
        │   ├── shared/
        │   │   ├── index.ts
        │   │   ├── components/
        │   │   │   ├── footer/
        │   │   │   │   ├── footer.component.html
        │   │   │   │   ├── footer.component.scss
        │   │   │   │   ├── footer.component.ts
        │   │   │   │   └── index.ts
        │   │   │   └── header/
        │   │   │       ├── header.component.html
        │   │   │       ├── header.component.scss
        │   │   │       ├── header.component.ts
        │   │   │       └── index.ts
        │   │   └── guards/
        │   │       ├── admin.guard.ts
        │   │       └── auth.guard.ts
        │   ├── signup/
        │   │   ├── signup.html
        │   │   ├── signup.scss
        │   │   ├── signup.spec.ts
        │   │   └── signup.ts
        │   └── verify-email/
        │       ├── verify-email.html
        │       ├── verify-email.scss
        │       └── verify-email.ts
        └── environments/
            ├── environment.prod.ts
            └── environment.ts
```

---

## ⚙️ Getting Started

### 1️⃣ Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js](https://nodejs.org/) (v18 or higher)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (Express or LocalDB)
- [Angular CLI](https://angular.dev/tools/cli)

### 2️⃣ Backend Setup

1. **Navigate to the backend directory:**
   ```bash
   cd backend/Bookstore.Web
   ```

2. **Configure your database connection string in `appsettings.json`:**
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=BookstoreDb;Trusted_Connection=True;TrustServerCertificate=True"
   }
   ```

3. **Apply Migrations and Seed Data:**  
   The application is configured to automatically apply migrations and seed initial data on startup.

4. **Run the API:**
   ```bash
   dotnet run
   ```
   - API will be available at: `https://localhost:7158` (or check console output)
   - Swagger UI: `https://localhost:7158/swagger`

### 3️⃣ Frontend Setup

1. **Navigate to the frontend directory:**
   ```bash
   cd frontend
   ```

2. **Install dependencies:**
   ```bash
   npm install
   ```

3. **Start the development server:**
   ```bash
   npm start
   ```
   - The app will be available at: `http://localhost:4200`

---

## 🔌 API Summary

### 🔐 Authentication & User

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/auth/signup` | Register a new user |
| POST | `/api/auth/login` | User login |
| POST | `/api/auth/verification/{token}` | Verify user account |
| POST | `/api/auth/forgot-password` | Request password reset |
| POST | `/api/auth/verify-otp` | Verify OTP |
| POST | `/api/auth/resend-otp` | Resend OTP |
| POST | `/api/auth/reset-password` | Reset password |

### 👤 User Profile & Addresses

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/users/profile` | Get user profile |
| PUT | `/api/users/profile` | Update user profile |
| PATCH | `/api/users/profile` | Partially update profile |
| POST | `/api/users/addresses` | Add new address |
| PUT | `/api/users/addresses/{addressId}` | Update address |
| DELETE | `/api/users/addresses/{addressId}` | Delete address |

### 📚 Products (Books)

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/books` | Get all books |
| GET | `/api/books/{id}` | Get book details |

### 🛒 Cart

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/cart/items` | Add item to cart |
| PUT | `/api/cart/items/{cartItem_id}` | Update cart item |
| DELETE | `/api/cart/items/{cartItem_id}` | Remove item from cart |
| GET | `/api/cart` | Get cart details |

### ❤️ Wishlist

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/wishlist/items/{product_id}` | Add item to wishlist |
| DELETE | `/api/wishlist/items/{product_id}` | Remove item from wishlist |
| GET | `/api/wishlist` | Get wishlist items |

### 📦 Orders

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/orders` | Place a new order |
| GET | `/api/orders` | Get user orders |
| GET | `/api/orders/{id}` | Get order details |

### ⭐ Feedback / Reviews

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/feedback/books/{product_id}` | Add feedback for a book |
| GET | `/api/feedback/books/{product_id}` | Get book feedback |

### 🖼️ Image Upload

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/upload/image` | Upload image |

### 🛠️ Admin Authentication

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/bookstore_user/admin/registration` | Admin registration |
| POST | `/bookstore_user/admin/login` | Admin login |

### 📚 Admin – Books Management

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/admin/books` | Get all books |
| POST | `/api/admin/books` | Add new book |
| GET | `/api/admin/books/{id}` | Get book by ID |
| PUT | `/api/admin/books/{id}` | Update book |
| DELETE | `/api/admin/books/{id}` | Delete book |

### 🧾 Admin – Product Management

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/bookstore_user/admin/add/book` | Add book (admin) |
| PUT | `/bookstore_user/admin/update/book/{product_id}` | Update book |
| DELETE | `/bookstore_user/admin/delete/book/{product_id}` | Delete book |

### 📦 Admin – Orders

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/bookstore_user/admin/get/order` | Get all orders |

### ⚙️ System

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/health` | Health check |

---

## 👨‍💻 Authors

### **Adarsh Kumar**
- 📧 Email: [adarshkumar102004@gmail.com](mailto:adarshkumar102004@gmail.com)
- 🐙 GitHub: [@Adarsh-Kumar001](https://github.com/Adarsh-Kumar001)

### **Gaurav Gupta**
- 📧 Email: [gaurav.gupta26102003@gmail.com](mailto:gaurav.gupta26102003@gmail.com)
- 🐙 GitHub: [@GGupta03](https://github.com/GGupta03)

### **Kotipalli Srikesh**
- 📧 Email: [srikesh2017@gmail.com](mailto:srikesh2017@gmail.com)
- 🐙 GitHub: [@Srikesh-Kotipalli](https://github.com/Srikesh-Kotipalli)

### **Md Dilshad Alam**
- 📧 Email: [dilshadand@gmail.com](mailto:dilshadand@gmail.com)
- 🐙 GitHub: [@alamdilshad87](https://github.com/alamdilshad87)

### **Om Bandyopadhyay**
- 📧 Email: [ombandyopadhyay@gmail.com](mailto:ombandyopadhyay@gmail.com)
- 🐙 GitHub: [@ob6561](https://github.com/ob6561)

### **Pranav Mahajan**
- 📧 Email: [pranavmahajan619@gmail.com](mailto:pranavmahajan619@gmail.com)
- 🐙 GitHub: [@parumahajan](https://github.com/parumahajan)

### **V Dinessh**
- 📧 Email: [dinessh.venkat28@gmail.com](mailto:dinessh.venkat28@gmail.com)
- 🐙 GitHub: [@Dinessh2815](https://github.com/Dinessh2815)

---

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.
