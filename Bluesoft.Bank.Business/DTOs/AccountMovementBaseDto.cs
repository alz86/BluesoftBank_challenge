using Bluesoft.Bank.Model;

namespace Bluesoft.Bank.Business.DTOs
{
    public class AccountMovementBaseDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }

        public AccountMovementType Type { get; set; }
    }
}
