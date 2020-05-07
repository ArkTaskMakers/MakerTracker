namespace MakerTracker.DBModels
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    ///     Model class representing the connection between a maker and equipment with metadata.
    /// </summary>
    public class MakerEquipment
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
        /// Gets or sets the profile.
        /// </summary>
        [ForeignKey(nameof(ProfileId))]
        public Profile Profile { get; set; }

        /// <summary>
        /// Gets or sets the equipment identifier.
        /// </summary>
        public int EquipmentId { get; set; }

        /// <summary>
        ///     Gets or sets the equipment the maker owns.
        /// </summary>
        [ForeignKey(nameof(EquipmentId))]
        public Equipment Equipment { get; set; }

        /// <summary>
        ///     Gets or sets the manufacturer of the equipment owned.
        /// </summary>
        [MaxLength(100)]
        public string Manufacturer { get; set; }

        /// <summary>
        ///     Gets or sets the model number of the equipment owned.
        /// </summary>
        [MaxLength(100)]
        public string ModelNumber { get; set; }
    }
}