using Bluesoft.Bank.Business.Repositories;
using Bluesoft.Bank.Model;
using Microsoft.EntityFrameworkCore;

namespace Bluesoft.Bank.DataAccess.EF.Repositories
{
    public class AccountMovementMonthlyConsolidationRepository : BaseRepository<AccountMovementMonthlyConsolidation>, IAccountMovementMonthlyConsolidationRepository
    {
        public AccountMovementMonthlyConsolidationRepository(BankDbContext dbContext) : base(dbContext)
        {
        }

        public Task<AccountMovementMonthlyConsolidation?> GetMonthConsolidation(int accountId, int month, int year)
        {
            return Values.FirstOrDefaultAsync(v => v.Account.Id == accountId && v.Date.Month == month && v.Date.Year == year);
        }
    }
}
