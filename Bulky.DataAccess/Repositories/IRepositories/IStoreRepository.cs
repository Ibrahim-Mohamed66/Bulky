using Bulky.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repositories.IRepositories
{
    public interface IStoreRepository : IRepository<Store, int>
    {
        void Update(Store store);
    }
}
