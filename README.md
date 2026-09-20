# MvcAuth

A simple ASP.NET Core MVC project for practicing authentication and authorization.

The project demonstrates user registration, login, cookie authentication, claims-based identity, role-based authorization, and password hashing.

## Technologies

- ASP.NET Core MVC
- C#
- SQL Server
- Dapper
- Cookie Authentication
- PBKDF2 Password Hashing

## Features

- User registration
- User login and logout
- Cookie-based authentication
- Claims-based user identity
- Role-based authorization
- User and Admin roles
- Protected profile page
- Admin-only pages
- Admin user list
- PBKDF2 password hashing with random salt
- Registration validation
- Duplicate account and email validation

## Architecture

The project separates responsibilities into the following layers:

- **Controller** - Handles HTTP requests, MVC flow, authentication cookies, views, and redirects.
- **Service** - Handles application and business logic such as authentication and registration.
- **Repository** - Handles database access using Dapper and SQL Server.

### Main Flow

```text
AccountController
    |
    +-- IAuthService
    |       |
    |       +-- IUserRepository
    |       +-- PasswordService
    |
    +-- IUserService
            |
            +-- IUserRepository
                    |
                    +-- SQL Server

```

## Authentication Flow

When a user logs in:

```text
Account + Password
        |
        v
AuthService validates credentials
        |
        v
PasswordService verifies the password using PBKDF2
        |
        v
Create Claims
(User ID, Account, Role)
        |
        v
ClaimsPrincipal
        |
        v
SignInAsync()
        |
        v
Authentication Cookie
        |
        v
Browser stores the cookie
```

For later requests:

```text
Browser sends Authentication Cookie
        |
        v
Authentication Middleware validates the cookie
        |
        v
HttpContext.User is created
        |
        v
[Authorize] / Role Authorization
        |
        v
Protected resource
```

## Password Storage

Passwords are not stored as plaintext.

During registration:

```text
Password
   +
Random Salt
   |
   v
PBKDF2
(100,000 iterations, SHA-256)
   |
   v
Derived Hash
   |
   v
Store:
iterations.salt.hash
```

During login, the stored iteration count and salt are used with the entered password to derive a new hash. The derived hash is then compared with the stored hash using `CryptographicOperations.FixedTimeEquals`.

## Database

The project uses SQL Server with a `Users` table.

| Column | Type | Description |
|---|---|---|
| Oid | UNIQUEIDENTIFIER | Primary key |
| Account | NVARCHAR(30) | User account |
| Email | NVARCHAR(50) | User email |
| PasswordHash | NVARCHAR(255) | PBKDF2 password hash |
| CreatedDate | DATETIME2 | Account creation date |
| FirstName | NVARCHAR(30) | First name |
| LastName | NVARCHAR(30) | Last name |
| Birthday | DATE | Optional birthday |
| Role | NVARCHAR(20) | User role (`User` or `Admin`) |

### Database Setup

Create a SQL Server database, then run:

`Database/CreateUsersTable.sql`

to create the required `Users` table.

Configure the database connection in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "your-connection-string"
  }
}
