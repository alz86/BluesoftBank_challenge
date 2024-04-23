    namespace Bluesoft.Bank.Model
{
    public class WithdrawalDetails : BaseEntity
    {
        public Branch? Branch { get; set; }

        public WithdrawalTypes Type { get; set; }

        public AccountMovement AccountMovement { get; set; }

        //More fields..
    }

}
