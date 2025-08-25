

using Bulky.Models.Models;
using Bulky.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Bulky.DataAccess.Data.DbInitializer;

public class DbInitializer : IDbInitializer
{
    private readonly BulkyDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public DbInitializer(BulkyDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task Initialize()
    {
        try
        {
            if(_context.Database.GetPendingMigrations().Count() > 0)
            {
                _context.Database.Migrate();
            }

        }
        catch (Exception ex)
        {

            throw;
        }
        if (!await _roleManager.RoleExistsAsync(StaticData.Role_Customer))
        {
            await _roleManager.CreateAsync(new IdentityRole(StaticData.Role_Customer));
            await _roleManager.CreateAsync(new IdentityRole(StaticData.Role_Admin));
            await _roleManager.CreateAsync(new IdentityRole(StaticData.Role_Employee));
            await _roleManager.CreateAsync(new IdentityRole(StaticData.Role_Company));
        }
        var adminUser = await _userManager.CreateAsync( new ApplicationUser
        {
            UserName = "admin_bulky@gmail.com",
            Email = "admin_bulky@gmail.com",
            EmailConfirmed = true,
            FirstName = "Admin",
            LastName = "User",
            DateOfBirth = (DateTime.Now.AddYears(-20)),
            PhoneNumber = "01234567891",
            StreetAddress = "123 Time Square",
            State = "New York",
            City = "Manhaten",
            PostalCode = "10001"
        },"Admin123*");

        var user = await _userManager.FindByEmailAsync("admin_bulky@gmail.com");
        await _userManager.AddToRoleAsync(user, StaticData.Role_Admin);
    }
}
