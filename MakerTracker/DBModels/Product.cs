using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MakerTracker.DBModels
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [Display(Name = "Instruction URL")]
        public string InstructionUrl { get; set; }

        public bool IsDeprecated { get; set; }

        public string ImageUrl { get; set; }

        public ICollection<NeedDetail> NeedRequests{ get; set; } = new List<NeedDetail>();

        public ICollection<MakerOrder> InMakerQueues { get; set; } = new List<MakerOrder>();

        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

        public ICollection<ProductRequirement> PrecursorsRequired { get; set; } = new List<ProductRequirement>();
        public ICollection<ProductRequirement> UsedInProducts { get; set; } = new List<ProductRequirement>();

    }
}