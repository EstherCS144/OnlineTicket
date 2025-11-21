# OnlineTicket Platform - Installation Guide

## Overview
This guide covers the installation and setup of the OnlineTicket platform, an ASP.NET Core 7.0 web application with PostgreSQL backend for managing events, venues, bookings, and ticket sales.

## Prerequisites
- .NET 7.0 SDK
- PostgreSQL 12 or higher
- Git
- Visual Studio Code or Visual Studio 2022 (optional)

## Installation Steps

### 1. Clone the Repository
```bash
git clone <repository-url>
cd OnlineTicket
```

### 2. Database Setup

#### Using Replit (Recommended for Development)
The database is automatically created when you start the application:
- PostgreSQL is managed by Replit
- Connection string is stored in `DATABASE_URL` environment variable
- Migrations run automatically on startup

#### Using Local PostgreSQL
Create a local PostgreSQL database:
```bash
createdb onlineticket
```

Update `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=onlineticket;Username=postgres;Password=yourpassword;SSL Mode=Require;Trust Server Certificate=true"
  }
}
```

### 3. Install Dependencies
```bash
dotnet restore
```

### 4. Build the Project
```bash
dotnet build
```

### 5. Run the Application
```bash
dotnet run --project OnlineTicket.csproj
```

The application will start on `http://localhost:5000`

### 6. Seed Initial Data
On first run, the application automatically:
- Creates database schema
- Seeds roles: Admin, Organizer, Customer
- Creates admin user: `admin@gmail.com` / `Admin123!`

## Configuration

### appsettings.json
Key configuration options:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "your-connection-string"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

### Environment Variables
- `DATABASE_URL` - PostgreSQL connection string (set automatically on Replit)
- `ASPNETCORE_ENVIRONMENT` - Set to "Development" or "Production"

## Database Schema

### Core Tables
- **AspNetUsers** - User accounts with Identity
- **AspNetRoles** - Role definitions (Admin, Organizer, Customer)
- **AspNetUserRoles** - User-Role mapping

### Domain Tables
- **Categories** - Event categories
- **Venues** - Event venues
- **Events** - Events with organizer, venue, category
- **Customers** - Customer profiles linked to users
- **TicketTypes** - Different ticket classes per event
- **Bookings** - Customer bookings
- **Tickets** - Individual tickets with QR codes
- **Payments** - Payment records
- **Promotions** - Discount codes and promotional offers

### Relationships
```
User (Organizer) -> Events -> Bookings -> Tickets
                              -> Payments
User (Customer) -> Customers -> Bookings
                              -> Payments
Events -> TicketTypes -> Promotions
Events -> Bookings -> Promotions
```

## Admin Dashboard Access

1. Navigate to `/admin`
2. Login with admin account: `admin@gmail.com` / `Admin123!`
3. Access features:
   - **Dashboard** - View platform statistics
   - **Manage Events** - CRUD operations for events
   - **Manage Venues** - CRUD operations for venues
   - **Manage Organizers** - Approve/suspend organizers
   - **View Bookings** - Monitor bookings and process refunds
## Running in Production

### Build Release Version
```bash
dotnet publish -c Release -o ./publish
```

