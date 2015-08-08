using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CardChequeModule.Areas.Establishment.Models
{
    public class DownloadViewModel
    {
        public string CARDNO { get; set; }
        public string BRANCHNAME { get; set; }
        public string CARDHOLDERNAME { get; set; }
        public string NoOfLeavesPerBook { get; set; }
        public string ROUTINGNO { get; set; }
        public int TransactionCode { get; set; }
    }
}