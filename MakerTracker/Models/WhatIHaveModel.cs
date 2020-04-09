using System.ComponentModel.DataAnnotations;

namespace MakerTracker.Models
{
    public class WhatIHaveModel
    {
        [Display(Name = "Product ID")]
        public int ProductId { get; set; }

        [Display(Name = "Product Name")]
        public string ProductName { get; set; }

        public int Amount { get; set; }

        public string ProductImageUrl { get; set; }
    }
}
