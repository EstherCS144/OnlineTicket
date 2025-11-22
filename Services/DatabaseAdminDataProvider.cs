using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineTicket.Data;
using OnlineTicket.Models;
using OnlineTicket.Models.Admin;

namespace OnlineTicket.Services
{
    public class DatabaseAdminDataProvider : IAdminDataProvider
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public DatabaseAdminDataProvider(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public DashboardStatsDto GetDashboardStats()
        {
            var totalUsers = _context.Users.Count();
            var totalEvents = _context.Events.Count(e => e.Status == "Active");
            var totalTickets = _context.Tickets.Count();
            var totalRevenue = _context.Payments.Sum(p => p.Amount);

            return new DashboardStatsDto
            {
                TotalEvents = totalEvents,
                TotalBookings = _context.Bookings.Count(),
                TotalTicketsSold = totalTickets,
                TotalRevenue = totalRevenue,
                PendingApprovals = _context.Events.Count(e => e.Status == "Pending")
            };
        }

        public List<OrganizerDto> GetOrganizers()
        {
            var organizers = _context.Users
                .Join(_context.UserRoles, u => u.Id, ur => ur.UserId, (u, ur) => new { u, ur })
                .Join(_context.Roles.Where(r => r.Name == "Organizer"), ur_pair => ur_pair.ur.RoleId, r => r.Id, (ur_pair, r) => ur_pair.u)
                .Select(u => new OrganizerDto
                {
                    Id = u.Id,
                    Name = u.UserName ?? "Unknown",
                    Email = u.Email ?? "",
                    Status = "Approved",
                    EventsCount = _context.Events.Count(e => e.OrganizerId == u.Id),
                    RegisteredDate = DateTime.UtcNow
                })
                .ToList();

            return organizers;
        }

        public List<EventDto> GetEvents()
        {
            return _context.Events
                .Include(e => e.Category)
                .Include(e => e.Venue)
                .Include(e => e.Organizer)
                .Include(e => e.Bookings)
                .AsNoTracking()
                .Select(e => new EventDto
                {
                    EventId = e.EventId,
                    Title = e.Title,
                    EventDate = e.EventDate,
                    Venue = e.Venue.Name,
                    Category = e.Category.Name,
                    Organizer = e.Organizer.UserName ?? "Unknown",
                    TotalSeats = e.TotalSeats,
                    BookedSeats = e.Bookings.Count(),
                    Status = e.Status,
                    CreatedAt = e.CreatedAt
                })
                .ToList();
        }

        public List<TicketDto> GetTickets()
        {
            return _context.TicketTypes
                .Include(tt => tt.Event)
                .AsNoTracking()
                .Select(tt => new TicketDto
                {
                    TicketId = tt.TicketTypeId,
                    EventTitle = tt.Event.Title,
                    TicketType = tt.Name,
                    Price = tt.Price,
                    TotalSeats = tt.TotalSeats,
                    SoldSeats = _context.Tickets.Count(t => t.TicketTypeId == tt.TicketTypeId),
                    Revenue = _context.Tickets
                        .Where(t => t.TicketTypeId == tt.TicketTypeId)
                        .Join(_context.Payments, t => t.BookingId, p => p.BookingId, (t, p) => p.Amount)
                        .Sum()
                })
                .ToList();
        }

        public List<PromotionDto> GetPromotions()
        {
            return _context.Promotions
                .Include(p => p.Event)
                .AsNoTracking()
                .Select(p => new PromotionDto
                {
                    PromotionId = p.PromotionId,
                    Name = p.Name,
                    Code = p.Code,
                    DiscountPercentage = p.DiscountPercentage,
                    EventTitle = p.Event.Title,
                    StartDate = p.StartDate,
                    EndDate = p.EndDate,
                    IsActive = p.IsActive,
                    UsageCount = _context.Bookings.Count(b => b.PromotionId == p.PromotionId)
                })
                .ToList();
        }

        public List<BookingDto> GetBookings()
        {
            return _context.Bookings
                .Include(b => b.Customer)
                .Include(b => b.Event)
                .Include(b => b.Payment)
                .AsNoTracking()
                .Select(b => new BookingDto
                {
                    BookingId = b.BookingId,
                    CustomerName = b.Customer.FullName,
                    EventTitle = b.Event.Title,
                    BookingDate = b.CreatedAt,
                    TicketCount = _context.Tickets.Count(t => t.BookingId == b.BookingId),
                    TotalAmount = b.TotalAmount,
                    PaymentStatus = b.PaymentStatus,
                    PaymentProvider = b.Payment != null ? b.Payment.Provider : "N/A"
                })
                .OrderByDescending(b => b.BookingDate)
                .ToList();
        }

        public List<EventReportDto> GetEventReports()
        {
            return _context.Events
                .Include(e => e.Bookings)
                .Include(e => e.Tickets)
                .AsNoTracking()
                .Select(e => new EventReportDto
                {
                    EventTitle = e.Title,
                    TicketsSold = e.Tickets.Count(),
                    Revenue = e.Bookings.Sum(b => b.TotalAmount)
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
                var monthlyBookings = _context.Bookings
                    .Where(b => b.CreatedAt.Month == month.Month && b.CreatedAt.Year == month.Year)
                    .ToList();

                reports.Add(new MonthlyReportDto
                {
                    Month = month.ToString("MMM yyyy"),
                    Bookings = monthlyBookings.Count,
                    Revenue = monthlyBookings.Sum(b => b.TotalAmount)
                });
            }
            return reports;
        }

        public PlatformSettingsDto GetPlatformSettings()
        {
            return new PlatformSettingsDto
            {
                SiteName = "OnlineTicket Platform",
                SiteEmail = "support@onlineticket.com",
                MaintenanceMode = false,
                AllowRegistration = true,
                RequireEmailConfirmation = false
            };
        }

        public bool ApproveOrganizer(string organizerId)
        {
            var user = _context.Users.Find(organizerId);
            if (user != null)
            {
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool SuspendOrganizer(string organizerId)
        {
            var user = _context.Users.Find(organizerId);
            if (user != null)
            {
                user.LockoutEnabled = true;
                user.LockoutEnd = DateTime.UtcNow.AddYears(10);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool ApproveEvent(int eventId)
        {
            var evt = _context.Events.Find(eventId);
            if (evt != null)
            {
                evt.Status = "Active";
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool RejectEvent(int eventId)
        {
            var evt = _context.Events.Find(eventId);
            if (evt != null)
            {
                evt.Status = "Rejected";
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool TogglePromotion(int promotionId)
        {
            var promotion = _context.Promotions.Find(promotionId);
            if (promotion != null)
            {
                promotion.IsActive = !promotion.IsActive;
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool ProcessRefund(int bookingId)
        {
            var booking = _context.Bookings.Find(bookingId);
            if (booking != null)
            {
                booking.PaymentStatus = "Refunded";
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool UpdateSettings(PlatformSettingsDto settings)
        {
            return true;
        }
    }
}
