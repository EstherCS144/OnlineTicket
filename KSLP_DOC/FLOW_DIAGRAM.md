# OnlineTicket Platform - Flow Diagram & Architecture

## 1. System Process Flow

```
┌─────────────────────────────────────────────────────────────────┐
│                    ONLINETICKET PLATFORM                        │
└─────────────────────────────────────────────────────────────────┘

PHASE 1: INFRASTRUCTURE SETUP
┌──────────────────────────────────────────┐
│   Database & Roles Setup                 │
├──────────────────────────────────────────┤
│ • Create SQL Server Database             │
│ • Create Tables:                         │
│   - AspNetUsers, AspNetRoles             │
│   - Customers, Organizers                │
│   - Events, Venues, Categories           │
│   - Bookings, Tickets, TicketTypes       │
│   - Payments, Promotions                 │
│ • Configure ASP.NET Identity Roles:      │
│   - Admin, Organizer, Customer           │
│ • Seed Admin Account                     │
│   - Email: admin@gmail.com               │
│   - Password: Admin123!@#                │
└──────────────────────────────────────────┘
           ⬇️
PHASE 2: AUTHENTICATION & DASHBOARD
┌──────────────────────────────────────────┐
│    Admin Login & Dashboard                │
├──────────────────────────────────────────┤
│ • Admin Login Page                        │
│ • Role-based Access Control               │
│ • Dashboard Metrics:                      │
│   - Total Users                          │
│   - Total Active Events                  │
│   - Total Tickets Sold                   │
│   - Total Revenue (All Payments)         │
│   - Bookings Overview                    │
└──────────────────────────────────────────┘
           ⬇️
PHASE 3A: EVENT & VENUE MANAGEMENT
┌──────────────────────────────────────────┐
│  Event & Venue Management                │
├──────────────────────────────────────────┤
│ Events:                                   │
│ • List all events with filters           │
│ • Create new events                      │
│ • Edit event details                     │
│ • Delete events                          │
│ • Activate/Deactivate status             │
│                                           │
│ Venues:                                   │
│ • List all venues                        │
│ • Create new venues                      │
│ • Edit venue details                     │
│ • Delete venues                          │
└──────────────────────────────────────────┘
           ⬇️
PHASE 3B: USER MANAGEMENT
┌──────────────────────────────────────────┐
│    User Management                        │
├──────────────────────────────────────────┤
│ • View all users (Customers, Organizers) │
│ • Display user roles                     │
│ • Enable/Disable user accounts           │
│   (Lockout Functionality)                │
│ • View bookings per user                 │
│ • User profile information               │
└──────────────────────────────────────────┘
           ⬇️
PHASE 4: REPORTING & ANALYTICS
┌──────────────────────────────────────────┐
│    Reports & Analytics                   │
├──────────────────────────────────────────┤
│ Query Data:                               │
│ • Total tickets sold per event           │
│ • Total revenue per event                │
│ • Monthly revenue trends (6 months)      │
│ • Active users count                     │
│ • Booking statistics                     │
│                                           │
│ Display:                                  │
│ • Summary tables                         │
│ • Revenue breakdown by event             │
│ • Monthly trends                         │
│ • User activity metrics                  │
└──────────────────────────────────────────┘
           ⬇️
PHASE 5: DOCUMENTATION
┌──────────────────────────────────────────┐
│    Documentation & Deployment             │
├──────────────────────────────────────────┤
│ • Installation Guide                     │
│   - SQL Server setup                     │
│   - Environment variables                │
│   - Connection string format             │
│ • System Configuration Manual             │
│   - Architecture overview                │
│   - Feature documentation                │
│ • ER Diagram                             │
│   - All tables and relationships         │
│ • Class Diagram                          │
│   - Models, Controllers, Services        │
└──────────────────────────────────────────┘
```

## 2. Admin Dashboard Architecture

```
┌────────────────────────────────────────────────────────────┐
│                      USER INTERFACE                         │
├────────────────────────────────────────────────────────────┤
│                                                              │
│  Login Page ──► Role Check ──► Admin Role Verified         │
│       │                                                     │
│       └──► Redirect to Dashboard                           │
│                                                              │
│  ┌──────────────────────────────────────────────────────┐  │
│  │           ADMIN DASHBOARD                           │  │
│  ├──────────────────────────────────────────────────────┤  │
│  │                                                       │  │
│  │  ┌─────────────────────┐  ┌─────────────────────┐  │  │
│  │  │  Total Users        │  │  Total Events       │  │  │
│  │  │  (Count Users)      │  │  (Active Events)    │  │  │
│  │  └─────────────────────┘  └─────────────────────┘  │  │
│  │                                                       │  │
│  │  ┌─────────────────────┐  ┌─────────────────────┐  │  │
│  │  │ Tickets Sold        │  │ Total Revenue       │  │  │
│  │  │ (Sum Tickets)       │  │ (Sum Payments)      │  │  │
│  │  └─────────────────────┘  └─────────────────────┘  │  │
│  │                                                       │  │
│  ├──────────────────────────────────────────────────────┤  │
│  │ MANAGEMENT SECTIONS:                                 │  │
│  │                                                       │  │
│  │ [👥 User Management]     [📅 Event Management]       │  │
│  │ [📍 Venue Management]    [📊 Reports & Analytics]   │  │
│  └──────────────────────────────────────────────────────┘  │
└────────────────────────────────────────────────────────────┘
           ⬇️          ⬇️           ⬇️          ⬇️
    ┌──────────────────────────────────────────────────┐
    │         BUSINESS LOGIC LAYER (Controllers)      │
    ├──────────────────────────────────────────────────┤
    │ • AdminController (Dashboard stats)             │
    │ • AdminUsersController (User management)        │
    │ • AdminEventsController (Event CRUD)            │
    │ • AdminVenuesController (Venue CRUD)            │
    └──────────────────────────────────────────────────┘
           ⬇️
    ┌──────────────────────────────────────────────────┐
    │      SERVICE LAYER (Data Provider)              │
    ├──────────────────────────────────────────────────┤
    │ • IAdminDataProvider                            │
    │ • DatabaseAdminDataProvider                     │
    │   - GetDashboardStats()                         │
    │   - GetEventReports()                           │
    │   - GetMonthlyReports()                         │
    └──────────────────────────────────────────────────┘
           ⬇️
    ┌──────────────────────────────────────────────────┐
    │   DATA ACCESS LAYER (Entity Framework Core)     │
    ├──────────────────────────────────────────────────┤
    │ • ApplicationDbContext                          │
    │ • DbSets for all entities                       │
    │ • Relationships & Foreign Keys                  │
    └──────────────────────────────────────────────────┘
           ⬇️
    ┌──────────────────────────────────────────────────┐
    │     DATABASE LAYER (SQL Server)                 │
    ├──────────────────────────────────────────────────┤
    │ • AspNetUsers table                             │
    │ • AspNetRoles table                             │
    │ • Customers, Events, Venues                     │
    │ • Bookings, Tickets, Payments                   │
    │ • Categories, Promotions                        │
    └──────────────────────────────────────────────────┘
```

## 3. Data Flow Examples

### User Login Flow
```
User Input (Email/Password)
    ⬇️
ASP.NET Identity Validation
    ⬇️
Role Check (Admin?)
    ⬇️
✓ Authorized → Redirect to Dashboard
✗ Not Authorized → Show Access Denied
```

### Dashboard Statistics Flow
```
Admin Accesses Dashboard
    ⬇️
AdminController.Index() called
    ⬇️
IAdminDataProvider.GetDashboardStats()
    ⬇️
Entity Framework Queries:
• COUNT(Users)
• COUNT(Events) WHERE Status='Active'
• COUNT(Tickets)
• SUM(Payments.Amount)
    ⬇️
Results collected into DashboardStatsDto
    ⬇️
Display in View (ViewBag.Stats)
```

### Event Management Flow
```
Admin Clicks "Event Management"
    ⬇️
GET /admin/events
    ⬇️
AdminEventsController.List()
    ⬇️
Queries all events from database
    ⬇️
Pass to EventsList view
    ⬇️
User Actions:
├─ Create → POST /admin/events/create
├─ Edit → GET/POST /admin/events/{id}/edit
├─ Delete → POST /admin/events/{id}/delete
└─ Activate/Deactivate → POST /admin/events/{id}/toggle-status
```

### Report Generation Flow
```
Admin Clicks "Reports & Analytics"
    ⬇️
GET /admin/reports
    ⬇️
AdminController.Reports()
    ⬇️
IAdminDataProvider.GetEventReports()
   └─ GROUP BY Event, SUM(Tickets), SUM(Revenue)
    ⬇️
IAdminDataProvider.GetMonthlyReports()
   └─ GROUP BY Month, SUM(Revenue) for last 6 months
    ⬇️
Results passed to Reports view
    ⬇️
Display tables:
├─ Top Events by Revenue
├─ Top Events by Tickets Sold
└─ Monthly Revenue Trends
```

## 4. Key API Endpoints

### Authentication
- `GET /account/login` - Login page
- `POST /account/login` - Submit login
- `GET /account/logout` - Logout

### Admin Dashboard
- `GET /admin` - Dashboard home (with stats)
- `GET /admin/reports` - Reports page

### User Management
- `GET /admin/users` - List users
- `POST /admin/users/toggle-status/{id}` - Enable/Disable user
- `GET /admin/users/{id}/bookings` - View user bookings

### Event Management
- `GET /admin/events` - List events
- `GET /admin/events/create` - Create event form
- `POST /admin/events/create` - Submit new event
- `GET /admin/events/{id}/edit` - Edit event form
- `POST /admin/events/{id}/edit` - Submit event update
- `POST /admin/events/{id}/delete` - Delete event
- `POST /admin/events/{id}/toggle-status` - Activate/Deactivate

### Venue Management
- `GET /admin/venues` - List venues
- `GET /admin/venues/create` - Create venue form
- `POST /admin/venues/create` - Submit new venue
- `GET /admin/venues/{id}/edit` - Edit venue form
- `POST /admin/venues/{id}/edit` - Submit venue update
- `POST /admin/venues/{id}/delete` - Delete venue

## 5. Database Schema Summary

| Table | Purpose | Key Relationships |
|-------|---------|-------------------|
| AspNetUsers | User authentication | Parent for Customers, Events |
| AspNetRoles | Role definitions | Admin, Organizer, Customer |
| Customers | Customer profiles | 1:N with Bookings |
| Events | Event listings | 1:N with Bookings, Tickets |
| Venues | Event locations | 1:N with Events |
| Categories | Event types | 1:N with Events |
| Bookings | Customer reservations | N:1 with Customers, Events |
| Tickets | Individual tickets | N:1 with Bookings, Events |
| TicketTypes | Ticket classifications | 1:N with Tickets |
| Payments | Payment records | 1:1 with Bookings |
| Promotions | Discount codes | Optional with Bookings |

## 6. Technical Stack

```
Frontend:
├─ ASP.NET Core Razor Views
├─ Bootstrap 5
├─ HTML5/CSS3/JavaScript
└─ Responsive Design

Backend:
├─ ASP.NET Core 7.0
├─ Entity Framework Core
├─ ASP.NET Identity
└─ SQL Server

Database:
├─ SQL Server 2019+
├─ SSMS Compatible
└─ Azure SQL Database Support

Architecture:
├─ MVC Pattern
├─ Repository-like Pattern (Data Provider)
├─ Dependency Injection
└─ Role-Based Authorization
```

