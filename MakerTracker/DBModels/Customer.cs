using System.Collections.Generic;

namespace MakerTracker.DBModels
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Profile OwnerProfile { get; set; }

        public ICollection<CustomerOrder> Orders { get; set; } = new List<CustomerOrder>();
        public ICollection<CustomerStock> Stocks { get; set; } = new List<CustomerStock>();

    }
}