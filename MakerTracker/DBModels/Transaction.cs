namespace MakerTracker.DBModels
{
    public class Transaction
    {
        public int Id { get; set;}
        public Product Product { get; set; }
        public Profile From { get; set; }
        public Profile To { get; set; }
        public int Amount { get; set; }
        public int ConfirmationCode { get; set; }

    }
}
