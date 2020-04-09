using System;

namespace MakerTracker.DBModels
{
    public class CustomerStock
    {
        public int Id { get; set; }
        public Customer Customer { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}