using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MakerTracker.DBModels;

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
