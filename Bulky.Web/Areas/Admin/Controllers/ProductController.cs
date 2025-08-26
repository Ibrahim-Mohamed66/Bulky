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
            CategoryList = (await _unitOfWork.Category.GetAllAsync(filter: c => !c.IsHidden, orderBy: q => q.OrderBy(c => c.Name)))
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
            productVm.Product = await _unitOfWork.Product.GetByIdAsync(id.Value,includes: p => p.ProductImages);
            if (productVm.Product == null)
            {
                return NotFound();

            }
            return View(productVm);

        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Upsert(ProductVM productVm, IEnumerable<IFormFile?> files)
    {
        if (ModelState.IsValid)
        {
           
            try
            {
                if (productVm.Product.Id == 0)
                {
                    await _unitOfWork.Product.AddAsync(productVm.Product);
                    await _unitOfWork.SaveChangesAsync();
                    TempData["success"] = "Product Created Successfully";
                }
                else
                {
                    _unitOfWork.Product.Update(productVm.Product);
                    TempData["success"] = "Product Updated Successfully";
                }
                var wwwRootPath = _webHostEnvironment.WebRootPath;
                if (files != null)
                {
                    foreach (var file in files)
                    {
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        var productsPath = @"images\products\product-" + productVm.Product.Id;
                        var FinalPath = Path.Combine(wwwRootPath, productsPath);

                        if (!Directory.Exists(FinalPath))
                        {
                            Directory.CreateDirectory(FinalPath);
                        }
                        using (var fileStream = new FileStream(Path.Combine(FinalPath, fileName), FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }
                        var productImage = new ProductImage
                        {
                            ProductId = productVm.Product.Id,
                            ImageUrl = @"\" + productsPath + @"\" + fileName
                        };
                        if (productVm.Product.ProductImages == null)
                        {
                            productVm.Product.ProductImages = new List<ProductImage>();
                        }
                        productVm.Product.ProductImages = productVm.Product.ProductImages.Append(productImage);
                        await _unitOfWork.ProductImage.AddAsync(productImage);
                    }
                    await _unitOfWork.SaveChangesAsync();
                }


            }   
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An error occurred while creating the product: {ex.Message}");
            }
        }

        productVm.CategoryList = (await _unitOfWork.Category.GetAllAsync(filter:c => !c.IsHidden,orderBy: q => q.OrderBy(c => c.Name)))
            .Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> DeleteImage(int imageId)
    {
        var image = await _unitOfWork.ProductImage.GetByIdAsync(imageId);
        var productId = image.ProductId;
        if (image != null)
        {
           if(!string.IsNullOrEmpty(image.ImageUrl))
           {
                var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath,
                                    image.ImageUrl.TrimStart('\\'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }

           }
            await _unitOfWork.ProductImage.RemoveAsync(imageId);
            await _unitOfWork.SaveChangesAsync();
            TempData["success"] = "Image Deleted Successfully";
        }
        return RedirectToAction(nameof(Upsert), new { id = productId });

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
        var productPath = @"images\products\product-" + id;
        var finalPath = Path.Combine(_webHostEnvironment.WebRootPath, productPath);
        if (Directory.Exists(finalPath))
        {
            var files = Directory.GetFiles(finalPath);
            foreach (var file in files)
            {
                System.IO.File.Delete(file);
            }
            Directory.Delete(finalPath, true);
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
