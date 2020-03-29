namespace MakerTracker.DBModels
{
    public class MakerQueue {
        public int Id { get; set; }
        public Maker Maker { get; set; }
        public Product Product { get; set; }
        public int PromisedCount { get; set; }
        public int CompletedCount { get; set; }
    }
}