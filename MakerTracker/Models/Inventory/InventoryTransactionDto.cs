namespace MakerTracker.Models.Inventory
{
    using MakerTracker.Models.Products;

    [TypeScriptModel]
    public class InventoryTransactionDto
    {
        public ProductDto Product { get; set; }

        public int Amount { get; set; }

        public int? NeedId { get; set; }
    }
}