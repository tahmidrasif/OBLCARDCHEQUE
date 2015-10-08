using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CardChequeModule.Models;
using PagedList;

namespace CardChequeModule.Areas.Dispatcher.Controllers
{
    [Authorize(Roles = "dispatcher,admin")]
    public class HomeController : Controller
    {
        //
        // GET: /Dispatcher/Home/
        private OBLCARDCHEQUEEntities db = new OBLCARDCHEQUEEntities();
        //public ActionResult Index()
        //{
        //    return View();
        //}
        public ActionResult Index(int? STATUS, string CARDNO, DateTime? CREATEDON, int? page)
        {
            Dictionary<int, string> statusID;
            OCCUSER user = (OCCUSER)Session["User"];
            if (user.TYPE == 1)
            {
                statusID = new Dictionary<int, string>() { { 4, "applied" }, { 5, "authorized" }, { 7, "received" }, { 8, "deny" }, { 16, "delivered" } };
            }
            else
            {
                statusID = new Dictionary<int, string>() { { 4, "applied" }, { 5, "authorized" }, { 7, "received" }, { 16, "delivered" } };
            }

            ViewBag.STATUS = new SelectList(statusID, "Key", "Value");
            var statusId =
                        db.OCCENUMERATION.Where(x => x.Type == "chequereq")
                            .Where(x => x.Name == "received")
                            .Select(x => x.ID)
                            .FirstOrDefault();

            var List = db.CARDCHEREUISITION.Include(c => c.BRANCHINFO).Include(c => c.OCCENUMERATION).Include(c => c.OCCUSER).Where(x => x.ISDELETE != true).Where(x => x.STATUS == statusId).OrderByDescending(x => x.ID).ToList();
            if (STATUS != null)
            {
                List = db.CARDCHEREUISITION.Include(c => c.BRANCHINFO).Include(c => c.OCCENUMERATION).Include(c => c.OCCUSER).Where(x => x.ISDELETE != true).Where(x => x.STATUS == STATUS).OrderByDescending(x => x.ID).ToList();
                ViewBag.STATUS = new SelectList(statusID, "Key", "Value", statusID.Where(x => x.Key == STATUS));
                ViewBag.currsts = STATUS;
                // ViewBag.STATUS = new SelectList(statusID, "Key", "Value", statusID.Where(x => x.Key == STATUS));
            }

            if (!String.IsNullOrEmpty(CARDNO))
            {
                CARDNO = CARDNO.Trim();
                List = List.Where(x => x.CARDNO.Contains(CARDNO)).Where(x => x.ISDELETE != true).ToList();
                ViewBag.CARDNO = CARDNO;
            }
            if (CREATEDON != null)
            {
                List = List.Where(x => x.ESTABLISHMENTON.Value.Date == CREATEDON).Where(x => x.ISDELETE != true).ToList();
                ViewBag.CREATEDON = CREATEDON;
            }

            int pageSize = ConstantConfig.PageSizes;
            int pageNumber = ((page ?? 1));
            return View(List.ToPagedList(pageNumber, pageSize));
        }


        public ActionResult DelivaryPost(IEnumerable<long> idList)
        {
            OCCUSER user = (OCCUSER)Session["User"];

            try
            {
                foreach (var id in idList)
                {
                    var chequereq = db.CARDCHEREUISITION.Find(id);
                    var statusId = db.OCCENUMERATION.Where(x => x.Type == "chequereq")
                            .Where(x => x.Name == "delivered")
                            .Select(x => x.ID)
                            .FirstOrDefault();
                    chequereq.DELIVEREDBY = user.ID;
                    chequereq.DELIVEREDON = DateTime.Now;
                    chequereq.STATUS = statusId;
                    db.Entry(chequereq).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
                return RedirectToAction("Error", "Home", new { Area = "" });
            }
        }
	}
}