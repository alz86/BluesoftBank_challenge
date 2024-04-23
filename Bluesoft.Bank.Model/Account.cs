namespace Bluesoft.Bank.Model
{

    public class Account : BaseEntity
    {
        public Client Client { get; set; }

        public DateTime Creation { get; set; }

        public decimal Balance { get; set; }

        public Branch Branch { get; set; }

        public virtual ICollection<AccountMovement> Movements { get; set; }

        public virtual ICollection<AccountMovementDailyConsolidation> AccountMovementDailyConsolidations { get; set; }
        public virtual ICollection<AccountMovementMonthlyConsolidation> AccountMovementMonthlyConsolidations { get; set; }

    }
}
