using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CardChequeModule.Models;

namespace CardChequeModule.Areas.Admin.Models
{
    public class AdminDashboardVM
    {
        public AdminDashboardVM()
        {
            EmployeeInfoVm=new EmployeeInfoVM();
        }
        public EmployeeInfoVM EmployeeInfoVm { get; set; }
        public List<CARDCHEREUISITION> Requisitions { get; set; }
        public List<DEPOSIT> Deposits { get; set; }
        public List<CARDCHTRAN> ChequeTrans { get; set; }
        public int TotalVisaPayment { get; set; }
        public decimal TotalVisaPaymentCollection { get; set; }
        public int TotalReqisitionRequest { get; set; }
        public decimal TotalCardPayment { get; set; }
        public int TotalChequePaymentNumber { get; set; }
    }
}