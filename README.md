# Bulky - E-Commerce Bookstore Application

[![.NET](https://img.shields.io/badge/.NET-9.0-blue.svg)](https://dotnet.microsoft.com/download)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-9.0-purple.svg)](https://docs.microsoft.com/en-us/aspnet/core/)
[![Entity Framework](https://img.shields.io/badge/Entity%20Framework-9.0-green.svg)](https://docs.microsoft.com/en-us/ef/)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-LocalDB-red.svg)](https://docs.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb)

A comprehensive e-commerce bookstore application built with ASP.NET Core 9.0, featuring a clean architecture, role-based authentication, and integrated payment processing through Stripe.

## 🚀 Features

### 🛒 E-Commerce Functionality

- **Product Catalog**: Browse books with detailed product information, images, and pricing
- **Shopping Cart**: Add/remove items, adjust quantities, and manage cart sessions
- **Order Management**: Complete order processing with status tracking
- **Payment Integration**: Secure payment processing via Stripe
- **Multi-Store Support**: Products can be associated with different stores

### 👥 User Management & Authentication

- **Role-Based Access Control**: Admin, Customer, Employee, and Company roles
- **Identity Integration**: ASP.NET Core Identity with custom user extensions
- **Social Login**: Facebook and Google authentication support
- **Company Accounts**: Special pricing and payment terms for business customers

### 📊 Administrative Features

- **Admin Dashboard**: Comprehensive management interface
- **Product Management**: CRUD operations for products, categories, and images
- **User Management**: User role assignments and account management
- **Order Processing**: Order fulfillment and status updates
- **Banner & Section Management**: Homepage content management
- **Company Management**: Business account administration

### 🎨 User Experience

- **Responsive Design**: Mobile-friendly interface with Bootstrap
- **Dynamic Content**: Homepage banners and section management
- **Search & Filtering**: Product search and category filtering
- **Order History**: Customer order tracking and history
- **Notifications**: Real-time user notifications

## 🏗️ Architecture

The application follows a clean, layered architecture pattern:

```
Bulky/
├── Bulky.Web/              # ASP.NET Core MVC Web Application
│   ├── Areas/
│   │   ├── Admin/          # Administrative area
│   │   ├── Customer/       # Customer-facing area
│   │   └── Identity/       # Authentication pages
│   ├── Components/         # View components
│   ├── Views/              # Razor views and layouts
│   └── wwwroot/            # Static assets (CSS, JS, images)
├── Bulky.Models/           # Domain models and view models
│   ├── Models/             # Entity models
│   └── ViewModels/         # Data transfer objects
├── Bulky.DataAccess/       # Data layer
│   ├── Data/               # DbContext and database initializer
│   ├── Migrations/         # Entity Framework migrations
│   └── Repositories/       # Repository pattern implementation
└── Bulky.Utility/          # Cross-cutting concerns and utilities
```

## 🛠️ Technology Stack

- **Framework**: ASP.NET Core 9.0
- **Database**: SQL Server with Entity Framework Core 9.0
- **Authentication**: ASP.NET Core Identity
- **Payment Processing**: Stripe.NET
- **Frontend**: Bootstrap, jQuery, HTML5, CSS3
- **Architecture Patterns**: Repository Pattern, Unit of Work, MVC
- **Social Authentication**: Facebook & Google OAuth

## 📋 Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (LocalDB or Express)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/)
- [Stripe Account](https://stripe.com/) for payment processing

## 🚀 Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/Ibrahim-Mohamed66/Bulky.git
cd Bulky
```

### 2. Database Setup

```bash
# Navigate to the project directory
cd Bulky.Web

# Update database with migrations
dotnet ef database update --project ../Bulky.DataAccess
```

### 3. Configuration

Update `appsettings.json` in the `Bulky.Web` project:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "server=.;Database=Bulky;Trusted_connection=True;TrustServerCertificate=True;"
  },
  "Stripe": {
    "PublishableKey": "your_stripe_publishable_key",
    "SecretKey": "your_stripe_secret_key"
  },
  "Authentication": {
    "Facebook": {
      "AppId": "your_facebook_app_id",
      "AppSecret": "your_facebook_app_secret"
    },
    "Google": {
      "ClientId": "your_google_client_id",
      "ClientSecret": "your_google_client_secret"
    }
  }
}
```

### 4. Run the Application

```bash
dotnet run
```

Navigate to `https://localhost:7049` to access the application.

## 📊 Database Schema

The application includes the following main entities:

- **Products**: Book catalog with pricing tiers
- **Categories**: Product categorization
- **Users**: Extended ASP.NET Core Identity users
- **Orders**: Order headers and details
- **Companies**: Business customer accounts
- **Carts**: Shopping cart management
- **Stores**: Multi-store support
- **Banners & Sections**: Homepage content management

## 🔐 User Roles & Permissions

| Role         | Permissions                                             |
| ------------ | ------------------------------------------------------- |
| **Admin**    | Full system access, user management, content management |
| **Employee** | Order processing, customer support                      |
| **Company**  | Special pricing, delayed payment terms                  |
| **Customer** | Product browsing, ordering, account management          |

## 🎯 Key Features Implementation

### Repository Pattern

The application implements the Repository pattern with Unit of Work for clean data access abstraction.

### Payment Processing

Integrated Stripe payment processing with support for immediate and delayed payments.

### Multi-Store Architecture

Products can be assigned to different stores with inventory management.

### Role-Based Security

Comprehensive role-based access control throughout the application.

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request



## 👨‍💻 Author

**Ibrahim Mohamed**

- GitHub: [@Ibrahim-Mohamed66](https://github.com/Ibrahim-Mohamed66)

## 🙏 Acknowledgments

- ASP.NET Core team for the excellent framework
- Stripe for seamless payment integration
- Bootstrap team for responsive design components
- Entity Framework team for robust ORM capabilities

---

For detailed documentation, please refer to the inline code comments and the project's wiki section.
