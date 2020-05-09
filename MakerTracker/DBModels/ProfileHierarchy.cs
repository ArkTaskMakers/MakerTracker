namespace MakerTracker.DBModels
{
    using System.ComponentModel.DataAnnotations.Schema;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    ///     Model class representing the relationship of a parent profile to a child profile.
    /// </summary>
    [Table(nameof(ProfileHierarchy))]
    public class ProfileHierarchy
    {
        /// <summary>
        ///     Gets or sets the parent profile identifier.
        /// </summary>
        public int ParentProfileId { get; set; }

        /// <summary>
        ///     Gets or sets the parent profile.
        /// </summary>
        public Profile ParentProfile { get; set; }

        /// <summary>
        ///     Gets or sets the child profile identifier.
        /// </summary>
        public int ChildProfileId { get; set; }

        /// <summary>
        ///     Gets or sets the child profile.
        /// </summary>
        public Profile ChildProfile { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether parent is owner on the supplied child profile.
        /// </summary>
        public bool ParentIsOwner { get; set; }

        /// <summary>
        ///     Configures the entity for EF.
        /// </summary>
        /// <param name="builder">The builder.</param>
        public static void ConfigureEntity(ModelBuilder builder)
        {
            builder.Entity<ProfileHierarchy>(entity =>
            {
                entity.HasKey(e => new { e.ParentProfileId, e.ChildProfileId });
                entity.HasOne(e => e.ParentProfile).WithMany(e => e.Children).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.ChildProfile).WithMany(e => e.Parents).OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
