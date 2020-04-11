namespace MakerTracker.Models.Products
{
    [TypeScriptModel]
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string InstructionUrl { get; set; }
        public bool IsDeprecated { get; set; }
        public string ImageUrl { get; set; }
    }
}