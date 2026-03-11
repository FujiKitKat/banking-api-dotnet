# Banking API

REST API built with ASP.NET Core for managing banking accounts and clients.
Includes JWT authentication and protected endpoints.

## Technologies
Backend
- C#
- ASP.NET Core
- Entity Framework Core
- SQL Database

Authentication & API
- JWT Authentication
- REST API
- Swagger

DevOps
- Docker
- GitHub Actions
- Render

Testing
- xUnit

## Deployment
The project is deployed using Render.

Live API:
https://banking-api-dotnet-2.onrender.com/swagger/index.html

## Features
- JWT-based authentication
- secure login endpoint
- protected API endpoints
- create and manage bank accounts
- retrieve account information
- update account status
- create and manage clients
- RESTful API design
- Swagger API documentation

## API Documentation
The API is documented using Swagger.

Swagger UI:
https://banking-api-dotnet-2.onrender.com/swagger/index.html

## Project Structure
Controllers – API endpoints
Data – database context
Configuration – application configuration and settings
DTOs – request/response models
Middleware – custom request/response pipeline components
Migrations – Entity Framework database migrations
Models – entities
Repositories – data access layer
Services – business logic    
Validators – request validation logic

## Architecture
Client applications interact with the API through HTTP requests.  
The request flows through controllers, services, repositories and finally the database.

```mermaid
flowchart TD
    Client[Client Application] -->|HTTP Request| Middleware[Middleware]

    Middleware --> Controllers[Controllers]

    Controllers --> Validators[Validators]
    Validators --> Services[Services]

    Controllers --> Services

    Services --> Repositories[Repositories]

    Repositories --> Database[(SQL Database)]

