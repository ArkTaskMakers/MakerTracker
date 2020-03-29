namespace MakerTracker.DBModels
{
    public class MakerMaterial
    {
        public int Id { get; set; }
        public Maker Maker { get; set; }
        public Material Material { get; set; }
    }
}