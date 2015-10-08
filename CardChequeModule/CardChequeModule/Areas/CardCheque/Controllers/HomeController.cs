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
using PagedList;

namespace CardChequeModule.Areas.CardCheque.Controllers
{
    [Authorize(Roles = "teller,admin")]
    public class HomeController : Controller
    {
        private OBLCARDCHEQUEEntities db = new OBLCARDCHEQUEEntities();
        

        // GET: /CardCheque/Home/
        public ActionResult Index(int? STATUS, string CARDNO, DateTime? CREATEDON, int? page)
        {
            try
            {
                OCCUSER user = (OCCUSER)Session["User"];
                var List = db.CARDCHTRAN.Include(c => c.CARDCHLEAF).Include(c => c.OCCUSER).Include(c => c.OCCUSER1).Where(x => x.CREATEDBY == user.ID).Where(x=>x.ISDELETE!=true).OrderByDescending(x => x.ID).ToList();
                var status = db.OCCENUMERATION.Where(x => x.Type == "cardcheque");
                ViewBag.STATUS = new SelectList(status, "ID", "Name");

                if (STATUS != null)
                {
                    List = List.Where(x => x.STATUS == STATUS).ToList();
                    ViewBag.STATUS = new SelectList(status, "ID", "Name",STATUS);
                    ViewBag.currsts = STATUS;
                }
                if (!String.IsNullOrEmpty(CARDNO))
                {
                    CARDNO = CARDNO.Trim();
                    List = List.Where(x => x.CARDNO.Contains(CARDNO)).ToList();
                    ViewBag.CARDNO = CARDNO;
                }
                if (CREATEDON != null)
                {
                    List = List.Where(x => x.CREATEDON == CREATEDON).ToList();
                    ViewBag.CREATEDON = CREATEDON;
                }
               // int pageSize = ConstantConfig.PageSizes;
                int pageSize = ConstantConfig.PageSizes;
                int pageNumber = ((page ?? 1));
                return View(List.ToPagedList(pageNumber,pageSize));
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
                         //   var signatureByte = (byte[]) dataRow[5];
                         //   var userPhotoByte = (byte[]) dataRow[4];
                            if (dataRow[5] != null)
                            {
                                signature = Convert.ToBase64String((byte[])dataRow[5]);
                            }
                            if (dataRow[4] != null)
                            {
                                userPhoto = Convert.ToBase64String((byte[])dataRow[4]);
                            }
                          
                            
                            clientname = (string)dataRow[2];
                        }


                        var model = new { leafID, cardno, userPhoto, signature, name = clientname };
                        return Json(model);
                    }
                    return Json(null);
                }
                else if (leafstatus == 11)
                {
                    return Json("used");
                }
                return Json("invalid");
            }
            catch (Exception exception)
            {
                return Json("invalid");
            }
           
        }

     
        public ActionResult Create()
        {
            try
            {
                List<string> currencyList = new List<string>() { "BDT" };
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
                int adminFlag = 0;
                OCCUSER user = (OCCUSER) Session["User"];
                cardchtran.CREATEDBY = user.ID;
                cardchtran.CREATEDON = DateTime.Now.Date;
                cardchtran.STATUS = 13;
                cardchtran.ISACTIVE = true;
                cardchtran.ISDELETE = false;

                var leaf=db.CARDCHLEAF.FirstOrDefault(x => x.ID == cardchtran.CHEQUELEAFID);
                
                leaf.LEAFSTATUS = db.OCCENUMERATION.Where(x=>x.Name=="used").Select(x=>x.ID).FirstOrDefault();
                db.Entry(leaf).State = EntityState.Modified;

                if (ModelState.IsValid)
                {
                    db.CARDCHTRAN.Add(cardchtran);
                    db.SaveChanges();
                    if (user.TYPE == 1)
                    {
                        adminFlag = 1;
                    }
                    var msg = "Data saved successfully. ";

                    return Json(new { msg, adminFlag }, JsonRequestBehavior.AllowGet);
                   
                }

                currencyList = new List<string>() { "BDT" };
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
