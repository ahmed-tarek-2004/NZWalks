🌏 NZWalks.API
NZWalks is a modular and well-structured ASP.NET Core Web API application built to handle information about walking trails and regions in New Zealand. It follows clean architecture principles, prioritizes maintainability, and is optimized for scalability and high performance.

⚡ Highlights
✔ Built with ASP.NET Core 9 for modern web API development
✔ Multiple API Versions supported (v1, v2)
✔ Integrated Entity Framework Core with SQL Server
✔ Implements Repository Pattern with Unit of Work for cleaner data access
✔ Dedicated Service Layer for business logic separation
✔ Redis Caching (v2) to improve response time and reduce DB calls
✔ JWT Authentication with role-based access control
✔ Advanced Query Features – Pagination, Filtering, Sorting
✔ Support for File Uploads
✔ Structured Logging via Serilog
✔ Built-in Health Check Endpoints (/health)
✔ Interactive API Docs using Swagger/OpenAPI
✔ Docker Support for containerized deployment
✔ Global Exception Handling via custom middleware
✔ Organized Dependency Injection through Extension Methods
✔ API Consumption example via ASP.NET MVC

🛠 Setup & Installation
Clone the repository

```bash
git clone https://github.com/ahmed-tarek-2004/NZWalks.git
cd NZWalks
Restore dependencies
```

```bash
dotnet restore
Update the database
```

```bash
dotnet ef database update
Run the application
```

```bash
dotnet run
```
📂 Project Structure
```java

NZWalks/
 ├── NZWalks.API         → API project (controllers, middleware, startup)
 ├── NZWalks.DataAccess  → EF Core DbContext, repositories, UoW
 ├── NZWalks.Services    → Business logic layer
 ├── NZWalks.Models      → Entities & DTOs
 └──  NZWalks.Cache       → Redis caching integration

```
