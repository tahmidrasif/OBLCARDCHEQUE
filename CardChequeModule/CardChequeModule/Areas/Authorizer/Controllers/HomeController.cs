using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using CardChequeModule.Models;

namespace CardChequeModule.Areas.Authorizer.Controllers
{
    public class HomeController : Controller
    {
        private OBLCARDCHEQUEEntities db = new OBLCARDCHEQUEEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RequisitionRequest()
        {
            Dictionary<int, string> statusID = new Dictionary<int, string>()
            {
                {4,"applied"},{5,"authorized"}
            };
            ViewBag.STATUS = new SelectList(statusID, "Key", "Value");
            var List = db.CARDCHEREUISITION.Include(c => c.BRANCHINFO).Include(c => c.OCCENUMERATION).Include(c => c.OCCUSER);
            List = List.Where(x => x.STATUS == 4);
            return View(List);
        }

        [HttpPost]
        public PartialViewResult GetAuthListPartial(int? STATUS, string CARDNO, DateTime? CREATEDON)
        {
            var List = db.CARDCHEREUISITION.Include(c => c.BRANCHINFO).Include(c => c.OCCENUMERATION).Include(c => c.OCCUSER).ToList();
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
                List = List.Where(x => x.CREATEDON == CREATEDON).ToList();
            }

            return PartialView("_AppliedList", List);
        }


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