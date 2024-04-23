using Bluesoft.Bank.Business.Repositories;
using Bluesoft.Bank.Model;

namespace Bluesoft.Bank.DataAccess.EF.Repositories
{
    public class AccountMovementRepository : BaseRepository<AccountMovement>, IAccountMovementRepository
    {
        public AccountMovementRepository(BankDbContext dbContext) : base(dbContext)
        {
        }
    }
}
