namespace Bluesoft.Bank.Model
{
    public abstract class AccountMovementBase : BaseEntity
    {
        public Account Account { get; set; }

        public decimal Amount { get; set; }

    }
}
