using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineTicket.Data;
using OnlineTicket.Models;

namespace OnlineTicket.Controllers
{
    [Authorize(Roles = "Customer")]

    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        public CustomerController(ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        // Helper: create customer profile if missing
        private async Task<Customer> EnsureCustomerProfileAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return null;

            var customer = await _db.Customers.FirstOrDefaultAsync(c => c.UserId == user.Id);
            if (customer != null) return customer;

            customer = new Customer
            {
                UserId = user.Id,
                FullName = user.UserName ?? user.Email ?? "Unnamed",
                PhoneNumber = "",
                CreatedAt = DateTime.UtcNow
            };

            _db.Customers.Add(customer);
            await _db.SaveChangesAsync();

            return customer;
        }
        //View Profile
        public async Task<IActionResult> Profile()
        {
            var customer = await EnsureCustomerProfileAsync();
            if (customer == null) return NotFound();

            var user = await _userManager.GetUserAsync(User);

            var vm = new CustomerEditVM
            {
                CustomerId = customer.CustomerId,
                FullName = customer.FullName,
                PhoneNumber = customer.PhoneNumber,
                Email = user.Email,
                CreatedAt = customer.CreatedAt
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(CustomerEditVM vm)
        {
            if (!ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                vm.Email = user.Email;
                return View(vm);
            }

            var customer = await _db.Customers.FirstOrDefaultAsync(c => c.CustomerId == vm.CustomerId);
            if (customer == null) return NotFound();

            customer.FullName = vm.FullName.Trim();
            customer.PhoneNumber = string.IsNullOrWhiteSpace(vm.PhoneNumber) ? null : vm.PhoneNumber.Trim();
            customer.UpdatedAt = DateTime.UtcNow;

            _db.Customers.Update(customer);
            await _db.SaveChangesAsync();

            TempData["ProfileSuccess"] = "Profile updated successfully.";
            return RedirectToAction(nameof(Profile));
        }

    }
}
