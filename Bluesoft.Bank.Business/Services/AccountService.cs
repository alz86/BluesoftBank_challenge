using AutoMapper;
using Bluesoft.Bank.Business.DTOs;
using Bluesoft.Bank.Business.Exceptions;
using Bluesoft.Bank.Business.Repositories;
using Bluesoft.Bank.Model;

namespace Bluesoft.Bank.Business.Services
{

    public interface IAccountService : IServiceBase
    {
        Task<decimal> GetBalance(int accountId);

        Task<IList<AccountMovementDto>> GetRecentOperations(int accountId, DateTime startDate, DateTime endDate);

        Task<MontlhyExtractDto> GetMontlhyExtract(int accountId, int year, int month);

        Task<decimal> Deposit(int accountId, decimal amount, DepositTypes type, int? branchId);

        Task<decimal> Withdraw(int accountId, decimal amount, WithdrawalTypes withdrawalType, int? branchId);

        Task Transfer(int sourceAccountId, int targetAccountId, decimal amount);
    }

    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IAccountMovementMonthlyConsolidationRepository _accountMovementMonthlyConsolidationRepository;
        private readonly IMapper _mapper;
        private readonly IAccountMovementRepository _accountMovementRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBranchRepository _branchRepository;


        public AccountService(IAccountRepository accountRepository, IAccountMovementMonthlyConsolidationRepository accountMovementMonthlyConsolidationRepository, IMapper mapper, IAccountMovementRepository accountMovementRepository, IUnitOfWork unitOfWork, IBranchRepository branchRepository)
        {
            _accountRepository = accountRepository;
            _accountMovementMonthlyConsolidationRepository = accountMovementMonthlyConsolidationRepository;
            _mapper = mapper;
            _accountMovementRepository = accountMovementRepository;
            _unitOfWork = unitOfWork;
            _branchRepository = branchRepository;
        }

        public async Task<decimal> GetBalance(int accountId)
        {
            var account = await _accountRepository.GetOneByIdAsync(accountId);
            if (account == null)
                throw new AccountNotFoundException();
            return account.Balance;
        }

        public async Task<MontlhyExtractDto> GetMontlhyExtract(int accountId, int year, int month)
        {
            //date validation
            var validDate = DateTime.TryParse($"{year}-{month}-01", out var reportDate);
            if (!validDate)
                throw new ArgumentException("Invalid date.");

            var monthConsolidation = await _accountMovementMonthlyConsolidationRepository.GetMonthConsolidation(accountId, month, year);

            //consolidation not found. You cannot ask for a monthly extract
            //(in real life, it would imply that the system is trying to access
            //an extract of the running month. Otherwise, the consolidation would exists)
            if (monthConsolidation == null)
                throw new ConsolidationNotFoundException();

            //consolidation found. let's look for all the movements for the 
            //month, to send to the user. Also, some values has to be calculated.
            var periodStart = reportDate;
            var periodEnd = periodStart.AddMonths(1).AddMilliseconds(-1);

            var movements = await _accountRepository.GetPeriodOperations(accountId, periodStart, periodEnd);

            return new MontlhyExtractDto
            {
                Month = month,
                Year = year,
                InitialBalance = monthConsolidation.InitialBalance,
                FinalBalance = monthConsolidation.FinalBalance,
                TotalCredits = movements.Where(m => m.Type == AccountMovementType.Deposit).Select(m => m.Amount).Aggregate((acc, m) => acc += m),
                TotalDebits = movements.Where(m => m.Type == AccountMovementType.Withdrawal).Select(m => m.Amount).Aggregate((acc, m) => acc += m),
                Movements = _mapper.Map<IList<AccountMovementDto>>(movements)
            };
        }

        public async Task<IList<AccountMovementDto>> GetRecentOperations(int accountId, DateTime startDate, DateTime endDate)
        {
            var reportDays = (endDate - startDate).TotalDays;
            if (reportDays > Consts.MaxReportDaySpan)
                throw new InvalidOperationException($"The request period exceeds the maximum report length of {Consts.MaxReportDaySpan} days."); ;

            var movements = await _accountRepository.GetPeriodOperations(accountId, startDate, endDate);

            return _mapper.Map<IList<AccountMovementDto>>(movements);
        }

        public async Task<decimal> Deposit(int accountId, decimal amount, DepositTypes type, int? branchId)
        {
            //validations
            if (amount <= 0)
                throw new ArgumentException("Invalid Amount.", nameof(amount));

            if (type == DepositTypes.Cashier && branchId == null)
                throw new ArgumentException("BranchId is required for Cashier deposits.", nameof(branchId));

            var account = await _accountRepository.GetOneByIdAsync(accountId);
            if (account == null)
                throw new AccountNotFoundException();

            Branch? branch = null;
            if (branchId != null)
            {
                branch = await _branchRepository.GetOneByIdAsync(branchId.Value);
                if (branch == null)
                    throw new ArgumentException("Branch not found.");
            }

            //update logic
            await _unitOfWork.BeginTransactionAsync();

            //updates the balance
            account.Balance += amount;
            await _accountRepository.UpdateAsync(account);

            var movement = new AccountMovement
            {
                Account = account,
                Amount = amount,
                Date = DateTime.Now,
                Type = AccountMovementType.Deposit,
                DepositDetails = new DepositDetails
                {
                    Type = type,
                    Branch = branch
                }
            };

            await _accountMovementRepository.InsertAsync(movement);

            await _unitOfWork.CommitAsync();

            return account.Balance;
        }

        public async Task<decimal> Withdraw(int accountId, decimal amount, WithdrawalTypes withdrawalType, int? branchId)
        {
            //validations   
            var account = await _accountRepository.GetOneByIdAsync(accountId);
            if (account == null)
                throw new AccountNotFoundException();

            if (account.Balance < amount)
                throw new ArgumentException("Tried to withdraw more money than available.", nameof(amount));

            Branch? branch = null;
            if (branchId != null)
            {
                branch = await _branchRepository.GetOneByIdAsync(branchId.Value);
                if (branch == null)
                    throw new ArgumentException("Branch not found.");
            }

            //update logic
            await _unitOfWork.BeginTransactionAsync();

            //updates the balance
            account.Balance -= amount;
            await _accountRepository.UpdateAsync(account);

            var movement = new AccountMovement
            {
                Account = account,
                Amount = amount,
                Date = DateTime.Now,
                Type = AccountMovementType.Withdrawal,
                WithdrawalDetails = new WithdrawalDetails
                {
                    Type = withdrawalType,
                    Branch = branch
                }
            };

            await _accountMovementRepository.InsertAsync(movement);

            await _unitOfWork.CommitAsync();

            return account.Balance;
        }

        public async Task Transfer(int sourceAccountId, int targetAccountId, decimal amount)
        {
            await _unitOfWork.BeginTransactionAsync();

            await Withdraw(sourceAccountId, amount, WithdrawalTypes.Transfer, null);
            await Deposit(targetAccountId, amount, DepositTypes.Transfer, null);

            await _unitOfWork.CommitAsync();

        }
    }
}
