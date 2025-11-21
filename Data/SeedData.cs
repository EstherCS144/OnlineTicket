using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineTicket.Data;
using OnlineTicket.Models;

public static class SeedData
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var services = scope.ServiceProvider;
            try
            {
                var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                var context = services.GetRequiredService<ApplicationDbContext>();

                await SeedAdminUserAndRoles(userManager, roleManager);
                await SeedDemoData(userManager, context);
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred while seeding the database.");
            }
        }
    }

    private static async Task SeedAdminUserAndRoles(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        string[] roles = { "Admin", "Organizer", "Customer" };
        foreach (var roleName in roles)
        {
            if (await roleManager.FindByNameAsync(roleName) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
                Console.WriteLine($"Role '{roleName}' created.");
            }
        }

        string adminEmail = "admin@gmail.com";
        string adminPassword = "Admin123!@#";

        if (await userManager.FindByEmailAsync(adminEmail) == null)
        {
            var adminUser = new IdentityUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };

            var createAdminResult = await userManager.CreateAsync(adminUser, adminPassword);

            if (createAdminResult.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
                Console.WriteLine($"Admin user '{adminEmail}' created and assigned to 'Admin' role.");
            }
            else
            {
                Console.WriteLine($"Error creating admin user: {string.Join(", ", createAdminResult.Errors.Select(e => e.Description))}");
            }
        }
    }

    private static async Task SeedDemoData(UserManager<IdentityUser> userManager, ApplicationDbContext context)
    {
        if (context.Categories.Any()) return; // Already seeded

        // Create Categories
        var categories = new List<Category>
        {
            new Category { Name = "Concert", Description = "Live music performances" },
            new Category { Name = "Theatre", Description = "Theatre performances and plays" },
            new Category { Name = "Conference", Description = "Business and tech conferences" },
            new Category { Name = "Sports", Description = "Sports events" }
        };
        context.Categories.AddRange(categories);
        await context.SaveChangesAsync();

        // Create Venues
        var venues = new List<Venue>
        {
            new Venue { Name = "Central Arena", Address = "123 Main St", City = "Colombo" },
            new Venue { Name = "Grand Hall", Address = "456 Park Ave", City = "Kandy" },
            new Venue { Name = "Convention Center", Address = "789 Business Blvd", City = "Galle" },
            new Venue { Name = "Outdoor Theatre", Address = "321 Green Lane", City = "Colombo" }
        };
        context.Venues.AddRange(venues);
        await context.SaveChangesAsync();

        // Create Demo Organizers
        var organizers = new List<string>();
        var organizerNames = new[] { "john.organizer@event.com", "jane.organizer@event.com", "mike.organizer@event.com" };
        
        foreach (var email in organizerNames)
        {
            if (await userManager.FindByEmailAsync(email) == null)
            {
                var user = new IdentityUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(user, "Demo123!@#");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Organizer");
                    organizers.Add(user.Id);
                }
            }
            else
            {
                var user = await userManager.FindByEmailAsync(email);
                organizers.Add(user.Id);
            }
        }

        // Create Demo Customers
        var customerEmails = new[] { "alice@customer.com", "bob@customer.com", "charlie@customer.com", "diana@customer.com" };
        var customers = new List<Customer>();

        foreach (var email in customerEmails)
        {
            if (await userManager.FindByEmailAsync(email) == null)
            {
                var user = new IdentityUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(user, "Demo123!@#");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Customer");
                    var customer = new Customer
                    {
                        UserId = user.Id,
                        FullName = email.Split('@')[0].ToUpper(),
                        PhoneNumber = $"+94-{new Random().Next(700000000, 799999999)}",
                        CreatedAt = DateTime.UtcNow
                    };
                    customers.Add(customer);
                }
            }
            else
            {
                var user = await userManager.FindByEmailAsync(email);
                var existing = await context.Customers.FirstOrDefaultAsync(c => c.UserId == user.Id);
                if (existing == null)
                {
                    var customer = new Customer
                    {
                        UserId = user.Id,
                        FullName = email.Split('@')[0].ToUpper(),
                        PhoneNumber = $"+94-{new Random().Next(700000000, 799999999)}",
                        CreatedAt = DateTime.UtcNow
                    };
                    customers.Add(customer);
                }
            }
        }
        context.Customers.AddRange(customers);
        await context.SaveChangesAsync();

        // Create Demo Events
        var events = new List<Event>
        {
            new Event
            {
                Title = "Summer Music Festival",
                EventDate = DateTime.UtcNow.AddMonths(2),
                TicketPrice = 50,
                Description = "A grand summer music festival featuring international artists",
                ImagePath = "/images/festival.jpg",
                CategoryId = categories[0].CategoryId,
                VenueId = venues[0].VenueId,
                OrganizerId = organizers[0],
                TotalSeats = 5000,
                Status = "Active",
                CreatedAt = DateTime.UtcNow.AddDays(-30)
            },
            new Event
            {
                Title = "Shakespeare in the Park",
                EventDate = DateTime.UtcNow.AddMonths(1),
                TicketPrice = 30,
                Description = "A classic Shakespeare performance in an outdoor setting",
                ImagePath = "/images/theatre.jpg",
                CategoryId = categories[1].CategoryId,
                VenueId = venues[3].VenueId,
                OrganizerId = organizers[1],
                TotalSeats = 800,
                Status = "Active",
                CreatedAt = DateTime.UtcNow.AddDays(-20)
            },
            new Event
            {
                Title = "Tech Conference 2025",
                EventDate = DateTime.UtcNow.AddMonths(3),
                TicketPrice = 200,
                Description = "Annual tech conference with industry leaders",
                ImagePath = "/images/conference.jpg",
                CategoryId = categories[2].CategoryId,
                VenueId = venues[2].VenueId,
                OrganizerId = organizers[2],
                TotalSeats = 2000,
                Status = "Active",
                CreatedAt = DateTime.UtcNow.AddDays(-10)
            },
            new Event
            {
                Title = "Jazz Night Live",
                EventDate = DateTime.UtcNow.AddDays(15),
                TicketPrice = 45,
                Description = "An evening of smooth jazz with live performances",
                ImagePath = "/images/jazz.jpg",
                CategoryId = categories[0].CategoryId,
                VenueId = venues[1].VenueId,
                OrganizerId = organizers[0],
                TotalSeats = 300,
                Status = "Active",
                CreatedAt = DateTime.UtcNow.AddDays(-5)
            }
        };
        context.Events.AddRange(events);
        await context.SaveChangesAsync();

        // Create Ticket Types
        var ticketTypes = new List<TicketType>
        {
            new TicketType { EventId = events[0].EventId, Name = "VIP", Price = 150, TotalSeats = 500 },
            new TicketType { EventId = events[0].EventId, Name = "Regular", Price = 50, TotalSeats = 4500 },
            new TicketType { EventId = events[1].EventId, Name = "Front Row", Price = 50, TotalSeats = 200 },
            new TicketType { EventId = events[1].EventId, Name = "General", Price = 30, TotalSeats = 600 },
            new TicketType { EventId = events[2].EventId, Name = "Early Bird", Price = 150, TotalSeats = 500 },
            new TicketType { EventId = events[2].EventId, Name = "Standard", Price = 200, TotalSeats = 1500 },
            new TicketType { EventId = events[3].EventId, Name = "Premium", Price = 60, TotalSeats = 100 },
            new TicketType { EventId = events[3].EventId, Name = "General", Price = 45, TotalSeats = 200 }
        };
        context.TicketTypes.AddRange(ticketTypes);
        await context.SaveChangesAsync();

        // Create Demo Bookings with Tickets and Payments
        var random = new Random();
        for (int i = 0; i < customers.Count; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                var eventIndex = random.Next(events.Count);
                var ticketType = ticketTypes.Where(t => t.EventId == events[eventIndex].EventId).First();
                var quantity = random.Next(1, 5);
                var totalAmount = quantity * events[eventIndex].TicketPrice;

                var booking = new Booking
                {
                    CustomerId = customers[i].CustomerId,
                    EventId = events[eventIndex].EventId,
                    CreatedAt = DateTime.UtcNow.AddDays(-random.Next(1, 30)),
                    TotalAmount = totalAmount,
                    PaymentStatus = "Paid",
                    PromotionId = null
                };
                context.Bookings.Add(booking);
                await context.SaveChangesAsync();

                // Create Payment
                var payment = new Payment
                {
                    BookingId = booking.BookingId,
                    Amount = totalAmount,
                    Provider = random.Next(2) == 0 ? "Stripe" : "PayPal",
                    Reference = $"TXN-{random.Next(100000, 999999)}",
                    PaidAt = booking.CreatedAt
                };
                context.Payments.Add(payment);

                // Create Tickets
                for (int k = 0; k < quantity; k++)
                {
                    var ticket = new Ticket
                    {
                        BookingId = booking.BookingId,
                        EventId = events[eventIndex].EventId,
                        TicketTypeId = ticketType.TicketTypeId,
                        SeatNumber = $"A{k + 1}",
                        QRCode = $"QR-{Guid.NewGuid().ToString().Substring(0, 8)}"
                    };
                    context.Tickets.Add(ticket);
                }
            }
        }
        await context.SaveChangesAsync();

        Console.WriteLine("Demo data seeded successfully!");
    }
}
