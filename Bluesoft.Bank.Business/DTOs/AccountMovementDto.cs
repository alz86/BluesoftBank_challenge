using Bluesoft.Bank.Model;

namespace Bluesoft.Bank.Business.DTOs
{
    public class AccountMovementDto : AccountMovementBaseDto
    {
        public DateTime Date { get; set; }

        public AccountMovementType Type { get; set; }

    }
}
