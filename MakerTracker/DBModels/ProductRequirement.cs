namespace MakerTracker.DBModels
{
    public class ProductRequirement
    {
        public int Id { get; set; }
        public Product Parent { get; set; }
        public Product Child { get; set; }
        public int ChildQuantityRequired { get; set; }
    }
}