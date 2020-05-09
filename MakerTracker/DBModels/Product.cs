namespace MakerTracker.DBModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Microsoft.EntityFrameworkCore;

    public class Product
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public string InstructionUrl { get; set; }

        public bool IsDeprecated { get; set; }

        public string ImageUrl { get; set; }

        public int ProductTypeId { get; set; }

        [ForeignKey(nameof(ProductTypeId))]
        public ProductType ProductType { get; set; }

        public ICollection<Need> NeedRequests{ get; set; } = new List<Need>();

        public ICollection<MakerOrder> InMakerQueues { get; set; } = new List<MakerOrder>();

        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

        public ICollection<ProductRequirement> PrecursorsRequired { get; set; } = new List<ProductRequirement>();

        public ICollection<ProductRequirement> UsedInProducts { get; set; } = new List<ProductRequirement>();

        public static void ConfigureEntity(ModelBuilder builder)
        {
            builder.Entity<Product>(entity =>
            {
                entity.HasMany(x => x.PrecursorsRequired).WithOne(x => x.Parent);
                entity.HasMany(x => x.UsedInProducts).WithOne(x => x.Child);
                entity.Property(x => x.ProductTypeId).HasDefaultValue(1);
            });
        }

    }
}