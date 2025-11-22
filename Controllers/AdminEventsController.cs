using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineTicket.Data;
using OnlineTicket.Models;

namespace OnlineTicket.Controllers;

[Authorize(Roles = "Admin")]
[Route("admin/events")]
public class AdminEventsController : Controller
{
    private readonly ApplicationDbContext _context;

    public AdminEventsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("")]
    public async Task<IActionResult> List()
    {
        var events = await _context.Events
            .Include(e => e.Category)
            .Include(e => e.Venue)
            .Include(e => e.Organizer)
            .ToListAsync();
        return View("~/Views/Admin/EventsList.cshtml", events);
    }

    [HttpGet("create")]
    public async Task<IActionResult> Create()
    {
        ViewBag.Categories = await _context.Categories.ToListAsync();
        ViewBag.Venues = await _context.Venues.ToListAsync();
        return View("~/Views/Admin/EventsCreate.cshtml");
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(Event @event)
    {
        if (ModelState.IsValid)
        {
            _context.Events.Add(@event);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Event created successfully.";
            return RedirectToAction(nameof(List));
        }
        ViewBag.Categories = await _context.Categories.ToListAsync();
        ViewBag.Venues = await _context.Venues.ToListAsync();
        return View("~/Views/Admin/EventsCreate.cshtml", @event);
    }

    [HttpGet("edit/{id}")]
    public async Task<IActionResult> Edit(int id)
    {
        var @event = await _context.Events.FindAsync(id);
        if (@event == null) return NotFound();
        ViewBag.Categories = await _context.Categories.ToListAsync();
        ViewBag.Venues = await _context.Venues.ToListAsync();
        return View("~/Views/Admin/EventsEdit.cshtml", @event);
    }

    [HttpPost("edit/{id}")]
    public async Task<IActionResult> Edit(int id, Event @event)
    {
        if (id != @event.EventId) return NotFound();
        if (ModelState.IsValid)
        {
            _context.Update(@event);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Event updated successfully.";
            return RedirectToAction(nameof(List));
        }
        ViewBag.Categories = await _context.Categories.ToListAsync();
        ViewBag.Venues = await _context.Venues.ToListAsync();
        return View("~/Views/Admin/EventsEdit.cshtml", @event);
    }

    [HttpPost("delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var @event = await _context.Events.FindAsync(id);
        if (@event != null)
        {
            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Event deleted successfully.";
        }
        return RedirectToAction(nameof(List));
    }

    [HttpPost("toggle-status/{id}")]
    public async Task<IActionResult> ToggleStatus(int id)
    {
        var @event = await _context.Events.FindAsync(id);
        if (@event != null)
        {
            @event.Status = @event.Status == "Active" ? "Inactive" : "Active";
            await _context.SaveChangesAsync();
            TempData["Success"] = "Event status updated.";
        }
        return RedirectToAction(nameof(List));
    }
}
