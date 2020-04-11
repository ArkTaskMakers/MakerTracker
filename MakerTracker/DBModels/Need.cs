using System;
using System.Collections.Generic;

namespace MakerTracker.DBModels
{
    public class Need   
    {
        public int Id { get; set; }
        public Profile Profile{ get; set; }

        public DateTime RequestedOn { get; set; }
        public DateTime FulFillByDate  { get; set; }

        public ICollection<NeedDetail> NeedDetails { get; set; } = new List<NeedDetail>();
    }
}