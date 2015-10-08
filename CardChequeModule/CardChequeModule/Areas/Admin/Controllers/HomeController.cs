using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CardChequeModule.Areas.Admin.Models;
using CardChequeModule.Areas.Teller.Models;
using CardChequeModule.Models;
using NPOI.SS.Formula.Functions;
using PagedList;

namespace CardChequeModule.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class HomeController : Controller
    {
        OBLCARDCHEQUEEntities db = new OBLCARDCHEQUEEntities();
        public ActionResult Index()
        {
            try
            {
                OCCUSER user = (OCCUSER) Session["User"];
                AdminDashboardVM aVm = new AdminDashboardVM();

                var visaList = db.DEPOSIT.Where(x => x.ISDELETE == false).OrderByDescending(x => x.ID).ToList();
                aVm.TotalVisaPayment = visaList.Count;
                aVm.TotalVisaPaymentCollection = (decimal) visaList.Sum(x => x.AMOUNT);
                visaList = visaList.Take(5).ToList();


                var requisitionList = db.CARDCHEREUISITION.Where(x=>x.ISDELETE!=true).OrderByDescending(x => x.ID).ToList();
                aVm.TotalReqisitionRequest = requisitionList.Count;
                requisitionList = requisitionList.Take(5).ToList();


                var cardchtanList = db.CARDCHTRAN.Where(x=>x.ISDELETE!=true).OrderByDescending(x => x.ID).ToList();
                aVm.TotalCardPayment = cardchtanList.Sum(x => x.AMOUNT);
                aVm.TotalChequePaymentNumber = cardchtanList.Count;
                cardchtanList = cardchtanList.Take(5).ToList();

                
                WebRef.OBLAPP oblApp = new WebRef.OBLAPP();
                DataTable dt = oblApp.GetByUserID(user.EMPLOYEEID);
                foreach (DataRow dataRow in dt.Rows)
                {
                    aVm.EmployeeInfoVm.BranchName = (string) dataRow[21];
                    aVm.EmployeeInfoVm.Email = (string) dataRow[9];
                    aVm.EmployeeInfoVm.EmployeeId = (string) dataRow[2];
                    aVm.EmployeeInfoVm.JobTitle = (string) dataRow[7];
                    aVm.EmployeeInfoVm.Name = (string) dataRow[3];
                    aVm.EmployeeInfoVm.PreDeptName = (string) dataRow[17];

                }
                

                aVm.Deposits = visaList;
                aVm.Requisitions = requisitionList;
                aVm.ChequeTrans = cardchtanList;

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
                    AdminDashboardVM aVm = new AdminDashboardVM();

                    var visaList = db.DEPOSIT.Where(x => x.ISDELETE == false).OrderByDescending(x => x.ID).ToList();
                    aVm.TotalVisaPayment = visaList.Count;
                    aVm.TotalVisaPaymentCollection = (decimal)visaList.Sum(x => x.AMOUNT);
                    visaList = visaList.Take(5).ToList();


                    var requisitionList = db.CARDCHEREUISITION.Where(x=>x.ISDELETE!=true).OrderByDescending(x => x.ID).ToList();
                    aVm.TotalReqisitionRequest = requisitionList.Count;
                    requisitionList = requisitionList.Take(5).ToList();


                    var cardchtanList = db.CARDCHTRAN.Where(x=>x.ISDELETE!=true).OrderByDescending(x => x.ID).ToList();
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
                        visaList = db.DEPOSIT.Where(x => x.ISDELETE == false).Where(x => x.CREATEDON.Value.Date >= startDate.Value.Date).Where(x => x.CREATEDON.Value.Date <= endDate.Value.Date).ToList();
                        aVm.TotalVisaPayment = visaList.Count;
                        aVm.TotalVisaPaymentCollection = (decimal)visaList.Sum(x => x.AMOUNT);
                        //aVm.Deposits = List;
                    }
                    else if (btnName == "requisition")
                    {
                        requisitionList = db.CARDCHEREUISITION.Where(x=>x.ISDELETE!=true).Where(x => x.CREATEDON.Date >= startDate.Value.Date).Where(x => x.CREATEDON.Date <= endDate.Value.Date).ToList();
                        aVm.TotalReqisitionRequest = requisitionList.Count;

                    }
                    else if (btnName == "cardcheque")
                    {
                        cardchtanList = db.CARDCHTRAN.Where(x=>x.ISDELETE!=true).Where(x => x.CREATEDON >= startDate).Where(x => x.CREATEDON <= endDate).ToList();
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
            catch (Exception exception)
            {
                return RedirectToAction("Error", "Home", new { Area = "" });
            }
          //  return View();
        }


        public ActionResult UserCreation()
        {
            ViewBag.BRANCH = new SelectList(db.BRANCHINFO.ToList(), "ID", "BRANCHNAME");
            ViewBag.TYPE = new SelectList(db.OCCENUMERATION.Where(x => x.Type == "user").ToList(), "ID", "Name");
            return View();
        }

        public ActionResult UserInfoByEmpId(string empId)
        
        {
            string userName = "";
            string branchCode = "";
            string branchName = "";
            try
            {

                WebRef.OBLAPP oblApp = new WebRef.OBLAPP();
                DataTable dt = oblApp.GetByUserID(empId);

                if (dt.Rows.Count > 0)
                {

               
                foreach (DataRow dataRow in dt.Rows)
                {
                    userName = (string)dataRow[3];
                    branchCode = (string)dataRow[22];
                    branchName = (string)dataRow[21];
                }

                long branchId = db.BRANCHINFO.Where(x => x.BRANCHCODE == branchCode).Select(x => x.ID).FirstOrDefault();
                //return Json(new { userName = "Rasif", branchId = 5, branchName = "Khatungonj" }, JsonRequestBehavior.AllowGet);

                return Json(new {flag=0, userName, branchId, branchName }, JsonRequestBehavior.AllowGet);
                }
                return Json(new{flag=1}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {

                return Json(new {flag=2,msg=exception.Message}, JsonRequestBehavior.DenyGet);
            }


        }

        [HttpPost]
        public ActionResult UserCreation(OCCUSER occuser, int? BranchId)
        {
            try
            {
                if (!UserAlreadyExist(occuser.EMPLOYEEID))
                {
                    OCCUSER user = (OCCUSER)Session["User"];
                    occuser.CREATEDBY = user.ID;
                    occuser.CREATEDON = DateTime.Now;
                    occuser.ISACTIVE = true;
                    occuser.BRANCH = BranchId;
                    if (ModelState.IsValid)
                    {
                        db.OCCUSER.Add(occuser);
                        db.SaveChanges();
                        var msg = "Data saved successfully. ";

                        return Json(msg, JsonRequestBehavior.AllowGet);
                    }


                    ViewBag.BRANCH = new SelectList(db.BRANCHINFO.ToList(), "ID", "BRANCHNAME", BranchId);
                    ViewBag.TYPE = new SelectList(db.OCCENUMERATION.Where(x => x.Type == "user").ToList(), "ID", "Name", occuser.TYPE);
                    return View();
                }
                var mymsg = "User Already Exists";
                return Json(mymsg, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                
                return Json(exception.Message,JsonRequestBehavior.AllowGet);
            }

        }

        private bool UserAlreadyExist(string employeeid)
        {
            var user = GetByEmpId(employeeid);
            if (user != null)
                return true;
            return false;
        }
        private OCCUSER GetByEmpId(string employeeid)
        {
            var user = db.OCCUSER.Where(x=>x.ISACTIVE!=false).FirstOrDefault(x => x.EMPLOYEEID == employeeid);
            return user;
        }

        public ActionResult UserList(int? TYPE, string EMPLOYEEID, string USERNAME, int? page)
        {
            ViewBag.TYPE = new SelectList(db.OCCENUMERATION.Where(x => x.Type == "user").ToList(), "ID", "Name");
            var List = db.OCCUSER.Include(c => c.BRANCHINFO).Include(c => c.OCCENUMERATION).Where(x => x.ISACTIVE == true).ToList();

            if (TYPE != null)
            {
                List = List.Where(x => x.TYPE == TYPE).ToList();
                ViewBag.TYPE = new SelectList(db.OCCENUMERATION.Where(x => x.Type == "user").ToList(), "ID", "Name", TYPE);
                ViewBag.currtype = TYPE;
                // ViewBag.STATUS = new SelectList(statusID, "Key", "Value", statusID.Where(x => x.Key == STATUS));
            }
            if (!String.IsNullOrEmpty(EMPLOYEEID))
            {
                EMPLOYEEID = EMPLOYEEID.Trim();
                List = List.Where(x => x.EMPLOYEEID.Contains(EMPLOYEEID)).ToList();
                ViewBag.curEmpId = EMPLOYEEID;
            }
            if (!String.IsNullOrEmpty(USERNAME))
            {
                USERNAME = USERNAME.Trim();
                List = List.Where(x => x.USERNAME.Contains(USERNAME)).ToList();
                ViewBag.curUserName = USERNAME;
            }
            int pageSize = ConstantConfig.PageSizes;
            int pageNumber = ((page ?? 1));
            return View(List.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult UserDetail(long? id)
        {
            try
            {
                var user = db.OCCUSER.FirstOrDefault(x => x.ID == id);
                if (user != null)
                {

                    ViewBag.BRANCH = new SelectList(db.BRANCHINFO.ToList(), "ID", "BRANCHNAME", user.BRANCH);
                    ViewBag.TYPE = new SelectList(db.OCCENUMERATION.Where(x => x.Type == "user").ToList(), "ID", "Name", user.TYPE);
                    return View(user);
                }
                return RedirectToAction("Error", "Home", new { Area = "" });
            }
            catch (Exception)
            {

                return RedirectToAction("Error", "Home", new { Area = "" });
            }


        }

        [HttpPost]
        public ActionResult UserDetail(OCCUSER aUser, string btnName)
        {
            try
            {
                OCCUSER user = (OCCUSER)Session["User"];
                if (String.Equals(btnName, "update"))
                {

                    aUser.MODIFIEDBY = user.ID;
                    aUser.MODIFIEDON = DateTime.Now;
                    db.Entry(aUser).State = EntityState.Modified;
                    db.SaveChanges();
                    var msg = "Successfully Updated";
                    return Json(msg, JsonRequestBehavior.AllowGet);
                }
                if (String.Equals(btnName, "delete"))
                {
                    aUser.MODIFIEDBY = user.ID;
                    aUser.MODIFIEDON = DateTime.Now;
                    aUser.ISACTIVE = false;
                    db.Entry(aUser).State = EntityState.Modified;
                    db.SaveChanges();
                    var msg = "Successfully Removed";
                    return Json(msg, JsonRequestBehavior.DenyGet);
                }
                return RedirectToAction("Error", "Home", new { Area = "" });
            }
            catch (Exception)
            {

                return RedirectToAction("Error", "Home", new { Area = "" });
            }


        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
