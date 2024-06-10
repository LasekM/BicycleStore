using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BicycleStore.DbContext;
using System.Xml.Linq;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddDefaultIdentity<IdentityUser>()       // dodać
    .AddRoles<IdentityRole>()                             //
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddMemoryCache();                        // dodać
builder.Services.AddSession();


// Connection string
var connectionString = builder.Configuration.GetConnectionString("AppDbContextConnection") ?? throw new InvalidOperationException("Connection string 'AppDbContextConnection' not found.");
var dbPath = Path.Combine(builder.Environment.ContentRootPath, "Bike.db");

// Add services to the container
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
app.MapRazorPages();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
