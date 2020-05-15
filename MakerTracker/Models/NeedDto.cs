namespace MakerTracker.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    ///     Model class representing a need for PPE
    /// </summary>
    [TypeScriptModel]
    public class NeedDto
    {
        /// <summary>
        ///     Gets or sets the identifier for the need.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        ///     Gets or sets the profile identifier.
        /// </summary>
        public int ProfileId { get; set; }

        /// <summary>
        ///     Gets or sets the date the need was created on.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        ///     Gets or sets the date the need requests to be fulfilled by.
        /// </summary>
        public DateTime? DueDate { get; set; }

        /// <summary>
        ///     Gets or sets the date the need was fulfilled on.
        /// </summary>
        public DateTime? FulfilledDate { get; set; }

        /// <summary>
        ///     Gets or sets the product identifier.
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        ///     Gets or sets the quantity.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        ///     Gets a value indicating whether this instance is overdue.
        /// </summary>
        public bool IsOverdue => DueDate != null && DueDate < DateTime.Today;

        /// <summary>
        ///     Gets or sets any special instructions tied to the Need.
        /// </summary>
        public string SpecialInstructions { get; set; }
    }
}