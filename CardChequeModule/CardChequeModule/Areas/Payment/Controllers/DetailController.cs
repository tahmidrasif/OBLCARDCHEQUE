using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CardChequeModule.Models;

namespace CardChequeModule.Areas.Payment.Controllers
{
    public class DetailController : Controller
    {
        private OBLCARDCHEQUEEntities db = new OBLCARDCHEQUEEntities();

        // GET: /Payment/Detail/
        
        // GET: /Payment/Detail/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DEPOSIT deposit = db.DEPOSIT.Find(id);
            if (deposit == null)
            {
                return HttpNotFound();
            }
            return View(deposit);

            return Json(new
            {
                ID=deposit.ID,
                BRANCHNAME=deposit.BRANCHINFO.BRANCHNAME,
                AMOUNT=deposit.AMOUNT

            },JsonRequestBehavior.AllowGet);
        }

        // GET: /Payment/Detail/Create
     
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
