namespace Bluesoft.Bank.Model
{
    public class DepositDetails : BaseEntity
    {
        public DepositTypes Type { get; set; }

        public Branch? Branch { get; set; }

        public AccountMovement AccountMovement { get; set; }

        //More Fields...
    }

}
