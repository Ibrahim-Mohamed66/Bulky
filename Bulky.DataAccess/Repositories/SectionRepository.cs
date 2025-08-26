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
    public class SectionRepository : Repository<Section, int>, ISectionRepository
    {
        private readonly BulkyDbContext _context;
        public SectionRepository(BulkyDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Section entity)
        {
            _context.Sections.Update(entity);
        }
    }
}
