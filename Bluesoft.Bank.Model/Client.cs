namespace Bluesoft.Bank.Model
{
    public class Client : BaseEntity
    {
        public User User { get; set; }

        public string FullName { get; set; }

        public ICollection<Account> Accounts { get; set; }
    }

}
