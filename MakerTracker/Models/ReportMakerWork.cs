using System;
using System.ComponentModel.DataAnnotations;

namespace MakerTracker.Models
{
    public class ReportMakerWork
    {
        [Display(Name = "Expected Completion Date")]
        public DateTime ExpectedFinished { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Product ID")]
        public int ProductId { get; set; }

        [Display(Name = "Product Name")]
        public string ProductName { get; set; }

        [Display(Name = "Finished?")]
        public bool IsFinished { get; set; }

        [Display(Name = "# Expected")]
        public int ExpectedCount { get; set; }
    }
}