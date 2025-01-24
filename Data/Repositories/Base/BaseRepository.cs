using Common.Entities;
using Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories.Base
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        private readonly DbSet<T> _table;
        public BaseRepository(AppDbContext context)
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

        public async Task CreateAsync(T data)
        {
            await _table.AddAsync(data);
        }

        public void Update(T data)
        {
            _table.Update(data);
        }

        public void Delete(T data)
        {
            _table.Remove(data);
        }
    }
}
