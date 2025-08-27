using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repositories.IRepositories;
using Bulky.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repositories
{
    public class ProductStoreRepository : Repository<ProductStore, int>, IProductStoreRepository
    {
        private readonly BulkyDbContext _context;
        public ProductStoreRepository(BulkyDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(ProductStore productStore)
        {
            //_context..Update(productStore);
        }
    }
}
