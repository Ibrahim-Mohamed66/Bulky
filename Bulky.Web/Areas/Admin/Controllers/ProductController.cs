using Bulky.DataAccess.Repositories.IRepositories;
using Bulky.Models.Models;
using Bulky.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyWeb.Areas.Admin.Controllers;

[Area("Admin")]
public class ProductController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
    {
        _unitOfWork = unitOfWork;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<IActionResult> Index()
    {
        var products = await _unitOfWork.Product.GetAllAsync(orderBy: q => q.OrderBy(p => p.Id),includes: p => p.Category);

        return View(products);  
    }

    [HttpGet]
    public async Task<IActionResult> Upsert(int? id)
    {
        var productVm = new ProductVM
        {
            Product = new Product(),
            CategoryList = (await _unitOfWork.Category.GetAllAsync(orderBy: q => q.OrderBy(c => c.Name)))
                .Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                })
        };
        if (id == null || id == 0)
        {
            // Create
            return View(productVm);
        }
        else
        {
            // Edit
            productVm.Product = await _unitOfWork.Product.GetByIdAsync(id.Value);
            if (productVm.Product == null)
            {
                return NotFound();

            }
            return View(productVm);

        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Upsert(ProductVM productVm, IFormFile? file)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var productsPath = Path.Combine(wwwRootPath, @"images\products");

                    if (!string.IsNullOrEmpty(productVm.Product.ImageUrl))
                    {
                        var oldImagePath =
                            Path.Combine(wwwRootPath, productVm.Product.ImageUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }


                    using (var fileStream = new FileStream(Path.Combine(productsPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    productVm.Product.ImageUrl = @"\images\products\" + fileName;
                }
                if (productVm.Product.Id == 0)
                {
                    await _unitOfWork.Product.AddAsync(productVm.Product);
                    TempData["success"] = "Product Created Successfully";
                }
                else
                {
                    _unitOfWork.Product.Update(productVm.Product);
                    TempData["success"] = "Product Updated Successfully";
                }

                await _unitOfWork.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }   
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An error occurred while creating the product: {ex.Message}");
            }
        }

        productVm.CategoryList = (await _unitOfWork.Category.GetAllAsync(orderBy: q => q.OrderBy(c => c.Name)))
            .Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });
        return View(productVm);
    }


    #region API CALLS
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var draw = Request.Query["draw"].FirstOrDefault();
        int start = int.TryParse(Request.Query["start"], out var s) ? s : 0;
        int length = int.TryParse(Request.Query["length"], out var l) && l > 0 ? l : 10;
        string? searchValue = Request.Query["search[value]"].FirstOrDefault();

        int pageNumber = (start / length) + 1;

        int totalRecords = await _unitOfWork.Product.GetCountAsync();
        IEnumerable<Product> products;
        int filteredRecords = totalRecords;

        if (string.IsNullOrEmpty(searchValue))
        {
            products = await _unitOfWork.Product.GetAllAsync(
                pageNumber: pageNumber,
                pageSize: length,
                includes: p => p.Category
            );
        }
        else
        {
            var (filteredProducts, filteredCount) = await _unitOfWork.Product.SearchAsync(
                pageNumber: pageNumber,
                pageSize: length,
                keyword: searchValue
            );
            products = filteredProducts;
            filteredRecords = filteredCount;
        }

        return Json(new
        {
            draw,
            recordsTotal = totalRecords,
            recordsFiltered = filteredRecords,
            data = products
        });
    }


    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _unitOfWork.Product.GetByIdAsync(id);
        if (product == null)
        {
            return Json(new { success = false, message = "Error while deleting" });
        }
        var oldImagePath =
               Path.Combine(_webHostEnvironment.WebRootPath,
               product.ImageUrl.TrimStart('\\'));
        if (System.IO.File.Exists(oldImagePath))
        {
            System.IO.File.Delete(oldImagePath);
        }
        try
        {
            await _unitOfWork.Product.RemoveAsync(id);
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
