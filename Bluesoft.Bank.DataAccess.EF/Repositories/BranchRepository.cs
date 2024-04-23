using Bluesoft.Bank.Business.Repositories;
using Bluesoft.Bank.Model;

namespace Bluesoft.Bank.DataAccess.EF.Repositories
{
    public class BranchRepository : BaseRepository<Branch>, IBranchRepository
    {
        public BranchRepository(BankDbContext dbContext) : base(dbContext)
        {
        }
    }
}
