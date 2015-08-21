using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CardChequeModule.Models;

namespace CardChequeModule.Areas.CallCenter.Controllers
{
    [Authorize(Roles = "call center")]
    public class HomeController : Controller
    {
        private OBLCARDCHEQUEEntities db = new OBLCARDCHEQUEEntities();


        public ActionResult Index()
        {
            var cardchereuisition = db.CARDCHEREUISITION.Include(c => c.BRANCHINFO).Include(c => c.OCCENUMERATION).Include(c => c.OCCUSER).Include(c => c.OCCUSER1).Where(x=>x.STATUS==7 && x.ISACTIVE==false);
            return View(cardchereuisition.ToList());
        }



        [HttpPost]
        public ActionResult Post(IEnumerable<long> idList)
        {

            OCCUSER user = (OCCUSER)Session["User"];

            try
            {
                foreach (var id in idList)
                {
                    var cheqreq = db.CARDCHEREUISITION.FirstOrDefault(x => x.ID == id);
                    cheqreq.MODIFIEDBY = user.ID;
                    cheqreq.MODIFIEDON = DateTime.Now;
                    cheqreq.ISACTIVE = true;
                    db.Entry(cheqreq).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception exception)
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
