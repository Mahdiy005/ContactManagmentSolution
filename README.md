
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
- **SQL Server LocalDB** (for local development)
- **SQL Server in Docker** (for containerized environment)

---

## 📦 Installation

### Prerequisites
Make sure you have installed:
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later  
- [SQL Server / LocalDB](https://learn.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb)  
- (Optional) [Docker Desktop](https://www.docker.com/products/docker-desktop) if you want to run with containers  

---

## ▶ Running App with Local Host

### 1. Clone the repository
```bash
git clone https://github.com/Mahdiy005/ContactManagmentSolution.git
cd ContactManagmentSolution
````

### 2. Install dependencies

```bash
dotnet restore
```

### 3. Configure database (Local)

By default, `appsettings.json` uses LocalDB:

```json
"ConnectionStrings": {
  "myQueryString": "Data Source=(localdb)\\ProjectModels;Initial Catalog=ContactDB;Integrated Security=True;Encrypt=False"
}
```

👉 This works when running with `dotnet run` on your local machine.

### 4. Apply EF Core migrations

```bash
dotnet ef database update
```

### 5. Run the project

```bash
dotnet run
```

* API: `https://localhost:5127`
* Swagger UI: `https://localhost:5127/swagger`

---

## 🐳 Running App with Docker

This project includes a `docker-compose.yml` file to run the API and database in separate containers.

### 1. Build and start containers

```bash
docker-compose up --build
```

### 2. Configure database (Docker)

When running in Docker, the API connects to the SQL Server container by **service name (`db`)**:

```json
"ConnectionStrings": {
  "myQueryString": "Server=db;Database=ContactDB;User=sa;Password=Your_strong_password123;TrustServerCertificate=True;MultipleActiveResultSets=true"
}
```

👉 This is different from LocalDB.
👉 Docker Compose automatically creates a private network, so `Server=db` resolves to the SQL Server container.

### 3. Apply EF migrations inside the container

```bash
docker-compose run api dotnet ef database update
```

### 4. Access the services

* API: [http://localhost:8080/swagger](http://localhost:8080/swagger)
* Database: `localhost,1433` (SQL Server)

### 5. Stop containers

```bash
docker-compose down
```

---

## 🔄 Switching Between Local & Docker

To avoid manually changing connection strings, you can use **environment-specific settings**:

* `appsettings.Development.json` → for local run (`dotnet run`)
* `appsettings.Docker.json` → for Docker run (`docker-compose`)

Example `docker-compose.yml` config:

```yaml
environment:
  - DOTNET_ENVIRONMENT=Docker
```

This tells ASP.NET Core to load `appsettings.Docker.json` automatically when running in Docker.

---

## 📡 API Endpoints

### 🔑 Auth

| Method | Endpoint             | Description       |
| ------ | -------------------- | ----------------- |
| POST   | `/api/auth/register` | Register new user |
| POST   | `/api/auth/login`    | Login and get JWT |

### 📒 Contacts (Authenticated only)

| Method | Endpoint             | Description                         |
| ------ | -------------------- | ----------------------------------- |
| POST   | `/api/contacts`      | Add new contact                     |
| GET    | `/api/contacts`      | List all contacts of logged-in user |
| GET    | `/api/contacts/{id}` | Get single contact                  |
| DELETE | `/api/contacts/{id}` | Delete contact                      |

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


