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
    public class StoreRepository : Repository<Store, int>, IStoreRepository
    {
        private readonly BulkyDbContext _context;
        public StoreRepository(BulkyDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Store store)
        {
            _context.Update(store);
        }
    }
}
