using Common.Entities;

namespace Data.Repositories.Base
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        Task<List<T>> GetAllAsync();
        Task<T> GetAsync(int id);
        Task CreateAsync(T data);
        void Update(T data);
        void Delete(T data);
    }
}
