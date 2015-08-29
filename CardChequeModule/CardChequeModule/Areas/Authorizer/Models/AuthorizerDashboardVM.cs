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
        public List<CARDCHEREUISITION> Requisitions { get; set; }
        public List<CARDCHTRAN> ChequeTrans { get; set; }
    }
}