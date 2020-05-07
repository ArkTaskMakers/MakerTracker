using System;
using System.ComponentModel.DataAnnotations;

namespace MakerTracker.DBModels
{
    [Display(Name = "Maker Order")]
    public class MakerOrder {
        public int Id { get; set; }

        public Profile Profile { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }


        [Display(Name = "Date Ordered")]
        public DateTime OrderedOn { get; set; }

        [Display(Name = "Expected Completion Date")]
        public DateTime ExpectedFinished { get; set; }

        [Display(Name = "# Promised")]
        public int PromisedCount { get; set; }

        public bool Finished { get; set; }
    }
}
