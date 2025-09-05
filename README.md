

---


# 📘 Contact Management API

A .NET 8 Web API for **user authentication (register/login)** and **personal contact management** using **ASP.NET Core Identity + JWT Authentication**.  
Each registered user can manage their own contacts securely.

---

## 🚀 Features
- User **registration** & **login** with hashed passwords.
- **JWT Authentication** for protecting endpoints.
- **Add, view, and manage contacts** for each authenticated user.
- Database powered by **Entity Framework Core (Code-First)**.
- **Swagger UI** for API testing.
- **CORS enabled** to allow frontend access.

---

## 🛠 Tech Stack
- **ASP.NET Core 8 Web API**
- **Entity Framework Core**
- **ASP.NET Core Identity**
- **JWT (JSON Web Tokens)**
- **SQL Server LocalDB** (default, can be swapped with Docker SQL Server)

---

## 📦 Installation

### 1. Prerequisites
Make sure you have installed:
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later
- [SQL Server / LocalDB](https://learn.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb)
- (Optional) [Docker Desktop](https://www.docker.com/products/docker-desktop) if you want to run with containers

---

### 2. Clone the repository
```bash
git clone https://github.com/Mahdiy005/ContactManagmentSolution.git
cd ContactManagmentSolution
````

---

### 3. Install dependencies

If packages are missing, run:

```bash
dotnet restore
```

Required packages include:

* `Microsoft.EntityFrameworkCore`
* `Microsoft.EntityFrameworkCore.SqlServer`
* `Microsoft.EntityFrameworkCore.Tools`
* `Microsoft.AspNetCore.Identity.EntityFrameworkCore`
* `Microsoft.AspNetCore.Authentication.JwtBearer`
* `Swashbuckle.AspNetCore` (Swagger)

---

### 4. Update database connection

Check `appsettings.json`:

```json
"ConnectionStrings": {
  "myQueryString": "Data Source=(localdb)\\ProjectModels;Initial Catalog=ContactDB;Integrated Security=True;Encrypt=False"
}
```

* Use this for **local development** with LocalDB.
* For **Docker**, update it to:

  ```json
  "ConnectionStrings": {
    "myQueryString": "Server=db;Database=ContactDB;User=sa;Password=Your_strong_password123;TrustServerCertificate=True;MultipleActiveResultSets=true"
  }
  ```

---

### 5. Apply EF Core migrations

```bash
dotnet ef database update
```

---

### 6. Run the project

```bash
dotnet run
```

The API will start (by default) at:
👉 `https://localhost:5127`

Swagger UI:
👉 `https://localhost:5127/swagger`

---

## 📡 API Endpoints

### 🔑 Auth

| Method | Endpoint             | Description       |
| ------ | -------------------- | ----------------- |
| POST   | `/api/auth/register` | Register new user |
| POST   | `/api/auth/login`    | Login and get JWT |

#### Example: Register

```json
POST /api/auth/register
{
  "userName": "john123",
  "email": "john@example.com",
  "password": "P@ssword123"
}
```

#### Example: Login

```json
POST /api/auth/login
{
  "email": "john@example.com",
  "password": "P@ssword123"
}
```

Response:

```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6..."
}
```

---

### 📒 Contacts (Authenticated only)

| Method | Endpoint             | Description                         |
| ------ | -------------------- | ----------------------------------- |
| POST   | `/api/contacts`      | Add new contact                     |
| GET    | `/api/contacts`      | List all contacts of logged-in user |
| GET    | `/api/contacts/{id}` | Get single contact                  |
| DELETE | `/api/contacts/{id}` | Delete contact                      |

#### Example: Add Contact

```json
POST /api/contacts
Authorization: Bearer <token>

{
  "firstName": "Jane",
  "lastName": "Doe",
  "phoneNumber": "123456789",
  "email": "jane.doe@example.com",
  "birthdate": "1995-05-10"
}
```

---

## 🗄 Database Schema

### Users (Identity)

* Id (string, PK)
* UserName
* Email
* PasswordHash
* (plus default Identity fields)

### Contacts

* Id (int, PK)
* FirstName
* LastName
* PhoneNumber
* Email
* Birthdate
* UserId (FK → Users.Id)

---

## 🐳 Running with Docker

This project includes a `docker-compose.yml` file to run the API and database in separate containers.

### 1. Build and start containers

```bash
docker-compose up --build
```

### 2. Access the services

* API: [http://localhost:8080/swagger](http://localhost:8080/swagger)
* Database: `localhost,1433` (SQL Server)

### 3. Apply EF migrations

Run migrations inside the API container:

```bash
docker-compose run api dotnet ef database update
```

### 4. Stop containers

```bash
docker-compose down
```

---

## 📜 License

MIT License

```

---

✅ Now the README is **organized, consistent, and production-ready**.  

👉 Do you also want me to add a **“Project Structure” section** showing the folder layout (`Controllers/`, `Models/`, `Data/`, etc.) so anyone opening the repo knows where to find things?
```
