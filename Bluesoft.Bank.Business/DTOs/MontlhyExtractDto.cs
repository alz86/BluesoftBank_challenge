using Bluesoft.Bank.Model;

namespace Bluesoft.Bank.Business.DTOs
{
    public class MontlhyExtractDto
    {
        public int Month { get; set; }

        public int Year { get; set; }

        public decimal InitialBalance { get; set; }

        public decimal FinalBalance { get; set; }

        public decimal TotalDebits { get; set; }

        public decimal TotalCredits { get; set; }

        public IList<AccountMovementDto> Movements { get; set; }
    }
}
