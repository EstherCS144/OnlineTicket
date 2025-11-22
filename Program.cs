using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineTicket.Data;

var builder = WebApplication.CreateBuilder(args);


//builder.WebHost.ConfigureKestrel(serverOptions =>
//{
//    serverOptions.ListenAnyIP(5000);
//});

//// Add services to the container.
//var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL");
//if (string.IsNullOrEmpty(connectionString))
//{
//    connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
//        ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
//}
//else
//{
//    var databaseUri = new Uri(connectionString);
//    var userInfo = databaseUri.UserInfo.Split(':');
//    var port = databaseUri.Port > 0 ? databaseUri.Port : 5432;
//    connectionString = $"Host={databaseUri.Host};Port={port};Database={databaseUri.LocalPath.TrimStart('/')};Username={userInfo[0]};Password={userInfo[1]};SSL Mode=Require;Trust Server Certificate=true";
//}

//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//// Add services to the container.
//var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL") 
//    ?? builder.Configuration.GetConnectionString("DefaultConnection") 
//    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' or 'DATABASE_URL' environment variable not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<OnlineTicket.Services.IAdminDataProvider, OnlineTicket.Services.DatabaseAdminDataProvider>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedFor | 
                                Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

var app = builder.Build();

app.UseForwardedHeaders();

// --- Apply Migrations and Seed Admin and Roles ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        await context.Database.EnsureCreatedAsync();
        Console.WriteLine("Database schema created successfully.");
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while creating database schema.");
    }
    
    await SeedData.InitializeAsync(services);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
