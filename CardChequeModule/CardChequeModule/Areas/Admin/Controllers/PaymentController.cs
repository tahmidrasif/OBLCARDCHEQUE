using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Xml;
using CardChequeModule.Models;
using Microsoft.SqlServer.Server;
using PagedList;

namespace CardChequeModule.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
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
                var list = db.DEPOSIT.Where(x => x.ISDELETE == false).OrderByDescending(x => x.ID).ToList();
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
                    list = list.Where(x => x.CARDNUMBER.Contains(CARDNO)).ToList();
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
            try
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
                List<string> currencyList = new List<string>() { "USD", "BDT" };

                ViewBag.BRANCH = new SelectList(db.BRANCHINFO.ToList(), "ID", "BRANCHNAME", deposit.BRANCH);
                ViewBag.CURRENCY = new SelectList(currencyList, null, null, deposit.CURRENCY);

                return View(deposit);
            }
            catch (Exception exception)
            {
                return RedirectToAction("Error", "Home", new { Area = "" });
            }

        }

        [HttpPost]
        public ActionResult Details(DEPOSIT deposit, string btnName)
        {
            try
            {
                OCCUSER user = (OCCUSER)Session["User"];
                if (String.Equals(btnName, "update"))
                {
                    DEPOSIT deposits = db.DEPOSIT.Find(deposit.ID);

                    deposit.CREATEDBY = deposits.CREATEDBY;
                    deposit.CREATEDON = deposits.CREATEDON;
                    deposit.MODIFIEDBY = user.ID;
                    deposit.MODIFIEDON = DateTime.Now;
                    deposit.ISACTIVE = true;
                    deposit.ISDELETE = false;
                    //db.Entry(deposit).State = EntityState.Modified;
                    db.DEPOSIT.AddOrUpdate(deposit);
                    db.SaveChanges();
                    var msg = "Successfully Updated";
                    return Json(msg, JsonRequestBehavior.AllowGet);
                }
                if (String.Equals(btnName, "delete"))
                {

                    DEPOSIT depostis = db.DEPOSIT.Find(deposit.ID);
                    depostis.ISDELETE = true;
                    db.Entry(depostis).State = EntityState.Modified;
                    db.SaveChanges();
                    var msg = "Successfully Removed";
                    return Json(msg, JsonRequestBehavior.AllowGet);
                }
                return RedirectToAction("Error", "Home", new { Area = "" });
            }
            catch (Exception exception)
            {
                var msg = exception.Message;
                return Json(msg, JsonRequestBehavior.AllowGet);
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
