namespace MakerTracker.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    ///     Data transfer object for an equipment assignment to a maker
    /// </summary>
    [TypeScriptModel]
    public class MakerEquipmentDto
    {
        /// <summary>
        ///     Gets or sets the identifier.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the profile identifier.
        /// </summary>
        public int ProfileId { get; set; }

        /// <summary>
        ///     Gets or sets the equipment identifier.
        /// </summary>
        [Required]
        public int EquipmentId { get; set; }

        /// <summary>
        /// Gets or sets the name of the equipment.
        /// </summary>
        public string EquipmentName { get; set; }

        /// <summary>
        ///     Gets or sets the manufacturer of the equipment owned.
        /// </summary>
        [MaxLength(100)]
        public string Manufacturer { get; set; }

        /// <summary>
        /// Gets or sets the model number of the equipment owned.
        /// </summary>
        [MaxLength(100)]
        public string ModelNumber { get; set; }
    }
}