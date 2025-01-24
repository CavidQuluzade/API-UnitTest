namespace Data.UnitOfWork
{
    public interface IUnitOfWork
    {
        Task CommitAsync();
    }
}
