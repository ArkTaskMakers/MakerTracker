namespace MakerTracker.Models
{
    [TypeScriptModel]
    public class AddInventoryDto
    {
        public int ProductId { get; set; }
        public int Amount { get; set; }
    }
}