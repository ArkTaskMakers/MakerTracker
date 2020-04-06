using MakerTracker.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakerTracker.Models
{
    public class TransactionViewModel
    {
        public int Id { get;set; }
        public int Product { get;set;}
        public int From { get;set;}
        public int To { get;set;}
        public int Amount { get;set; }
        public int ConfirmationCode { get;set;}

       
    }
}
