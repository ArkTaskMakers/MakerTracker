namespace MakerTracker.DBModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using MakerTracker.Models.Products;

    /// <summary>
    ///     Model class representing a type or category of products.
    /// </summary>
    [TypeScriptModel]
    public class ProductTypeDto
    {
        /// <summary>
        ///     Gets or sets the identifier for this product type.
        /// </summary>
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
        public virtual ICollection<ProductDto> Products { get; set; } = new List<ProductDto>();
    }
}