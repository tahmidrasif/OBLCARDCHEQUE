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
using CardChequeModule.OraDBCardInfo;

namespace CardChequeModule.Areas.CardCheque.Controllers
{
    [Authorize(Roles = "teller")]
    public class HomeController : Controller
    {
        private OBLCARDCHEQUEEntities db = new OBLCARDCHEQUEEntities();
        

        // GET: /CardCheque/Home/
        public ActionResult Index()
        {
            try
            {
                OCCUSER user = (OCCUSER)Session["User"];
                var cardchtran = db.CARDCHTRAN.Include(c => c.CARDCHLEAF).Include(c => c.OCCUSER).Include(c => c.OCCUSER1).Where(x => x.CREATEDBY == user.ID).OrderByDescending(x => x.ID);
                var status = db.OCCENUMERATION.Where(x => x.Type == "cardcheque");
                ViewBag.STATUS = new SelectList(status, "ID", "Name");
                return View(cardchtran.ToList());
            }
            catch (Exception)
            {

                return RedirectToAction("Error", "Home", new { Area = "" });
            }
           
        }

        public ActionResult GetInfo(string leafno)
        {
            try
            {
                var leafstatus = db.CARDCHLEAF.Where(x => x.LEAFNO == leafno).Select(x => x.LEAFSTATUS).FirstOrDefault();
                if (leafstatus == 10)
                {
                    var chequeId = db.CARDCHLEAF.Where(x => x.LEAFNO == leafno).Select(x => x.CHEQUEID).FirstOrDefault();

                    var leafID = db.CARDCHLEAF.Where(x => x.LEAFNO == leafno).Select(x => x.ID).FirstOrDefault();
                    var cardno = db.CARDCHEREUISITION.Where(x => x.ID == chequeId).Select(x => x.CARDNO).FirstOrDefault();

                    OradbaccessSoap service = new OradbaccessSoapClient();
                    DataTable dt = service.GetCCardDetail(cardno);

                    if (dt != null)
                    {
                        string userPhoto="";
                        string signature="";
                        string clientname = "";
                        foreach (DataRow dataRow in dt.Rows)
                        {
                            userPhoto = Convert.ToBase64String((byte[]) dataRow[4]);
                            signature = Convert.ToBase64String((byte[]) dataRow[5]);
                            clientname = (string)dataRow[2];
                        }


                        var model = new { leafID, cardno, userPhoto, signature, name = clientname };
                        return Json(model);
                    }
                    return Json(null);
                }

                return Json("used");
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home", new { Area = "" });
            }
           
        }

     
        public ActionResult Create()
        {
            try
            {
                List<string> currencyList = new List<string>() { "USD", "BDT" };
                ViewBag.BRANCHCODE = new SelectList(db.BRANCHINFO.ToList(), "ID", "BRANCHNAME");
                ViewBag.CURRENCY = new SelectList(currencyList);
                return View();
            }
            catch (Exception)
            {

                return RedirectToAction("Error", "Home", new { Area = "" });
            }
            
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
                cardchtran.ISACTIVE = true;

                var leaf=db.CARDCHLEAF.FirstOrDefault(x => x.ID == cardchtran.CHEQUELEAFID);
                leaf.LEAFSTATUS = 11;
                db.Entry(leaf).State = EntityState.Modified;

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

        public ActionResult SearchResult(string CARDNO, int? STATUS, DateTime? CREATEDON)
        {

            OCCUSER user = (OCCUSER) Session["User"];
            var searchResult =
                db.CARDCHTRAN.Include(c => c.BRANCHINFO)
                    .Include(c => c.CARDCHLEAF)
                    .Include(c => c.OCCENUMERATION).Where(x => x.CREATEDBY == user.ID).OrderByDescending(x => x.ID)
                    .ToList();
            if (!String.IsNullOrEmpty(CARDNO))
            {
                searchResult = searchResult.Where(x => x.CARDNO == CARDNO).ToList();
            }
            if (STATUS != null)
            {
                searchResult = searchResult.Where(x => x.STATUS == STATUS).ToList();
            }
            if (CREATEDON != null)
            {
                searchResult = searchResult.Where(x => x.REQUESTDATE == CREATEDON).ToList();
            }
            return PartialView("_CardChAppliedList",searchResult);
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
