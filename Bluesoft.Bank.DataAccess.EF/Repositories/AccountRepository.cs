using Bluesoft.Bank.Business.Repositories;
using Bluesoft.Bank.Model;
using Microsoft.EntityFrameworkCore;

namespace Bluesoft.Bank.DataAccess.EF.Repositories
{
    public class AccountRepository : BaseRepository<Account>, IAccountRepository
    {
        public AccountRepository(BankDbContext dbContext) : base(dbContext)
        {
        }

        public Task<List<AccountMovement>> GetPeriodOperations(int accountId, DateTime startDate, DateTime endDate)
        {
            return Values.Where(v => v.Id == accountId)
                .SelectMany(v => v.Movements)
                .Where(m => m.Date >= startDate && m.Date <= endDate)
                .OrderByDescending(m => m.Date)
                .ToListAsync();
        }
    }
}
