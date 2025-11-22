# OnlineTicket Platform - Class Diagram & Architecture

## 1. Domain Models Architecture

```
┌─────────────────────────────────────────────────────────────────┐
│                    DOMAIN MODELS LAYER                          │
└─────────────────────────────────────────────────────────────────┘

┌──────────────────────────┐
│     IdentityUser         │ (ASP.NET Identity)
├──────────────────────────┤
│ - Id: string             │
│ - UserName: string       │
│ - Email: string          │
│ - PasswordHash: string   │
│ - LockoutEnd: DateTime?  │
│ - LockoutEnabled: bool   │
└──────────────────────────┘
         △ △ △
         │ │ └─── (1:1) ─→ Customer
         │ └────── (Many) → AspNetUserRole
         └──────── (1:N) ─→ Event (as OrganizerId)

┌──────────────────────────┐
│      Customer            │
├──────────────────────────┤
│ - CustomerId: int        │
│ - UserId: string (FK)    │
│ - FullName: string       │
│ - PhoneNumber: string    │
│ - CreatedAt: DateTime    │
│ - UpdatedAt: DateTime?   │
├──────────────────────────┤
│ Relationships:           │
│ + User: IdentityUser     │
│ + Bookings: List<Booking>│
└──────────────────────────┘
         │
         │ (1:N)
         ↓
┌──────────────────────────┐
│      Booking             │
├──────────────────────────┤
│ - BookingId: int         │
│ - CustomerId: int (FK)   │
│ - EventId: int (FK)      │
│ - TotalAmount: decimal   │
│ - PaymentStatus: string  │
│ - CreatedAt: DateTime    │
│ - PromotionId: int? (FK) │
├──────────────────────────┤
│ Relationships:           │
│ + Customer: Customer     │
│ + Event: Event           │
│ + Payment: Payment       │
│ + Tickets: List<Ticket>  │
│ + Promotion: Promotion?  │
└──────────────────────────┘
         │ │
         │ └────────────────┐
         │ (1:1)            │ (1:N)
         ↓                  ↓
┌──────────────────────────┐ ┌──────────────────────────┐
│      Payment             │ │      Ticket              │
├──────────────────────────┤ ├──────────────────────────┤
│ - PaymentId: int         │ │ - TicketId: int          │
│ - BookingId: int (FK)    │ │ - BookingId: int (FK)    │
│ - Amount: decimal        │ │ - EventId: int (FK)      │
│ - Provider: string       │ │ - TicketTypeId: int (FK) │
│ - Reference: string      │ │ - SeatNumber: string     │
│ - PaidAt: DateTime       │ │ - QRCode: string         │
├──────────────────────────┤ ├──────────────────────────┤
│ Relationships:           │ │ Relationships:           │
│ + Booking: Booking       │ │ + Booking: Booking       │
└──────────────────────────┘ │ + Event: Event           │
                             │ + TicketType: TicketType │
                             └──────────────────────────┘
                                     △
                                     │ (N:1)
                                     │
┌──────────────────────────┐        │
│      TicketType          │◄───────┘
├──────────────────────────┤
│ - TicketTypeId: int      │
│ - EventId: int (FK)      │
│ - Name: string           │
│ - Price: decimal         │
│ - TotalSeats: int        │
├──────────────────────────┤
│ Relationships:           │
│ + Event: Event           │
│ + Tickets: List<Ticket>  │
└──────────────────────────┘

┌──────────────────────────┐
│       Event              │
├──────────────────────────┤
│ - EventId: int           │
│ - Title: string          │
│ - Description: string    │
│ - EventDate: DateTime    │
│ - TicketPrice: decimal   │
│ - CategoryId: int (FK)   │
│ - VenueId: int (FK)      │
│ - OrganizerId: string    │
│ - TotalSeats: int        │
│ - Status: string         │
│ - ImagePath: string      │
│ - CreatedAt: DateTime    │
├──────────────────────────┤
│ Relationships:           │
│ + Category: Category     │
│ + Venue: Venue           │
│ + Organizer: IdentityUser│
│ + TicketTypes: List      │
│ + Bookings: List         │
│ + Tickets: List          │
└──────────────────────────┘
         △ △
         │ │
         │ └──────────────┐
         │ (N:1)          │ (N:1)
         │                │
┌──────────────────────────┐ ┌──────────────────────────┐
│      Category            │ │       Venue              │
├──────────────────────────┤ ├──────────────────────────┤
│ - CategoryId: int        │ │ - VenueId: int           │
│ - Name: string           │ │ - Name: string           │
│ - Description: string    │ │ - Address: string        │
├──────────────────────────┤ │ - City: string           │
│ Relationships:           │ │ - CreatedAt: DateTime    │
│ + Events: List<Event>    │ ├──────────────────────────┤
└──────────────────────────┘ │ Relationships:           │
                             │ + Events: List<Event>    │
                             └──────────────────────────┘

┌──────────────────────────┐
│     Promotion            │
├──────────────────────────┤
│ - PromotionId: int       │
│ - Code: string           │
│ - DiscountPercentage: int│
│ - ExpiryDate: DateTime   │
│ - IsActive: bool         │
├──────────────────────────┤
│ Relationships:           │
│ + Bookings: List<Booking>│
└──────────────────────────┘
```

## 2. Service & Controller Architecture

```
┌─────────────────────────────────────────────────────────────────┐
│                CONTROLLERS & SERVICES LAYER                     │
└─────────────────────────────────────────────────────────────────┘

┌──────────────────────────────────────┐
│      AdminController                 │
├──────────────────────────────────────┤
│ - _dataProvider: IAdminDataProvider   │
├──────────────────────────────────────┤
│ Methods:                             │
│ + Index(): IActionResult             │
│   └─ GET /admin                      │
│   └─ Returns Dashboard with stats    │
│ + Reports(): IActionResult           │
│   └─ GET /admin/reports              │
│   └─ Displays reports & analytics    │
└──────────────────────────────────────┘
         │
         │ (uses)
         ↓
┌──────────────────────────────────────┐
│   IAdminDataProvider Interface       │
├──────────────────────────────────────┤
│ Methods:                             │
│ + GetDashboardStats(): DashboardStats│
│ + GetEventReports(): List<EventReport>
│ + GetMonthlyReports(): List<Monthly> │
│ + GetOrganizers(): List<OrganizerDto>│
│ + GetEvents(): List<EventDto>        │
│ + GetUsers(): List<UserDto>          │
│ + GetBookings(): List<BookingDto>    │
└──────────────────────────────────────┘
         △
         │ (implemented by)
         │
┌──────────────────────────────────────┐
│ DatabaseAdminDataProvider            │
├──────────────────────────────────────┤
│ - _context: ApplicationDbContext     │
├──────────────────────────────────────┤
│ Implementation:                      │
│ + GetDashboardStats()                │
│   ├─ COUNT Users                     │
│   ├─ COUNT Active Events             │
│   ├─ COUNT Tickets                   │
│   └─ SUM Payments Amount             │
│ + GetEventReports()                  │
│   └─ GROUP Events, SUM Tickets/Rev   │
│ + GetMonthlyReports()                │
│   └─ GROUP by Month, SUM Revenue     │
└──────────────────────────────────────┘
         │
         │ (uses)
         ↓
┌──────────────────────────────────────┐
│  ApplicationDbContext                │
├──────────────────────────────────────┤
│ - options: DbContextOptions          │
├──────────────────────────────────────┤
│ DbSets:                              │
│ + Events: DbSet<Event>               │
│ + Bookings: DbSet<Booking>           │
│ + Tickets: DbSet<Ticket>             │
│ + Customers: DbSet<Customer>         │
│ + Payments: DbSet<Payment>           │
│ + TicketTypes: DbSet<TicketType>     │
│ + Venues: DbSet<Venue>               │
│ + Categories: DbSet<Category>        │
│ + Promotions: DbSet<Promotion>       │
├──────────────────────────────────────┤
│ Methods:                             │
│ # OnModelCreating(builder)           │
│   ├─ Configure relationships         │
│   ├─ Set foreign keys                │
│   └─ Configure cascade rules         │
└──────────────────────────────────────┘

┌──────────────────────────────────────┐
│  AdminUsersController                │
├──────────────────────────────────────┤
│ - _context: ApplicationDbContext     │
│ - _userManager: UserManager          │
├──────────────────────────────────────┤
│ Methods:                             │
│ + Index(): IActionResult             │
│ + ToggleStatus(id): IActionResult    │
│ + ViewBookings(id): IActionResult    │
└──────────────────────────────────────┘

┌──────────────────────────────────────┐
│  AdminEventsController               │
├──────────────────────────────────────┤
│ - _context: ApplicationDbContext     │
├──────────────────────────────────────┤
│ Methods:                             │
│ + List(): IActionResult              │
│ + Create(): IActionResult (GET/POST) │
│ + Edit(id): IActionResult (GET/POST) │
│ + Delete(id): IActionResult (POST)   │
│ + ToggleStatus(id): IActionResult    │
└──────────────────────────────────────┘

┌──────────────────────────────────────┐
│  AdminVenuesController               │
├──────────────────────────────────────┤
│ - _context: ApplicationDbContext     │
├──────────────────────────────────────┤
│ Methods:                             │
│ + List(): IActionResult              │
│ + Create(): IActionResult (GET/POST) │
│ + Edit(id): IActionResult (GET/POST) │
│ + Delete(id): IActionResult (POST)   │
└──────────────────────────────────────┘
```

## 3. Data Transfer Objects (DTOs)

```
┌─────────────────────────────────────────────────────────────────┐
│            DATA TRANSFER OBJECTS (DTOs)                         │
└─────────────────────────────────────────────────────────────────┘

DashboardStatsDto
├─ TotalUsers: int
├─ TotalEvents: int
├─ TotalBookings: int
├─ TotalTicketsSold: int
├─ TotalRevenue: decimal
└─ PendingApprovals: int

EventReportDto
├─ EventId: int
├─ EventTitle: string
├─ TicketsSold: int
├─ TotalRevenue: decimal
└─ Status: string

MonthlyReportDto
├─ Month: string
├─ Revenue: decimal
├─ BookingsCount: int
└─ TicketsCount: int

UserDto
├─ Id: string
├─ UserName: string
├─ Email: string
├─ Role: string
├─ BookingCount: int
├─ IsLocked: bool
└─ LockoutEnd: DateTime?

EventDto
├─ EventId: int
├─ Title: string
├─ EventDate: DateTime
├─ TicketPrice: decimal
├─ VenueName: string
├─ CategoryName: string
├─ Status: string
└─ TotalSeats: int

VenueDto
├─ VenueId: int
├─ Name: string
├─ Address: string
├─ City: string
└─ EventCount: int
```

## 4. Dependency Injection Setup

```
┌─────────────────────────────────────────────────────────────────┐
│                 DEPENDENCY INJECTION CHAIN                      │
└─────────────────────────────────────────────────────────────────┘

Program.cs Registrations:
├─ DbContext<ApplicationDbContext>
│  └─ options.UseSqlServer(connectionString)
│
├─ Identity
│  ├─ UserManager<IdentityUser>
│  ├─ RoleManager<IdentityRole>
│  └─ SignInManager<IdentityUser>
│
├─ IAdminDataProvider
│  └─ DatabaseAdminDataProvider (Scoped)
│
└─ Controllers & Views
   └─ Razor Pages support

Runtime Flow:
Request → Controller Constructor
         → IAdminDataProvider injected
         → ApplicationDbContext injected
         → Controller Method Executes
         → Query Executed against SQL Server
         → Results Returned
         → View Rendered
```

## 5. Authentication & Authorization Flow

```
┌─────────────────────────────────────────────────────────────────┐
│        AUTHENTICATION & AUTHORIZATION FLOW                      │
└─────────────────────────────────────────────────────────────────┘

User Access Attempt → /account/login
        ↓
┌─────────────────────────────────────┐
│ Is user authenticated?              │
├─────────────────────────────────────┤
│ No  → Show Login Form               │
│       ↓                             │
│ Enter credentials                   │
│       ↓                             │
│ SignInManager.PasswordSignInAsync()  │
│       ↓                             │
│ Valid? YES → Set Auth Cookie        │
│ Valid? NO  → Show Error             │
│       ↓                             │
└─────────────────────────────────────┘
        ↓ (Authenticated)
User can access protected pages
        ↓
Attempt Access → /admin (Admin-only area)
        ↓
┌─────────────────────────────────────┐
│ Check [Authorize(Roles="Admin")]    │
├─────────────────────────────────────┤
│ Is user in Admin role?              │
│ YES → Grant Access                  │
│ NO  → Show Access Denied (403)      │
└─────────────────────────────────────┘
        ↓
Admin Dashboard Loads
        ↓
Display all management features
```

## 6. Complete Request/Response Cycle

```
┌──────────────────────────────────────────────────────────────────┐
│             COMPLETE REQUEST/RESPONSE CYCLE                      │
└──────────────────────────────────────────────────────────────────┘

Example: GET /admin/events

1. HTTP Request
   └─ GET /admin/events

2. Route Matching
   └─ Matches to AdminEventsController.List()

3. Authorization Check
   └─ [Authorize(Roles="Admin")] validates role

4. Controller Execution
   └─ AdminEventsController.List()
      ├─ Inject ApplicationDbContext
      ├─ Query: context.Events.ToListAsync()
      └─ Pass results to view

5. Database Query
   └─ SELECT * FROM Events

6. Entity Framework Mapping
   └─ Database rows → Event objects

7. View Rendering
   └─ Events.cshtml
      ├─ Loop through events
      ├─ Generate HTML
      └─ Include action buttons (Edit/Delete)

8. HTTP Response
   └─ 200 OK with HTML content

9. Browser Rendering
   └─ Display events table
```

## 7. Exception Handling Flow

```
┌──────────────────────────────────────────────────────────────────┐
│              EXCEPTION HANDLING ARCHITECTURE                     │
└──────────────────────────────────────────────────────────────────┘

Unhandled Exception Occurs
        ↓
┌──────────────────────────────────────┐
│ Middleware Exception Handler         │
├──────────────────────────────────────┤
│ app.UseExceptionHandler()            │
│ app.UseMigrationsEndPoint()          │
└──────────────────────────────────────┘
        ↓
┌──────────────────────────────────────┐
│ Environment Check                    │
├──────────────────────────────────────┤
│ Development?                         │
│ YES → Show detailed error            │
│ NO  → Show generic error page        │
└──────────────────────────────────────┘
        ↓
Error Page Rendered
└─ Stack trace (development only)
└─ Error message
└─ Status code
```

