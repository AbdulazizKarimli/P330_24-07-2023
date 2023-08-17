using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using P330Pronia.Contexts;
using P330Pronia.Middlewares;
using P330Pronia.Models.Identity;
using P330Pronia.Services.Implementations;
using P330Pronia.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
    //options.UseSqlServer(builder.Configuration["ConnectionStrings:Default"]);
});
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 8;

    options.User.RequireUniqueEmail = true;
    options.Lockout.MaxFailedAccessAttempts = 3;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
    options.Lockout.AllowedForNewUsers = false;
}).AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddScoped<IFileService, FileService>();

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddTransient<IMailService, MailService>();

builder.Services.AddScoped<AppDbContextInitializer>();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(30);
});

builder.Services.ConfigureApplicationCookie(c =>
{
    c.LoginPath = "/Auth/Login";
});

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

app.UseBlockBrowserMiddleware();

app.UseStaticFiles();

app.UseSession();

using (var scope = app.Services.CreateScope())
{
    var initializer = scope.ServiceProvider.GetRequiredService<AppDbContextInitializer>();
    await initializer.InitializeAsync();
    await initializer.UserSeedAsync();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{Id?}"
);

app.Run();
