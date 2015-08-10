using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CardChequeModule.Models;

namespace CardChequeModule.Areas.CardCheque.Controllers
{
    public class HomeController : Controller
    {
        private OBLCARDCHEQUEEntities db = new OBLCARDCHEQUEEntities();
        TestImageEntities entities=new TestImageEntities();

        // GET: /CardCheque/Home/
        public ActionResult Index()
        {
            var cardchtran = db.CARDCHTRAN.Include(c => c.CARDCHLEAF).Include(c => c.OCCUSER).Include(c => c.OCCUSER1);
            return View(cardchtran.ToList());
        }

        public ActionResult GetInfo(string leafno)
        {

            var chequeId=db.CARDCHLEAF.Where(x => x.LEAFNO == leafno).Select(x => x.CHEQUEID).FirstOrDefault();
            var leafID = db.CARDCHLEAF.Where(x => x.LEAFNO == leafno).Select(x => x.ID).FirstOrDefault();
            var cardno=db.CARDCHEREUISITION.Where(x => x.ID == chequeId).Select(x => x.CARDNO).FirstOrDefault();
            var imageEntities = entities.ImageTable.FirstOrDefault(x => x.CardNumber == cardno);
            if (imageEntities != null)
            {

                string userPhoto = Convert.ToBase64String(imageEntities.Photo);
                string signature = Convert.ToBase64String(imageEntities.Signature);

                var model = new {leafID, cardno, userPhoto,signature, name = "XXX" };
                return Json(model);
            }
            return Json(null);
        }

     
        public ActionResult Create()
        {

            List<string> currencyList = new List<string>() { "USD", "BDT" };
            ViewBag.BRANCHCODE = new SelectList(db.BRANCHINFO.ToList(), "ID", "BRANCHNAME");
            ViewBag.CURRENCY = new SelectList(currencyList);
            return View();
        }

       
        [HttpPost]
        public ActionResult Create(CARDCHTRAN cardchtran)
        {
            List<string> currencyList;
           
            try
            { 
                OCCUSER user = (OCCUSER) Session["User"];
                cardchtran.CREATEDBY = user.ID;
                cardchtran.CREATEDON = DateTime.Now.Date;
                cardchtran.STATUS = 13;


                if (ModelState.IsValid)
                {
                    db.CARDCHTRAN.Add(cardchtran);
                    db.SaveChanges();
                    var msg = "Data saved successfully. ";

                    return Json(msg, JsonRequestBehavior.AllowGet);
                   
                }

                currencyList = new List<string>() { "USD", "BDT" };
                ViewBag.BRANCHCODE = new SelectList(db.BRANCHINFO.ToList(), "ID", "BRANCHNAME");
                ViewBag.CURRENCY = new SelectList(currencyList);
                return View("Index");
               
            }

            catch (DbEntityValidationException dbEntityValidationException)
            {

                return RedirectToAction("Error", "Home", new { Area = "" });
            }
            catch (InvalidOperationException invalidOperationException)
            {
                return RedirectToAction("Error", "Home", new { Area = "" });
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
