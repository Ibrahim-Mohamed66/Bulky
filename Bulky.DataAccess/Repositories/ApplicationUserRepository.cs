using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repositories.IRepositories;
using Bulky.Models.Models;


namespace Bulky.DataAccess.Repositories;

public class ApplicationUserRepository : Repository<ApplicationUser, string>, IApplicationUserRepository
{
    private readonly BulkyDbContext _context;

    public ApplicationUserRepository(BulkyDbContext context) : base(context)
    {
        _context = context;
    }


    

}
