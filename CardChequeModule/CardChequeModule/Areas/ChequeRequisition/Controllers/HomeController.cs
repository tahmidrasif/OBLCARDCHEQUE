using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CardChequeModule.Models;

namespace CardChequeModule.Areas.ChequeRequisition.Controllers
{
    public class HomeController : Controller
    {
        private OBLCARDCHEQUEEntities db = new OBLCARDCHEQUEEntities();
        private OCCUSER user;
        // GET: /ChequeRequisition/Home/
        public ActionResult Index()
        {
            var cardchereuisition = db.CARDCHEREUISITION.Include(c => c.BRANCHINFO);
            return View(cardchereuisition.ToList());
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

                if (cardchereuisition.ID==0)
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


















        // GET: /ChequeRequisition/Home/Create
        public ActionResult Create()
        {
            user = (OCCUSER)Session["User"];
            ViewBag.BRANCH = new SelectList(db.BRANCHINFO, "ID", "BRANCHNAME",user.BRANCH);
            CARDCHEREUISITION ccreq = new CARDCHEREUISITION();
            if (user.BRANCH != null)
            {
            ccreq.BRANCHCODE = (long) user.BRANCH;
            }
            long status = db.OCCENUMERATION.Where(x => x.Name == "applied").Select(x => x.ID).FirstOrDefault();
            ViewBag.STATUSID = new SelectList(db.OCCENUMERATION.Where(x => x.Type == "chequereq"), "ID", "Name",status);
            ccreq.STATUS = status;
            return View(ccreq);
        }



        // POST: /ChequeRequisition/Home/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CARDCHEREUISITION cardchereuisition)
        {
            try
            {
                user = (OCCUSER)Session["User"];

                cardchereuisition.LEAFNO = 10;
                cardchereuisition.ISACTIVE = false;
                cardchereuisition.CREATEDBY = user.ID;

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
            catch (Exception)
            {
                return RedirectToAction("Error", "Home", new { Area = "" });
            }
            
        }


        public ActionResult GetCardDetails(string cardNo)
        {
            try
            {
                var cardDetails = db.CARDCHEREUISITION.Where(x => x.CARDNO == cardNo).Last();
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
        // GET: /ChequeRequisition/Home/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CARDCHEREUISITION cardchereuisition = db.CARDCHEREUISITION.Find(id);
            if (cardchereuisition == null)
            {
                return HttpNotFound();
            }
            ViewBag.BRANCHCODE = new SelectList(db.BRANCHINFO, "ID", "BRANCHCODE", cardchereuisition.BRANCHCODE);
            return View(cardchereuisition);
        }

        // POST: /ChequeRequisition/Home/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,CARDNO,BRANCHCODE,REQUESTDATE,STATUS,LEAFNO,LEAFFROM,LEAFTO,LEAFNEXT,REMARKS,ISACTIVE,CREATEDBY,CREATEDON,MODIFIEDBY,MODIFIEDON")] CARDCHEREUISITION cardchereuisition)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cardchereuisition).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BRANCHCODE = new SelectList(db.BRANCHINFO, "ID", "BRANCHCODE", cardchereuisition.BRANCHCODE);
            return View(cardchereuisition);
        }

        // GET: /ChequeRequisition/Home/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CARDCHEREUISITION cardchereuisition = db.CARDCHEREUISITION.Find(id);
            if (cardchereuisition == null)
            {
                return HttpNotFound();
            }
            return View(cardchereuisition);
        }

        // POST: /ChequeRequisition/Home/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            CARDCHEREUISITION cardchereuisition = db.CARDCHEREUISITION.Find(id);
            db.CARDCHEREUISITION.Remove(cardchereuisition);
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
