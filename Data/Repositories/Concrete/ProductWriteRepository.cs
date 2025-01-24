using Common.Entities;
using Data.Contexts;
using Data.Repositories.Abstract;
using Data.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.Concrete
{
    public class ProductWriteRepository : BaseWriteRepository<Product>, IProductWriteRepository
    {
        private readonly AppDbContext _context;
        public ProductWriteRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
