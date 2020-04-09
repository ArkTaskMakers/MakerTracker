namespace MakerTracker.Models
{
    public class InventoryProductSummaryDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductImageUrl { get; set; }
        public int Amount { get; set; }
    }
}