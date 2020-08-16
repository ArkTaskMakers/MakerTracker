namespace MakerTracker.DBModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    ///     Model class representing user feedback.
    /// </summary>
    [Table(nameof(Feedback))]
    public class Feedback
    {
        /// <summary>
        ///     Gets or sets the identifier.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        ///     Gets or sets the profile identifier of the user sending this feedback.
        /// </summary>
        [Required]
        public int ProfileId { get; set; }

        /// <summary>
        ///     Gets or sets the type of feedback this is meant to indicate.
        /// </summary>
        [Required]
        public FeedbackType Type { get; set; }

        /// <summary>
        ///     Gets or sets the comment the user has submitted.
        /// </summary>
        [Required]
        [MaxLength(2000)]
        public string Comment { get; set; }

        /// <summary>
        ///     Gets or sets the URL of the page the user submitted the feedback from.
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string Url { get; set; }

        /// <summary>
        ///     Gets or sets the created date.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the profile of the user sending this feedback.
        /// </summary>
        [ForeignKey(nameof(ProfileId))]
        public virtual Profile Profile { get; set; }
    }
}
