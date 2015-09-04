using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CardChequeModule.Models;

namespace CardChequeModule.Areas.Payment.Controllers
{
    [Authorize(Roles = "teller,admin")]
    public class EditController : Controller
    {
        private OBLCARDCHEQUEEntities db = new OBLCARDCHEQUEEntities();

      

        // GET: /Payment/Edit/Edit/5
        public ActionResult Edit(long? id)
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

            ViewBag.BRANCH = new SelectList(db.BRANCHINFO.ToList(), "ID", "BRANCHNAME",deposit.BRANCH);
            ViewBag.CURRENCY = new SelectList(currencyList,null,null,deposit.CURRENCY);
            return View(deposit);
        }

        // POST: /Payment/Edit/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
  
        public ActionResult Edit(DEPOSIT deposit)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    OCCUSER user = (OCCUSER)Session["User"];
                    deposit.CREATEDBY = user.ID;
                    deposit.ISACTIVE = true;
                    deposit.ISDELETE = false;
                   
                    db.Entry(deposit).State = EntityState.Modified;
                    //db.DEPOSIT.AddOrUpdate(x=>x.ID==deposit.ID);
                    db.SaveChanges();
                    var msg = "Successfully Updated";
                    return Json(msg, JsonRequestBehavior.AllowGet);

                }
                List<string> currencyList = new List<string>() { "USD", "BDT" };

                ViewBag.BRANCH = new SelectList(db.BRANCHINFO.ToList(), "ID", "BRANCHNAME", db.DEPOSIT.Select(x => x.BRANCH));
                ViewBag.CURRENCY = new SelectList(currencyList, null, null, db.DEPOSIT.Select(x => x.CURRENCY));
                return View(deposit);
            }
            catch (Exception)
            {

                return RedirectToAction("Error", "Home", new { Area = "" });
            }
           
        }
        //[Bind(Include = "ID,CARDNUMBER,AMOUNT,BRANCH,CREATEDBY,CREATEDON,ISAUTHORIZED,REFERENCENUMBER,PAYMENTTYPE,CURRENCY,ADI,PDCBANK,PDCBRANCH,PDCCHEQUENO,PDCDATE")] 

        // GET: /Payment/Edit/Delete/5
      
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
