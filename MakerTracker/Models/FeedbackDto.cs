namespace MakerTracker.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using MakerTracker.DBModels;

    /// <summary>
    ///     Model class representing user feedback.
    /// </summary>
    [TypeScriptModel]
    public class FeedbackDto
    {
        /// <summary>
        ///     Gets or sets the identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     Gets or sets the type of feedback this is meant to indicate.
        /// </summary>
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

        public string SubmittedBy { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
}
