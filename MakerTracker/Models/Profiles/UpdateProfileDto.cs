﻿namespace MakerTracker.Models.Profiles
{
    [TypeScriptModel]
    public class UpdateProfileDto
    {
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
        public string ZipCode { get; set; }
        public bool IsDropOffPoint { get; set; }
        public bool IsSupplier { get; set; }
        public bool IsRequestor { get; set; }
        public bool HasCadSkills { get; set; }
    }
}