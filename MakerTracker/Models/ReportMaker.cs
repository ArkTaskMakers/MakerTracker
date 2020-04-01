namespace MakerTracker.Models
{
    public class ReportMaker
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public int ProductsQueued { get; set; }
        public int ProductsFinished { get; set; }
    }
}