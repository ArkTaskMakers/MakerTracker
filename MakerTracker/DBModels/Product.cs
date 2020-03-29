using System.Collections.Generic;

namespace MakerTracker.DBModels
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string InstructionUrl { get; set; }

        public ICollection<MakerQueue> InMakerQueues { get; set; } = new List<MakerQueue>();
    }
}