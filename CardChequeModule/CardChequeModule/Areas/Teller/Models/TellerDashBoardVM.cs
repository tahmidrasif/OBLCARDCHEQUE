using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CardChequeModule.Models;

namespace CardChequeModule.Areas.Teller.Models
{
    public class TellerDashBoardVM
    {
        public string Name { get; set; }
        public string BranchName { get; set; }
        public string Email { get; set; }
        public string JobTitle { get; set; }
        public string EmployeeId { get; set; }
        public string PreDeptName { get; set; }
        public List<CARDCHEREUISITION> Requisitions { get; set; }
        public List<DEPOSIT> Deposits { get; set; }
        public List<CARDCHTRAN> ChequeTrans { get; set; }
        public int TotalVisaPayment { get; set; }
        public decimal TotalVisaPaymentCollection { get; set; }

    }
}