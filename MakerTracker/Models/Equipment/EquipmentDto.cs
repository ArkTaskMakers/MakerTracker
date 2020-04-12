namespace MakerTracker.Models.Equipment
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    ///     Data transfer object for equipment
    /// </summary>
    [TypeScriptModel]
    public class EquipmentDto
    {
        /// <summary>
        ///     Gets or sets the identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the description.
        /// </summary>
        [MaxLength(500)]
        public string Description { get; set; }
    }
}