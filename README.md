# NZWalks

![NZWalks Logo](https://img.shields.io/badge/NZWalks-.NET%207.0-blue)  
A web application for exploring, managing, and sharing beautiful walking tracks in New Zealand.

---

## Table of Contents

- [About the Project](#about-the-project)
- [Features](#features)
- [Tech Stack](#tech-stack)
- [Getting Started](#getting-started)
- [Prerequisites](#prerequisites)
- [Installation](#installation)
- [Database Setup](#database-setup)
- [Running the Application](#running-the-application)
- [Project Structure](#project-structure)
- [API Endpoints](#api-endpoints)
- [Contributing](#contributing)
- [License](#license)

---

## About the Project

**NZWalks** is a full-stack ASP.NET Core Web API project that allows users to discover, list, and manage walking tracks across New Zealand. The project is built with clean architecture principles and is designed to be scalable, maintainable, and easy to extend.

Typical use cases include:
- Listing all available walks and their details
- Filtering walks by region, length, or difficulty
- CRUD (Create, Read, Update, Delete) operations for admins
- User authentication and authorization
- Serving data to a frontend (SPA or mobile app)

---

## Features

- 🗺️ **Walk Listings:** Browse and filter a catalog of walks.
- 📍 **Regions:** Walks are categorized by region.
- 🌄 **Difficulty Levels:** Each walk has a difficulty (Easy, Medium, Hard).
- 📝 **CRUD Operations:** Create, update, and delete walks and regions (admin only).
- 🔒 **Authentication:** Secure endpoints using JWT authentication.
- 🧩 **RESTful API:** Well-documented API endpoints for integration.
- 📦 **Extensible:** Easy to add new features or integrate with frontend clients.

---

## Tech Stack

- **Backend:** C#, ASP.NET Core 7 Web API
- **ORM:** Entity Framework Core
- **Database:** SQL Server (can be changed to SQLite/InMemory for testing)
- **Authentication:** JWT Bearer Tokens
- **Frontend:** (Optional) Can be paired with React, Angular, Vue, or any client
- **Other:** Swagger/OpenAPI for documentation

---

## Getting Started

Follow these instructions to set up and run NZWalks locally.

### Prerequisites

- [.NET 7 SDK](https://dotnet.microsoft.com/download/dotnet/7.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (or use SQL Express/LocalDB)
- [Git](https://git-scm.com/)

### Installation

1. **Clone the repository:**
   ```bash
   git clone https://github.com/ahmed-tarek-2004/NZWalks.git
   cd NZWalks
   ```

2. **Restore NuGet packages:**
   ```bash
   dotnet restore
   ```

### Database Setup

1. **Configure the connection string:**

   - Open `appsettings.json` and edit the `DefaultConnection` string under `ConnectionStrings` to point to your SQL Server instance.

   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=YOUR_SERVER;Database=NZWalksDb;Trusted_Connection=True;MultipleActiveResultSets=true"
   }
   ```

2. **Run database migrations:**
   ```bash
   dotnet ef database update
   ```
   > _Note: If you don't have Entity Framework CLI tools, install them with:_
   > `dotnet tool install --global dotnet-ef`

### Running the Application

```bash
dotnet run
```

The API will be available at `https://localhost:5001/` by default.

- **Swagger UI:** [https://localhost:5146/swagger](https://localhost:5146/swagger)

---

## Project Structure

```
NZWalks/
├── Controllers/        # API endpoints
├── Data/               # DbContext and Migrations
├── Models/             # Entity Models
├── Repositories/       # Data access abstraction
├── Services/           # Business logic 
├── DTOs/               # Data Transfer Objects
├── Profiles/           # AutoMapper profiles
├── Middleware/         # Custom middleware (JWT, Exception handling)
├── Program.cs          # Entry point
├── appsettings.json    # Configuration
└── ...
```

---

## API Endpoints

Some example endpoints (full list available via Swagger):

- `GET /api/walks` - List all walks
- `GET /api/walks/{id}` - Get details of a specific walk
- `POST /api/walks` - Create a new walk (admin only)
- `PUT /api/walks/{id}` - Update a walk (admin only)
- `DELETE /api/walks/{id}` - Delete a walk (admin only)
- `GET /api/regions` - List all regions
- `POST /api/auth/login` - Obtain JWT token

> For full details and request/response schemas, use the built-in Swagger UI.

---

## Contributing

Contributions are welcome!  
To contribute:

1. Fork this repository
2. Create a new branch (`git checkout -b feature/your-feature`)
3. Commit your changes
4. Push to your branch (`git push origin feature/your-feature`)
5. Open a Pull Request

Please follow the existing code style and add tests where appropriate.

---
## Contact

For questions, suggestions, or support, please open an [issue](https://github.com/ahmed-tarek-2004/NZWalks/issues) or contact the repository owner.

---
