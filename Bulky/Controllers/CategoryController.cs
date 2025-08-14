using Bulky.Data;
using Bulky.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bulky.Controllers;

public class CategoryController : Controller
{
    private readonly BulkyDbContext _context;

    public CategoryController(BulkyDbContext context)
    {
        _context = context;
    }

    [HttpGet]

    public async Task<IActionResult> Index()
    {
        var categories = await _context.Categories.ToListAsync();
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
        if (category.Name.Any(char.IsDigit)) 
        {
            ModelState.AddModelError("Name", "The Name field cannot be numeric.");
        }

        if (ModelState.IsValid)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            TempData["success"] = "Category Created Successfuly";
            return RedirectToAction(nameof(Index));
        }
        else
            return View(category);
        
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var category = await _context.Categories.FindAsync(id);
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
        
        
        if (category.Name.Any(char.IsDigit)) 
            ModelState.AddModelError("Name", "The Name field cannot be numeric.");
            
        if (ModelState.IsValid)
        {
            category.UpdatedAt = DateTime.UtcNow;
            _context.Update(category);
            await _context.SaveChangesAsync();
            TempData["success"] = "Category Updated Successfuly";

            return RedirectToAction(nameof(Index));
        }
        return View(category);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var category = await _context.Categories.FindAsync(id);
        if (category == null)
            return NotFound();

        return View(category);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        if (id == null)
            return NotFound();

        var category = await _context.Categories.FindAsync(id);
        if (category == null)
            return NotFound();

        _context.Categories.Remove(category);  
        await _context.SaveChangesAsync();
        TempData["success"] = "Category Deleted successfuly";

        return RedirectToAction(nameof(Index));
    }

}
