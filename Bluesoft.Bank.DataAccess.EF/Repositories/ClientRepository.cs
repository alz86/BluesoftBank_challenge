using Bluesoft.Bank.Business.Repositories;
using Bluesoft.Bank.Model;
using Microsoft.EntityFrameworkCore;

namespace Bluesoft.Bank.DataAccess.EF.Repositories
{
    public class ClientRepository : BaseRepository<Client>, IClientRepository
    {
        public ClientRepository(BankDbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public Task<List<Tuple<int, string, int>>> GetClientTransactionsByMonth(int year, int month)
        {
            return dbContext.MonthlyConsolidations
                .Where(m => m.Date.Year == year && m.Date.Month == month)
                .Select(m => new Tuple<int, string, int>(m.Account.Client.Id, m.Account.Client.FullName, m.TotalOperations))
                .ToListAsync();
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public async Task<List<Tuple<int, string, decimal>>> GetWithdrawalsOutOfCity(int minimumAmount, DateTime? rangeStart, DateTime? rangeEnd)
        {
            var clientsWithHighWithdrawals = await Values
                .Select(client => new
                {
                    ClientId = client.Id,
                    ClientName = client.FullName,
                    TotalWithdrawalAmount = client.Accounts.SelectMany(account => account.Movements
                        .Where(m => m.Type == AccountMovementType.Withdrawal
                                    && m.WithdrawalDetails.Type == WithdrawalTypes.Cashier
                                    && m.WithdrawalDetails.Branch.City != account.Branch.City
                                    && (!rangeStart.HasValue || m.Date >= rangeStart.Value)
                                    && (!rangeEnd.HasValue || m.Date <= rangeEnd.Value)))
                        .Select(m => m.Amount)
                        .Sum()
                })
                .Where(client => client.TotalWithdrawalAmount >= minimumAmount)
                .ToListAsync();

            return clientsWithHighWithdrawals
                    .Select(c => new Tuple<int, string, decimal>(c.ClientId, c.ClientName, c.TotalWithdrawalAmount))
                    .ToList();
        }
    }
}
