namespace Bluesoft.Bank.Model
{
    public class Branch : BaseEntity
    {
        public City City { get; set; }

        public string Name { get; set; }

        public ICollection<Account> Accounts { get; set; }
        public ICollection<DepositDetails> DepositDetails { get; set; }

        public ICollection<WithdrawalDetails> WithdrawalDetails { get; set; }

        //More fields..
    }
}
