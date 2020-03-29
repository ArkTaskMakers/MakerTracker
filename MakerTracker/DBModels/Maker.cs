using System.Collections.Generic;

namespace MakerTracker.DBModels
{
    public class Maker
    {
        public int Id { get; set; }
        public Profile OwnerProfile { get; set; }
        public ICollection<MakerStock> Stocks { get; set; } = new List<MakerStock>();
        public ICollection<MakerEquipment> Equipment { get; set; } = new List<MakerEquipment>();
        public ICollection<MakerMaterial> Materials { get; set; } = new List<MakerMaterial>();
        public ICollection<MakerOrder> MakerQueue { get; set; } = new List<MakerOrder>();

        public bool HasCadSkills { get; set; }
        public bool AccessToFaceMask { get; set; }
        public bool AccessToGloves { get; set; }
    }
}
