using Bluesoft.Bank.Business.DTOs;
using Bluesoft.Bank.Business.Exceptions;
using Bluesoft.Bank.Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bluesoft.Bank.API.Controllers
{
    [ApiVersion("1.0")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("{id}/Deposits/Cashier")]
        public async Task<ActionResult<decimal>> CashierDeposit(int id, [FromBody] DepositRequest depositRequest)
        {
            try
            {
                var balance = await _accountService.Deposit(id, depositRequest.Amount, Model.DepositTypes.Cashier, depositRequest.BranchId);
                return Ok(balance);
            }
            catch (AccountNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }


        [HttpGet("{id}/Balance")]
        public async Task<ActionResult<decimal>> GetBalance(int id)
        {
            try
            {
                var balance = await _accountService.GetBalance(id);
                return Ok(balance);
            }
            catch (AccountNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{id}/Operations")]
        public async Task<ActionResult<IList<AccountMovementDto>>> GetPeriodOperations(int id, DateTime startDate, DateTime endDate)
        {
            var operations = await _accountService.GetRecentOperations(id, startDate, endDate);
            return Ok(operations);
        }

        [HttpGet("{id}/Reports/Monthly/{year}/{month}")]
        public async Task<ActionResult<IList<AccountMovementDto>>> GetMonthlyReports(int id, int year, int month)
        {
            try
            {
                var operations = await _accountService.GetMontlhyExtract(id, year, month);
                return Ok(operations);
            }
            catch (ConsolidationNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }


        [HttpPost("{id}/Withdraws/Cashier")]
        public async Task<ActionResult<decimal>> CashierWithdraw(int id, [FromBody] WithdrawRequest request)
        {
            try
            {
                var balance = await _accountService.Withdraw(id, request.Amount, Model.WithdrawalTypes.Cashier, request.BranchId);
                return Ok(balance);
            }
            catch (AccountNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }


        [HttpPost("{id}/Withdraws/Transfer")]
        public async Task<ActionResult> Transfer(int id, [FromBody] TransferRequest request)
        {
            try
            {
                await _accountService.Transfer(id, request.TargetAccountId, request.Amount);
                return Ok();
            }
            catch (AccountNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
