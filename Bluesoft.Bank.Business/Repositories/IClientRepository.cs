using Bluesoft.Bank.Model;

namespace Bluesoft.Bank.Business.Repositories
{
    public interface IClientRepository : IBaseRepository<Client>
    {
        /// <summary>
        /// Gets the transactions of all clients for a specific month and year.
        /// </summary>
        Task<List<Tuple<int, string, int>>> GetClientTransactionsByMonth(int year, int month);

        /// <summary>
        /// Gets transactions of clients greater than a certain amount, for
        /// a particular date range.
        /// </summary>
        Task<List<Tuple<int, string, decimal>>> GetWithdrawalsOutOfCity(int minimumAmount, DateTime? rangeStart, DateTime? rangeEnd);
    }
}
