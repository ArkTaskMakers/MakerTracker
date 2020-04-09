using System.Collections.Generic;

namespace MakerTracker.DBModels
{
    public class Equipment
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<MakerEquipment> UsedBy { get; set; } = new List<MakerEquipment>();
    }
}