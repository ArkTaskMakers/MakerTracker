namespace MakerTracker.Models
{
    using System;

    [TypeScriptModel]
    public class NeedLookupDto
    {
        public int NeedId { get; set; }

        public int ProductId { get; set; }

        public string ProfileDisplayName { get; set; }

        public int OutstandingQuantity { get; set; }

        public DateTime? DueDate { get; set; }
    }
}