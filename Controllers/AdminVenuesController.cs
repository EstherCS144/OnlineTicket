using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineTicket.Data;
using OnlineTicket.Models;

namespace OnlineTicket.Controllers;

[Authorize(Roles = "Admin")]
[Route("admin/venues")]
public class AdminVenuesController : Controller
{
    private readonly ApplicationDbContext _context;

    public AdminVenuesController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("")]
    public async Task<IActionResult> List()
    {
        var venues = await _context.Venues.ToListAsync();
        return View("~/Views/Admin/VenuesList.cshtml", venues);
    }

    [HttpGet("create")]
    public IActionResult Create()
    {
        return View("~/Views/Admin/VenuesCreate.cshtml");
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(Venue venue)
    {
        if (ModelState.IsValid)
        {
            _context.Venues.Add(venue);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Venue created successfully.";
            return RedirectToAction(nameof(List));
        }
        return View("~/Views/Admin/VenuesCreate.cshtml", venue);
    }

    [HttpGet("edit/{id}")]
    public async Task<IActionResult> Edit(int id)
    {
        var venue = await _context.Venues.FindAsync(id);
        if (venue == null) return NotFound();
        return View("~/Views/Admin/VenuesEdit.cshtml", venue);
    }

    [HttpPost("edit/{id}")]
    public async Task<IActionResult> Edit(int id, Venue venue)
    {
        if (id != venue.VenueId) return NotFound();
        if (ModelState.IsValid)
        {
            _context.Update(venue);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Venue updated successfully.";
            return RedirectToAction(nameof(List));
        }
        return View("~/Views/Admin/VenuesEdit.cshtml", venue);
    }

    [HttpPost("delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var venue = await _context.Venues.FindAsync(id);
        if (venue != null)
        {
            _context.Venues.Remove(venue);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Venue deleted successfully.";
        }
        return RedirectToAction(nameof(List));
    }
}
