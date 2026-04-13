# Online Travel Booking

A comprehensive travel booking platform built with .NET 9, featuring hotel reservations, flight bookings, car rentals, and guided tours. The application uses a clean architecture pattern with a layered approach for maintainability and scalability.

## Table of Contents

- [Overview](#overview)
- [Project Structure](#project-structure)
- [Features](#features)
- [Technologies](#technologies)
- [Architecture](#architecture)
- [Getting Started](#getting-started)
- [Database](#database)
- [API Endpoints](#api-endpoints)
- [Authentication](#authentication)
- [Payment Integration](#payment-integration)

## Overview

Online Travel Booking is a full-stack travel reservation system that allows users to:
- Book hotels and manage room reservations
- Search and book flights
- Rent cars for travel
- Discover and book guided tours
- Make secure payments
- Review and rate travel services

## Project Structure

The solution is organized into multiple projects following clean architecture principles:

### **OnlineTravelBooking.Domain**
- **Purpose**: Core business entities and domain logic
- **Contains**:
  - Entity models (Hotel, Flight, Car, Tour, User, Booking, etc.)
  - Enums for domain-specific types (RoomStatus, FlightStatus, BookingStatus, etc.)
  - Specifications and interfaces for business rules
  - Base entities with soft delete support

### **OnlineTravelBooking.Application**
- **Purpose**: Application logic, use cases, and DTOs
- **Contains**:
  - MediatR commands and query handlers
  - CQRS pattern implementation
  - AutoMapper profiles for DTO mappings
  - FluentValidation validators
  - Service interfaces and abstractions
  - Business logic orchestration

### **OnlineTravelBooking.Infrastructure**
- **Purpose**: External integrations and data persistence
- **Contains**:
  - Entity Framework Core DbContext
  - Database migrations
  - Repository implementations (Generic and specialized repos)
  - Payment service integration (Stripe)
  - Email service (MailKit)
  - Identity and JWT authentication
  - Redis caching
  - Soft delete interceptors

### **OnlineTravelBooking** (API Project)
- **Purpose**: REST API endpoints
- **Contains**:
  - API controllers for Hotels, Flights, Cars, Tours, Bookings
  - Payment webhooks handling
  - Authentication controllers
  - Review controllers
  - Global exception handling
  - MediatR request pipelines

### **OnlineTravelBooking.MVC**
- **Purpose**: Admin dashboard and management interface
- **Contains**:
  - ASP.NET Core MVC controllers
  - Razor views for CRUD operations
  - Admin panel for managing hotels, flights, cars, tours
  - Booking management interface
  - Authentication UI (Login/Register)

## Features

### Hotel Management
- Create, read, update, and delete hotels
- Manage hotel rooms with pricing and availability
- Hotel images and details
- Room inventory management
- StarRating system

### Flight Management
- Flight CRUD operations
- Seat availability tracking
- Multiple cabin classes (Economy, Business, First)
- Passenger management
- Flight booking with seat selection

### Car Rental
- Browse and manage car inventory
- Car brands and categories
- Rental pricing tiers
- Car availability and rental locations
- Car images and details

### Tours & Experiences
- Tour creation and management
- Tour categories and scheduling
- Tour images and descriptions
- Tour booking with schedule management
- Tour availability tracking

### Booking & Reservations
- Hotel room bookings with date selection
- Flight bookings with passenger information
- Car rental bookings
- Tour bookings with schedules
- Booking status tracking (Pending, Confirmed, Cancelled)

### Payment Processing
- Stripe integration for payment processing
- Payment confirmation and validation
- Refund processing
- Payment webhooks for transaction updates
- Multiple payment status handling

### User Management
- User registration and authentication
- JWT token-based authentication
- Email verification
- Password reset functionality
- OTP verification
- Role-based access control

### Reviews & Ratings
- Hotel review system
- Rating functionality
- User feedback management
- Review queries and retrieval

### Additional Features
- Search functionality for hotels and flights
- Pagination for large datasets
- Soft delete support for data entities
- Redis caching for performance optimization
- Global exception handling
- Email notifications (MailKit)
- Favorites/Wishlist functionality

## Technologies

### Core Framework
- **.NET 9**
- **ASP.NET Core** - Web framework

### Database & ORM
- **Entity Framework Core 9.0.0** - ORM
- **SQL Server** - Database
- **NetTopologySuite** - Spatial data support
- **EF Core Migrations** - Database versioning

### Architecture & Patterns
- **MediatR 14.0.0** - CQRS implementation
- **AutoMapper 16.0.0** - Object mapping
- **FluentValidation 12.1.1** - Input validation
- **Specification Pattern** - Advanced queries

### Authentication & Security
- **JWT Bearer** - Token-based authentication
- **ASP.NET Core Identity** - User management
- **User Secrets** - Secure configuration

### Payment & External Services
- **Stripe.net 50.1.0** - Payment processing
- **MailKit 4.14.1** - Email sending

### Caching
- **Redis** (StackExchangeRedis 9.0.0) - Distributed caching

### API & Documentation
- **Swashbuckle.AspNetCore 10.1.0** - Swagger/OpenAPI documentation

## Architecture

The project follows **Clean Architecture** with clear separation of concerns:

```
Domain Layer
    ?
Application Layer (Business Logic, DTOs, Specifications)
    ?
Infrastructure Layer (Data Access, External Services)
    ?
Presentation Layer (API & MVC Controllers)
```

### Design Patterns Used
- **Repository Pattern** - Data access abstraction
- **CQRS Pattern** - Separation of read and write operations
- **Specification Pattern** - Complex query building
- **Mediator Pattern** - Decoupled request handling
- **Dependency Injection** - IoC container

## Getting Started

### Prerequisites
- .NET 9 SDK
- SQL Server
- Redis (optional, for caching)
- Stripe account (for payment integration)

### Installation

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd OnlineTravelBooking
   ```

2. **Configure Database Connection**
   - Update connection string in `appsettings.json` in the API project

3. **Configure User Secrets**
   ```bash
   cd OnlineTravelBooking
   dotnet user-secrets set "Stripe:SecretKey" "your_stripe_secret_key"
   dotnet user-secrets set "Email:Password" "your_email_password"
   ```

4. **Apply Migrations**
   ```bash
   dotnet ef database update --project OnlineTravelBooking.Infrastructure
   ```

5. **Build the Solution**
   ```bash
   dotnet build
   ```

6. **Run the Application**
   
   For API:
   ```bash
   cd OnlineTravelBooking
   dotnet run
   ```
   
   For MVC:
   ```bash
   cd OnlineTravelBooking.MVC
   dotnet run
   ```

## Database

### Key Entities
- **ApplicationUser** - User accounts
- **Hotel, Room, HotelImage, RoomImage** - Accommodation data
- **Flight, FlightBooking, Passenger** - Flight management
- **Car, CarBrand, CarCategory, CarAvailability, CarPricingTier** - Car rental data
- **Tour, TourCategory, TourSchedule, BookingTour** - Tour management
- **Booking, BookingRoom, BaseBooking** - Reservation tracking
- **Payment** - Payment records
- **Review, Favourite** - User interactions

### Migrations
Database migrations are managed in the Infrastructure project under `Persistence/Migrations/`. Key migrations include:
- Initial database setup
- Soft delete support
- Flight and user updates
- Seat table management
- Base entity updates

## API Endpoints

### Authentication
- `POST /api/auth/register` - User registration
- `POST /api/auth/login` - User login
- `POST /api/auth/refresh` - Refresh JWT token
- `POST /api/auth/logout` - User logout
- `POST /api/auth/forgot-password` - Password reset request
- `POST /api/auth/reset-password` - Reset password
- `POST /api/auth/verify-email` - Email verification

### Hotels
- `GET /api/hotel` - List all hotels (paginated)
- `POST /api/hotel` - Create hotel
- `GET /api/hotel/{id}` - Get hotel details
- `PUT /api/hotel/{id}` - Update hotel
- `DELETE /api/hotel/{id}` - Delete hotel
- `POST /api/hotel/{id}/search` - Search hotels

### Flights
- `GET /api/flight` - List all flights
- `POST /api/flight` - Create flight
- `GET /api/flight/{id}` - Get flight details
- `PUT /api/flight/{id}` - Update flight
- `DELETE /api/flight/{id}` - Delete flight
- `POST /api/flightbookings` - Create flight booking

### Cars
- `GET /api/cars` - List all cars
- `POST /api/cars` - Create car
- `GET /api/cars/{id}` - Get car details
- `PUT /api/cars/{id}` - Update car
- `DELETE /api/cars/{id}` - Delete car

### Tours
- `GET /api/tours` - List all tours
- `POST /api/tours` - Create tour
- `POST /api/tours/{id}/book` - Book a tour

### Bookings
- `GET /api/bookings` - List all bookings
- `POST /api/bookings/room` - Book a room
- `POST /api/bookings/tour` - Book a tour
- `DELETE /api/bookings/{id}` - Cancel booking

### Payments
- `POST /api/payments/create` - Create payment intent
- `POST /api/payments/confirm` - Confirm payment
- `POST /api/webhooks/stripe` - Stripe webhook handler

### Reviews
- `POST /api/reviews/hotel` - Create hotel review
- `GET /api/reviews/hotel/{id}` - Get hotel reviews

## Authentication

The application uses **JWT (JSON Web Tokens)** for API authentication:

1. User registers/logs in
2. System generates JWT token with user claims
3. Token sent in request header: `Authorization: Bearer <token>`
4. Token includes user ID, email, and roles
5. Tokens can be refreshed for extended sessions

### User Roles
- **User** - Customer role for bookings
- **Admin** - Administrator role for content management

## Payment Integration

### Stripe Integration
- Secure payment processing via Stripe API
- Payment intent creation and confirmation
- Webhook handlers for transaction events
- Refund support
- Multiple payment status tracking:
  - Pending
  - Completed
  - Failed
  - Refunded

## Configuration

### Appsettings
Key configuration sections:
- **ConnectionStrings** - Database and Redis connections
- **Stripe** - Payment provider settings
- **Jwt** - Authentication token settings
- **Email** - SMTP configuration for notifications
- **Database** - Entity Framework settings

### User Secrets (Development)
Sensitive data stored in User Secrets:
- Stripe API keys
- Email credentials
- Database passwords
- JWT secrets

## Contributing

When contributing to this project:
1. Follow the existing code structure and patterns
2. Ensure all tests pass
3. Update documentation as needed
4. Use meaningful commit messages
5. Create feature branches from `main`

## License

This project is private and proprietary.

---

**Last Updated**: January 2025
**Framework Version**: .NET 9
