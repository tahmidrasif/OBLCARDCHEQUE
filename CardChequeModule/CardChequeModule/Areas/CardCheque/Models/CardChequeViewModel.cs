using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CardChequeModule.Models;

namespace CardChequeModule.Areas.CardCheque.Models
{
    public class CardChequeViewModel
    {
        public ClientDetails ClientDetails { get; set; }
        public DEPOSIT Deposit { get; set; }
    }
}