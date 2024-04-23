namespace Bluesoft.Bank.Business.Exceptions
{
    public class AccountNotFoundException : ExceptionBase
    {
        public AccountNotFoundException() : base("Account not found")
        {
        }
    }
}
