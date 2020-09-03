using System.ComponentModel.DataAnnotations;
using NetTopologySuite.Geometries;

namespace MakerTracker.DBModels
{
    using System;
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;

    public class Profile
    {
        public int Id { get; set; }
        public string Auth0Id { get; set; }
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
        public bool? IsSelfQuarantined { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ZipCode { get; set; }
        public bool? IsDropOffPoint { get; set; }
        public bool IsSupplier { get; set; }
        public bool IsRequestor { get; set; }
        public bool HasCadSkills { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public Point Location { get; set; }

        public string AdminNotes { get; set; }

        //If the profile is imported you can set this to help track it back in the external source
        [MaxLength(50)]
        public string ImportId { get; set; }

        public ICollection<Transaction> TransactionFrom { get; set; } = new List<Transaction>();
        public ICollection<Transaction> TransactionTo { get; set; } = new List<Transaction>();



        public ICollection<ProfileHierarchy> Parents { get; set; } = new List<ProfileHierarchy>();
        public ICollection<ProfileHierarchy> Children { get; set; } = new List<ProfileHierarchy>();

        public ICollection<Need> Needs { get; set; } = new List<Need>();
        public ICollection<MakerEquipment> MakerEquipments { get; set; } = new List<MakerEquipment>();

        public bool IsVerified { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this user has gone through the new onboarding process.
        /// </summary>
        public bool HasOnboarded { get; set; }

        public static void ConfigureEntity(ModelBuilder builder)
        {
            builder.Entity<Profile>(entity =>
            {
                entity.HasMany(x => x.TransactionFrom).WithOne(x => x.From);
                entity.HasMany(x => x.TransactionTo).WithOne(x => x.To);
            });
        }
    }
}