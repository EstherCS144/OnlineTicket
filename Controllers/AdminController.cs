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

    public IActionResult Reports()
    {
        var eventSales = _dataProvider.GetEventReports();
        var monthlySales = _dataProvider.GetMonthlyReports();

        ViewBag.EventSales = eventSales;
        ViewBag.MonthlySales = monthlySales;
        return View();
    }
}
