namespace OnlineTicket.Models.Admin
{
    public class DashboardStatsDto
    {
        public int TotalEvents { get; set; }
        public int TotalBookings { get; set; }
        public int TotalTicketsSold { get; set; }
        public decimal TotalRevenue { get; set; }
        public int PendingApprovals { get; set; }
    }

    public class OrganizerDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public int EventsCount { get; set; }
        public DateTime RegisteredDate { get; set; }
    }

    public class EventDto
    {
        public int EventId { get; set; }
        public string Title { get; set; }
        public DateTime EventDate { get; set; }
        public string Venue { get; set; }
        public string Category { get; set; }
        public string Organizer { get; set; }
        public int TotalSeats { get; set; }
        public int BookedSeats { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class TicketDto
    {
        public int TicketId { get; set; }
        public string EventTitle { get; set; }
        public string TicketType { get; set; }
        public decimal Price { get; set; }
        public int TotalSeats { get; set; }
        public int SoldSeats { get; set; }
        public decimal Revenue { get; set; }
    }

    public class PromotionDto
    {
        public int PromotionId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public decimal DiscountPercentage { get; set; }
        public string EventTitle { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public int UsageCount { get; set; }
    }

    public class BookingDto
    {
        public int BookingId { get; set; }
        public string CustomerName { get; set; }
        public string EventTitle { get; set; }
        public DateTime BookingDate { get; set; }
        public int TicketCount { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentStatus { get; set; }
        public string PaymentProvider { get; set; }
    }

    public class EventReportDto
    {
        public string EventTitle { get; set; }
        public int TicketsSold { get; set; }
        public decimal Revenue { get; set; }
    }

    public class MonthlyReportDto
    {
        public string Month { get; set; }
        public int Bookings { get; set; }
        public decimal Revenue { get; set; }
    }

    public class PlatformSettingsDto
    {
        public string SiteName { get; set; }
        public string SiteEmail { get; set; }
        public bool MaintenanceMode { get; set; }
        public bool AllowRegistration { get; set; }
        public bool RequireEmailConfirmation { get; set; }
    }
}
