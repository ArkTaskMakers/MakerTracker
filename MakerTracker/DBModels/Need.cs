namespace MakerTracker.DBModels
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    ///     Model class representing a need for PPE
    /// </summary>
    [Table(nameof(Need))]
    public class Need
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
        ///     Gets or sets the profile.
        /// </summary>
        [ForeignKey(nameof(ProfileId))]
        public Profile Profile { get; set; }

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
        ///     Gets or sets the profile.
        /// </summary>
        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }

        /// <summary>
        ///     Gets or sets the quantity.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        ///     Gets or sets any special instructions tied to the Need.
        /// </summary>
        public string SpecialInstructions {get;set;}

        /// <summary>
        ///     Gets or sets any administrative notes attached to the need.
        /// </summary>
        public string AdminNotes { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}