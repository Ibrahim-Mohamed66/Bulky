
using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repositories.IRepositories;
using Bulky.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace Bulky.DataAccess.Repositories;

public class ProductRepository : Repository<Product, int>, IProductRepository
{
    private readonly BulkyDbContext _context;
    public ProductRepository(BulkyDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<(IEnumerable<Product> Products, int TotalCount)> SearchAsync(string keyword, int pageNumber = 1, int pageSize = 10)
    {
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = 10;

        var query = _context.Products
            .Include(p => p.Category)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            query = query.Where(p => 
            p.Title.Contains(keyword) || 
            p.Author.Contains(keyword) || 
            p.Category.Name.Contains(keyword) ||
            (p.Description != null && p.Description.Contains(keyword)));
        }

        int totalCount = await query.CountAsync();

        var products = await query
            .OrderBy(p => p.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (products, totalCount);
    }

    public void Update(Product product)
    {
        var existingProduct = _context.Products.Find(product.Id);
        if (existingProduct != null)
        {
            product.CreatedAt = existingProduct.CreatedAt;

            product.UpdatedAt = DateTime.UtcNow;
            _context.Entry(existingProduct).CurrentValues.SetValues(product);
        }
    }
}
