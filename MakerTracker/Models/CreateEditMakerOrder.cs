using System;
using System.ComponentModel.DataAnnotations;

namespace MakerTracker.Models
{
    public class CreateEditMakerOrder
    {
        public int MakerOrderId { get; set; }

        [Display(Name = "Product")]
        public int ProductId { get; set; }

        [Display(Name = "Expected Finish Date")]
        [DataType(DataType.Date)]
        public DateTime ExpectedFinished { get; set; }

        [Display(Name = "Number Expected")]
        public int PromisedCount { get; set; }

        [Display(Name = "Already Finished?")]
        public bool Finished { get; set; }
    }
}
