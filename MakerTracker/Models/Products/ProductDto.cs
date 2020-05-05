namespace MakerTracker.Models.Products
{
    using System.ComponentModel.DataAnnotations;

    [TypeScriptModel]
    public class ProductDto
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string InstructionUrl { get; set; }

        public bool IsDeprecated { get; set; }

        public string ImageUrl { get; set; }

        public int ProductTypeId { get; set; }
    }
}