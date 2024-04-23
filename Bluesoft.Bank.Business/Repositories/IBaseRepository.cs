using Bluesoft.Bank.Model;

namespace Bluesoft.Bank.Business.Repositories
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        Task<T?> GetOneByIdAsync(int id);

        Task UpdateAsync(T entity);

        Task InsertAsync(T entity);

        //More methods..
    }
}
