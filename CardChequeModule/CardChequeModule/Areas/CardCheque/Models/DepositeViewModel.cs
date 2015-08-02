using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CardChequeModule.Models;

namespace CardChequeModule.Areas.CardCheque.Models
{
    public class DepositeViewModel
    {
        public int Id { get; set; }
        public string CardNumber { get; set; }
        public string CardHolderName { get; set; }
        public DateTime DeapositeDate { get; set; }
        public decimal Amount{ get; set; }
        public string BranchName { get; set; }
        public string RefarenceNo { get; set; }
        public virtual DEPOSIT Deposit { get; set; }
        public virtual BRANCHINFO Branchinfo { get; set; }
        public virtual ClientDetails ClientDetails { get; set; }
    }
}