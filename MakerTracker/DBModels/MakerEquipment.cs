namespace MakerTracker.DBModels
{
    public class MakerEquipment
    {
        public int Id { get; set; }
        public Maker Maker { get; set; }
        public Equipment Equipment { get; set; }
    }
}