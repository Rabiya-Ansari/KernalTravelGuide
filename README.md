# Kernal Travel Guide

Kernal Travel Guide is an ASP.NET Core web application built with **.NET 10**, **Entity Framework Core**, **SQL Server**, and **ASP.NET Core Identity**.

## Overview

The project is configured as an ASP.NET Core MVC application with:

- ASP.NET Core MVC
- Entity Framework Core with SQL Server
- ASP.NET Core Identity
- Identity Roles
- Razor Pages for Identity UI
- Application user support through `ApplicationUser`
- Automatic role seeding
- Automatic admin-user seeding
- HTTPS redirection and static-file support

## Technologies Used

| Technology | Version / Details |
|---|---|
| .NET | 10.0 |
| ASP.NET Core | 10 |
| Entity Framework Core | 10.0.10 |
| SQL Server Provider | Microsoft.EntityFrameworkCore.SqlServer 10.0.10 |
| ASP.NET Core Identity | 10.0.10 |
| Project Type | ASP.NET Core Web |
| Pattern | MVC + Razor Pages |

## Project Configuration

The project targets `.NET 10` and has nullable reference types enabled.

Main project file:

```text
KernalTravelGuide.csproj
```

The project includes the following NuGet packages:

- `Microsoft.AspNetCore.Identity.EntityFrameworkCore`
- `Microsoft.AspNetCore.Identity.UI`
- `Microsoft.EntityFrameworkCore.SqlServer`
- `Microsoft.EntityFrameworkCore.Tools`

## Database

The application uses SQL Server through Entity Framework Core.

The configured connection string is:

```text
Server=localhost\SQLEXPRESS;Database=KernalTourDb;Trusted_Connection=True;TrustServerCertificate=true
```

The database name is:

```text
KernalTourDb
```

Before running the application, make sure SQL Server / SQL Server Express is installed and the configured server instance is available.

> **Security note:** For a shared or production environment, do not commit real database passwords or other secrets to source control. Use environment variables, user secrets, or another secure configuration provider.

## Application Startup

The application's startup and service configuration is handled in:

```text
Program.cs
```

The application:

1. Creates the web application builder.
2. Reads the `AppDbContext` connection string.
3. Registers `AppDbContext` with SQL Server.
4. Configures ASP.NET Core Identity.
5. Enables Identity roles.
6. Uses Entity Framework Core as the Identity store.
7. Registers MVC controllers and views.
8. Registers Razor Pages.
9. Seeds roles when the application starts.
10. Seeds the admin user when the application starts.
11. Configures the HTTP request pipeline.
12. Maps the default MVC route.
13. Maps Razor Pages for Identity.

## Authentication & Authorization

ASP.NET Core Identity is configured with:

- `ApplicationUser`
- Identity roles
- Entity Framework Core stores
- Confirmed-account sign-in requirement

The project also runs role seeding through:

```text
SeedRoles.SeedAsync(roleManager)
```

The admin account is initialized through:

```text
SeedAdmin.SeedAdminAsync(userManager)
```

These seed operations are executed when the application starts.

## MVC Routing

The default MVC route is:

```text
{controller=Home}/{action=Index}/{id?}
```

This means the default page is:

```text
Home/Index
```

## Razor Pages

Razor Pages are enabled because ASP.NET Core Identity uses Razor Pages for its built-in authentication UI.

The application maps them with:

```text
app.MapRazorPages();
```

## Static Files

Static files are enabled through:

```text
app.UseStaticFiles();
```

The project also contains a configured web assets location:

```text
wwwroot/client/assets/
```

## Configuration Files

### `appsettings.json`

Contains application configuration including:

- Logging configuration
- Allowed hosts
- SQL Server connection string

### `appsettings.Development.json`

Contains development-specific logging configuration.

## Running the Project

### Prerequisites

Install:

- .NET 10 SDK
- SQL Server or SQL Server Express
- Visual Studio 2022/2026 or another compatible .NET IDE

### Step 1: Clone the repository

```bash
git clone <repository-url>
cd KernalTravelGuide
```

Replace `<repository-url>` with the actual repository URL.

### Step 2: Restore dependencies

```bash
dotnet restore
```

### Step 3: Verify the database connection

Check the connection string in:

```text
appsettings.json
```

Make sure the SQL Server instance and database configuration are correct.

### Step 4: Apply Entity Framework migrations

If migrations are available in the project, run:

```bash
dotnet ef database update
```

If the `dotnet ef` command is not installed, install it with:

```bash
dotnet tool install --global dotnet-ef
```

### Step 5: Run the application

```bash
dotnet run
```

Or run the project directly from Visual Studio.

## Development

For development, the project uses:

```text
appsettings.Development.json
```

The application also enables the development-specific ASP.NET Core behavior automatically through the standard environment configuration.

## HTTP Request Pipeline

The configured request pipeline includes:

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

In production, the application also uses:

- Exception handler: `/Home/Error`
- HSTS

## Solution

The solution file is:

```text
KernalTravelGuide.slnx
```

It references:

```text
KernalTravelGuide/KernalTravelGuide.csproj
```

## Git

The project includes a Visual Studio-oriented `.gitignore`.

It excludes common generated files and folders such as:

- `bin/`
- `obj/`
- `Debug/`
- `Release/`
- `.vs/`
- NuGet packages
- Visual Studio user-specific files
- Build output
- SQL Server database files such as `.mdf` and `.ldf`

## Important Notes

- Make sure the SQL Server instance matches the connection string.
- Make sure the database is available before starting the application if the application expects it.
- Identity roles and the admin user are seeded during application startup.
- Keep production secrets outside the repository.
- If database migrations are part of the project, apply them before first use.

## Project Status

This README is based on the project files provided with the application, including the project configuration, startup configuration, application settings, and solution configuration.

Additional controllers, models, views, services, migrations, and other project-specific features should be documented here as they are added to the project.
