using Bluesoft.Bank.Model;

namespace Bluesoft.Bank.Business.Repositories
{
    public interface IAccountMovementMonthlyConsolidationRepository : IBaseRepository<AccountMovementMonthlyConsolidation>
    {
        Task<AccountMovementMonthlyConsolidation?> GetMonthConsolidation(int accountId, int month, int year);
    }
}
