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

        public string Address { get; set; }

        public string Address2 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string ZipCode { get; set; }

        public bool IsDropOffPoint { get; set; }

        public string ContactEmail { get; set; }

        public string SpecialInstructions { get; set; }
    }
}