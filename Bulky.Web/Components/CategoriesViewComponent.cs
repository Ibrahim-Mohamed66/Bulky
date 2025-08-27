using Bulky.DataAccess.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Components
{
    public class CategoriesViewComponent : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoriesViewComponent(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IViewComponentResult> InvokeAsync(string viewName = "Default")
        {
            var categories = await _unitOfWork.Category.GetAllAsync(
                pageNumber:1,
                pageSize:5,
                filter: c => !c.IsHidden
                );
            return View(viewName, categories);
        }
    }
}
