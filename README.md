Transaction API
A comprehensive .NET 8 Web API for managing commercial transactions, built with Clean Architecture, DDD principles, and CQRS pattern. This system handles sales records with complete CRUD operations, business rules validation, and event-driven architecture.

📋 Table of Contents

Overview
Features
Architecture
Prerequisites
Getting Started
API Documentation
Business Rules
Testing
Project Structure
Contributing

🎯 Overview
This API manages commercial transactions for Ambev's distribution network, handling sales records with customers (Business Partners), operational units (branches), products, and comprehensive business logic including discount calculations and cancellation workflows.

Key Entities

Commercial Transaction (Sale): Main transaction entity with code, date, amounts
Business Partner (Customer): External customers with denormalized data
Operational Unit (Branch): Sales locations/branches
Product: Available products with categories and pricing
Transaction Item: Individual line items with quantities, prices, and discounts

✨ Features

Core Functionality

✅ Complete CRUD Operations for commercial transactions
✅ Business Rules Validation (max 20 identical items, discount calculations)
✅ Item-level Operations (add, update, cancel individual items)
✅ Transaction Cancellation (full transaction or individual items)
✅ Automatic Discount Calculation based on quantity rules
✅ External Identity Pattern for cross-domain references

Technical Features

✅ Clean Architecture with DDD principles
✅ CQRS with MediatR
✅ JWT Authentication with role-based authorization
✅ Entity Framework Core with PostgreSQL
✅ AutoMapper for object mapping
✅ FluentValidation for input validation
✅ Swagger/OpenAPI documentation
✅ Docker Support with multi-container setup
✅ Comprehensive Unit Tests with xUnit and NSubstitute
✅ Event-Driven Architecture (TransactionCreated, ItemCancelled events)

🏗️ Architecture
├── 📁 Domain/              # Business entities, rules, and interfaces
├── 📁 Application/         # Use cases, commands, queries, handlers
├── 📁 Infrastructure/      # External concerns (EF, repositories)
├── 📁 WebApi/             # Controllers, DTOs, middleware
├── 📁 IoC/                # Dependency injection configuration
├── 📁 Common/             # Shared utilities and extensions
└── 📁 Tests/              # Unit and integration tests

🔧 Prerequisites

Before running this application, ensure you have:

Docker Desktop installed and running
.NET 8 SDK (for local development)
Git for cloning the repository

 Getting Started

1. Clone this Repository

git clone <your-github-repository-url>
cd ambev-developer-evaluation

2. Run with Docker (Recommended)

The easiest way to run the entire application stack:

# Start all services (API + PostgreSQL + MongoDB + Redis)
docker-compose up -d

# View logs
docker-compose logs 
This will start:

PostgreSQL: localhost:5432
MongoDB: localhost:27017
Redis: localhost:6379

3. Database Setup
The database will be automatically created with migrations and seed data when the application starts.

Default seed data includes:

Admin User: dev_admin / Admin@123 (for JWT authentication)
Business Partners: 2 sample distributors
Operational Units: 2 brewery locations
Products: 5 sample products (beers, soft drinks, water)

4. Alternative: Local Development
If you prefer to run locally without Docker:


# Start only the database services
docker-compose up -d ambev.developerevaluation.database

# Update connection string in appsettings.json if needed
# Run the application
cd src/Ambev.DeveloperEvaluation.WebApi
dotnet run

📚 API Documentation
Swagger UI
Once the application is running, access the interactive API documentation:

Swagger UI: http://localhost:8080/swagger
Authentication
The API uses JWT Bearer authentication. To access protected endpoints:

Login with the default admin user:

json

POST /api/auth/login
{
  "username": "dev_admin",
  "password": "Admin@123"
}

Use the token in subsequent requests:

Authorization: Bearer <your-jwt-token>

Main Endpoints
🔐 Authentication


POST   /api/auth/login           # Authenticate user
POST   /api/auth/register        # Register new user

📊 Transactions


POST   /api/transactions                           # Create transaction
GET    /api/transactions                           # List transactions (with filters)
GET    /api/transactions/{id}                      # Get transaction by ID
DELETE /api/transactions/{id}/cancel               # Cancel entire transaction
DELETE /api/transactions/{id}/items/{itemId}/cancel # Cancel specific item

Sample Request: Create Transaction
json


POST /api/transactions
{
  "transactionCode": "TXN-001",
  "businessPartner": {
    "externalId": "BP-001",
    "name": "Distribuidora São Paulo",
    "email": "contato@distribuidoraspaulo.com.br",
    "document": "12.345.678/0001-90"
  },
  "operationalUnit": {
    "externalId": "OU-SP-001",
    "name": "Cervejaria São Paulo",
    "location": "Avenida Paulista, 1000 - São Paulo, SP"
  },
  "items": [
    {
      "product": {
        "externalId": "P-BRAHMA-001",
        "name": "Brahma Chopp 600ml",
        "category": "Cerveja",
        "standardPrice": 5.50
      },
      "quantity": 12,
      "itemPrice": 5.50
    }
  ]
}

📋 Business Rules

Quantity Limits
Maximum 20 identical items per transaction
Validation occurs at both item and transaction level
Discount Rules

4-9 items: 10% discount
10-20 items: 20% discount
1-3 items: No discount
Discounts apply automatically based on total quantity
Transaction States

Active: Normal transaction state
Cancelled: Transaction is cancelled (no further modifications)
Item States
Active: Normal item state
Cancelled: Item is cancelled (excluded from totals)

🧪 Testing
Run Unit Tests

# Run all tests
dotnet test

# Run with coverage
dotnet test --collect:"XPlat Code Coverage"

# Run specific test project
dotnet test tests/Ambev.DeveloperEvaluation.Unit.Tests/
Test Coverage
The project includes comprehensive unit tests covering:

✅ Application Handlers (Create, Cancel, List transactions)
✅ Domain Entities and business rules
✅ Validation Logic
✅ Repository Patterns
✅ Error Scenarios
Test Data
Tests use Bogus library for generating realistic test data and NSubstitute for mocking dependencies.

📁 Project Structure
src/
├── Ambev.DeveloperEvaluation.Domain/
│   ├── Entities/                    # Domain entities
│   ├── Enums/                      # Domain enumerations
│   ├── Events/                     # Domain events
│   ├── Repositories/               # Repository interfaces
│   └── Validation/                 # Domain validation rules
│
├── Ambev.DeveloperEvaluation.Application/
│   ├── Transactions/               # Transaction use cases
│   │   ├── CreateCommercialTransaction/
│   │   ├── CancelTransaction/
│   │   ├── CancelTransactionItem/
│   │   ├── GetTransaction/
│   │   └── ListTransactions/
│   └── Users/                      # User management use cases
│
├── Ambev.DeveloperEvaluation.ORM/
│   ├── Configurations/             # EF entity configurations
│   ├── Migrations/                 # Database migrations
│   ├── Repositories/               # Repository implementations
│   └── Extensions/                 # Seed data and extensions
│
├── Ambev.DeveloperEvaluation.WebApi/
│   ├── Features/                   # Feature-based organization
│   │   ├── Transactions/           # Transaction controllers & DTOs
│   │   └── Users/                  # User controllers & DTOs
│   ├── Common/                     # Shared API components
│   └── Middleware/                 # Custom middleware
│
├── Ambev.DeveloperEvaluation.IoC/
│   └── DependencyInjection/        # DI container configuration
│
└── Ambev.DeveloperEvaluation.Common/
    ├── HealthChecks/               # Health check configurations
    ├── Logging/                    # Logging setup
    ├── Security/                   # JWT and security
    └── Validation/                 # Validation behaviors

🔧 Configuration

Environment Variables
The application uses the following configuration:


{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=developer_evaluation;Username=developer;Password=ev@luAt10n;"
  },
  "Jwt": {
    "SecretKey": "YourSuperSecretKeyForJwtTokenGenerationThatShouldBeAtLeast32BytesLong"
  }
}

Docker Services
Service	Port	Credentials
API	8080/8081	-
PostgreSQL	5432	developer / ev@luAt10n
MongoDB	27017	developer / ev@luAt10n
Redis	6379	Password: ev@luAt10n

��️ Development

Adding New Features
Domain First: Add entities and business rules in Domain/
Application Layer: Create commands/queries in Application/
Infrastructure: Implement repositories in ORM/
API Layer: Add controllers and DTOs in WebApi/
Tests: Write comprehensive unit tests
Database Migrations


# Add new migration
dotnet ef migrations add <MigrationName> --project src/Ambev.DeveloperEvaluation.ORM

# Update database
dotnet ef database update --project src/Ambev.DeveloperEvaluation.ORM

📝 Notes

External Identity Pattern: Used for referencing entities from other domains with denormalized data
Event-Driven: Domain events are published for transaction lifecycle changes
Clean Architecture: Strict separation of concerns with dependency inversion
CQRS: Command and Query separation for better scalability