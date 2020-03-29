using System.Collections.Generic;

namespace MakerTracker.DBModels
{
    public class Maker
    {
        public int Id { get; set; }
        public Profile Profile { get; set; }
        public ICollection<MakerEquipment> Equipment { get; set; } = new List<MakerEquipment>();
        public ICollection<MakerMaterial> Materials { get; set; } = new List<MakerMaterial>();
        public ICollection<MakerQueue> MakerQueue { get; set; } = new List<MakerQueue>();

        public bool HasCadSkills { get; set; }
        public bool AccessToFaceMask { get; set; }
        public bool AccessToGloves { get; set; }
    }

    public class MakerEquipment
    {
        public int Id { get; set; }
        public Maker Maker { get; set; }
        public Equipment Equipment { get; set; }
    }

    public class MakerMaterial
    {
        public int Id { get; set; }
        public Maker Maker { get; set; }
        public Material Material { get; set; }
    }
}
