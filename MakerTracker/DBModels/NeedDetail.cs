namespace MakerTracker.DBModels
{
    public class NeedDetail
    {
        public int Id { get; set; }
        public Need Need{ get; set; }

        public Product Product { get; set; }

        public int Quantity { get; set; }
    }
}