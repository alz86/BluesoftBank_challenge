namespace Bluesoft.Bank.Model
{
    public class AccountMovementConsolidationBase : AccountMovementBase
    {
        public DateOnly Date { get; set; }
        public int TotalOperations { get; set; }

        public decimal InitialBalance { get; set; }

        public decimal FinalBalance { get; set; }
    }

}
