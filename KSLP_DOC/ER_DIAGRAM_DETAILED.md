# OnlineTicket Platform - Entity Relationship Diagram (Detailed)

## 1. Complete ER Diagram (Crow's Foot Notation)

```
┌─────────────────────────────────────────────────────────────────────────┐
│                    ENTITY RELATIONSHIP DIAGRAM                          │
│                      (Crow's Foot Notation)                             │
└─────────────────────────────────────────────────────────────────────────┘

Legend:
─ One-to-One (1:1)
─ One-to-Many (1:N) where N has crow's foot
─ Many-to-Many (N:N) - when applicable


┌────────────────────────────────┐
│ AspNetUsers (PK: Id)           │
├────────────────────────────────┤
│ Id (PK)                        │
│ UserName (unique)              │
│ NormalizedUserName             │
│ Email                          │
│ NormalizedEmail                │
│ EmailConfirmed                 │
│ PasswordHash                   │
│ SecurityStamp                  │
│ ConcurrencyStamp               │
│ PhoneNumber                    │
│ PhoneNumberConfirmed           │
│ TwoFactorEnabled               │
│ LockoutEnd                     │
│ LockoutEnabled                 │
│ AccessFailedCount              │
└────────────────────────────────┘
           │ │
    ┌──────┘ │
    │        └────────────┐
    │                     │
    │ (1:1)              │ (1:N)
    │                     │
    ↓                     ↓
┌────────────────┐  ┌──────────────────────────────┐
│ Customers      │  │ AspNetUserRoles (Junction)  │
│ (PK: Custom...)│  │ (PK: UserId, RoleId)        │
├────────────────┤  ├──────────────────────────────┤
│ CustomerId (PK)│  │ UserId (FK) → AspNetUsers    │
│ UserId (FK)*   │  │ RoleId (FK) → AspNetRoles   │
│ FullName       │  └──────────────────────────────┘
│ PhoneNumber    │           ▲
│ CreatedAt      │           │ (N:1)
│ UpdatedAt      │           │
└────────────────┘  ┌──────────────────────┐
        │           │ AspNetRoles (PK: Id)│
        │           ├──────────────────────┤
        │ (1:N)     │ Id (PK)              │
        │           │ Name (unique)        │
        │           │ - Admin              │
        │           │ - Organizer          │
        │           │ - Customer           │
        │           │ NormalizedName       │
        │           │ ConcurrencyStamp     │
        │           └──────────────────────┘
        │
        ↓
┌──────────────────────────────────┐
│ Bookings (PK: BookingId)         │
├──────────────────────────────────┤
│ BookingId (PK)                   │
│ CustomerId (FK) → Customers      │
│ EventId (FK) → Events            │
│ TotalAmount (decimal)            │
│ PaymentStatus                    │
│ - Paid, Pending, Refunded        │
│ CreatedAt                        │
│ PromotionId (FK, nullable)       │
└──────────────────────────────────┘
        │ │
        │ └──────────────┐
    (1:1)│               │(1:N)
        │                │
        ↓                ↓
┌──────────────────────┐ ┌──────────────────────────┐
│ Payments             │ │ Tickets                  │
│ (PK: PaymentId)      │ │ (PK: TicketId)           │
├──────────────────────┤ ├──────────────────────────┤
│ PaymentId (PK)       │ │ TicketId (PK)            │
│ BookingId (FK)***    │ │ BookingId (FK)           │
│ Amount (decimal)     │ │ EventId (FK)             │
│ Provider             │ │ TicketTypeId (FK)        │
│ - Stripe             │ │ SeatNumber               │
│ - PayPal             │ │ QRCode (unique)          │
│ Reference            │ │ IsScanned (bool)         │
│ PaidAt (DateTime)    │ └──────────────────────────┘
└──────────────────────┘        │ │
                                │ └─────────┐
                            (N:1)│          │(N:1)
                                │          │
                                ↓          ↓
                        ┌──────────────┐ ┌──────────────────┐
                        │ Events       │ │ TicketTypes      │
                        │ (PK: EventId)│ │ (PK: TicketTypeId)
                        ├──────────────┤ ├──────────────────┤
                        │ EventId (PK) │ │ TicketTypeId (PK)│
                        │ Title        │ │ EventId (FK)*    │
                        │ Description  │ │ Name             │
                        │ EventDate    │ │ Price            │
                        │ TicketPrice  │ │ TotalSeats       │
                        │ CategoryId   │ │ SoldSeats        │
                        │ (FK) ────┐   │ └──────────────────┘
                        │ VenueId  │   │         △
                        │ (FK) ──┐ │   │         │ (1:N)
                        │ Orient │ │   │         │
                        │ Id(FK) │ │   │    ┌────┴──────────────────┐
                        │ Total  │ │   │    │ Promotions           │
                        │ Seats  │ │   │    │ (PK: PromotionId)    │
                        │ Status │ │   │    ├──────────────────────┤
                        │ -Active│ │   │    │ PromotionId (PK)     │
                        │ -Pend. │ │   │    │ Code (unique)        │
                        │ Create │ │   │    │ Discount%            │
                        │ At     │ │   │    │ ExpiryDate           │
                        └──────────┤   │    │ IsActive             │
                                  │   │    └──────────────────────┘
                              (N:1)│   │
                                  │   │
                          ┌───────┴───┴────────┐
                          │                    │
                          ↓                    ↓
                  ┌──────────────────┐  ┌──────────────────┐
                  │ Categories       │  │ Venues           │
                  │ (PK: CategoryId) │  │ (PK: VenueId)    │
                  ├──────────────────┤  ├──────────────────┤
                  │ CategoryId (PK)  │  │ VenueId (PK)     │
                  │ Name             │  │ Name             │
                  │ Description      │  │ Address          │
                  └──────────────────┘  │ City             │
                                        │ CreatedAt        │
                                        └──────────────────┘


* One Customer can have one associated AspNetUser account
*** One Booking must have exactly one Payment
  Diagram shows foreign key relationships with arrow direction

```

## 2. Relationship Summary Table

| From Table | To Table | Type | FK Column | Cardinality | Delete |
|-----------|----------|------|-----------|-------------|--------|
| Customers | AspNetUsers | 1:1 | UserId | 1 customer : 1 user | Cascade |
| Bookings | Customers | N:1 | CustomerId | N bookings : 1 customer | Cascade |
| Bookings | Events | N:1 | EventId | N bookings : 1 event | Restrict |
| Bookings | Promotions | N:1 | PromotionId | N bookings : 1 promotion | SetNull |
| Payments | Bookings | 1:1 | BookingId | 1 payment : 1 booking | Cascade |
| Tickets | Bookings | N:1 | BookingId | N tickets : 1 booking | Cascade |
| Tickets | Events | N:1 | EventId | N tickets : 1 event | Restrict |
| Tickets | TicketTypes | N:1 | TicketTypeId | N tickets : 1 type | Restrict |
| TicketTypes | Events | N:1 | EventId | N types : 1 event | Cascade |
| Events | Categories | N:1 | CategoryId | N events : 1 category | Restrict |
| Events | Venues | N:1 | VenueId | N events : 1 venue | Restrict |
| Events | AspNetUsers | N:1 | OrganizerId | N events : 1 organizer | Restrict |
| AspNetUserRoles | AspNetUsers | N:1 | UserId | N roles : 1 user | Cascade |
| AspNetUserRoles | AspNetRoles | N:1 | RoleId | N users : 1 role | Cascade |

## 3. SQL Server Table Definitions

```sql
-- Core Identity Tables (Auto-generated by EF Core)
CREATE TABLE AspNetUsers (
    Id nvarchar(450) PRIMARY KEY,
    UserName nvarchar(256),
    NormalizedUserName nvarchar(256) UNIQUE,
    Email nvarchar(256),
    NormalizedEmail nvarchar(256),
    EmailConfirmed bit,
    PasswordHash nvarchar(max),
    ...
);

CREATE TABLE AspNetRoles (
    Id nvarchar(450) PRIMARY KEY,
    Name nvarchar(256),
    NormalizedName nvarchar(256) UNIQUE,
    ...
);

CREATE TABLE AspNetUserRoles (
    UserId nvarchar(450) FOREIGN KEY REFERENCES AspNetUsers(Id),
    RoleId nvarchar(450) FOREIGN KEY REFERENCES AspNetRoles(Id),
    PRIMARY KEY (UserId, RoleId)
);

-- Custom Application Tables
CREATE TABLE Categories (
    CategoryId int PRIMARY KEY IDENTITY(1,1),
    Name nvarchar(100) NOT NULL UNIQUE,
    Description nvarchar(max)
);

CREATE TABLE Venues (
    VenueId int PRIMARY KEY IDENTITY(1,1),
    Name nvarchar(100) NOT NULL,
    Address nvarchar(200),
    City nvarchar(100),
    CreatedAt datetime2 DEFAULT GETUTCDATE()
);

CREATE TABLE Events (
    EventId int PRIMARY KEY IDENTITY(1,1),
    Title nvarchar(200) NOT NULL,
    Description nvarchar(max),
    EventDate datetime2,
    TicketPrice decimal(10,2),
    CategoryId int FOREIGN KEY REFERENCES Categories(CategoryId),
    VenueId int FOREIGN KEY REFERENCES Venues(VenueId),
    OrganizerId nvarchar(450) FOREIGN KEY REFERENCES AspNetUsers(Id),
    TotalSeats int,
    Status nvarchar(20), -- Active, Pending, Cancelled
    ImagePath nvarchar(500),
    CreatedAt datetime2 DEFAULT GETUTCDATE()
);

CREATE TABLE Customers (
    CustomerId int PRIMARY KEY IDENTITY(1,1),
    UserId nvarchar(450) FOREIGN KEY REFERENCES AspNetUsers(Id) ON DELETE CASCADE,
    FullName nvarchar(200),
    PhoneNumber nvarchar(20),
    CreatedAt datetime2 DEFAULT GETUTCDATE(),
    UpdatedAt datetime2
);

CREATE TABLE TicketTypes (
    TicketTypeId int PRIMARY KEY IDENTITY(1,1),
    EventId int FOREIGN KEY REFERENCES Events(EventId),
    Name nvarchar(100),
    Price decimal(10,2),
    TotalSeats int
);

CREATE TABLE Promotions (
    PromotionId int PRIMARY KEY IDENTITY(1,1),
    Code nvarchar(50) UNIQUE,
    DiscountPercentage int,
    ExpiryDate datetime2,
    IsActive bit DEFAULT 1
);

CREATE TABLE Bookings (
    BookingId int PRIMARY KEY IDENTITY(1,1),
    CustomerId int FOREIGN KEY REFERENCES Customers(CustomerId),
    EventId int FOREIGN KEY REFERENCES Events(EventId),
    TotalAmount decimal(10,2),
    PaymentStatus nvarchar(50), -- Paid, Pending, Refunded
    PromotionId int FOREIGN KEY REFERENCES Promotions(PromotionId),
    CreatedAt datetime2 DEFAULT GETUTCDATE()
);

CREATE TABLE Payments (
    PaymentId int PRIMARY KEY IDENTITY(1,1),
    BookingId int UNIQUE FOREIGN KEY REFERENCES Bookings(BookingId),
    Amount decimal(10,2),
    Provider nvarchar(50), -- Stripe, PayPal
    Reference nvarchar(100),
    PaidAt datetime2
);

CREATE TABLE Tickets (
    TicketId int PRIMARY KEY IDENTITY(1,1),
    BookingId int FOREIGN KEY REFERENCES Bookings(BookingId),
    EventId int FOREIGN KEY REFERENCES Events(EventId),
    TicketTypeId int FOREIGN KEY REFERENCES TicketTypes(TicketTypeId),
    SeatNumber nvarchar(50),
    QRCode nvarchar(255) UNIQUE,
    IsScanned bit DEFAULT 0
);
```

## 4. Data Integrity Constraints

```
Primary Keys:
├─ CategoryId (auto-increment)
├─ VenueId (auto-increment)
├─ EventId (auto-increment)
├─ CustomerId (auto-increment)
├─ BookingId (auto-increment)
├─ PaymentId (auto-increment)
├─ TicketId (auto-increment)
├─ TicketTypeId (auto-increment)
├─ PromotionId (auto-increment)
└─ Id (AspNetUsers, AspNetRoles)

Unique Constraints:
├─ AspNetUsers: UserName, NormalizedEmail
├─ AspNetRoles: Name, NormalizedName
├─ Categories: Name
├─ Promotions: Code
├─ Tickets: QRCode
└─ Payments: BookingId (one payment per booking)

NOT NULL Constraints:
├─ Events: Title, EventDate
├─ Categories: Name
├─ Venues: Name
├─ Customers: UserId, FullName
├─ Bookings: CustomerId, EventId, TotalAmount
├─ Payments: BookingId, Amount
└─ Tickets: BookingId, EventId, TicketTypeId

Foreign Key Constraints:
├─ CASCADE: Customers → AspNetUsers, Bookings → Customers, Tickets → Bookings
├─ RESTRICT: Events ← Bookings/Tickets, TicketTypes ← Tickets, 
│           Categories ← Events, Venues ← Events, Events ← AspNetUsers
└─ SET NULL: Bookings → Promotions
```

## 5. Indexing Strategy

```
Primary Key Indexes (Automatic):
├─ PK_Categories (CategoryId)
├─ PK_Venues (VenueId)
├─ PK_Events (EventId)
├─ PK_Customers (CustomerId)
├─ PK_Bookings (BookingId)
├─ PK_Payments (PaymentId)
├─ PK_Tickets (TicketId)
├─ PK_TicketTypes (TicketTypeId)
├─ PK_Promotions (PromotionId)
└─ PK_AspNetUsers (Id)

Foreign Key Indexes (Automatic):
├─ Customers.UserId
├─ Bookings.CustomerId
├─ Bookings.EventId
├─ Bookings.PromotionId
├─ Payments.BookingId
├─ Tickets.BookingId
├─ Tickets.EventId
├─ Tickets.TicketTypeId
├─ TicketTypes.EventId
├─ Events.CategoryId
├─ Events.VenueId
└─ Events.OrganizerId

Recommended Additional Indexes:
├─ Bookings.CreatedAt (for date range queries)
├─ Events.Status (for filtering active/pending)
├─ Payments.PaidAt (for payment reports)
└─ Tickets.QRCode (already unique)
```

## 6. Data Flow Examples

### Event Creation Data Flow
```
Admin Form → Event Model → DbContext.Events.Add()
→ INSERT INTO Events → EventId Generated
→ Redirect to Event List
```

### Booking & Payment Data Flow
```
Customer Booking → Booking Record Created
→ INSERT INTO Bookings (status: Pending)
→ Payment Processing (external system)
→ UPDATE Bookings SET PaymentStatus='Paid'
→ INSERT INTO Payments (1:1 with Booking)
→ INSERT INTO Tickets (1:N with Booking)
```

### User Enable/Disable Flow
```
Admin Action → AspNetUsers UPDATE
→ SET LockoutEnd = DateTime (disable) or NULL (enable)
→ SET LockoutEnabled = true
→ User cannot login until re-enabled
```


