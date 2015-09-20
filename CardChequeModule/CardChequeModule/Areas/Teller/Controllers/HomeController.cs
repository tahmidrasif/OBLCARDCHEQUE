using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CardChequeModule.Areas.Teller.Models;
using CardChequeModule.Models;
using CardChequeModule.ServiceReference;

namespace CardChequeModule.Areas.Teller.Controllers
{
    [Authorize(Roles = "teller")]
    public class HomeController : Controller
    {
        //
        // GET: /Teller/Home/


        OBLCARDCHEQUEEntities db = new OBLCARDCHEQUEEntities();

        public ActionResult Index()
        {
            try
            {
                OCCUSER user = (OCCUSER)Session["User"];
                TellerDashBoardVM aVm = new TellerDashBoardVM();

                var visaList = db.DEPOSIT.Where(x => x.ISDELETE == false).Where(x => x.CREATEDBY == user.ID).OrderByDescending(x => x.ID).ToList();
                aVm.TotalVisaPayment = visaList.Count;
                aVm.TotalVisaPaymentCollection = (decimal)visaList.Sum(x => x.AMOUNT);
                visaList = visaList.Take(5).ToList();


                var requisitionList = db.CARDCHEREUISITION.Where(x => x.CREATEDBY == user.ID).Where(x=>x.ISDELETE!=true).OrderByDescending(x => x.ID).ToList();
                aVm.TotalReqisitionRequest = requisitionList.Count;
                requisitionList = requisitionList.Take(5).ToList();


                var cardchtanList = db.CARDCHTRAN.Where(x => x.CREATEDBY == user.ID).Where(x=>x.ISDELETE!=true).OrderByDescending(x => x.ID).ToList();
                aVm.TotalCardPayment = cardchtanList.Sum(x => x.AMOUNT);
                aVm.TotalChequePaymentNumber = cardchtanList.Count;
                cardchtanList = cardchtanList.Take(5).ToList();


                WebRef.OBLAPP oblApp = new WebRef.OBLAPP();
                DataTable dt = oblApp.GetByUserID(user.EMPLOYEEID);
                foreach (DataRow dataRow in dt.Rows)
                {
                    aVm.EmployeeInfoVm.BranchName = (string)dataRow[21];
                    aVm.EmployeeInfoVm.Email = (string)dataRow[9];
                    aVm.EmployeeInfoVm.EmployeeId = (string)dataRow[2];
                    aVm.EmployeeInfoVm.JobTitle = (string)dataRow[7];
                    aVm.EmployeeInfoVm.Name = (string)dataRow[3];
                    aVm.EmployeeInfoVm.PreDeptName = (string)dataRow[17];

                }
               
                
                //var visaPaymentList = db.DEPOSIT.Where(x => x.CREATEDBY == user.ID).ToList();
                aVm.Deposits = visaList;
                aVm.Requisitions = requisitionList;
                aVm.ChequeTrans = cardchtanList;
               // aVm.TotalVisaPayment = visaPaymentList.Count;
               // aVm.TotalVisaPaymentCollection = (decimal)visaPaymentList.Sum(x => x.AMOUNT);
                return View(aVm);
            }
            catch (Exception exception)
            {

                return RedirectToAction("Error", "Home", new { Area = "" });
            }

        }

        [HttpPost]
        public ActionResult Index(DateTime? startDate, DateTime? endDate, string btnName)
        {
            try
            {
                if (startDate != null || endDate != null)
                {
                    OCCUSER user = (OCCUSER)Session["User"];
                    TellerDashBoardVM aVm = new TellerDashBoardVM();

                    var visaList = db.DEPOSIT.Where(x => x.ISDELETE == false).Where(x => x.CREATEDBY == user.ID).Where(x=>x.ISDELETE!=true).OrderByDescending(x => x.ID).ToList();
                    aVm.TotalVisaPayment = visaList.Count;
                    aVm.TotalVisaPaymentCollection = (decimal)visaList.Sum(x => x.AMOUNT);
                    visaList = visaList.Take(5).ToList();


                    var requisitionList = db.CARDCHEREUISITION.Where(x => x.CREATEDBY == user.ID).Where(x=>x.ISDELETE!=true).OrderByDescending(x => x.ID).ToList();
                    aVm.TotalReqisitionRequest = requisitionList.Count;
                    requisitionList = requisitionList.Take(5).ToList();


                    var cardchtanList = db.CARDCHTRAN.Where(x => x.CREATEDBY == user.ID).Where(x=>x.ISDELETE!=true).OrderByDescending(x => x.ID).ToList();
                    aVm.TotalCardPayment = cardchtanList.Sum(x => x.AMOUNT);
                    aVm.TotalChequePaymentNumber = cardchtanList.Count;
                    cardchtanList = cardchtanList.Take(5).ToList();


                    
                    WebRef.OBLAPP oblApp = new WebRef.OBLAPP();
                    DataTable dt = oblApp.GetByUserID(user.EMPLOYEEID);
                    foreach (DataRow dataRow in dt.Rows)
                    {
                    aVm.EmployeeInfoVm.BranchName = (string)dataRow[21];
                    aVm.EmployeeInfoVm.Email = (string)dataRow[9];
                    aVm.EmployeeInfoVm.EmployeeId = (string)dataRow[2];
                    aVm.EmployeeInfoVm.JobTitle = (string)dataRow[7];
                    aVm.EmployeeInfoVm.Name = (string)dataRow[3];
                    aVm.EmployeeInfoVm.PreDeptName = (string)dataRow[17];
                    }
                   
                    if (btnName == "visa")
                    {
                        visaList = db.DEPOSIT.Where(x => x.ISDELETE != false).Where(x => x.CREATEDON >= startDate).Where(x => x.CREATEDON <= endDate).Where(x => x.CREATEDBY == user.ID).ToList();
                        aVm.TotalVisaPayment = visaList.Count;
                        aVm.TotalVisaPaymentCollection = (decimal)visaList.Sum(x => x.AMOUNT);
                        //aVm.Deposits = List;
                    }
                    else if (btnName == "requisition")
                    {
                        requisitionList = db.CARDCHEREUISITION.Where(x => x.CREATEDON >= startDate).Where(x=>x.ISDELETE!=true).Where(x => x.CREATEDON <= endDate).Where(x => x.CREATEDBY == user.ID).ToList();
                        aVm.TotalReqisitionRequest = requisitionList.Count;

                    }
                    else if (btnName == "cardcheque")
                    {
                        cardchtanList = db.CARDCHTRAN.Where(x=>x.ISDELETE!=true).Where(x => x.CREATEDON >= startDate).Where(x => x.CREATEDON <= endDate).Where(x => x.CREATEDBY == user.ID).ToList();
                        aVm.TotalCardPayment = cardchtanList.Sum(x => x.AMOUNT);
                        aVm.TotalChequePaymentNumber = cardchtanList.Count;

                    }
                    aVm.Deposits = visaList;
                    aVm.Requisitions = requisitionList;
                    aVm.ChequeTrans = cardchtanList;

                    return View(aVm);
                }
                return RedirectToAction("Index");
            }
            catch (Exception)
            {

                return RedirectToAction("Error","Home",new {Area=""});
            }

        }

        public ActionResult SearchReasult(DateTime startDate, DateTime endDate, string btnName)
        {
            try
            {
                if (btnName == "visa")
                {
                    OCCUSER user = (OCCUSER)Session["User"];
                    var List = db.DEPOSIT.Where(x => x.CREATEDON >= startDate).Where(x => x.CREATEDON <= endDate).Where(x => x.CREATEDBY == user.ID).ToList();                   
                }
                return View();
            }
            catch (Exception)
            {

                return Json("Error");
            }

        }
    }
}