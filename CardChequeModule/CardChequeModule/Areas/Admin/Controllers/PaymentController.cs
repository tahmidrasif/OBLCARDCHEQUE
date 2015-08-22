using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CardChequeModule.Models;
using PagedList;

namespace CardChequeModule.Areas.Admin.Controllers
{
    public class PaymentController : Controller
    {
        private OBLCARDCHEQUEEntities db = new OBLCARDCHEQUEEntities();

        // GET: Admin/Payment
        public ActionResult Index(int? BRANCH, string CARDNO, DateTime? CREATEDON, int? page, int? serial)
        {
           // var dEPOSIT = db.DEPOSIT.Include(d => d.BRANCHINFO).Include(d => d.OCCUSER);
         //   return View(dEPOSIT.ToList());
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            OCCUSER user = (OCCUSER)Session["User"];
            try
            {
                var list = db.DEPOSIT.OrderByDescending(x => x.ID).ToList();
                var branch = db.BRANCHINFO.Select(x => new { x.BRANCHNAME, x.ID }).OrderBy(x => x.BRANCHNAME);
                ViewBag.BRANCH = new SelectList(branch, "ID", "BRANCHNAME");

                if (serial != null)
                {
                    @ViewBag.Sln = (pageNumber * pageSize) - 9;
                }
                else
                {
                    @ViewBag.Sln = 1;
                }

                if (BRANCH != null)
                {
                    list = list.Where(x => x.BRANCH == BRANCH).ToList();
                    ViewBag.BRANCH = new SelectList(branch, "ID", "BRANCHNAME", BRANCH);
                    ViewBag.currentBranch = BRANCH;
                }
                if (!String.IsNullOrEmpty(CARDNO))
                {
                    CARDNO = CARDNO.Trim();
                    list = list.Where(x => x.CARDNUMBER == CARDNO).ToList();
                    ViewBag.CARDNO = CARDNO;
                }
                if (CREATEDON != null)
                {
                    list = list.Where(x => x.CREATEDON == CREATEDON).ToList();
                    ViewBag.CREATEDON = CREATEDON;
                }

                return View(list.ToPagedList(pageNumber, pageSize));
                // return View(list);
            }
            catch (Exception exception)
            {
                return RedirectToAction("Error", "Home", new { Area = "" });
            }     
            
        }

        // GET: Admin/Payment/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DEPOSIT dEPOSIT = db.DEPOSIT.Find(id);
            if (dEPOSIT == null)
            {
                return HttpNotFound();
            }
            return View(dEPOSIT);
        }

        // GET: Admin/Payment/Create
        public ActionResult Create()
        {
            ViewBag.BRANCH = new SelectList(db.BRANCHINFO, "ID", "BRANCHCODE");
            ViewBag.CREATEDBY = new SelectList(db.OCCUSER, "ID", "EMPLOYEEID");
            return View();
        }

        // POST: Admin/Payment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,CARDNUMBER,BRANCH,CREATEDBY,CREATEDON,ISAUTHORIZED,REFERENCENUMBER,PAYMENTTYPE,AMOUNT,CURRENCY,ADI,PDCBANK,PDCBRANCH,PDCCHEQUENO,PDCDATE,CARDHOLDERNAME,MOBILE")] DEPOSIT dEPOSIT)
        {
            if (ModelState.IsValid)
            {
                db.DEPOSIT.Add(dEPOSIT);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BRANCH = new SelectList(db.BRANCHINFO, "ID", "BRANCHCODE", dEPOSIT.BRANCH);
            ViewBag.CREATEDBY = new SelectList(db.OCCUSER, "ID", "EMPLOYEEID", dEPOSIT.CREATEDBY);
            return View(dEPOSIT);
        }

        // GET: Admin/Payment/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DEPOSIT dEPOSIT = db.DEPOSIT.Find(id);
            if (dEPOSIT == null)
            {
                return HttpNotFound();
            }
            ViewBag.BRANCH = new SelectList(db.BRANCHINFO, "ID", "BRANCHCODE", dEPOSIT.BRANCH);
            ViewBag.CREATEDBY = new SelectList(db.OCCUSER, "ID", "EMPLOYEEID", dEPOSIT.CREATEDBY);
            return View(dEPOSIT);
        }

        // POST: Admin/Payment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,CARDNUMBER,BRANCH,CREATEDBY,CREATEDON,ISAUTHORIZED,REFERENCENUMBER,PAYMENTTYPE,AMOUNT,CURRENCY,ADI,PDCBANK,PDCBRANCH,PDCCHEQUENO,PDCDATE,CARDHOLDERNAME,MOBILE")] DEPOSIT dEPOSIT)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dEPOSIT).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BRANCH = new SelectList(db.BRANCHINFO, "ID", "BRANCHCODE", dEPOSIT.BRANCH);
            ViewBag.CREATEDBY = new SelectList(db.OCCUSER, "ID", "EMPLOYEEID", dEPOSIT.CREATEDBY);
            return View(dEPOSIT);
        }

        // GET: Admin/Payment/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DEPOSIT dEPOSIT = db.DEPOSIT.Find(id);
            if (dEPOSIT == null)
            {
                return HttpNotFound();
            }
            return View(dEPOSIT);
        }

        // POST: Admin/Payment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            DEPOSIT dEPOSIT = db.DEPOSIT.Find(id);
            db.DEPOSIT.Remove(dEPOSIT);
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
