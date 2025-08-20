
using Bulky.Models.Models;

namespace Bulky.DataAccess.Repositories.IRepositories;

public interface ICategoryRepository: IRepository<Category,int>
{
    void Update(Category category);
}
