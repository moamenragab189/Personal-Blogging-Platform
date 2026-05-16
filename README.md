# Personal Blogging Platform API

A robust RESTful API for a Personal Blogging Platform built with .NET 8, PostgreSQL, and Entity Framework Core. It enables users to register with email verification (OTP), securely authenticate via JWT, and manage their own blog posts and comments — with strict ownership enforcement ensuring users can only edit or delete content they created. Public endpoints allow anyone to browse all posts, while write operations are fully protected. The project demonstrates secure, scalable backend architecture including JWT authorization, rate limiting, global error handling, and structured logging.

---

## Tech Stack

| Layer | Technology |
|-------|------------|
| Framework | ASP.NET Core Web API (.NET 8) |
| Database | PostgreSQL |
| ORM | Entity Framework Core (Code-First) |
| Authentication | JWT Bearer Tokens |
| Validation | Data Annotations & FluentValidation-ready |
| Mapping | AutoMapper |
| Logging | Serilog (Console + File sinks) |
| Email | MailKit / MimeKit |
| Documentation | Swagger / OpenAPI |

---

---

## Features

- **JWT Authentication**: Secure login and registration with hashed passwords (BCrypt).
- **Email Verification**: OTP-based email verification via SMTP.
- **Role-based Ownership**: Users can only update/delete their own posts and comments.
- **Global Error Handling**: Centralized exception handling with consistent `ProblemDetails` responses.
- **Rate Limiting**: Fixed-window rate limiting to protect auth endpoints and general API routes.
- **Structured Logging**: Serilog writes to both console and daily rolling files (`logs/`).
- **AutoMapper**: Clean separation between Entities and DTOs.
- **Swagger UI**: Fully documented and testable endpoints with JWT Bearer authorization support.

---
## API Endpoints

### Authentication
| Method | Endpoint | Access | Description |
|--------|----------|--------|-------------|
| `POST` | `/api/auth/register` | Public | Register a new user account |
| `PATCH` | `/api/auth/verify-email` | Public | Verify email using OTP code |
| `POST` | `/api/auth/login` | Public | Authenticate and receive JWT token |

### Posts
| Method | Endpoint | Access | Description |
|--------|----------|--------|-------------|
| `GET` | `/api/post` | Public | Retrieve all blog posts |
| `POST` | `/api/post` | Protected | Create a new blog post |
| `PUT` | `/api/post/{id}` | Protected | Update own post by ID |
| `DELETE` | `/api/post/{id}` | Protected | Delete own post by ID |

### Comments
| Method | Endpoint | Access | Description |
|--------|----------|--------|-------------|
| `POST` | `/api/comment` | Protected | Add a comment to a post |
| `GET` | `/api/comment/{postId}` | Protected | Get all comments for a post |
| `DELETE` | `/api/comment/{id}` | Protected | Delete own comment by ID |
## Project Structure


### Architecture Overview

| Layer | Responsibility |
|-------|---------------|
| **Controllers** | Handle HTTP requests, validate input, return HTTP responses |
| **Services** | Contain business logic, orchestrate data flow, enforce rules |
| **Repositories** | Abstract database access (CRUD operations via EF Core) |
| **Entities** | Domain models and `DbContext` configuration |
| **DTOs** | Define API request/response contracts |
| **Mapping** | AutoMapper profiles for Entity ↔ DTO transformation |
| **Exceptions** | Custom exception hierarchy for predictable error responses |

---


---

# Getting Started

## 1. Clone the Repository

```bash
git clone https://github.com/your-username/personal-blogging-platform.git
cd personal-blogging-platform
```

---

## 2. Configure appsettings.json

Update your `appsettings.json` file:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=BlogDb;Username=postgres;Password=yourpassword"
  },

  "JWT": {
    "Key": "YourSecretKey",
    "Issuer": "YourIssuer",
    "Audience": "YourAudience"
  }
}
```

---

## 3. Install PostgreSQL

Download PostgreSQL:

- https://www.postgresql.org/download/

Create a database named:

```bash
BlogDb
```

---

## 4. Apply Migrations

Run the following commands:

```bash
dotnet ef database update
```

---

## 5. Run the Project



