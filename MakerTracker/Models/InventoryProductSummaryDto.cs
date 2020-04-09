namespace MakerTracker.Models
{
    /// <summary>
    ///     Transfer object for inventory summary
    /// </summary>
    [TypeScriptModel]
    public class InventoryProductSummaryDto
    {
        /// <summary>
        ///     Gets or sets the product identifier.
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        ///     Gets or sets the name of the product.
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        ///     Gets or sets the product image URL.
        /// </summary>
        public string ProductImageUrl { get; set; }

        /// <summary>
        ///     Gets or sets the amount.
        /// </summary>
        public int Amount { get; set; }
    }
}