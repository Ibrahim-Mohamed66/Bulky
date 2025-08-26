using Bulky.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repositories.IRepositories;

public interface ISectionRepository:IRepository<Section,int> 
{
    void Update(Section entity);
}

