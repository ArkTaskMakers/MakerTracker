using System;
using System.Collections.Generic;

namespace MakerTracker.DBModels
{
    public class CustomerOrder
    {
        public int Id { get; set; }
        public Customer Customer { get; set; }

        public DateTime RequestedOn { get; set; }
        public DateTime FulFillByDate  { get; set; }

        public ICollection<CustomerOrderDetail> OrderDetails { get; set; } = new List<CustomerOrderDetail>();
    }
}