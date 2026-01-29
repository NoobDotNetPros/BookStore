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

| Method | Endpoint | Description |
| :--- | :--- | :--- |
| **POST** | `/api/UserAuth/register` | Register a new user |
| **POST** | `/api/UserAuth/login` | Login and receive JWT |
| **GET** | `/api/Book` | Get all books |
| **GET** | `/api/Book/{id}` | Get book details |
| **POST** | `/api/Cart` | Add item to cart |
| **POST** | `/api/Order` | Place a new order |
| **GET** | `/api/Admin/books` | Admin: List all books |

---

## 👨‍💻 Author

**Kotipalli Srikesh**
- 📧 Email: [srikesh2017@gmail.com](mailto:srikesh2017@gmail.com)
- 🐙 GitHub: [@Srikesh-Kotipalli](https://github.com/Srikesh-Kotipalli)

---

## 📄 License
This project is licensed under the MIT License - see the LICENSE file for details.
