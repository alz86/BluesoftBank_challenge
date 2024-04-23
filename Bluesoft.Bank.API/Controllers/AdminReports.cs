using Bluesoft.Bank.Business.DTOs;
using Bluesoft.Bank.Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bluesoft.Bank.API.Controllers
{
    [ApiVersion("1.0")]
    public class AdminReportsController : ControllerBase
    {
        private readonly IAdminReportsService _adminReportsService;

        public AdminReportsController(IAdminReportsService accountService)
        {
            _adminReportsService = accountService;
        }

        [HttpGet("/TransactionsByMonth/{year}/{month}")]
        public async Task<ActionResult<List<ClientTransactionsDto>>> GetClientTransactionsByMonth(int year, int month)
        {
            var transactions = await _adminReportsService.GetClientTransactionsByMonth(year, month);
            return Ok(transactions);
        }

        [HttpGet("WithdrawalsOutOfCity")]
        public async Task<ActionResult<IList<AccountMovementDto>>> GetBigWithdrawalsOutOfCity(DateTime? startDate, DateTime? endDate)
        {
            //as specified in the Doc, we look for withdrawals over 1M
            const int MinimumAmount = 1_000_000;
            var operations = await _adminReportsService.GetWithdrawalsOutOfCity(MinimumAmount, startDate, endDate);
            return Ok(operations);
        }
    }
}
