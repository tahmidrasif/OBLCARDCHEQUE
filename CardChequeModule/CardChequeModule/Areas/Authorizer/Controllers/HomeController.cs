using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CardChequeModule.Areas.Authorizer.Models;
using CardChequeModule.Models;

namespace CardChequeModule.Areas.Authorizer.Controllers
{
    public class HomeController : Controller
    {
        //Entities db=new Entities();
      
        //public ActionResult Index()
        //{
        //    return View();
        //}
        //public ActionResult PendingDeposit()
        //{
        //    var list=db.DEPOSIT.Where(x => x.ISAUTHORIZED == false).ToList();
            
        //    List<PendingViewModel> vmlist=new List<PendingViewModel>();
        //    foreach (var deposit in list)
        //    {
        //        PendingViewModel vm = new PendingViewModel();
        //        vm.Amount = (decimal) deposit.AMOUNT;
        //        string branchName = db.BRANCHINFO.Where(x => x.BRANCHCODE == deposit.BRANCHCODE).Select(x => x.BRANCHNAME).FirstOrDefault();
        //        vm.BranchName = branchName;
        //        vm.CardHolderName =
        //          db.OCCCARDINFO.Where(x => x.CARDNO == deposit.CARDNUMBER).Select(x => x.CHNAME).FirstOrDefault();

        //        vm.CardNumber = deposit.CARDNUMBER;
        //        vm.DeapositeDate = (DateTime) deposit.DATE;
        //        vmlist.Add(vm);

        //    }
        //    return View(vmlist);
        //}
	}
}