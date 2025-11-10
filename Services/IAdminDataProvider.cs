using OnlineTicket.Models.Admin;

namespace OnlineTicket.Services
{
    public interface IAdminDataProvider
    {
        DashboardStatsDto GetDashboardStats();
        List<OrganizerDto> GetOrganizers();
        List<EventDto> GetEvents();
        List<TicketDto> GetTickets();
        List<PromotionDto> GetPromotions();
        List<BookingDto> GetBookings();
        List<EventReportDto> GetEventReports();
        List<MonthlyReportDto> GetMonthlyReports();
        PlatformSettingsDto GetPlatformSettings();
        
        bool ApproveOrganizer(string organizerId);
        bool SuspendOrganizer(string organizerId);
        bool ApproveEvent(int eventId);
        bool RejectEvent(int eventId);
        bool TogglePromotion(int promotionId);
        bool ProcessRefund(int bookingId);
        bool UpdateSettings(PlatformSettingsDto settings);
    }
}
