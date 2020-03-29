using System;

namespace MakerTracker.DBModels
{
    public class MakerStock
    {
        public int Id { get; set; }
        public Maker Maker { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}