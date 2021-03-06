namespace MakerTracker.DBModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    ///     Model class representing a type of equipment that can be owned and used by makers.
    /// </summary>
    public class Equipment
    {
        /// <summary>
        ///     Gets or sets the identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        [MaxLength(100)]
        [Required]
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the description.
        /// </summary>
        [MaxLength(500)]
        public string Description { get; set; }

        /// <summary>
        ///     Gets or sets the existing connections where this equipment is used by a maker.
        /// </summary>
        public ICollection<MakerEquipment> UsedBy { get; set; } = new List<MakerEquipment>();
    }
}