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
                var visaList = db.DEPOSIT.Where(x => x.CREATEDBY == user.ID).OrderByDescending(x => x.ID).Take(5).ToList();
                var requisitionList = db.CARDCHEREUISITION.Where(x => x.CREATEDBY == user.ID).OrderByDescending(x => x.ID).Take(5).ToList();
                var cardchtanList = db.CARDCHTRAN.Where(x => x.CREATEDBY == user.ID).OrderByDescending(x => x.ID).Take(5).ToList();

                //TAHMID
                //WebRef.OBLAPP oblApp = new WebRef.OBLAPP();
                //DataTable dt = oblApp.GetByUserID(user.EMPLOYEEID);
                //foreach (DataRow dataRow in dt.Rows)
                //{
                //    aVm.BranchName = (string)dataRow[21];
                //    aVm.Email = (string)dataRow[9];
                //    aVm.EmployeeId = (string)dataRow[2];
                //    aVm.JobTitle = (string)dataRow[7];
                //    aVm.Name = (string)dataRow[3];
                //    aVm.PreDeptName = (string)dataRow[17];

                //}
                aVm.BranchName = "MyBranchName";
                aVm.Email = "myMailId";
                aVm.EmployeeId = "MyEmpId";
                aVm.JobTitle = "MyJobTitle";
                aVm.Name = "MyName";
                aVm.PreDeptName = "MyPresentDepartment";
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
        public ActionResult Index(DateTime startDate, DateTime endDate, string btnName,TellerDashBoardVM vm)
        {
            try
            {
                OCCUSER user = (OCCUSER)Session["User"];
                TellerDashBoardVM aVm = new TellerDashBoardVM();
                var visaList = db.DEPOSIT.Where(x => x.CREATEDBY == user.ID).OrderByDescending(x => x.ID).Take(5).ToList();
                var requisitionList = db.CARDCHEREUISITION.Where(x => x.CREATEDBY == user.ID).OrderByDescending(x => x.ID).Take(5).ToList();
                var cardchtanList = db.CARDCHTRAN.Where(x => x.CREATEDBY == user.ID).OrderByDescending(x => x.ID).Take(5).ToList();


                //TAHMID
                //WebRef.OBLAPP oblApp = new WebRef.OBLAPP();
                //DataTable dt = oblApp.GetByUserID(user.EMPLOYEEID);
                //foreach (DataRow dataRow in dt.Rows)
                //{
                //    aVm.BranchName = (string)dataRow[21];
                //    aVm.Email = (string)dataRow[9];
                //    aVm.EmployeeId = (string)dataRow[2];
                //    aVm.JobTitle = (string)dataRow[7];
                //    aVm.Name = (string)dataRow[3];
                //    aVm.PreDeptName = (string)dataRow[17];

                //}
                //aVm.BranchName = vm.BranchName;
                //aVm.Email = vm.Email;
                //aVm.EmployeeId = vm.EmployeeId;
                //aVm.JobTitle = vm.JobTitle;
                //aVm.Name = vm.Name;
                //aVm.PreDeptName = vm.PreDeptName;
                aVm.BranchName = "MyBranchName";
                aVm.Email = "myMailId";
                aVm.EmployeeId = "MyEmpId";
                aVm.JobTitle = "MyJobTitle";
                aVm.Name = "MyName";
                aVm.PreDeptName = "MyPresentDepartment";

                if (btnName == "visa")
                {
                    visaList = db.DEPOSIT.Where(x => x.CREATEDON >= startDate).Where(x => x.CREATEDON <= endDate).Where(x => x.CREATEDBY == user.ID).Take(5).ToList();
                    //aVm.Deposits = List;
                }
                else if (btnName == "requisition")
                {
                     requisitionList = db.CARDCHEREUISITION.Where(x => x.CREATEDON >= startDate).Where(x => x.CREATEDON <= endDate).Where(x => x.CREATEDBY == user.ID).Take(5).ToList();
                   
                }
                else if (btnName == "cardcheque")
                {
                     cardchtanList = db.CARDCHTRAN.Where(x => x.CREATEDON >= startDate).Where(x => x.CREATEDON <= endDate).Where(x => x.CREATEDBY == user.ID).Take(5).ToList();
                  
                }
                return View();
            }
            catch (Exception)
            {

                return Json("Error");
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