using Bulky.DataAccess.Repositories.IRepositories;
using Bulky.Models.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Areas.Admin.Controllers;

[Area("Admin")]
public class CategoryController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public CategoryController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet]

    public async Task<IActionResult> Index()
    {
        var categories = await _unitOfWork.Category.GetAllAsync();
        return View(categories);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Category category)
    {
        ValidateCategoryName(category);

        if (ModelState.IsValid)
        {
            try
            {
                await _unitOfWork.Category.AddAsync(category);
                await _unitOfWork.SaveChangesAsync();
                TempData["success"] = "Category Created Successfully";
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An error occurred while creating the category: {ex.Message}");
            }
        }
        return View(category);
        
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var category = await _unitOfWork.Category.GetByIdAsync(id);
        if (category == null)
            return NotFound();
        
        return View(category);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Category category)
    {
        if (id != category.Id)
            return NotFound();
        
        
        ValidateCategoryName(category);

        if (ModelState.IsValid)
        {
            try
            {
                _unitOfWork.Category.Update(category);
                await _unitOfWork.SaveChangesAsync();
                TempData["success"] = "Category Updated Successfully";

                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An error occurred while Updating the category: {ex.Message}");

            }
        }
        return View(category);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {

        var category = await _unitOfWork.Category.GetByIdAsync(id);
        if (category == null)
            return NotFound();

        return View(category);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            await _unitOfWork.Category.RemoveAsync(id);
            await _unitOfWork.SaveChangesAsync();
            TempData["success"] = "Category Deleted Successfully";
            return RedirectToAction(nameof(Index));

        }
        catch (Exception ex)
        {
            TempData["error"] = $"An error occurred while deleting the category: {ex.Message}";
            return RedirectToAction(nameof(Index));
        }
    }
    private void ValidateCategoryName(Category category)
    {
        if (!string.IsNullOrEmpty(category?.Name) && category.Name.Any(char.IsDigit))
        {
            ModelState.AddModelError("Name", "The Name field cannot contain numeric characters.");
        }
    }
}