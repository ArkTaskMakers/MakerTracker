namespace MakerTracker.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    [TypeScriptModel]
    public class MakerStockDto
    {
        [Key]
        public int Id { get; set; }

        public int MakerId {get;set;}

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public int Quantity { get; set; }

        public DateTime UpdatedOn { get; set; }
    }
}
