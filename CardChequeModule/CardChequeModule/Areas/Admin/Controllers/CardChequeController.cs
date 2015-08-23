using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CardChequeModule.Models;

namespace CardChequeModule.Areas.Admin.Controllers
{
    public class CardChequeController : Controller
    {
        private OBLCARDCHEQUEEntities db = new OBLCARDCHEQUEEntities();

        // GET: Admin/CardCheque
        public ActionResult Index()
        {
            var cARDCHTRAN = db.CARDCHTRAN.Include(c => c.BRANCHINFO).Include(c => c.BRANCHINFO1).Include(c => c.CARDCHLEAF).Include(c => c.OCCENUMERATION).Include(c => c.OCCUSER).Include(c => c.OCCUSER1);
            return View(cARDCHTRAN.ToList());
        }

        // GET: Admin/CardCheque/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CARDCHTRAN cARDCHTRAN = db.CARDCHTRAN.Find(id);
            if (cARDCHTRAN == null)
            {
                return HttpNotFound();
            }
            return View(cARDCHTRAN);
        }

        // GET: Admin/CardCheque/Create
        public ActionResult Create()
        {
            ViewBag.BRANCHCODE = new SelectList(db.BRANCHINFO, "ID", "BRANCHCODE");
            ViewBag.BRANCHCODE = new SelectList(db.BRANCHINFO, "ID", "BRANCHCODE");
            ViewBag.CHEQUELEAFID = new SelectList(db.CARDCHLEAF, "ID", "LEAFNO");
            ViewBag.STATUS = new SelectList(db.OCCENUMERATION, "ID", "Type");
            ViewBag.CREATEDBY = new SelectList(db.OCCUSER, "ID", "EMPLOYEEID");
            ViewBag.MODIFIEDBY = new SelectList(db.OCCUSER, "ID", "EMPLOYEEID");
            return View();
        }

        // POST: Admin/CardCheque/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,CHEQUELEAFID,CARDNO,CARDHOLDERNAME,BRANCHCODE,REQUESTDATE,BENEFICIARINFO,AMOUNT,ISSIGNATUREVERIFIED,STATUS,APPROVALNO,ISACTIVE,CREATEDBY,CREATEDON,MODIFIEDBY,MODIFIEDON")] CARDCHTRAN cARDCHTRAN)
        {
            if (ModelState.IsValid)
            {
                db.CARDCHTRAN.Add(cARDCHTRAN);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BRANCHCODE = new SelectList(db.BRANCHINFO, "ID", "BRANCHCODE", cARDCHTRAN.BRANCHCODE);
            ViewBag.BRANCHCODE = new SelectList(db.BRANCHINFO, "ID", "BRANCHCODE", cARDCHTRAN.BRANCHCODE);
            ViewBag.CHEQUELEAFID = new SelectList(db.CARDCHLEAF, "ID", "LEAFNO", cARDCHTRAN.CHEQUELEAFID);
            ViewBag.STATUS = new SelectList(db.OCCENUMERATION, "ID", "Type", cARDCHTRAN.STATUS);
            ViewBag.CREATEDBY = new SelectList(db.OCCUSER, "ID", "EMPLOYEEID", cARDCHTRAN.CREATEDBY);
            ViewBag.MODIFIEDBY = new SelectList(db.OCCUSER, "ID", "EMPLOYEEID", cARDCHTRAN.MODIFIEDBY);
            return View(cARDCHTRAN);
        }

        // GET: Admin/CardCheque/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CARDCHTRAN cARDCHTRAN = db.CARDCHTRAN.Find(id);
            if (cARDCHTRAN == null)
            {
                return HttpNotFound();
            }
            ViewBag.BRANCHCODE = new SelectList(db.BRANCHINFO, "ID", "BRANCHCODE", cARDCHTRAN.BRANCHCODE);
            ViewBag.BRANCHCODE = new SelectList(db.BRANCHINFO, "ID", "BRANCHCODE", cARDCHTRAN.BRANCHCODE);
            ViewBag.CHEQUELEAFID = new SelectList(db.CARDCHLEAF, "ID", "LEAFNO", cARDCHTRAN.CHEQUELEAFID);
            ViewBag.STATUS = new SelectList(db.OCCENUMERATION, "ID", "Type", cARDCHTRAN.STATUS);
            ViewBag.CREATEDBY = new SelectList(db.OCCUSER, "ID", "EMPLOYEEID", cARDCHTRAN.CREATEDBY);
            ViewBag.MODIFIEDBY = new SelectList(db.OCCUSER, "ID", "EMPLOYEEID", cARDCHTRAN.MODIFIEDBY);
            return View(cARDCHTRAN);
        }

        // POST: Admin/CardCheque/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,CHEQUELEAFID,CARDNO,CARDHOLDERNAME,BRANCHCODE,REQUESTDATE,BENEFICIARINFO,AMOUNT,ISSIGNATUREVERIFIED,STATUS,APPROVALNO,ISACTIVE,CREATEDBY,CREATEDON,MODIFIEDBY,MODIFIEDON")] CARDCHTRAN cARDCHTRAN)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cARDCHTRAN).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BRANCHCODE = new SelectList(db.BRANCHINFO, "ID", "BRANCHCODE", cARDCHTRAN.BRANCHCODE);
            ViewBag.BRANCHCODE = new SelectList(db.BRANCHINFO, "ID", "BRANCHCODE", cARDCHTRAN.BRANCHCODE);
            ViewBag.CHEQUELEAFID = new SelectList(db.CARDCHLEAF, "ID", "LEAFNO", cARDCHTRAN.CHEQUELEAFID);
            ViewBag.STATUS = new SelectList(db.OCCENUMERATION, "ID", "Type", cARDCHTRAN.STATUS);
            ViewBag.CREATEDBY = new SelectList(db.OCCUSER, "ID", "EMPLOYEEID", cARDCHTRAN.CREATEDBY);
            ViewBag.MODIFIEDBY = new SelectList(db.OCCUSER, "ID", "EMPLOYEEID", cARDCHTRAN.MODIFIEDBY);
            return View(cARDCHTRAN);
        }

        // GET: Admin/CardCheque/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CARDCHTRAN cARDCHTRAN = db.CARDCHTRAN.Find(id);
            if (cARDCHTRAN == null)
            {
                return HttpNotFound();
            }
            return View(cARDCHTRAN);
        }

        // POST: Admin/CardCheque/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            CARDCHTRAN cARDCHTRAN = db.CARDCHTRAN.Find(id);
            db.CARDCHTRAN.Remove(cARDCHTRAN);
            db.SaveChanges();
            return RedirectToAction("Index");
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
