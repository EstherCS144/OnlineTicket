using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineTicket.Data;
using OnlineTicket.Models;

namespace OnlineTicket.Controllers;

[Authorize(Roles = "Admin")]
[Route("admin/users")]
public class AdminUsersController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public AdminUsersController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var users = await _userManager.Users.ToListAsync();
        var userList = new List<dynamic>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.UserId == user.Id);
            var bookingCount = customer != null ? _context.Bookings.Count(b => b.CustomerId == customer.CustomerId) : 0;

            userList.Add(new
            {
                user.Id,
                user.UserName,
                user.Email,
                Role = string.Join(", ", roles),
                BookingCount = bookingCount,
                IsLocked = user.LockoutEnabled && user.LockoutEnd > DateTime.UtcNow,
                user.LockoutEnd
            });
        }

        return View("~/Views/Admin/UsersList.cshtml", userList);
    }

    [HttpPost("toggle-status/{id}")]
    public async Task<IActionResult> ToggleStatus(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user != null)
        {
            if (user.LockoutEnabled && user.LockoutEnd > DateTime.UtcNow)
            {
                user.LockoutEnd = null;
                TempData["Success"] = "User account enabled successfully.";
            }
            else
            {
                user.LockoutEnabled = true;
                user.LockoutEnd = DateTime.UtcNow.AddYears(10);
                TempData["Success"] = "User account disabled successfully.";
            }
            await _userManager.UpdateAsync(user);
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("{id}/bookings")]
    public async Task<IActionResult> ViewBookings(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return NotFound();

        var customer = await _context.Customers.FirstOrDefaultAsync(c => c.UserId == id);
        if (customer == null)
        {
            return View("~/Views/Admin/UserBookings.cshtml", new { UserName = user.UserName, Bookings = new List<Booking>() });
        }

        var bookings = await _context.Bookings
            .Include(b => b.Event)
            .Include(b => b.Payment)
            .Where(b => b.CustomerId == customer.CustomerId)
            .ToListAsync();

        ViewBag.UserName = user.UserName;
        return View("~/Views/Admin/UserBookings.cshtml", bookings);
    }
}
