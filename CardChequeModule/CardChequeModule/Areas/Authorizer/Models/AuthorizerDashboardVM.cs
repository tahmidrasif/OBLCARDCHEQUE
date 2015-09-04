using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CardChequeModule.Models;

namespace CardChequeModule.Areas.Authorizer.Models
{
    public class AuthorizerDashboardVM
    {
        public AuthorizerDashboardVM()
        {
            EmployeeInfoVm=new EmployeeInfoVM();
        }
        public EmployeeInfoVM EmployeeInfoVm { get; set; }
        public List<CARDCHEREUISITION> PendingRequisitionList { get; set; }
        public List<CARDCHTRAN> PendingCardChequeList { get; set; }
        public List<CARDCHEREUISITION> AuthorizedRequisitionList { get; set; }
        public List<CARDCHTRAN> AuthorizedCardChequeList { get; set; }
        public int PendingRequisitionCount { get; set; }
        public int PendingCardChequeCount { get; set; }
        public int AuthRequisitionCount { get; set; }
        public int AuthCardCHequeCount{ get; set; }
        public decimal PendingCardChequeAmount { get; set; }
        public decimal AuthorizedCardChequeAmount { get; set; }

    }
}