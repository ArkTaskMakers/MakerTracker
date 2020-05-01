namespace MakerTracker.DBModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    ///     Model class representing a type or category of products.
    /// </summary>
    public class ProductType
    {
        /// <summary>
        ///     Gets or sets the identifier for this product type.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        ///     Gets or sets the name of this product type.
        /// </summary>
        [MaxLength(100)]
        [Required]
        public string Name { get; set; }

        public int SortOrder { get; set; }

        /// <summary>
        ///     Gets or sets the products for this product type.
        /// </summary>
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();

        public static void ConfigureEntity(ModelBuilder builder)
        {
            builder.Entity<ProductType>(entity =>
            {
                entity.HasData(new ProductType
                {
                    Id = 1,
                    Name = "Other",
                    SortOrder = 9999
                });
            });
        }
    }
}