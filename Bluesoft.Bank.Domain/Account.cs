namespace Bluesoft.Bank.Model
{
    public class BaseEntity
    {
        public int Id { get; set; }
    }

    public class User : BaseEntity
    {
        public string Username { get; set; }

        //NOTE: other fields, like password, email, etc. were not included
        //for the sake of brevity
    }

    public class Client : BaseEntity
    {
        public User User { get; set; }

        public string FullName { get; set; }

        //NOTE: Cities would have their own database table in a real application
        //for the sake of simplicity, we just use a string
        public City City { get; set; }
    }

    public class City : BaseEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

    }

    public class Account : BaseEntity
    {
        public Client Client { get; set; }

        public DateTime Creation { get; set; }

        public decimal Balance { get; set; }
    }

    public abstract class AccountMovementBase : BaseEntity
    {
        public Account Account { get; set; }


        public decimal Value { get; set; }

    }

    public enum AccountMovementType
    {
        Deposit,
        Withdrawal
    }

    public class AccountMovement : AccountMovementBase
    {
        public DateTime Date { get; set; }

        public AccountMovementType Type { get; set; }

        public WithdrawalDetails? WithdrawalDetails { get; set; }
    }

    public class AccountMovementConsolidationBase : AccountMovementBase
    {
        public DateOnly Date { get; set; }
        public int Quantity { get; set; }
    }

    public class WithdrawalDetails : BaseEntity
    {
        public ATM? ATM { get; set; }

        public WithdrawalTypes Type { get; set; }

        //More fields..
    }

    public enum WithdrawalTypes
    {
        Cashier,
        ATM
    }

    public class ATM : BaseEntity
    {
        public string Address { get; set; }

        public City City { get; set; }

        //More fields..
    }

    public class AccountMovementDailyConsolidation : AccountMovementConsolidationBase { }

    public class AccountMovementMonthlyConsolidation : AccountMovementConsolidationBase { }

}
