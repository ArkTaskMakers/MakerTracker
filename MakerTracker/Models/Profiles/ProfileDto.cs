namespace MakerTracker.Models.Profiles
{
    using System;
    using System.ComponentModel.DataAnnotations;

    [TypeScriptModel]
    public class ProfileDto
    {
        [Key]
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Bio { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public bool IsSelfQuarantined { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ZipCode { get; set; }
        public bool IsDropOffPoint { get; set; }
        public bool IsSupplier { get; set; }
        public bool IsRequestor { get; set; }
        public bool HasCadSkills { get; set; }
        public string DisplayName => $"{FirstName} {LastName} ({Email})";

        /// <summary>
        ///     Gets or sets a value indicating whether this user has gone through the new onboarding process.
        /// </summary>
        public bool HasOnboarded { get; set; }

    }
}
