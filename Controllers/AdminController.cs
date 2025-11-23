using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineTicket.Services;
using OnlineTicket.Models.Admin;

namespace OnlineTicket.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly IAdminDataProvider _dataProvider;

    public AdminController(IAdminDataProvider dataProvider)
    {
        _dataProvider = dataProvider;
    }

    public IActionResult Index()
    {
        var stats = _dataProvider.GetDashboardStats();
        ViewBag.Stats = stats;
        return View();
    }

    public IActionResult Organizers()
    {
        var organizers = _dataProvider.GetOrganizers();
        return View(organizers);
    }

    [HttpPost]
    public IActionResult ApproveOrganizer(string userId)
    {
        if (_dataProvider.ApproveOrganizer(userId))
        {
            TempData["Success"] = "Organizer approved successfully.";
        }
        return RedirectToAction(nameof(Organizers));
    }

    [HttpPost]
    public IActionResult SuspendOrganizer(string userId)
    {
        if (_dataProvider.SuspendOrganizer(userId))
        {
            TempData["Success"] = "Organizer suspended successfully.";
        }
        return RedirectToAction(nameof(Organizers));
    }

    public IActionResult Events()
    {
        var events = _dataProvider.GetEvents();
        return View(events);
    }

    [HttpPost]
    public IActionResult ApproveEvent(int eventId)
    {
        if (_dataProvider.ApproveEvent(eventId))
        {
            TempData["Success"] = "Event approved successfully.";
        }
        return RedirectToAction(nameof(Events));
    }

    [HttpPost]
    public IActionResult RejectEvent(int eventId)
    {
        if (_dataProvider.RejectEvent(eventId))
        {
            TempData["Success"] = "Event rejected.";
        }
        return RedirectToAction(nameof(Events));
    }

    public IActionResult Tickets()
    {
        var ticketStats = _dataProvider.GetTickets();
        ViewBag.TicketStats = ticketStats;
        return View();
    }

    public IActionResult Promotions()
    {
        var promotions = _dataProvider.GetPromotions();
        return View(promotions);
    }

    [HttpPost]
    public IActionResult TogglePromotion(int promotionId)
    {
        if (_dataProvider.TogglePromotion(promotionId))
        {
            TempData["Success"] = "Promotion status updated successfully.";
        }
        return RedirectToAction(nameof(Promotions));
    }

    public IActionResult Bookings()
    {
        var bookings = _dataProvider.GetBookings();
        return View(bookings);
    }

    [HttpPost]
    public IActionResult ProcessRefund(int bookingId)
    {
        if (_dataProvider.ProcessRefund(bookingId))
        {
            TempData["Success"] = "Refund processed successfully.";
        }
        return RedirectToAction(nameof(Bookings));
    }

    public IActionResult Reports()
    {
        var eventSales = _dataProvider.GetEventReports();
        var monthlySales = _dataProvider.GetMonthlyReports();

        ViewBag.EventSales = eventSales;
        ViewBag.MonthlySales = monthlySales;
        return View();
    }

    public IActionResult Settings()
    {
        var settings = _dataProvider.GetPlatformSettings();
        return View(settings);
    }

    [HttpPost]
    public IActionResult UpdateSettings(PlatformSettingsDto settings)
    {
        if (_dataProvider.UpdateSettings(settings))
        {
            TempData["Success"] = "Settings updated successfully.";
        }
        return RedirectToAction(nameof(Settings));
    }

}
