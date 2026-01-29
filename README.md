# 📚 BookStore - Full Stack Online Bookstore

**Modern E-commerce Platform | .NET 8 | Angular 19 | Clean Architecture | SQL Server**

Welcome to **BookStore**, a comprehensive full-stack e-commerce application designed for book enthusiasts. This project features a robust **.NET 8 Web API** backend following **N-Tier Architecture** and a dynamic **Angular 19** frontend.

---

## 🚀 Key Features

### 👤 User Features
- **User Authentication**: Secure Signup, Login, and Forgot Password with JWT-based authentication.
- **Browse Books**: Search and explore a wide collection of books with detailed descriptions.
- **Shopping Cart**: Seamlessly add, remove, and manage quantities of books in your cart.
- **Wishlist**: Save your favorite books for future purchases.
- **Order Management**: Secure checkout process with order history and success tracking.
- **User Profile**: Manage personal details and delivery addresses.
- **Feedback & Reviews**: Share your thoughts on books with a built-in feedback system.

### 🛡️ Admin Features
- **Admin Dashboard**: Overview of store performance and management.
- **Book Management**: Full CRUD operations (Add, Update, Delete) for the book catalog.
- **Order Monitoring**: View and manage customer orders.
- **User Management**: Monitor registered users and their activities.

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
BookStore/
├── backend/
│   ├── Bookstore.Web/           # API Endpoints, Controllers, Middleware
│   ├── Bookstore.Business/      # Business Logic, Services, Interfaces
│   ├── Bookstore.DataAccess/    # DB Context, Repositories, Migrations
│   └── Bookstore.Models/        # Entities, DTOs, Enums
├── frontend/
│   ├── src/
│   │   ├── app/
│   │   │   ├── Components/      # UI Components (BookList, Cart, etc.)
│   │   │   ├── Services/        # API Integrated Services
│   │   │   └── Models/          # Client-side Interfaces
│   │   └── assets/              # Global Images & Styles
│   └── angular.json             # Angular Workspace Config
└── README.md
```

---

## ⚙️ Getting Started

### 1️⃣ Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js](https://nodejs.org/) (v18 or higher)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (Express or LocalDB)
- [Angular CLI](https://angular.dev/tools/cli)

### 2️⃣ Backend Setup
1.  Navigate to the backend directory:
    ```bash
    cd backend/Bookstore.Web
    ```
2.  Configure your database connection string in `appsettings.json`:
    ```json
    "ConnectionStrings": {
      "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=BookstoreDb;Trusted_Connection=True;TrustServerCertificate=True"
    }
    ```
3.  Apply Migrations and Seed Data:
    The application is configured to automatically apply migrations and seed initial data on startup.
4.  Run the API:
    ```bash
    dotnet run
    ```
    API will be available at: `https://localhost:7158` (or check console output)
    Swagger UI: `https://localhost:7158/swagger`

### 3️⃣ Frontend Setup
1.  Navigate to the frontend directory:
    ```bash
    cd frontend
    ```
2.  Install dependencies:
    ```bash
    npm install
    ```
3.  Start the development server:
    ```bash
    npm start
    ```
    The app will be available at: `http://localhost:4200`

---

## 🔌 API Summary

### 🔐 Authentication & User

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | /api/auth/signup | Register a new user |
| POST | /api/auth/login | User login |
| POST | /api/auth/verification/{token} | Verify user account |
| POST | /api/auth/forgot-password | Request password reset |
| POST | /api/auth/verify-otp | Verify OTP |
| POST | /api/auth/resend-otp | Resend OTP |
| POST | /api/auth/reset-password | Reset password |

---

### 👤 User Profile & Addresses

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /api/users/profile | Get user profile |
| PUT | /api/users/profile | Update user profile |
| PATCH | /api/users/profile | Partially update profile |
| POST | /api/users/addresses | Add new address |
| PUT | /api/users/addresses/{addressId} | Update address |
| DELETE | /api/users/addresses/{addressId} | Delete address |

---

### 📚 Products (Books)

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /api/books | Get all books |
| GET | /api/books/{id} | Get book details |

---

### 🛒 Cart

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | /api/cart/items | Add item to cart |
| PUT | /api/cart/items/{cartItem_id} | Update cart item |
| DELETE | /api/cart/items/{cartItem_id} | Remove item from cart |
| GET | /api/cart | Get cart details |

---

### ❤️ Wishlist

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | /api/wishlist/items/{product_id} | Add item to wishlist |
| DELETE | /api/wishlist/items/{product_id} | Remove item from wishlist |
| GET | /api/wishlist | Get wishlist items |

---

### 📦 Orders

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | /api/orders | Place a new order |
| GET | /api/orders | Get user orders |
| GET | /api/orders/{id} | Get order details |

---

### ⭐ Feedback / Reviews

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | /api/feedback/books/{product_id} | Add feedback for a book |
| GET | /api/feedback/books/{product_id} | Get book feedback |

---

### 🖼️ Image Upload

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | /api/upload/image | Upload image |

---

### 🛠️ Admin Authentication

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | /bookstore_user/admin/registration | Admin registration |
| POST | /bookstore_user/admin/login | Admin login |

---

### 📚 Admin – Books Management

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /api/admin/books | Get all books |
| POST | /api/admin/books | Add new book |
| GET | /api/admin/books/{id} | Get book by ID |
| PUT | /api/admin/books/{id} | Update book |
| DELETE | /api/admin/books/{id} | Delete book |

---

### 🧾 Admin – Product Management

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | /bookstore_user/admin/add/book | Add book (admin) |
| PUT | /bookstore_user/admin/update/book/{product_id} | Update book |
| DELETE | /bookstore_user/admin/delete/book/{product_id} | Delete book |

---

### 📦 Admin – Orders

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /bookstore_user/admin/get/order | Get all orders |

---

### ⚙️ System

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /api/health | Health check |



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
