namespace Bluesoft.Bank.Business
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
        Task SaveAsync();  // Save changes asynchronously without committing the transaction
    }
}
