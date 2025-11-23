using OnlineTicket.Models.Admin;

namespace OnlineTicket.Services
{
    public class InMemoryAdminDataProvider : IAdminDataProvider
    {
        private readonly List<OrganizerDto> _organizers;
        private readonly List<EventDto> _events;
        private readonly List<TicketDto> _tickets;
        private readonly List<PromotionDto> _promotions;
        private readonly List<BookingDto> _bookings;
        private PlatformSettingsDto _settings;

        public InMemoryAdminDataProvider()
        {
            _organizers = InitializeOrganizers();
            _events = InitializeEvents();
            _tickets = InitializeTickets();
            _promotions = InitializePromotions();
            _bookings = InitializeBookings();
            _settings = InitializeSettings();
        }

        private List<OrganizerDto> InitializeOrganizers()
        {
            return new List<OrganizerDto>
            {
                new OrganizerDto { Id = "1", Name = "John Smith", Email = "john.smith@events.com", Status = "Approved", EventsCount = 5, RegisteredDate = DateTime.Now.AddMonths(-6) },
                new OrganizerDto { Id = "2", Name = "Sarah Johnson", Email = "sarah.j@concerts.com", Status = "Approved", EventsCount = 3, RegisteredDate = DateTime.Now.AddMonths(-4) },
                new OrganizerDto { Id = "3", Name = "Michael Chen", Email = "m.chen@theatre.com", Status = "Pending", EventsCount = 0, RegisteredDate = DateTime.Now.AddDays(-5) },
                new OrganizerDto { Id = "4", Name = "Emma Wilson", Email = "emma.w@showtime.com", Status = "Approved", EventsCount = 8, RegisteredDate = DateTime.Now.AddMonths(-8) },
                new OrganizerDto { Id = "5", Name = "David Brown", Email = "d.brown@festivals.com", Status = "Suspended", EventsCount = 2, RegisteredDate = DateTime.Now.AddMonths(-3) }
            };
        }

        private List<EventDto> InitializeEvents()
        {
            return new List<EventDto>
            {
                new EventDto { EventId = 1, Title = "Summer Music Festival 2025", EventDate = DateTime.Now.AddMonths(2), Venue = "Central Park Arena", Category = "Concert", Organizer = "John Smith", TotalSeats = 5000, BookedSeats = 3200, Status = "Active", CreatedAt = DateTime.Now.AddMonths(-1) },
                new EventDto { EventId = 2, Title = "Shakespeare in the Park", EventDate = DateTime.Now.AddMonths(1), Venue = "Outdoor Theatre", Category = "Theatre", Organizer = "Michael Chen", TotalSeats = 500, BookedSeats = 0, Status = "Pending", CreatedAt = DateTime.Now.AddDays(-3) },
                new EventDto { EventId = 3, Title = "Jazz Night Live", EventDate = DateTime.Now.AddDays(15), Venue = "Blue Note Club", Category = "Concert", Organizer = "Sarah Johnson", TotalSeats = 300, BookedSeats = 285, Status = "Active", CreatedAt = DateTime.Now.AddDays(-20) },
                new EventDto { EventId = 4, Title = "Tech Conference 2025", EventDate = DateTime.Now.AddMonths(3), Venue = "Convention Center", Category = "Conference", Organizer = "Emma Wilson", TotalSeats = 2000, BookedSeats = 1450, Status = "Active", CreatedAt = DateTime.Now.AddMonths(-2) },
                new EventDto { EventId = 5, Title = "Classical Symphony Night", EventDate = DateTime.Now.AddDays(30), Venue = "Grand Hall", Category = "Concert", Organizer = "John Smith", TotalSeats = 800, BookedSeats = 620, Status = "Active", CreatedAt = DateTime.Now.AddDays(-15) },
                new EventDto { EventId = 6, Title = "Comedy Show Extravaganza", EventDate = DateTime.Now.AddDays(-5), Venue = "Laugh Factory", Category = "Comedy", Organizer = "David Brown", TotalSeats = 400, BookedSeats = 400, Status = "Cancelled", CreatedAt = DateTime.Now.AddDays(-30) }
            };
        }

        private List<TicketDto> InitializeTickets()
        {
            return new List<TicketDto>
            {
                new TicketDto { TicketId = 1, EventTitle = "Summer Music Festival 2025", TicketType = "VIP", Price = 150.00m, TotalSeats = 500, SoldSeats = 320, Revenue = 48000.00m },
                new TicketDto { TicketId = 2, EventTitle = "Summer Music Festival 2025", TicketType = "Regular", Price = 50.00m, TotalSeats = 4500, SoldSeats = 2880, Revenue = 144000.00m },
                new TicketDto { TicketId = 3, EventTitle = "Jazz Night Live", TicketType = "Premium", Price = 80.00m, TotalSeats = 100, SoldSeats = 95, Revenue = 7600.00m },
                new TicketDto { TicketId = 4, EventTitle = "Jazz Night Live", TicketType = "General", Price = 45.00m, TotalSeats = 200, SoldSeats = 190, Revenue = 8550.00m },
                new TicketDto { TicketId = 5, EventTitle = "Tech Conference 2025", TicketType = "Early Bird", Price = 199.00m, TotalSeats = 500, SoldSeats = 500, Revenue = 99500.00m },
                new TicketDto { TicketId = 6, EventTitle = "Tech Conference 2025", TicketType = "Standard", Price = 299.00m, TotalSeats = 1500, SoldSeats = 950, Revenue = 284050.00m },
                new TicketDto { TicketId = 7, EventTitle = "Classical Symphony Night", TicketType = "Orchestra", Price = 100.00m, TotalSeats = 200, SoldSeats = 150, Revenue = 15000.00m },
                new TicketDto { TicketId = 8, EventTitle = "Classical Symphony Night", TicketType = "Balcony", Price = 60.00m, TotalSeats = 600, SoldSeats = 470, Revenue = 28200.00m }
            };
        }

        private List<PromotionDto> InitializePromotions()
        {
            return new List<PromotionDto>
            {
                new PromotionDto { PromotionId = 1, Name = "Early Bird Special", Code = "EARLY2025", DiscountPercentage = 20.00m, EventTitle = "Summer Music Festival 2025", StartDate = DateTime.Now.AddDays(-30), EndDate = DateTime.Now.AddDays(10), IsActive = true, UsageCount = 450 },
                new PromotionDto { PromotionId = 2, Name = "Student Discount", Code = "STUDENT20", DiscountPercentage = 15.00m, EventTitle = "Classical Symphony Night", StartDate = DateTime.Now.AddDays(-10), EndDate = DateTime.Now.AddDays(25), IsActive = true, UsageCount = 85 },
                new PromotionDto { PromotionId = 3, Name = "Group Booking", Code = "GROUP10", DiscountPercentage = 10.00m, EventTitle = "Tech Conference 2025", StartDate = DateTime.Now.AddDays(-20), EndDate = DateTime.Now.AddMonths(2), IsActive = true, UsageCount = 125 },
                new PromotionDto { PromotionId = 4, Name = "VIP Access", Code = "VIPJAZZ", DiscountPercentage = 25.00m, EventTitle = "Jazz Night Live", StartDate = DateTime.Now.AddDays(-15), EndDate = DateTime.Now.AddDays(5), IsActive = false, UsageCount = 35 },
                new PromotionDto { PromotionId = 5, Name = "Last Minute Deal", Code = "LASTMIN30", DiscountPercentage = 30.00m, EventTitle = "Shakespeare in the Park", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(20), IsActive = true, UsageCount = 0 }
            };
        }

        private List<BookingDto> InitializeBookings()
        {
            return new List<BookingDto>
            {
                new BookingDto { BookingId = 1, CustomerName = "Alice Cooper", EventTitle = "Summer Music Festival 2025", BookingDate = DateTime.Now.AddDays(-5), TicketCount = 2, TotalAmount = 300.00m, PaymentStatus = "Paid", PaymentProvider = "Stripe" },
                new BookingDto { BookingId = 2, CustomerName = "Bob Dylan", EventTitle = "Jazz Night Live", BookingDate = DateTime.Now.AddDays(-3), TicketCount = 4, TotalAmount = 320.00m, PaymentStatus = "Paid", PaymentProvider = "PayPal" },
                new BookingDto { BookingId = 3, CustomerName = "Charlie Parker", EventTitle = "Tech Conference 2025", BookingDate = DateTime.Now.AddDays(-10), TicketCount = 1, TotalAmount = 199.00m, PaymentStatus = "Paid", PaymentProvider = "Stripe" },
                new BookingDto { BookingId = 4, CustomerName = "Diana Ross", EventTitle = "Classical Symphony Night", BookingDate = DateTime.Now.AddDays(-2), TicketCount = 2, TotalAmount = 200.00m, PaymentStatus = "Pending", PaymentProvider = "Stripe" },
                new BookingDto { BookingId = 5, CustomerName = "Elvis Presley", EventTitle = "Summer Music Festival 2025", BookingDate = DateTime.Now.AddDays(-7), TicketCount = 6, TotalAmount = 900.00m, PaymentStatus = "Paid", PaymentProvider = "PayPal" },
                new BookingDto { BookingId = 6, CustomerName = "Freddie Mercury", EventTitle = "Jazz Night Live", BookingDate = DateTime.Now.AddDays(-1), TicketCount = 2, TotalAmount = 160.00m, PaymentStatus = "Refunded", PaymentProvider = "Stripe" }
            };
        }

        private PlatformSettingsDto InitializeSettings()
        {
            return new PlatformSettingsDto
            {
                SiteName = "OnlineTicket Platform",
                SiteEmail = "support@onlineticket.com",
                MaintenanceMode = false,
                AllowRegistration = true,
                RequireEmailConfirmation = true
            };
        }

        public DashboardStatsDto GetDashboardStats()
        {
            return new DashboardStatsDto
            {
                TotalEvents = _events.Count(e => e.Status == "Active"),
                TotalBookings = _bookings.Count,
                TotalTicketsSold = _tickets.Sum(t => t.SoldSeats),
                TotalRevenue = _tickets.Sum(t => t.Revenue),
                PendingApprovals = _events.Count(e => e.Status == "Pending")
            };
        }

        public List<OrganizerDto> GetOrganizers() => _organizers;
        public List<EventDto> GetEvents() => _events;
        public List<TicketDto> GetTickets() => _tickets;
        public List<PromotionDto> GetPromotions() => _promotions;
        public List<BookingDto> GetBookings() => _bookings;

        public List<EventReportDto> GetEventReports()
        {
            return _tickets
                .GroupBy(t => t.EventTitle)
                .Select(g => new EventReportDto
                {
                    EventTitle = g.Key,
                    TicketsSold = g.Sum(t => t.SoldSeats),
                    Revenue = g.Sum(t => t.Revenue)
                })
                .OrderByDescending(r => r.Revenue)
                .Take(10)
                .ToList();
        }

        public List<MonthlyReportDto> GetMonthlyReports()
        {
            var reports = new List<MonthlyReportDto>();
            for (int i = 5; i >= 0; i--)
            {
                var month = DateTime.Now.AddMonths(-i);
                var monthlyBookings = _bookings.Where(b => b.BookingDate.Month == month.Month && b.BookingDate.Year == month.Year).ToList();
                
                reports.Add(new MonthlyReportDto
                {
                    Month = month.ToString("MMM yyyy"),
                    Bookings = monthlyBookings.Count,
                    Revenue = monthlyBookings.Sum(b => b.TotalAmount)
                });
            }
            return reports;
        }

        public PlatformSettingsDto GetPlatformSettings() => _settings;

        public bool ApproveOrganizer(string organizerId)
        {
            var organizer = _organizers.FirstOrDefault(o => o.Id == organizerId);
            if (organizer != null)
            {
                organizer.Status = "Approved";
                return true;
            }
            return false;
        }

        public bool SuspendOrganizer(string organizerId)
        {
            var organizer = _organizers.FirstOrDefault(o => o.Id == organizerId);
            if (organizer != null)
            {
                organizer.Status = "Suspended";
                return true;
            }
            return false;
        }

        public bool ApproveEvent(int eventId)
        {
            var evt = _events.FirstOrDefault(e => e.EventId == eventId);
            if (evt != null)
            {
                evt.Status = "Active";
                return true;
            }
            return false;
        }

        public bool RejectEvent(int eventId)
        {
            var evt = _events.FirstOrDefault(e => e.EventId == eventId);
            if (evt != null)
            {
                evt.Status = "Rejected";
                return true;
            }
            return false;
        }

        public bool TogglePromotion(int promotionId)
        {
            var promotion = _promotions.FirstOrDefault(p => p.PromotionId == promotionId);
            if (promotion != null)
            {
                promotion.IsActive = !promotion.IsActive;
                return true;
            }
            return false;
        }

        public bool ProcessRefund(int bookingId)
        {
            var booking = _bookings.FirstOrDefault(b => b.BookingId == bookingId);
            if (booking != null)
            {
                booking.PaymentStatus = "Refunded";
                return true;
            }
            return false;
        }

        public bool UpdateSettings(PlatformSettingsDto settings)
        {
            _settings = settings;
            return true;
        }
    }
}
