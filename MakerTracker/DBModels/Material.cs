using System.Collections.Generic;

namespace MakerTracker.DBModels
{
    public class Material
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<MakerMaterial> UsedBy { get; set; } = new List<MakerMaterial>();
    }
}