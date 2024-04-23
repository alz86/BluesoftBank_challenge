namespace Bluesoft.Bank.Model
{
    public class AccountMovement : AccountMovementBase
    {
        public DateTime Date { get; set; }

        public AccountMovementType Type { get; set; }

        public WithdrawalDetails? WithdrawalDetails { get; set; }

        public DepositDetails? DepositDetails { get; set; }
    }
}
