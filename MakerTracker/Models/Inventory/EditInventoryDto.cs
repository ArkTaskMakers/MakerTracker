namespace MakerTracker.Models.Inventory
{
    [TypeScriptModel]
    public class EditInventoryDto
    {
        public int ProductId { get; set; }
        public int NewAmount { get; set; }
    }
}