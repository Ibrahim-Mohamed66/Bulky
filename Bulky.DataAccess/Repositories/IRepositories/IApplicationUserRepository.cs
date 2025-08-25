
using Bulky.Models.Models;

namespace Bulky.DataAccess.Repositories.IRepositories;

public interface IApplicationUserRepository : IRepository<ApplicationUser, string>
{
    void Update(ApplicationUser entity);
}
