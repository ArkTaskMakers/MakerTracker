using System;
using System.Collections.Generic;

namespace MakerTracker.DBModels
{
    public class Profile
    {
        public int Id { get; set; }
        public string Auth0Id { get; set; }
        public bool CompanyName { get; set; }
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

        public ICollection<Transaction> TransactionFrom { get; set; } = new List<Transaction>();
        public ICollection<Transaction> TransactionTo { get; set; } = new List<Transaction>();

    }
}