# TravelInsight Full

A demo project for a travel platform.

## Features

- ASP.NET Core 8 Web API
- React frontend with multiple pages
- Clean layered architecture
- CRUD for flight deals
- JWT authentication
- Role-based authorization
- OAuth demo endpoint
- CORS
- Cookies
- Memory cache and output cache
- Model validation
- Global exception middleware
- Rate limiting
- Health check endpoint
- EF Core InMemory database
- Pagination, sorting and filtering
- Soft delete
- Structured logs
- AI incident diagnostics demo
- Docker setup
- MSTest unit tests

## Demo users

Admin:

```text
admin@travelinsight.demo
Admin1234
```

User:

```text
user@travelinsight.demo
User1234
```

## Run backend

```powershell
cd src/TravelInsight.API
dotnet run
```

Open:

```text
https://localhost:5001/swagger
```

## Run frontend

Install Node.js LTS first.

```powershell
cd src/TravelInsight.Frontend
npm install
npm start
```

Open:

```text
http://localhost:3000
```

## Explanation

This project demonstrates production-style engineering, not just CRUD. It shows authentication, authorization, observability, validation, caching, reliability and full-stack UI workflows.
