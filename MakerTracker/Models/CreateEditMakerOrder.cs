using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MakerTracker.DBModels;

namespace MakerTracker.Models
{
    public class CreateEditMakerOrder
    {
        public int MakerOrderId { get; set; }
        public int ProductId { get; set; }
        public DateTime ExpectedFinished { get; set; }
        public int PromisedCount { get; set; }
    }
}
