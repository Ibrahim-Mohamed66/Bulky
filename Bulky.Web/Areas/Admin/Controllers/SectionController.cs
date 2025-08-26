using Bulky.DataAccess.Repositories.IRepositories;
using Bulky.Models.Models;
using Bulky.Models.ViewModels;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticData.Role_Admin)]
    public class SectionController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public SectionController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var sections = await _unitOfWork.Section.GetAllAsync();
            return View(sections);
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Section section, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                var wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var sectionPath = Path.Combine(wwwRootPath, "images", "sections");

                    // Ensure folder exists
                    if (!Directory.Exists(sectionPath))
                    {
                        Directory.CreateDirectory(sectionPath);
                    }

                    if (!string.IsNullOrEmpty(section.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, section.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(sectionPath, fileName), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }

                    // Save file path to DB
                    section.ImageUrl = @"\images\sections\" + fileName;

                    await _unitOfWork.Section.AddAsync(section);
                    await _unitOfWork.SaveChangesAsync();

                    TempData["success"] = "Section created successfully";
                    return RedirectToAction("Index");
                }
            }

            TempData["error"] = "Error while creating section";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var section = await _unitOfWork.Section.GetByIdAsync(id);
            if (section == null)
                return NotFound();

            return View(section);
        }



        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeletePost(Section section)
        {
            var existingSection = await _unitOfWork.Section.GetByIdAsync(section.Id);
            if (existingSection == null)
            {
                TempData["error"] = "Section not found";
                return RedirectToAction("Index");
            }

            // Delete the section image if it exists
            if (!string.IsNullOrEmpty(existingSection.ImageUrl))
            {
                var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, existingSection.ImageUrl.TrimStart('\\', '/'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            // Remove the section from database
            await _unitOfWork.Section.RemoveAsync(existingSection.Id);
            await _unitOfWork.SaveChangesAsync();

            TempData["success"] = "Section deleted successfully";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var section = await _unitOfWork.Section.GetByIdAsync(id);
            if (section == null)
                return NotFound();
            return View(section);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Section section, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                var existingSection = await _unitOfWork.Section.GetByIdAsync(section.Id);
                if (existingSection == null)
                {
                    TempData["error"] = "Section not found";
                    return RedirectToAction("Index");
                }
                existingSection.Title = section.Title;
                existingSection.Description = section.Description;
                existingSection.IsHidden = section.IsHidden;
                existingSection.IsHomePage = section.IsHomePage;


                var wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var sectionPath = Path.Combine(wwwRootPath, "images", "sections");
                    // Ensure folder exists
                    if (!Directory.Exists(sectionPath))
                    {
                        Directory.CreateDirectory(sectionPath);
                    }
                    if (!string.IsNullOrEmpty(existingSection.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, existingSection.ImageUrl.TrimStart('\\', '/'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    using (var fileStream = new FileStream(Path.Combine(sectionPath, fileName), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    // Save file path to DB
                    existingSection.ImageUrl = @"\images\sections\" + fileName;
                }
                _unitOfWork.Section.Update(existingSection);
                await _unitOfWork.SaveChangesAsync();
                TempData["success"] = "Section updated successfully";
                return RedirectToAction("Index");
            }
            TempData["error"] = "Error while updating section";
            return RedirectToAction("Index");
        }


    }
}
