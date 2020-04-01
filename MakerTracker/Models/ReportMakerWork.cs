using System;

namespace MakerTracker.Models
{
    public class ReportMakerWork
    {
        public DateTime ExpectedFinished { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public bool IsFinished { get; set; }
        public int ExpectedCount { get; set; }
    }
}