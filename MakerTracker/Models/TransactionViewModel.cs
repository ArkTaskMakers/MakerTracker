using System.ComponentModel.DataAnnotations;

namespace MakerTracker.Models
{
    public class TransactionViewModel
    {
        public int Id { get; set; }
        public int Product { get; set; }
        public int From { get; set; }
        public int To { get; set; }
        public int Amount { get; set; }

        [Display(Name = "Confirmation Code")]
        public string ConfirmationCode { get; set; }
    }
}
