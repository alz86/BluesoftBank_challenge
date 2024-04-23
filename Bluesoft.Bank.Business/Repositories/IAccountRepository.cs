using Bluesoft.Bank.Model;

namespace Bluesoft.Bank.Business.Repositories
{
    public interface IAccountRepository : IBaseRepository<Account>
    {
        Task<List<AccountMovement>> GetPeriodOperations(int accountId, DateTime startDate, DateTime endDate);
    }
}
