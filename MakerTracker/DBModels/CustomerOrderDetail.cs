namespace MakerTracker.DBModels
{
    public class CustomerOrderDetail
    {
        public int Id { get; set; }
        public CustomerOrder CustomerOrder { get; set; }

        public Product Product { get; set; }

        public int Quantity { get; set; }
    }
}