using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CardChequeModule.Areas.CardCheque.Models;
using CardChequeModule.Models;
using CardChequeModule.WebReference;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CardChequeModule.Areas.CardCheque.Controllers
{
    
    public class HomeController : Controller
    {
        //private Entities db = new Entities();

        //private JsonSerializer serializer=new JsonSerializer();
        //public ActionResult Index()
        //{
        //    return View();
        //}


        //[Authorize(Roles = "admin,user")]
        //public PartialViewResult Find(string cardno, DateTime billingDate, string currency)
        //{
        //    ClientDetails details;
           
        //    ViewBag.DepositSaveMsg = "";
   
        //    string SrchDate = billingDate.ToString("dd/MM/yyyy");

        //    WebReference.CCMService ccmService = new CCMService();
        //    string clientDetails = ccmService.GetClientDetails(SrchDate, currency, cardno);
        //    OCCCARDINFO cardInfo=new OCCCARDINFO();
            
        //    if (clientDetails == "null")
        //    {
               
        //            ViewBag.Msg = "No Data Found";
        //            ViewBag.Flag = 1;
        //        ViewBag.Branch = db.BRANCHINFO.ToList();

        //        return PartialView("TestPartial", new CardChequeViewModel());
        //    }
        //    else
        //    {
        //        JObject json = JObject.Parse(clientDetails);
        //        details = (ClientDetails)serializer.Deserialize(new JTokenReader(json), typeof(ClientDetails));

               
        //        cardInfo.CHNAME = details.CLIENTNAME;
        //        cardInfo.CARDNO = details.CARDNO;
        //        var cardInfoCheck = db.OCCCARDINFO.FirstOrDefault(x => x.CARDNO == cardInfo.CARDNO);
        //        if (cardInfoCheck==null)
        //        {
        //            db.OCCCARDINFO.Add(cardInfo);
        //            db.SaveChanges();
        //        }
                


        //        ViewBag.Msg = "";
        //        ViewBag.Flag = 0;
        //        CardChequeViewModel viewModel=new CardChequeViewModel();
        //        viewModel.ClientDetails = details;
        //        viewModel.Deposit=new DEPOSIT();
        //        viewModel.Deposit.CARDNUMBER = details.CARDNO;

        //        ViewBag.Branch = db.BRANCHINFO.ToList();

        //        return PartialView("TestPartial", viewModel);
        //    }


        //}

     


       
        //[AllowAnonymous]
        //[HttpPost]
        //[Authorize(Roles = "admin,user")]
        //public ActionResult DepositPost(DEPOSIT deposit,ClientDetails clientDetails)
        //{

        //   // deposit.CARDNUMBER = (string) Session["CardNo"];
        //    //if (ModelState.IsValid)
        //    //{

        //    try
        //    {
        //        OCCUSER user = (OCCUSER) Session["User"];
        //        deposit.CREATEDBY = user.ID;
        //        deposit.ISAUTHORIZED = false;
        //        var userId = user.EMPLOYEEID;
        //        var length = userId.Length;
        //        char first = userId[0];
        //        char last = userId[userId.Length - 1];
        //        char middle = userId[(userId.Length - 1) / 2];
        //        var refNo = (first + middle + last).ToString(CultureInfo.InvariantCulture);
        //        refNo += length;
        //        refNo += DateTime.Now.ToString("ddMMyyHHmmssfff");
        //        refNo += deposit.CARDNUMBER.Substring(deposit.CARDNUMBER.Length - 4);
        //        deposit.REFERENCENUMBER = refNo;
        //        //deposit.REFERENCENUMBER;
        //        db.DEPOSIT.Add(deposit);
        //        db.SaveChanges();
        //        Session["DepositSaveMsg"] = "Data saved successfully.\nReferance Number is: " + refNo;
        //        //ViewBag.DepositSaveMsg = "Data saved successfully";

        //    }
        //    catch (Exception exception)
        //    {
        //        Session["DepositSaveMsg"] = "Data is not saved successfully";
        //        //ViewBag.DepositSaveMsg = "Data not saved";
        //    }

        //    ViewBag.Branch = db.BRANCHINFO.ToList();
        //    //return PartialView("TestPartial",new CardChequeViewModel());
        //    return RedirectToAction("DepositShow");



        //}
        //public void DestroySession()
        //{
        //    Session["DepositSaveMsg"] = null;
        //}

        //[Authorize(Roles = "admin,authorizer")]
        //public ActionResult DepositShow()
        //{
        //   ViewBag.branch = new SelectList(db.BRANCHINFO.ToList(), "BRANCHCODE", "BRANCHNAME");
        //    DepositeViewModel depositeViewModel;
        //   List<DepositeViewModel> list=new List<DepositeViewModel>();
        //    int count = 0;
        //    foreach (var deposit in db.DEPOSIT.ToList())
        //    {
        //        count++;
        //        depositeViewModel = new DepositeViewModel();
        //        depositeViewModel.Id = count;
        //        depositeViewModel.Amount = (decimal) deposit.AMOUNT;
        //        string branchName = db.BRANCHINFO.Where(x => x.BRANCHCODE == deposit.BRANCHCODE).Select(x=>x.BRANCHNAME).FirstOrDefault();
        //        depositeViewModel.BranchName = branchName;
        //        depositeViewModel.CardHolderName =
        //            db.OCCCARDINFO.Where(x => x.CARDNO == deposit.CARDNUMBER).Select(x => x.CHNAME).FirstOrDefault();
        //        depositeViewModel.CardNumber = deposit.CARDNUMBER;
        //        depositeViewModel.DeapositeDate = (DateTime) deposit.DATE;
        //        depositeViewModel.RefarenceNo = deposit.REFERENCENUMBER;
        //        list.Add(depositeViewModel);
        //    }
        //   return View(list);
 
        //}

        //[HttpPost]
        //[Authorize(Roles = "admin,authorizer")]
        //public ActionResult DepositShow(string cardno, string branch, DateTime? depositDate)
        //{
        //    var query = db.DEPOSIT.ToList();
        //    if (!String.IsNullOrEmpty(cardno))
        //    {
        //        cardno = cardno.Trim();
        //        query =  query.Where(x => x.CARDNUMBER == cardno).ToList();
        //    }
        //    if (!String.IsNullOrEmpty(branch))
        //    {
        //        query = query.Where(x => x.BRANCHCODE == branch).ToList();
        //    }
        //    if (depositDate!=null)
        //    {
        //        query = query.Where(x => x.DATE == depositDate).ToList();
        //    }

        //    DepositeViewModel depositeViewModel;
        //    List<DepositeViewModel> list = new List<DepositeViewModel>();
        //    int count = 0;
        //    foreach (var deposit in query)
        //    {
        //        count++;
        //        depositeViewModel = new DepositeViewModel();
        //        depositeViewModel.Id = count;
        //        depositeViewModel.Amount = (decimal)deposit.AMOUNT;
        //        string branchName = db.BRANCHINFO.Where(x => x.BRANCHCODE == deposit.BRANCHCODE).Select(x => x.BRANCHNAME).FirstOrDefault();
        //        depositeViewModel.BranchName = branchName;
        //        depositeViewModel.CardHolderName =
        //            db.OCCCARDINFO.Where(x => x.CARDNO == deposit.CARDNUMBER).Select(x => x.CHNAME).FirstOrDefault();
        //        depositeViewModel.CardNumber = deposit.CARDNUMBER;
        //        depositeViewModel.DeapositeDate = (DateTime)deposit.DATE;
        //        list.Add(depositeViewModel);
        //    }

        //    ViewBag.branch = new SelectList(db.BRANCHINFO.ToList(), "BRANCHCODE", "BRANCHNAME");
        //    return View(list);
        //}


        ///// <summary>
        ///// ///////////////////////////////////////////////////////////////////////////////////////////////
        ///// </summary>
        ///// <returns></returns>


        //[Authorize(Roles = "admin,user")]
        //public ActionResult DepositShowForTaler()
        //{
        //    ViewBag.branch = new SelectList(db.BRANCHINFO.ToList(), "BRANCHCODE", "BRANCHNAME");
        //    DepositeViewModel depositeViewModel;
        //    List<DepositeViewModel> list = new List<DepositeViewModel>();
        //    int count = 0;
        //    foreach (var deposit in db.DEPOSIT.ToList())
        //    {
        //        count++;
        //        depositeViewModel = new DepositeViewModel();
        //        depositeViewModel.Id = count;
        //        depositeViewModel.Amount = (decimal)deposit.AMOUNT;
        //        string branchName = db.BRANCHINFO.Where(x => x.BRANCHCODE == deposit.BRANCHCODE).Select(x => x.BRANCHNAME).FirstOrDefault();
        //        depositeViewModel.BranchName = branchName;
        //        depositeViewModel.CardHolderName =
        //            db.OCCCARDINFO.Where(x => x.CARDNO == deposit.CARDNUMBER).Select(x => x.CHNAME).FirstOrDefault();
        //        depositeViewModel.CardNumber = deposit.CARDNUMBER;
        //        depositeViewModel.DeapositeDate = (DateTime)deposit.DATE;
        //        list.Add(depositeViewModel);
        //    }
        //    return View(list);

        //}

        //[HttpPost]
        //[Authorize(Roles = "admin,user")]
        //public ActionResult DepositShowForTaler(string cardno, string branch, DateTime? depositDate)
        //{
        //    var query = db.DEPOSIT.ToList();
        //    if (!String.IsNullOrEmpty(cardno))
        //    {
        //        cardno = cardno.Trim();
        //        query = query.Where(x => x.CARDNUMBER == cardno).ToList();
        //    }
        //    if (!String.IsNullOrEmpty(branch))
        //    {
        //        query = query.Where(x => x.BRANCHCODE == branch).ToList();
        //    }
        //    if (depositDate != null)
        //    {
        //        query = query.Where(x => x.DATE == depositDate).ToList();
        //    }

        //    DepositeViewModel depositeViewModel;
        //    List<DepositeViewModel> list = new List<DepositeViewModel>();
        //    int count = 0;
        //    foreach (var deposit in query)
        //    {
        //        count++;
        //        depositeViewModel = new DepositeViewModel();
        //        depositeViewModel.Id = count;
        //        depositeViewModel.Amount = (decimal)deposit.AMOUNT;
        //        string branchName = db.BRANCHINFO.Where(x => x.BRANCHCODE == deposit.BRANCHCODE).Select(x => x.BRANCHNAME).FirstOrDefault();
        //        depositeViewModel.BranchName = branchName;
        //        depositeViewModel.CardHolderName =
        //            db.OCCCARDINFO.Where(x => x.CARDNO == deposit.CARDNUMBER).Select(x => x.CHNAME).FirstOrDefault();
        //        depositeViewModel.CardNumber = deposit.CARDNUMBER;
        //        depositeViewModel.DeapositeDate = (DateTime)deposit.DATE;
        //        list.Add(depositeViewModel);
        //    }

        //    ViewBag.branch = new SelectList(db.BRANCHINFO.ToList(), "BRANCHCODE", "BRANCHNAME");
        //    return View(list);
        //}

        //public ActionResult Cheque()
        //{
        //    ViewBag.branch = new SelectList(db.BRANCHINFO.ToList(), "BRANCHCODE", "BRANCHNAME");
        //    return View();
        //}
        //[HttpPost]
        //public ActionResult Cheque(string cardNo)
        //{
        //    ClientDetails details;
        //    ViewBag.branch = new SelectList(db.BRANCHINFO.ToList(), "BRANCHCODE", "BRANCHNAME");
        //    WebReference.CCMService ccmService = new CCMService();
        //    string clientDetails = ccmService.GetClientDetails("10/11/2013", "BDT", cardNo);
        //    OCCCARDINFO cardInfo = new OCCCARDINFO();
        //    if (clientDetails == "null")
        //    {

        //        ViewBag.Msg = "No Data Found";
               
        //    }
        //    else
        //    {
        //        JObject json = JObject.Parse(clientDetails);
        //        details = (ClientDetails)serializer.Deserialize(new JTokenReader(json), typeof(ClientDetails));


        //        return Json(details, JsonRequestBehavior.AllowGet);
        //    }
        //    return View();

        //}

        //[HttpPost]
        //public ActionResult ChequePost(string branch, DateTime? depositDate, string name, string mobile, string cardNo,decimal? amount,decimal? usdAmount)
        //{
        //    if (amount != null || usdAmount != null)
        //    {


        //        try
        //        {
        //            DEPOSIT deposit = new DEPOSIT();
        //            OCCUSER user = (OCCUSER) Session["User"];
        //            //deposit.CREATEDBY = user.ID;
        //            deposit.BRANCHCODE = branch;
        //            deposit.DATE = depositDate;
        //            deposit.CARDNUMBER = cardNo;
        //            deposit.ISAUTHORIZED = false;
        //            if (amount != null)
        //            {
        //                deposit.AMOUNT = amount;
        //            }
        //            else
        //            {
        //                deposit.AMOUNT = usdAmount;
        //            }
        //            var userId = user.EMPLOYEEID;
        //            var length = userId.Length;
        //            char first = userId[0];
        //            char last = userId[userId.Length - 1];
        //            char middle = userId[(userId.Length - 1) / 2];
        //            var refNo = (first + middle + last).ToString(CultureInfo.InvariantCulture);
        //            refNo += length;
        //            refNo += DateTime.Now.ToString("ddMMyyHHmmssfff");
        //            refNo += deposit.CARDNUMBER.Substring(deposit.CARDNUMBER.Length - 4);
        //            deposit.REFERENCENUMBER = refNo;

        //            db.DEPOSIT.Add(deposit);
        //            db.SaveChanges();
        //        //    Session["DepositSaveMsg"] = "Data saved successfully.\nReferance Number is: ";
        //            //ViewBag.DepositSaveMsg = "Data saved successfully";

        //        }
        //        catch (Exception exception)
        //        {
        //            return RedirectToAction("Cheque");
        //        }

        //    }
        //    return RedirectToAction("Cheque");

        //}
    }
}
