using Bulky.DataAccess.Repositories.IRepositories;
using Bulky.Models.Models;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticData.Role_Admin)]
    public class BannerController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BannerController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var banner = await _unitOfWork.Banner
                .GetFirstOrDefaultAsync();
            if (banner != null)
            {

                return RedirectToAction(nameof(Update), new { id = banner.Id });
            }
            else
            {
                return RedirectToAction(nameof(Create));
            }
        }


        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Banner banner, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                if((await _unitOfWork.Banner.GetCountAsync()) > 0)
                {

                    TempData["error"] = "Cannot Create More Than one Banener while creating banner";
                    return RedirectToAction("Index");

                }
                if (file == null || file.Length == 0)
                {
                    TempData["error"] = "Please upload an image for the banner";
                    return View(banner);
                }
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var bannerPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "banners");
                if (!Directory.Exists(bannerPath))
                {
                    Directory.CreateDirectory(bannerPath);
                }
                using (var fileStream = new FileStream(Path.Combine(bannerPath, fileName), FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                // Save file path to DB
                banner.ImageUrl = @"\images\banners\" + fileName;
                await _unitOfWork.Banner.AddAsync(banner);
                await _unitOfWork.SaveChangesAsync();
                TempData["success"] = "Banner created successfully";
                return RedirectToAction("Index");
            }
            TempData["error"] = "Error while creating banner";
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update(int id)
        {
            var banner = await _unitOfWork.Banner.GetByIdAsync(id);
            if (banner == null)
            {
                return NotFound();
            }
            return View(banner);
        }
        [HttpPost]
        public async Task<IActionResult> Update(Banner banner, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                var existingBanner = await _unitOfWork.Banner.GetByIdAsync(banner.Id);
                if (existingBanner == null)
                {
                    TempData["error"] = "Banner not found";
                    return RedirectToAction("Index");
                }
                existingBanner.Title = banner.Title;
                existingBanner.Description = banner.Description;
                var wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var bannerPath = Path.Combine(wwwRootPath, "images", "banners");
                    // Ensure folder exists
                    if (!Directory.Exists(bannerPath))
                    {
                        Directory.CreateDirectory(bannerPath);
                    }
                    if (!string.IsNullOrEmpty(existingBanner.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, existingBanner.ImageUrl.TrimStart('\\', '/'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    using (var fileStream = new FileStream(Path.Combine(bannerPath, fileName), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    // Save file path to DB
                    existingBanner.ImageUrl = @"\images\banners\" + fileName;
                }
                _unitOfWork.Banner.Update(existingBanner);
                await _unitOfWork.SaveChangesAsync();
                TempData["success"] = "Banners updated successfully";
                return RedirectToAction("Index");
            }
            TempData["error"] = "Error while updating Banner";
            return RedirectToAction("Index");
        }
        
    }
}

