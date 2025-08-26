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
    public class BannerRepository : Repository<Banner, int>, IBannerRepository
    {
        private readonly BulkyDbContext _context;
        public BannerRepository(BulkyDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Banner banner)
        {
            _context.Banners.Update(banner);
        }
    }
}
