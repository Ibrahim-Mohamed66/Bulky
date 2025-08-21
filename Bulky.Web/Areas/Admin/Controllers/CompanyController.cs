using Bulky.DataAccess.Repositories.IRepositories;
using Bulky.Models.Models;
using Bulky.Models.ViewModels;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyWeb.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = StaticData.Role_Admin)]
public class CompanyController : Controller
{
    private readonly IUnitOfWork _unitOfWork;


    public CompanyController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IActionResult> Index()
    {
        var Companys = await _unitOfWork.Company.GetAllAsync(orderBy: q => q.OrderBy(p => p.Id));

        return View(Companys);  
    }

    [HttpGet]
    public async Task<IActionResult> Upsert(int? id)
    {
        var company = new Company();
        if (id == null || id == 0)
        {
            // Create
            return View(company);
        }
        else
        {
            // Edit
            company = await _unitOfWork.Company.GetByIdAsync(id.Value);
            if (company == null)
            {
                return NotFound();

            }
            return View(company);

        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Upsert(Company company)
    {
        if (ModelState.IsValid)
        {
            try
            {
                if (company.Id == 0)
                {
                    await _unitOfWork.Company.AddAsync(company);
                    TempData["success"] = "Company Created Successfully";
                }
                else
                {
                    _unitOfWork.Company.Update(company);
                    TempData["success"] = "Company Updated Successfully";
                }

                await _unitOfWork.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }   
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An error occurred while creating the Company: {ex.Message}");
            }
        }

        return View(company);
    }


    #region API CALLS
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var draw = Request.Query["draw"].FirstOrDefault();
        int start = int.TryParse(Request.Query["start"], out var s) ? s : 0;
        int length = int.TryParse(Request.Query["length"], out var l) && l > 0 ? l : 10;

        int pageNumber = (start / length) + 1;

        int totalRecords = await _unitOfWork.Company.GetCountAsync();
        IEnumerable<Company> Companys;
        int filteredRecords = totalRecords;


        Companys = await _unitOfWork.Company.GetAllAsync(
            pageNumber: pageNumber,
            pageSize: length);

    

        return Json(new
        {
            draw,
            recordsTotal = totalRecords,
            recordsFiltered = filteredRecords,
            data = Companys
        });
    }


    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        var Company = await _unitOfWork.Company.GetByIdAsync(id);
        if (Company == null)
        {
            return Json(new { success = false, message = "Error while deleting" });
        }
       
        try
        {
            await _unitOfWork.Company.RemoveAsync(id);
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
