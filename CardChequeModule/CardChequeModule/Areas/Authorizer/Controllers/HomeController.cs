using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CardChequeModule.Areas.Admin.Models;
using CardChequeModule.Areas.Authorizer.Models;
using CardChequeModule.Models;
using Microsoft.Office.Interop.Excel;
using PagedList;

namespace CardChequeModule.Areas.Authorizer.Controllers
{
    [Authorize(Roles = "authorizer,admin")]
    public class HomeController : Controller
    {
        private OBLCARDCHEQUEEntities db = new OBLCARDCHEQUEEntities();
        public ActionResult Index()
        {
            try
            {
                OCCUSER user = (OCCUSER)Session["User"];
                AuthorizerDashboardVM aVm = new AuthorizerDashboardVM();

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
                aVm.EmployeeInfoVm.BranchName = "MyBranchName";
                aVm.EmployeeInfoVm.Email = "myMailId";
                aVm.EmployeeInfoVm.EmployeeId = "MyEmpId";
                aVm.EmployeeInfoVm.JobTitle = "MyJobTitle";
                aVm.EmployeeInfoVm.Name = "MyName";
                aVm.EmployeeInfoVm.PreDeptName = "MyPresentDepartment";
                return View(aVm);
            }
            catch (Exception)
            {

                return RedirectToAction("Error", "Home", new { Area = "" });
            }
           
           // return View();
        }

        [HttpPost]
        public ActionResult Index(DateTime? startDate, DateTime? endDate, string btnName)
        {
            try
            {
                if (startDate != null || endDate != null)
                {
                }
                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
                return RedirectToAction("Error", "Home", new { Area = "" });
            }
        }


        #region Cheque Reequistion Authorization

        public ActionResult RequisitionRequest(int? STATUS, string CARDNO, DateTime? CREATEDON, int? page)
        {
            Dictionary<int, string> statusID;
            OCCUSER user = (OCCUSER) Session["User"];
            if (user.TYPE == 1)
            {
                statusID =  new Dictionary<int, string>(){ {4,"applied"},{5,"authorized"},{7,"received"},{8,"deny"}}; 
            }
            else
            {
                statusID = new Dictionary<int, string>(){{4,"applied"},{5,"authorized"} };
            }
           
            ViewBag.STATUS = new SelectList(statusID, "Key", "Value");


            var List = db.CARDCHEREUISITION.Include(c => c.BRANCHINFO).Include(c => c.OCCENUMERATION).Include(c => c.OCCUSER).OrderByDescending(x=>x.ID).ToList();
            if (STATUS != null)
            {
                List = List.Where(x => x.STATUS == STATUS).ToList();
                ViewBag.STATUS = new SelectList(statusID, "Key", "Value", statusID.Where(x => x.Key == STATUS));
                ViewBag.currsts = STATUS;
                // ViewBag.STATUS = new SelectList(statusID, "Key", "Value", statusID.Where(x => x.Key == STATUS));
            }
            else
            {
                List = List.Where(x => x.STATUS == 4).ToList();
            }
            if (!String.IsNullOrEmpty(CARDNO))
            {
                CARDNO = CARDNO.Trim();
                List = List.Where(x => x.CARDNO == CARDNO).ToList();
                ViewBag.CARDNO = CARDNO;
            }
            if (CREATEDON != null)
            {
                List = List.Where(x => x.CREATEDON == CREATEDON).ToList();
                ViewBag.CREATEDON = CREATEDON;
            }

            int pageSize = ConstantConfig.PageSizes;
            int pageNumber = ((page ?? 1));
            //return Request.IsAjaxRequest()? (ActionResult) PartialView("_AppliedList", List.ToPagedList(pageNumber, pageSize)): View(List.ToPagedList(pageNumber,pageSize));
            return View(List.ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        public ActionResult PostAuthList(IEnumerable<long> idList)
        {
            List<CARDCHEREUISITION> UpdatedList = new List<CARDCHEREUISITION>();
            long count = 1;
            
            try
            {
                OCCUSER user = (OCCUSER)Session["User"];

                foreach (var id in idList)
                {
                    var cheueReq = db.CARDCHEREUISITION.FirstOrDefault(x => x.ID == id);
                    cheueReq.STATUS = 5;
                    cheueReq.AUTHORIZEDBY = user.ID;
                    cheueReq.AUTHORIZEDON = DateTime.Now.Date;
                    UpdatedList.Add(cheueReq);
                }

                foreach (var editedChqRq in UpdatedList)
                {
                    db.Entry(editedChqRq).State = EntityState.Modified;
                    db.SaveChanges();
                }
                // var authorizedList = db.CARDCHEREUISITION.Where(x => x.STATUS == 4).ToList();
                return RedirectToAction("RequisitionRequest");
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home", new { Area = "" });
            }


        }

        #endregion

        #region Cheque CardCheque Authorization
        public ActionResult CardChequePendingRequest(string CARDNO, DateTime? CREATEDON, int? page, int? STATUS)
        {
            var statusID = db.OCCENUMERATION.Where(x => x.Type == "cardcheque");
            List<CARDCHTRAN> List=new List<CARDCHTRAN>();
            ViewBag.STATUS = new SelectList(statusID, "ID", "Name");
            OCCUSER user=(OCCUSER) Session["User"];
           // db.CARDCHTRAN.Include(c => c.BRANCHINFO).Include(c => c.OCCENUMERATION).Include(c => c.OCCUSER).Where(x=>x.)
            if (STATUS != null)
            {
               // List = List.Where(x => x.STATUS == STATUS).ToList();
                if (STATUS == 13)
                {

                    List = db.CARDCHTRAN.Include(c => c.BRANCHINFO).Include(c => c.OCCENUMERATION).Include(c => c.OCCUSER).Where(x => x.STATUS == 13).ToList();       
                }
                else
                {
                    List= db.CARDCHTRAN.Include(c => c.BRANCHINFO).Include(c => c.OCCENUMERATION).Include(c => c.OCCUSER).Where(x => x.STATUS == 14).ToList();        
                }
              
                ViewBag.currentStatus = STATUS;
                ViewBag.STATUS = new SelectList(statusID, "ID", "Name", STATUS);
            }
            else 
            {
                List = db.CARDCHTRAN.Include(c => c.BRANCHINFO).Include(c => c.OCCENUMERATION).Include(c => c.OCCUSER).Where(x => x.STATUS == 13).ToList();       
            }
              

          
            if (!String.IsNullOrEmpty(CARDNO))
            {
                List = List.Where(x => x.CARDNO == CARDNO).ToList();
                ViewBag.currentCardNo = CARDNO;
            }
            if (CREATEDON != null)
            {
                List = List.Where(x => x.CREATEDON == CREATEDON).ToList();
                ViewBag.currentDate = CREATEDON;
            }

           // int pageSize = 3;
            int pageSize = ConstantConfig.PageSizes;
            int pageNumber = ((page ?? 1));
            return View(List.ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        public ActionResult CardChAuthPost(FormCollection formCollection)
        
        {
            List<int> ID=new List<int>();
            List<string> APPROVALNO=new List<string>();
            try
            {
                OCCUSER user = (OCCUSER)Session["User"];
                List<CardChequeAuthorizrVM> vm = new List<CardChequeAuthorizrVM>();

                foreach (var key in formCollection.AllKeys)
                {
                    if (key.Contains("ID"))
                    {
                        int id = (int) Convert.ToInt64(formCollection[key]);
                         ID.Add(id);
                    }
                    if (key.Contains("APPROVAL"))
                    {
                        string approval = formCollection[key];
                        APPROVALNO.Add(approval);                        
                    }
                    var value = formCollection[key];
                }
                for (int i = 0; i < ID.Count; i++)
                {

                    var aVm = new CardChequeAuthorizrVM() {APPROVALNO = APPROVALNO[i], ID = ID[i]};
                    vm.Add(aVm);
                }
                foreach (var aViewModel in vm)
                {
                    var cardChq = db.CARDCHTRAN.FirstOrDefault(x => x.ID == aViewModel.ID);
                    cardChq.APPROVALNO = aViewModel.APPROVALNO;
                    cardChq.STATUS = 14;
                    cardChq.MODIFIEDBY = user.ID;
                    cardChq.MODIFIEDON = DateTime.Now.Date;
                    db.Entry(cardChq).State = EntityState.Modified;
                    db.SaveChanges();
                }

                return RedirectToAction("CardChequePendingRequest");
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home", new { Area = "" });
            }


        }

        #endregion

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


//[HttpPost]
//public ActionResult GetAuthListPartial(int? STATUS, string CARDNO, DateTime? CREATEDON,int? page)
//{
//    var List = db.CARDCHEREUISITION.Include(c => c.BRANCHINFO).Include(c => c.OCCENUMERATION).Include(c => c.OCCUSER).ToList();
//    if (STATUS != null)
//    {
//        List = List.Where(x => x.STATUS == STATUS).ToList();
//    }
//    if (!String.IsNullOrEmpty(CARDNO))
//    {
//        CARDNO = CARDNO.Trim();
//        List = List.Where(x => x.CARDNO == CARDNO).ToList();
//    }
//    if (CREATEDON != null)
//    {
//        List = List.Where(x => x.CREATEDON == CREATEDON).ToList();
//    }
//    ViewBag.Flag = 2;
//    int pageSize = 5;
//    int pageNumber = ((page ?? 1));
//  //  return View(List.ToPagedList(pageNumber, pageSize));
//    return PartialView("_AppliedList", List.ToPagedList(pageNumber, pageSize));
//    //return PartialView("_AppliedList", List);
//}


//[HttpPost]
//public PartialViewResult SearchCardChList(int? STATUS, string CARDNO, DateTime? CREATEDON)
//{
//    var List = db.CARDCHTRAN.Include(c => c.BRANCHINFO).Include(c => c.OCCENUMERATION).Include(c => c.OCCUSER).ToList();
//    if (STATUS != null)
//    {
//        List = List.Where(x => x.STATUS == STATUS).ToList();
//    }
//    if (!String.IsNullOrEmpty(CARDNO))
//    {
//        CARDNO = CARDNO.Trim();
//        List = List.Where(x => x.CARDNO == CARDNO).ToList();
//    }
//    if (CREATEDON != null)
//    {
//        List = List.Where(x => x.REQUESTDATE == CREATEDON).ToList();
//    }

//    return PartialView("_AppliedCardChList", List);
//}


//foreach (var id in idList)
//{
//    var cardchq = db.CARDCHTRAN.FirstOrDefault(x => x.ID == id);


//    var userId = user.EMPLOYEEID;
//    var length = userId.Length;
//    char first = userId[0];
//    char last = userId[userId.Length - 1];
//    char middle = userId[(userId.Length - 1) / 2];
//    var refNo = (first + middle + last).ToString(CultureInfo.InvariantCulture);
//    refNo += length;
//    refNo += DateTime.Now.ToString("ddMMyyHHmmssfff");
//    refNo += cardchq.CARDNO.Substring(cardchq.CARDNO.Length - 4);
//    cardchq.APPROVALNO = refNo;


//    cardchq.STATUS = 14;
//    cardchq.MODIFIEDBY = user.ID;
//    cardchq.MODIFIEDON = DateTime.Now.Date;
//    UpdatedList.Add(cardchq);
//}

//foreach (var editedChqRq in UpdatedList)
//{
//    db.Entry(editedChqRq).State = EntityState.Modified;
//    db.SaveChanges();
//}