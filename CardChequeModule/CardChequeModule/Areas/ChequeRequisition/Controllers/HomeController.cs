using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CardChequeModule.Models;
using PagedList;

namespace CardChequeModule.Areas.ChequeRequisition.Controllers
{
    public class HomeController : Controller
    {
        private OBLCARDCHEQUEEntities db = new OBLCARDCHEQUEEntities();
        private OCCUSER user;

        // GET: /ChequeRequisition/Home/
        public ActionResult Index(int? STATUS, string CARDNO, DateTime? CREATEDON, int? page)
        {
            try
            {

                user = (OCCUSER)Session["User"];
                Dictionary<int, string> statusID = new Dictionary<int, string>()
                {
                {4,"applied"},{5,"authorized"}
                };
                ViewBag.STATUS = new SelectList(statusID, "Key", "Value");

                var List = db.CARDCHEREUISITION.Include(c => c.BRANCHINFO).Where(x => x.CREATEDBY == user.ID).OrderByDescending(x => x.CREATEDON).ToList();

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
                return View(List.ToPagedList(pageNumber, pageSize));
     
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { Area = "" });
            }
        }


        public ActionResult GetCardDetails(string cardNo)
        {
            try
            {
                var cardDetails = db.CARDCHEREUISITION.Last(x => x.CARDNO == cardNo);
                if (cardDetails == null)
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
                return Json(cardDetails, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {

                return Json(exception, JsonRequestBehavior.DenyGet);
            }

        }

        public ActionResult Form(long? id)
        {
            try
            {
                if (id == null)
                {
                    user = (OCCUSER)Session["User"];
                    ViewBag.BRANCH = new SelectList(db.BRANCHINFO, "ID", "BRANCHNAME", user.BRANCH);
                    CARDCHEREUISITION ccreq = new CARDCHEREUISITION();
                    if (user.BRANCH != null)
                    {
                        ccreq.BRANCHCODE = (long)user.BRANCH;
                    }
                    long status = db.OCCENUMERATION.Where(x => x.Name == "applied").Select(x => x.ID).FirstOrDefault();
                    ViewBag.STATUSID = new SelectList(db.OCCENUMERATION.Where(x => x.Type == "chequereq"), "ID", "Name", status);
                    ccreq.STATUS = status;
                    ccreq.CREATEDON = DateTime.Now;
                    return View(ccreq);
                }
                CARDCHEREUISITION cardchereuisition = db.CARDCHEREUISITION.Find(id);
                if (cardchereuisition == null)
                {
                    return HttpNotFound();
                }
                ViewBag.BRANCH = new SelectList(db.BRANCHINFO, "ID", "BRANCHNAME", cardchereuisition.BRANCHCODE);

                ViewBag.STATUSID = new SelectList(db.OCCENUMERATION.Where(x => x.Type == "chequereq"), "ID", "Name", cardchereuisition.STATUS);

                return View(cardchereuisition);
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home", new { Area = "" });
            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Form(CARDCHEREUISITION cardchereuisition)
        {
            try
            {
                user = (OCCUSER)Session["User"];

                cardchereuisition.LEAFNO = 10;
                cardchereuisition.ISACTIVE = false;
                cardchereuisition.CREATEDBY = user.ID;

                if (cardchereuisition.ID == 0)
                {
                    if (ModelState.IsValid)
                    {
                        db.CARDCHEREUISITION.Add(cardchereuisition);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    ViewBag.BRANCH = new SelectList(db.BRANCHINFO, "ID", "BRANCHNAME", user.BRANCH);
                    CARDCHEREUISITION ccreq = new CARDCHEREUISITION();
                    if (user.BRANCH != null)
                    {
                        ccreq.BRANCHCODE = (long)user.BRANCH;
                    }

                    long status = db.OCCENUMERATION.Where(x => x.Name == "applied").Select(x => x.ID).FirstOrDefault();
                    ViewBag.STATUSID = new SelectList(db.OCCENUMERATION.Where(x => x.Type == "chequereq"), "ID", "Name", status);
                    cardchereuisition.STATUS = status;
                    return View(cardchereuisition);
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        db.Entry(cardchereuisition).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    ViewBag.BRANCH = new SelectList(db.BRANCHINFO, "ID", "BRANCHNAME", cardchereuisition.BRANCHCODE);

                    ViewBag.STATUSID = new SelectList(db.OCCENUMERATION.Where(x => x.Type == "chequereq"), "ID", "Name", cardchereuisition.STATUS);

                    return View(cardchereuisition);
                }

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













