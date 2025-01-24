using Common.Entities;
using Data.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.Base
{
    public class BaseReadRepository<T> : IBaseReadRepository<T> where T : BaseEntity
    {
        private readonly DbSet<T> _table;
        public BaseReadRepository(AppDbContext context)
        {
            _table = context.Set<T>();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _table.ToListAsync();
        }

        public async Task<T> GetAsync(int id)
        {
            return await _table.FindAsync(id);
        }
    }
}
