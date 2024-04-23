namespace Bluesoft.Bank.Model
{
    public class City : BaseEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }


        public ICollection<Branch> Branches { get; set; }
    }
}
