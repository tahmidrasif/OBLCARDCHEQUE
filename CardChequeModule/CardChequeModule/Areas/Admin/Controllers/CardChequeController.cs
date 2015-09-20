using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CardChequeModule.Models;
using CardChequeModule.OraDBCardInfo;
using PagedList;

namespace CardChequeModule.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class CardChequeController : Controller
    {
        private OBLCARDCHEQUEEntities db = new OBLCARDCHEQUEEntities();

        // GET: Admin/CardCheque
        public ActionResult Index(int? STATUS, string CARDNO, DateTime? CREATEDON, int? page)
        {
            try
            {
                OCCUSER user = (OCCUSER)Session["User"];
                var List = db.CARDCHTRAN.Include(c => c.CARDCHLEAF).Include(c => c.OCCUSER).Include(c => c.OCCUSER1).Where(x=>x.ISDELETE!=true).OrderByDescending(x => x.ID).ToList();
                var status = db.OCCENUMERATION.Where(x => x.Type == "cardcheque");
                ViewBag.STATUS = new SelectList(status, "ID", "Name");

                if (STATUS != null)
                {
                    List = List.Where(x => x.STATUS == STATUS).ToList();
                    ViewBag.STATUS = new SelectList(status, "ID", "Name", STATUS);
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
                return View(List.ToPagedList(pageNumber, pageSize));
            }
            catch (Exception)
            {

                return RedirectToAction("Error", "Home", new { Area = "" });
            }

        }

        
        public ActionResult Details(long? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                List<string> currencyList = new List<string>() { "BDT" };
               
                CARDCHTRAN tran = db.CARDCHTRAN.Find(id);
                if (tran == null)
                {
                    return HttpNotFound();
                }

                ViewBag.BRANCHCODE = new SelectList(db.BRANCHINFO.ToList(), "ID", "BRANCHNAME",tran.BRANCHCODE);
                ViewBag.CURRENCY = new SelectList(currencyList);

                return View(tran);
            }
            catch (Exception exception)
            {
                return RedirectToAction("Error", "Home", new { Area = "" });
            }

        }

        [HttpPost]
        public ActionResult Details(CARDCHTRAN tran,string btnName)
        {
            try
            {
                OCCUSER user = (OCCUSER)Session["User"];
                if (String.Equals(btnName, "update"))
                {
                    var updatedTrn = db.CARDCHTRAN.Find(tran.ID);
                    updatedTrn.CHEQUELEAFID = tran.CHEQUELEAFID;
                    updatedTrn.CARDNO = tran.CARDNO;
                    updatedTrn.BENEFICIARINFO = tran.BENEFICIARINFO;
                    updatedTrn.AMOUNT = tran.AMOUNT;
                    updatedTrn.BRANCHCODE = tran.BRANCHCODE;
                    updatedTrn.REQUESTDATE = tran.REQUESTDATE;

                    updatedTrn.MODIFIEDBY = user.ID;
                    updatedTrn.MODIFIEDON = DateTime.Now;
                   // updatedTrn.ISACTIVE = true;
                    db.Entry(updatedTrn).State = EntityState.Modified;
                    db.SaveChanges();
                    var msg = "Successfully Updated";
                    return Json(msg, JsonRequestBehavior.AllowGet);
                }
                if (String.Equals(btnName, "delete"))
                {
                    //aUser.MODIFIEDBY = user.ID;
                    //aUser.MODIFIEDON = DateTime.Now;
                    //aUser.ISACTIVE = false;
                    //db.Entry(aUser).State = EntityState.Modified;
                    //db.SaveChanges();
                    var updatedTrn = db.CARDCHTRAN.Find(tran.ID);
                    updatedTrn.ISDELETE = true;
                    db.Entry(updatedTrn).State = EntityState.Modified;
                    db.SaveChanges();
                    var msg = "Successfully Removed";
                    return Json(msg, JsonRequestBehavior.AllowGet);
                }
                var msg1 = "Error in operation";
                return Json(msg1, JsonRequestBehavior.DenyGet);
              //  return View(tran);
            }
            catch (Exception exception)
            {
                return Json(exception.Message, JsonRequestBehavior.AllowGet);
                //return RedirectToAction("Error", "Home", new { Area = "" });
            }

        }

        public ActionResult GetInfo(string leafno)
        {
            try
            {
               // var leafstatus = db.CARDCHLEAF.Where(x => x.LEAFNO == leafno).Select(x => x.LEAFSTATUS).FirstOrDefault();
                //if (leafstatus == 10)
                //{
                    var chequeId = db.CARDCHLEAF.Where(x => x.LEAFNO == leafno).Select(x => x.CHEQUEID).FirstOrDefault();

                    var leafID = db.CARDCHLEAF.Where(x => x.LEAFNO == leafno).Select(x => x.ID).FirstOrDefault();
                    var cardno = db.CARDCHEREUISITION.Where(x => x.ID == chequeId).Select(x => x.CARDNO).FirstOrDefault();

                    OradbaccessSoap service = new OradbaccessSoapClient();
                    DataTable dt = service.GetCCardDetail(cardno);

                    if (dt != null)
                    {
                        string userPhoto = "";
                        string signature = "";
                        string clientname = "";
                        foreach (DataRow dataRow in dt.Rows)
                        {
                            userPhoto = Convert.ToBase64String((byte[])dataRow[4]);
                            signature = Convert.ToBase64String((byte[])dataRow[5]);
                            clientname = (string)dataRow[2];
                        }


                        var model = new { leafID, cardno, userPhoto, signature, name = clientname };
                        return Json(model);
                    }
                    return Json(null);
                //}
                //else if (leafstatus == 11)
                //{
                //    return Json("used");
                //}
                //return Json("invalid");
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
