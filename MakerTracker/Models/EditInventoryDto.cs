namespace MakerTracker.Models
{
    [TypeScriptModel]
    public class EditInventoryDto
    {
        public int ProductId { get; set; }
        public int NewAmount { get; set; }
    }
}