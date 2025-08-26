using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repositories.IRepositories;
using Bulky.Models.Models;
using Bulky.Models.ViewModels;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyWeb.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = StaticData.Role_Admin)]
public class UserController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly BulkyDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;


    public UserController(IUnitOfWork unitOfWork, BulkyDbContext context, UserManager<ApplicationUser> userManager)
    {
        _unitOfWork = unitOfWork;
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {

        return View();
    }

    public async Task<IActionResult> RoleManagement(string userId)
    {

        var user = await _unitOfWork.ApplicationUser.GetByIdAsync(userId, u => u.Company);

        var userRoles = await _userManager.GetRolesAsync(user);

        var roleManagementVm = new RoleManagementVM
        {
            ApplicationUser = user,

            Roles = _context.Roles.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Name
            }),

            Companies = _context.Companies.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            })
        };

        roleManagementVm.ApplicationUser.Role = userRoles.FirstOrDefault();

        return View(roleManagementVm);
    }

    [HttpPost]
    public async Task<IActionResult> RoleManagement(RoleManagementVM roleManagementVm)
    {
        var user = await _userManager.FindByIdAsync(roleManagementVm.ApplicationUser.Id);
        if (user == null)
        {
            TempData["error"] = "User not found.";
            return RedirectToAction("Index");
        }

        var oldRoles = await _userManager.GetRolesAsync(user);
        var oldRole = oldRoles.FirstOrDefault();

        // --- Handle CompanyId update regardless of role change ---
        if (roleManagementVm.ApplicationUser.Role == StaticData.Role_Company)
        {
            // Always update CompanyId if role is Company
            user.CompanyId = roleManagementVm.ApplicationUser.CompanyId;
        }
        else if (oldRole == StaticData.Role_Company)
        {
            // If moving away from Company role, clear CompanyId
            user.CompanyId = null;
        }

        // --- Handle Role change ---
        if (roleManagementVm.ApplicationUser.Role != oldRole)
        {
            await _userManager.RemoveFromRoleAsync(user, oldRole);
            await _userManager.AddToRoleAsync(user, roleManagementVm.ApplicationUser.Role);
            TempData["success"] = "User role updated successfully";
        }
        else
        {
            TempData["success"] = "User company updated successfully";
        }

        _unitOfWork.ApplicationUser.Update(user);
        await _unitOfWork.SaveChangesAsync();

        return RedirectToAction("Index");
    }





    #region API CALLS
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {

        var users = await _unitOfWork.ApplicationUser.GetAllAsync(orderBy: q => q.OrderBy(u => u.Id), includes: u => u.Company);

        var roles = _context.Roles.ToList();
        foreach (var user in users)
        {
            var roleId = _context.UserRoles.FirstOrDefault(u => u.UserId == user.Id).RoleId;
            user.Role = roles.FirstOrDefault(r => r.Id == roleId).Name;
        }

        if (users == null)
        {
            return Json(new { success = false, message = "No users found." });
        }
        return Json(new
        {
            sucess = true,
            data = users
        });
    }

    [HttpPost]
    public async Task<IActionResult> LockUnlock([FromBody] string id)
    {
        var user = await _unitOfWork.ApplicationUser.GetByIdAsync(id);
        if (user == null)
        {
            return Json(new { success = false, message = "Error while locking/unlocking" });
        }
        if (user.LockoutEnd != null && user.LockoutEnd > DateTime.Now)
        {
            user.LockoutEnd = DateTime.Now;
        }
        else
        {
            user.LockoutEnd = DateTime.Now.AddYears(1000);
        }
        _unitOfWork.ApplicationUser.Update(user);
        await _unitOfWork.SaveChangesAsync();
        return Json(new { success = true, message = "Operation successful." });
    }

}
#endregion

