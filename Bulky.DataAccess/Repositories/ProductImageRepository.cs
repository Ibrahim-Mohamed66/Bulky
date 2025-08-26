

using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repositories.IRepositories;
using Bulky.Models.Models;

namespace Bulky.DataAccess.Repositories
{
    public class ProductImageRepository : Repository<ProductImage, int>, IProductImageRepository
    {
        private readonly BulkyDbContext _context;
        public ProductImageRepository(BulkyDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(ProductImage productImage)
        {
            _context.ProductImages.Update(productImage);
        }
    }
}
