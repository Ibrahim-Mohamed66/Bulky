using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repositories.IRepositories;
using Bulky.Models.Models;


namespace Bulky.DataAccess.Repositories;

public class CategoryRepository : Repository<Category, int>, ICategoryRepository
{
    private readonly BulkyDbContext _context;

    public CategoryRepository(BulkyDbContext context) : base(context)
    {
        _context = context;
    }

    public void Update(Category category)
    {
        var existingCategory = _context.Categories.Find(category.Id);
        if (existingCategory != null)
        {
            category.CreatedAt = existingCategory.CreatedAt;

            category.UpdatedAt = DateTime.UtcNow;

            _context.Entry(existingCategory).CurrentValues.SetValues(category);
        }
    }


}
