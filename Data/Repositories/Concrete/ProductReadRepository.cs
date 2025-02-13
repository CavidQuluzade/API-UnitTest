﻿using Common.Entities;
using Data.Contexts;
using Data.Repositories.Abstract;
using Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.Concrete
{
    public class ProductReadRepository : BaseReadRepository<Product>, IProductReadRepository
    {
        private readonly AppDbContext _context;
        public ProductReadRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Product> GetByNameAsync(string name)
        {
            return await _context.Products.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower());
        }
    }
}
