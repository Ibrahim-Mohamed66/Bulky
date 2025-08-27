using Bulky.DataAccess.Repositories.IRepositories;
using Bulky.Models.Models;
using Bulky.Models.ViewModels;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BulkyWeb.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = StaticData.Role_Admin)]
public class StoreController : Controller
{
    private readonly IUnitOfWork _unitOfWork;


    public StoreController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IActionResult> Index()
    {
        var stores = await _unitOfWork.Store.GetAllAsync(orderBy: q => q.OrderBy(p => p.Id));

        return View(stores);  
    }

    [HttpGet]
    public async Task<IActionResult> Upsert(int? id)
    {
        var store = new Store();
        if (id == null || id == 0)
        {
            // Create
            return View(store);
        }
        else
        {
            // Edit
            store = await _unitOfWork.Store.GetByIdAsync(id.Value);
            if (store == null)
            {
                return NotFound();

            }
            return View(store);

        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Upsert(Store Store)
    {
        if (ModelState.IsValid)
        {
            try
            {
                if (Store.Id == 0)
                {
                    await _unitOfWork.Store.AddAsync(Store);
                    TempData["success"] = "Store Created Successfully";
                }
                else
                {
                    _unitOfWork.Store.Update(Store);
                    TempData["success"] = "Store Updated Successfully";
                }

                await _unitOfWork.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }   
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An error occurred while creating the Store: {ex.Message}");
            }
        }

        return View(Store);
    }


    #region API CALLS
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {

        var stores = await _unitOfWork.Store.GetAllAsync(orderBy: q => q.OrderBy(u => u.Id));

        
        return Json(new
        {
            sucess = true,
            data = stores
        });
    }


    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        var Store = await _unitOfWork.Store.GetByIdAsync(id);
        if (Store == null)
        {
            return Json(new { success = false, message = "Error while deleting" });
        }
       
        try
        {
            await _unitOfWork.Store.RemoveAsync(id);
            await _unitOfWork.SaveChangesAsync();
            return Json(new { success = true, message = "Delete Successful" });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = $"Error while deleting: {ex.Message}" });
        }
    }
    #endregion
}
