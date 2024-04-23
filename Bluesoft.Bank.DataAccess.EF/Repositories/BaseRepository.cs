using Bluesoft.Bank.Business.Repositories;
using Bluesoft.Bank.Model;
using Microsoft.EntityFrameworkCore;

namespace Bluesoft.Bank.DataAccess.EF.Repositories
{
    public abstract class BaseRepository<T> : IBaseRepository<T>
        where T : BaseEntity, new()
    {
        protected readonly BankDbContext dbContext;
        protected BaseRepository(BankDbContext dbContext)
        {
            this.dbContext = dbContext;
            Values = dbContext.Set<T>();
        }

        protected DbSet<T> Values { get; private set; }

        public Task<T?> GetOneByIdAsync(int id) => Values.FirstOrDefaultAsync(v => v.Id == id);

        public Task UpdateAsync(T entity)
        {
            Values.Update(entity);
            return Task.CompletedTask;
        }

        public Task InsertAsync(T entity)
        {
            Values.Add(entity);
            return Task.CompletedTask;
        }
    }
}
