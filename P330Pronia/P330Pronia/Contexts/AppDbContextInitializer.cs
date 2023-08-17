using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using P330Pronia.Models.Identity;
using P330Pronia.Utils.Enums;

namespace P330Pronia.Contexts;

public class AppDbContextInitializer
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<AppUser> _userManager;
    private readonly AppDbContext _context;

    public AppDbContextInitializer(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager, AppDbContext context)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _context = context;
    }

    public async Task InitializeAsync()
    {
        if (_context.Database.IsSqlServer())
        {
            await _context.Database.MigrateAsync();
        }
    }

    public async Task UserSeedAsync()
    {
        foreach (var role in Enum.GetValues(typeof(Roles)))
        {
            await _roleManager.CreateAsync(new IdentityRole { Name = role.ToString() });
        }

        AppUser adminUser = new AppUser
        {
            UserName = "admin",
            Email = "admin@gmail.com",
            Fullname = "adminadmin",
            IsActive = true
        };

        await _userManager.CreateAsync(adminUser, "Salam123!");
        await _userManager.AddToRoleAsync(adminUser, Roles.Admin.ToString());
    }
}