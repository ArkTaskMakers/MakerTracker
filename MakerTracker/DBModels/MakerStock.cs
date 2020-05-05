namespace MakerTracker.DBModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class MakerStock
    {
        [Key]
        public int Id { get; set; }

        public int MakerId { get; set; }

        [ForeignKey(nameof(MakerId))]
        public Maker Maker { get; set; }

        public int ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }

        public int Quantity { get; set; }

        public DateTime UpdatedOn { get; set; }
    }
}