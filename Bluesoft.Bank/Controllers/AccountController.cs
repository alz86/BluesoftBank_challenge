using Bluesoft.Bank.Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bluesoft.Bank.Controllers
{
    [ApiVersion("1.0")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet()]
        public async Task<ActionResult<AccountDto>> GetBalance(int id)
        {
            var account = await _accountService.GetAccount(id);
            if (account == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<AccountDto>(account));
        }

        [HttpGet]
        public async Task<ActionResult<IList<AccountMovementDto>>> GetPeriodOperations(int accountId, DateTime startDate, DateTime endDate)
        {
            var operations = await _accountService.GetPeriodOperations(accountId, startDate, endDate);
            return Ok(_mapper.Map<IList<AccountMovementDto>>(operations));
        }

        [HttpPost]
        public async Task<ActionResult<AccountDto>> CreateAccount([FromBody] AccountDto accountDto)
        {
            var account = _mapper.Map<Account>(accountDto);
            await _accountService.CreateAccount(account);
            return CreatedAtAction(nameof(GetAccount), new { id = account.Id }, _mapper.Map<AccountDto>(account));
        }

        [HttpPost]
        public async Task<ActionResult<AccountMovementDto>> CreateAccountMovement([FromBody] AccountMovementDto accountMovementDto)
        {
            var accountMovement = _mapper.Map<AccountMovement>(accountMovementDto);
            await _accountService.CreateAccountMovement(accountMovement);
            return CreatedAtAction(nameof(GetPeriodOperations), new { accountId = accountMovement.AccountId, startDate = accountMovement.Date, endDate = accountMovement.Date }, _mapper.Map<AccountMovementDto>(accountMovement));
        }

        [HttpPut]
        public async Task<ActionResult<AccountDto>> UpdateAccount(int id, [FromBody] AccountDto accountDto)
        {
            var account = _mapper.Map<Account>(accountDto);
            await _accountService.UpdateAccount(id, account);
            return Ok(_mapper.Map<AccountDto>(account));
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteAccount(int id)
        {
            await _accountService.DeleteAccount(id);
            return NoContent();
        }
    }

}
