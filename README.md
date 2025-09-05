


A .NET 7 Web API for **user authentication (register/login)** and **personal contact management** using **ASP.NET Core Identity + JWT Authentication**.  
Each registered user can manage their own contacts securely.

---

##  Features
- User **registration** & **login** with hashed passwords.
- **JWT Authentication** for protecting endpoints.
- **Add, view, and manage contacts** for each authenticated user.
- Database powered by **Entity Framework Core (Code-First)**.
- **Swagger UI** for API testing.
- **CORS enabled** to allow frontend access.

---

##  Tech Stack
- **ASP.NET Core 8 Web API**
- **Entity Framework Core**
- **ASP.NET Core Identity**
- **JWT (JSON Web Tokens)**
- **SQL Server LocalDB**

---



##  Installation (Updated)

### 1. Prerequisites

Make sure you have installed:

* [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later
* [SQL Server / LocalDB](https://learn.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb)



### 2. Clone the repository

```bash
git clone https://github.com/Mahdiy005/ContactManagmentSolution.git
```


### 3. Install dependencies (NuGet packages)

If packages are missing, run:

```bash
dotnet restore
```

This will install all NuGet packages listed in the `.csproj` file.
The project depends on (at least):

* `Microsoft.EntityFrameworkCore`
* `Microsoft.EntityFrameworkCore.SqlServer`
* `Microsoft.EntityFrameworkCore.Tools`
* `Microsoft.AspNetCore.Identity.EntityFrameworkCore`
* `Microsoft.AspNetCore.Authentication.JwtBearer`
* `Swashbuckle.AspNetCore` (for Swagger)



### 4. Update database connection

Check `appsettings.json`:

```json
"ConnectionStrings": {
  "myQueryString": "Data Source=(localdb)\\ProjectModels;Initial Catalog=ContactDB;Integrated Security=True;Encrypt=False"
}
```

* Update this if you want to use another SQL Server instance.
* For Docker, you will need to replace with your container’s connection string.


### 5. Apply EF Core migrations

```bash
dotnet ef database update
```



### 6. Run the project

```bash
dotnet run
```

By default the API will run at:
 `https://localhost:5127`

Swagger UI will be available at:
 `https://localhost:5127/swagger`



---

##  API Endpoints

###  Auth

| Method | Endpoint             | Description       |
| ------ | -------------------- | ----------------- |
| POST   | `/api/auth/register` | Register new user |
| POST   | `/api/auth/login`    | Login and get JWT |

###  Contacts (Authenticated only)

| Method | Endpoint             | Description                         |
| ------ | -------------------- | ----------------------------------- |
| POST   | `/api/contacts`      | Add new contact                     |
| GET    | `/api/contacts`      | List all contacts of logged-in user |
| GET    | `/api/contacts/{id}` | Get single contact                  |
| DELETE | `/api/contacts/{id}` | Delete contact                      |

---

##  Database Schema

### Users (Identity)

* Id (int, PK)
* UserName
* Email
* PasswordHash
* (plus Identity fields)

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
````

### 2. Access the services

* API: [http://localhost:8080/swagger](http://localhost:8080/swagger)
* Database: localhost,1433 (SQL Server)

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

### 3. **Update Connection String Section**
In your `appsettings.json`, explain that **when running in Docker** the connection string should point to `Server=db;...` instead of `(localdb)`.






