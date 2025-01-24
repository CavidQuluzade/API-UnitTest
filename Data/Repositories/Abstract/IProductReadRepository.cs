using Common.Entities;
using Data.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.Abstract
{
    public interface IProductReadRepository : IBaseReadRepository<Product>
    {
        Task<Product> GetByNameAsync(string name);
    }
}
