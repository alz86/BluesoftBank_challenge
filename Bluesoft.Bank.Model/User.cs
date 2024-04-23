namespace Bluesoft.Bank.Model
{
    public class User : BaseEntity
    {
        public Client Client { get; set; }
        public string Username { get; set; }

        //NOTE: other fields, like password, email, etc. were not included
        //for the sake of brevity
    }

}
