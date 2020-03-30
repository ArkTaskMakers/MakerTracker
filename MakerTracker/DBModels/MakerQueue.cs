using System;

namespace MakerTracker.DBModels
{
    public class MakerOrder {
        public int Id { get; set; }
        public Maker Maker { get; set; }
        
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public DateTime OrderedOn { get; set; }
        public DateTime ExpectedFinished { get; set; }
        public int PromisedCount { get; set; }
    }
}
