# Kernal Travel Guide

Kernal Travel Guide is an **ASP.NET Core MVC web application** built with **.NET 10**, **Entity Framework Core**, **SQL Server**, and **ASP.NET Core Identity**.

The application is designed as a travel-guide platform where users can explore travel-related content, while administrators manage the application's data and users.

---

## Project Overview

### Technologies Used

| Technology | Version / Details |
|---|---|
| .NET | 10.0 |
| ASP.NET Core | 10 |
| Entity Framework Core | 10.0.10 |
| Database | Microsoft SQL Server |
| Authentication | ASP.NET Core Identity |
| Authorization | Identity Roles |
| UI | ASP.NET Core MVC + Razor Pages |
| Pattern | MVC |

---

## Main Features

- ASP.NET Core MVC architecture
- ASP.NET Core Identity authentication
- Role-based authorization
- Admin dashboard
- Admin user management
- Travel-related data management
- SQL Server database
- Entity Framework Core migrations
- Razor Pages for Identity UI
- Responsive public-facing travel website
- Static assets through `wwwroot`
- Automatic role seeding
- Automatic administrator account seeding

---

# Database Configuration

The project uses SQL Server with the following database configuration:

```text
Server=localhost\SQLEXPRESS;
Database=KernalTourDb;
Trusted_Connection=True;
TrustServerCertificate=true
```

### Database Name

```text
KernalTourDb
```

### SQL Server Instance

```text
localhost\SQLEXPRESS
```

Make sure SQL Server Express is installed and the SQL Server instance is running before starting the application.

---

# Admin Login Credentials

Use the following credentials to access the administrator account:

### Admin Email

```text
admin@karnel.com
```

### Admin Password

```text
Admin@123
```

### Admin Role

```text
Admin
```

> **Security Warning:** These credentials are included here for development/demo purposes. Change the password before deploying the application to a production environment.

---

# Prerequisites

Install the following software before running the project:

- .NET 10 SDK
- SQL Server / SQL Server Express
- Visual Studio 2022/2026 or another compatible .NET IDE
- .NET Entity Framework Core CLI tools

Check the installed .NET version:

```bash
dotnet --version
```

---

# Project Structure

The main project follows the standard ASP.NET Core MVC structure:

```text
KernalTravelGuide/
│
├── Controllers/
│
├── Data/
│   └── AppDbContext
│
├── Models/
│
├── Views/
│   ├── Home/
│   ├── Shared/
│   └── ...
│
├── Areas/
│   └── Identity/
│
├── Services/
│
├── Migrations/
│
├── wwwroot/
│   └── client/
│       └── assets/
│
├── Program.cs
├── appsettings.json
├── appsettings.Development.json
└── KernalTravelGuide.csproj
```

The exact folders may grow as additional features are added.

---

# Authentication & Authorization

The application uses **ASP.NET Core Identity** for authentication.

Identity provides:

- User registration/login
- Password management
- User authentication
- Role management
- Authorization
- Account confirmation support
- Razor Pages-based Identity UI

The application also supports role-based access.

Example:

```csharp
[Authorize(Roles = "Admin")]
```

This restricts an action or controller to administrators.

---

# Role Seeding

Roles are automatically initialized when the application starts.

The project uses:

```text
SeedRoles.SeedAsync(roleManager)
```

This ensures required roles exist in the database.

---

# Admin Seeding

The administrator account is automatically initialized during application startup.

The project uses:

```text
SeedAdmin.SeedAdminAsync(userManager)
```

The configured administrator credentials are:

```text
Email: admin@karnel.com
Password: Admin@123
Role: Admin
```

If the account already exists, the seeding process should avoid creating a duplicate account.

---

# MVC Routing

The default MVC route is:

```text
{controller=Home}/{action=Index}/{id?}
```

Therefore, the default application page is:

```text
Home/Index
```

---

# Razor Pages

Razor Pages are enabled because ASP.NET Core Identity uses Razor Pages for its built-in authentication interface.

The application maps Razor Pages using:

```csharp
app.MapRazorPages();
```

---

# Static Files

Static files are enabled through:

```csharp
app.UseStaticFiles();
```

The project contains web assets under:

```text
wwwroot/client/assets/
```

These assets can include:

- CSS
- JavaScript
- Images
- Fonts
- Vendor libraries

---

# Program Startup

The main application configuration is handled inside:

```text
Program.cs
```

The startup process includes:

1. Create the web application builder.
2. Read the database connection string.
3. Register `AppDbContext`.
4. Configure SQL Server.
5. Configure ASP.NET Core Identity.
6. Enable Identity roles.
7. Register Entity Framework Core Identity stores.
8. Register MVC controllers and views.
9. Register Razor Pages.
10. Seed application roles.
11. Seed the administrator account.
12. Configure the HTTP request pipeline.
13. Enable static files.
14. Enable routing.
15. Enable authentication.
16. Enable authorization.
17. Map MVC routes.
18. Map Razor Pages.

---

# HTTP Request Pipeline

The application follows this general request pipeline:

```text
HTTPS Redirection
        ↓
Static Files
        ↓
Routing
        ↓
Authentication
        ↓
Authorization
        ↓
MVC / Razor Pages
```

In production, the application can also use:

- HSTS
- Exception handling
- `/Home/Error`

---

# Database Migration

If migrations are already included in the project, update the database using:

```bash
dotnet ef database update
```

If Entity Framework CLI is not installed, install it with:

```bash
dotnet tool install --global dotnet-ef
```

Then run:

```bash
dotnet ef database update
```

---

# Restore Dependencies

After cloning or extracting the project, restore NuGet packages:

```bash
dotnet restore
```

Then build the project:

```bash
dotnet build
```

---

# Run the Application

Run the project using:

```bash
dotnet run
```

Or open the solution in Visual Studio and press:

```text
F5
```

or:

```text
Ctrl + F5
```

---

# Configuration

The main configuration file is:

```text
appsettings.json
```

It contains application settings such as:

- Connection strings
- Logging configuration
- Allowed hosts

Development-specific settings are located in:

```text
appsettings.Development.json
```

---

# Connection String

The development database connection string is:

```text
Server=localhost\SQLEXPRESS;Database=KernalTourDb;Trusted_Connection=True;TrustServerCertificate=true
```

For production, use a secure configuration provider instead of committing sensitive connection information to source control.

Recommended options include:

- Environment variables
- User Secrets
- Azure Key Vault
- Other secure secret-management systems

---

# Git

The project uses a Visual Studio-oriented `.gitignore`.

Generated files and folders such as the following should not be committed:

```text
bin/
obj/
.vs/
Debug/
Release/
*.user
*.suo
```

Database files such as:

```text
*.mdf
*.ldf
```

should also remain excluded when appropriate.

---

# Troubleshooting

## SQL Server Connection Error

If the application cannot connect to the database:

1. Make sure SQL Server Express is installed.
2. Make sure the SQL Server service is running.
3. Verify the instance name:

```text
localhost\SQLEXPRESS
```

4. Check the connection string in `appsettings.json`.
5. Run:

```bash
dotnet ef database update
```

---

## Login Does Not Work

Verify the administrator credentials:

```text
Email: admin@karnel.com
Password: Admin@123
```

Also make sure the admin seeding code has executed successfully.

---

## Role Authorization Problem

If an administrator receives an authorization error, verify that the user has the:

```text
Admin
```

role in the Identity database.

---

# Development Notes

This project is intended for development and educational/project purposes.

When preparing the project for production:

- Change the default admin password.
- Remove credentials from public documentation.
- Move secrets to secure configuration.
- Use HTTPS.
- Review authorization rules.
- Validate all user input.
- Keep NuGet packages updated.
- Apply database migrations carefully.
- Configure production error handling.

---

# Solution

The solution file is:

```text
KernalTravelGuide.slnx
```

The main project is:

```text
KernalTravelGuide/KernalTravelGuide.csproj
```

---

# Quick Start

For a quick setup:

```bash
# Restore packages
dotnet restore

# Build project
dotnet build

# Apply database migrations
dotnet ef database update

# Run application
dotnet run
```

Then open the application in your browser using the URL displayed by `dotnet run`.

For administrator access, use:

```text
Email: admin@karnel.com
Password: Admin@123
```

---

# Project Status

**Project:** Kernal Travel Guide

**Framework:** ASP.NET Core / .NET 10

**Database:** SQL Server

**Authentication:** ASP.NET Core Identity

**Architecture:** MVC + Razor Pages

**Status:** Development

---

## Security Notice

The administrator credentials in this README are development/demo credentials:

```text
admin@karnel.com
Admin@123
```

Do not use these credentials unchanged in a production environment.
