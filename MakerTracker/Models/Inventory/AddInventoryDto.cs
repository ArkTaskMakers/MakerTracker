namespace MakerTracker.Models.Inventory
{
    [TypeScriptModel]
    public class AddInventoryDto
    {
        public int ProductId { get; set; }
        public int Amount { get; set; }
    }
}