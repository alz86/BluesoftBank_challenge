namespace Bluesoft.Bank.API
{
    public record class DepositRequest(decimal Amount, int BranchId);

    public record class WithdrawRequest(decimal Amount, int BranchId);

    public record class TransferRequest(decimal Amount, int TargetAccountId);

}
