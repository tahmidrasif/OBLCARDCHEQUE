using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CardChequeModule.Areas.Authorizer.Models
{
    public class CardChequeAuthorizrVM
    {
        public long ID { get; set; }
        public string APPROVALNO { get; set; }
        public int ChqStatus { get; set; }
        public string REMARKS { get; set; }
    }
}