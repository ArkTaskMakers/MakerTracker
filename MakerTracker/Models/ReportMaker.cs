using System.ComponentModel.DataAnnotations;

namespace MakerTracker.Models
{
    public class ReportMaker
    {
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }

        [Display(Name = "# Products Queued")]
        public int ProductsQueued { get; set; }

        [Display(Name = "# Products Finished")]
        public int ProductsFinished { get; set; }
    }
}