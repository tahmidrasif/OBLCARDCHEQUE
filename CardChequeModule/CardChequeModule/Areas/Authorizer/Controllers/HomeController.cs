using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using CardChequeModule.Models;
using Microsoft.Office.Interop.Excel;
using PagedList;

namespace CardChequeModule.Areas.Authorizer.Controllers
{
    public class HomeController : Controller
    {
        private OBLCARDCHEQUEEntities db = new OBLCARDCHEQUEEntities();
        public ActionResult Index()
        {
            return View();
        }

        #region Cheque Reequistion Authorization

        public ActionResult RequisitionRequest(int? STATUS, string CARDNO, DateTime? CREATEDON, int? page)
        {
            Dictionary<int, string> statusID = new Dictionary<int, string>()
            {
                {4,"applied"},{5,"authorized"}
            };
            ViewBag.STATUS = new SelectList(statusID, "Key", "Value");


            var List = db.CARDCHEREUISITION.Include(c => c.BRANCHINFO).Include(c => c.OCCENUMERATION).Include(c => c.OCCUSER).ToList();
            if (STATUS != null)
            {
                List = List.Where(x => x.STATUS == STATUS).ToList();
                ViewBag.STATUS = new SelectList(statusID, "Key", "Value", statusID.Where(x => x.Key == STATUS));
                ViewBag.currsts = STATUS;
               // ViewBag.STATUS = new SelectList(statusID, "Key", "Value", statusID.Where(x => x.Key == STATUS));
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

            int pageSize = 3;
            int pageNumber = ((page ?? 1));
            //return Request.IsAjaxRequest()? (ActionResult) PartialView("_AppliedList", List.ToPagedList(pageNumber, pageSize)): View(List.ToPagedList(pageNumber,pageSize));
            return View(List.ToPagedList(pageNumber, pageSize));
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


        [HttpPost]
        public ActionResult PostAuthList(IEnumerable<long> idList)
        {
            List<CARDCHEREUISITION> UpdatedList = new List<CARDCHEREUISITION>();
            long count = 1;
            OCCUSER user = (OCCUSER)Session["User"];
            try
            {

                foreach (var id in idList)
                {
                    var cheueReq = db.CARDCHEREUISITION.FirstOrDefault(x => x.ID == id);
                    cheueReq.STATUS = 5;
                    cheueReq.MODIFIEDBY = user.ID;
                    cheueReq.MODIFIEDON = DateTime.Now.Date;
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
        public ActionResult CardChequePendingRequest()
        {
            var statusID=db.OCCENUMERATION.Where(x => x.Type == "cardcheque");
            ViewBag.STATUS = new SelectList(statusID, "ID", "Name");
            var List = db.CARDCHTRAN.Include(c => c.BRANCHINFO).Include(c => c.OCCENUMERATION).Include(c => c.OCCUSER);
            List = List.Where(x => x.STATUS == 13);
            return View(List);
        }

        [HttpPost]
        public PartialViewResult SearchCardChList(int? STATUS, string CARDNO, DateTime? CREATEDON)
        {
            var List = db.CARDCHTRAN.Include(c => c.BRANCHINFO).Include(c => c.OCCENUMERATION).Include(c => c.OCCUSER).ToList();
            if (STATUS != null)
            {
                List = List.Where(x => x.STATUS == STATUS).ToList();
            }
            if (!String.IsNullOrEmpty(CARDNO))
            {
                CARDNO = CARDNO.Trim();
                List = List.Where(x => x.CARDNO == CARDNO).ToList();
            }
            if (CREATEDON != null)
            {
                List = List.Where(x => x.REQUESTDATE == CREATEDON).ToList();
            }

            return PartialView("_AppliedCardChList", List);
        }


        [HttpPost]
        public ActionResult CardChAuthPost(IEnumerable<long> idList)
        {
            List<CARDCHTRAN> UpdatedList = new List<CARDCHTRAN>();
            long count = 1;
            OCCUSER user = (OCCUSER)Session["User"];

            try
            {

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