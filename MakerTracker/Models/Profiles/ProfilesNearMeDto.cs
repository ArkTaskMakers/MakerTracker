using System.ComponentModel.DataAnnotations;
using NetTopologySuite.Geometries;

namespace MakerTracker.Models.Profiles
{
    [TypeScriptModel]
    public class ProfilesNearMeDto
    {
        [Key]
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public bool IsSupplier { get; set; }
        public bool IsRequestor { get; set; }
        public double DistanceInMiles { get; set; }
    }
}