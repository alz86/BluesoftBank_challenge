using Bluesoft.Bank.Business.DTOs;
using Bluesoft.Bank.Business.Repositories;

namespace Bluesoft.Bank.Business.Services
{
    /// <summary>
    /// Service for generating reports for admin
    /// users.
    /// </summary>
    public interface IAdminReportsService : IServiceBase
    {
        /// <summary>
        /// Gets the client transactions by month.
        /// </summary>
        Task<List<ClientTransactionsDto>> GetClientTransactionsByMonth(int year, int month);


        /// <summary>
        /// Gets transactions of clients greater than a certain amount, for
        /// a particular date range.
        /// </summary>
        Task<List<WithdrawalsOutOfCityDto>> GetWithdrawalsOutOfCity(int minimumAmount, DateTime? rangeStart, DateTime? rangeEnd);

    }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public class AdminReportsService : IAdminReportsService
    {
        private readonly IClientRepository _clientRepository;

        public AdminReportsService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public async Task<List<ClientTransactionsDto>> GetClientTransactionsByMonth(int year, int month)
        {
            //invalid date
            var validDate = DateTime.TryParse($"{year}-{month}-01", out var reportDate);
            if (!validDate)
                throw new ArgumentException("Invalid date.");

            //report can be requested only for finished months
            var monthStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            if (reportDate > monthStart)
                throw new ArgumentException("The report can only be requested for finished months.");

            var data = await _clientRepository.GetClientTransactionsByMonth(year, month);
            return data.Select(t => new ClientTransactionsDto
            {
                ClientId = t.Item1,
                ClientName = t.Item2,
                TransactionCount = t.Item3
            }).ToList();
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public async Task<List<WithdrawalsOutOfCityDto>> GetWithdrawalsOutOfCity(int minimumAmount, DateTime? rangeStart, DateTime? rangeEnd)
        {
            if (rangeStart.HasValue && rangeEnd.HasValue)
            {
                if (rangeEnd < rangeStart)
                    throw new ArgumentException("End date must be greater than start date.");

                var reportDayRange = (rangeEnd.Value - rangeStart.Value).TotalDays;

                if (reportDayRange > Consts.MaxReportDaySpan)
                    throw new ArgumentException($"Date range must be less than {Consts.MaxReportDaySpan} days.");
            }

            var data = await _clientRepository.GetWithdrawalsOutOfCity(minimumAmount, rangeStart, rangeEnd);
            return data.Select(t => new WithdrawalsOutOfCityDto
            {
                ClientId = t.Item1,
                ClientName = t.Item2,
                TotalAmount = t.Item3
            }).ToList();
        }
    }
}
