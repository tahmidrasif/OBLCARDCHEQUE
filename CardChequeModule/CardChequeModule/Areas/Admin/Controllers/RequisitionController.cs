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
    [Authorize(Roles = "admin")]
    public class RequisitionController : Controller
    {
        private OBLCARDCHEQUEEntities db = new OBLCARDCHEQUEEntities();

        // GET: Admin/Requisition
        public ActionResult Index(int? STATUS, string CARDNO, DateTime? CREATEDON, int? page)
        {
            try
            {

                OCCUSER user = (OCCUSER)Session["User"];
                Dictionary<int, string> statusID = new Dictionary<int, string>()
                {
                {4,"applied"},{5,"authorized"},{7,"received"},{8,"deny"}
                };
                ViewBag.STATUS = new SelectList(statusID, "Key", "Value");

                var List = db.CARDCHEREUISITION.Include(c => c.BRANCHINFO).Where(x=>x.ISDELETE==false).OrderByDescending(x => x.ID).ToList();

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

                int pageSize = ConstantConfig.PageSizes;
                int pageNumber = ((page ?? 1));
                return View(List.ToPagedList(pageNumber, pageSize));

            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { Area = "" });
            }
        }

        // GET: Admin/Requisition/Details/5
        public ActionResult Details(long? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                CARDCHEREUISITION requisition = db.CARDCHEREUISITION.Find(id);
                if (requisition == null)
                {
                    return HttpNotFound();
                }
                ViewBag.BRANCHCODE = new SelectList(db.BRANCHINFO, "ID", "BRANCHNAME", requisition.BRANCHCODE);

                ViewBag.STATUS = new SelectList(db.OCCENUMERATION.Where(x => x.Type == "chequereq"&&x.IsActive==true), "ID", "Name", requisition.STATUS);

                
                return View(requisition);
            }
            catch (Exception ex)
            {

                return RedirectToAction("Error", "Home", new { Area = "" });
            }
          
        }

        [HttpPost]
        public ActionResult Details(CARDCHEREUISITION cardchereuisition,string btnName)
        {
            try
            {
                OCCUSER user = (OCCUSER)Session["User"];
                if (String.Equals(btnName, "delete"))
                {
                    CARDCHEREUISITION ccr = db.CARDCHEREUISITION.Find(cardchereuisition.ID);
                    ccr.ISDELETE = true;
                    //db.Entry(ccr).State = EntityState.Modified;
                    db.SaveChanges();
                    var msg = "Successfully Removed";
                    return Json(msg, JsonRequestBehavior.AllowGet);
                }
                if (String.Equals(btnName, "update"))
                {
                    
                    var updatedModel = db.CARDCHEREUISITION.Find(cardchereuisition.ID);
                    updatedModel.CARDNO = cardchereuisition.CARDNO;
                    updatedModel.CREATEDON = cardchereuisition.CREATEDON;
                    updatedModel.BRANCHCODE = cardchereuisition.BRANCHCODE;
                    updatedModel.STATUS = cardchereuisition.STATUS;
                    updatedModel.REFERENCENO = cardchereuisition.REFERENCENO;
                    updatedModel.REMARKS = cardchereuisition.REMARKS;
                    //if (cardchereuisition.ISACTIVE)
                    //{
                    //    updatedModel.ISACTIVE = cardchereuisition.ISACTIVE;
                    //}
                    //if (ModelState.IsValid)
                    //{

                        //db.Entry(cardchereuisition).State = EntityState.Modified;
                        db.SaveChanges();
                    //}

                    var msg = "Successfully Updated";
                    return Json(msg, JsonRequestBehavior.AllowGet);
                }
                return Json("Error",JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {

                return Json(exception.Message, JsonRequestBehavior.AllowGet);
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

































//// GET: Admin/Requisition/Create
//public ActionResult Create()
//{
//    ViewBag.BRANCHCODE = new SelectList(db.BRANCHINFO, "ID", "BRANCHCODE");
//    ViewBag.STATUS = new SelectList(db.OCCENUMERATION, "ID", "Type");
//    ViewBag.CREATEDBY = new SelectList(db.OCCUSER, "ID", "EMPLOYEEID");
//    ViewBag.MODIFIEDBY = new SelectList(db.OCCUSER, "ID", "EMPLOYEEID");
//    return View();
//}

//// POST: Admin/Requisition/Create
//// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
////[HttpPost]
////[ValidateAntiForgeryToken]
////public ActionResult Create([Bind(Include = "ID,CARDNO,BRANCHCODE,STATUS,LEAFNO,LEAFFROM,LEAFTO,LEAFNEXT,REMARKS,ISACTIVE,CREATEDBY,CREATEDON,MODIFIEDBY,MODIFIEDON,REFERENCENO")] CARDCHEREUISITION cARDCHEREUISITION)
////{
////    if (ModelState.IsValid)
////    {
////        db.CARDCHEREUISITION.Add(cARDCHEREUISITION);
////        db.SaveChanges();
////        return RedirectToAction("Index");
////    }

////    ViewBag.BRANCHCODE = new SelectList(db.BRANCHINFO, "ID", "BRANCHCODE", cARDCHEREUISITION.BRANCHCODE);
////    ViewBag.STATUS = new SelectList(db.OCCENUMERATION, "ID", "Type", cARDCHEREUISITION.STATUS);
////    ViewBag.CREATEDBY = new SelectList(db.OCCUSER, "ID", "EMPLOYEEID", cARDCHEREUISITION.CREATEDBY);
////    ViewBag.MODIFIEDBY = new SelectList(db.OCCUSER, "ID", "EMPLOYEEID", cARDCHEREUISITION.MODIFIEDBY);
////    return View(cARDCHEREUISITION);
////}

//// GET: Admin/Requisition/Edit/5
////public ActionResult Edit(long? id)
////{
////    if (id == null)
////    {
////        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
////    }
////    CARDCHEREUISITION cARDCHEREUISITION = db.CARDCHEREUISITION.Find(id);
////    if (cARDCHEREUISITION == null)
////    {
////        return HttpNotFound();
////    }
////    ViewBag.BRANCHCODE = new SelectList(db.BRANCHINFO, "ID", "BRANCHCODE", cARDCHEREUISITION.BRANCHCODE);
////    ViewBag.STATUS = new SelectList(db.OCCENUMERATION, "ID", "Type", cARDCHEREUISITION.STATUS);
////    ViewBag.CREATEDBY = new SelectList(db.OCCUSER, "ID", "EMPLOYEEID", cARDCHEREUISITION.CREATEDBY);
////    ViewBag.MODIFIEDBY = new SelectList(db.OCCUSER, "ID", "EMPLOYEEID", cARDCHEREUISITION.MODIFIEDBY);
////    return View(cARDCHEREUISITION);
////}

//// POST: Admin/Requisition/Edit/5
//// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
////[HttpPost]
////[ValidateAntiForgeryToken]
////public ActionResult Edit([Bind(Include = "ID,CARDNO,BRANCHCODE,STATUS,LEAFNO,LEAFFROM,LEAFTO,LEAFNEXT,REMARKS,ISACTIVE,CREATEDBY,CREATEDON,MODIFIEDBY,MODIFIEDON,REFERENCENO")] CARDCHEREUISITION cARDCHEREUISITION)
////{
////    if (ModelState.IsValid)
////    {
////        db.Entry(cARDCHEREUISITION).State = EntityState.Modified;
////        db.SaveChanges();
////        return RedirectToAction("Index");
////    }
////    ViewBag.BRANCHCODE = new SelectList(db.BRANCHINFO, "ID", "BRANCHCODE", cARDCHEREUISITION.BRANCHCODE);
////    ViewBag.STATUS = new SelectList(db.OCCENUMERATION, "ID", "Type", cARDCHEREUISITION.STATUS);
////    ViewBag.CREATEDBY = new SelectList(db.OCCUSER, "ID", "EMPLOYEEID", cARDCHEREUISITION.CREATEDBY);
////    ViewBag.MODIFIEDBY = new SelectList(db.OCCUSER, "ID", "EMPLOYEEID", cARDCHEREUISITION.MODIFIEDBY);
////    return View(cARDCHEREUISITION);
////}

////// GET: Admin/Requisition/Delete/5
//public ActionResult Delete(long? id)
//{
//    if (id == null)
//    {
//        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//    }
//    CARDCHEREUISITION cARDCHEREUISITION = db.CARDCHEREUISITION.Find(id);
//    if (cARDCHEREUISITION == null)
//    {
//        return HttpNotFound();
//    }
//    return View(cARDCHEREUISITION);
//}

//// POST: Admin/Requisition/Delete/5
//[HttpPost, ActionName("Delete")]
//[ValidateAntiForgeryToken]
//public ActionResult DeleteConfirmed(long id)
//{
//    CARDCHEREUISITION cARDCHEREUISITION = db.CARDCHEREUISITION.Find(id);
//    db.CARDCHEREUISITION.Remove(cARDCHEREUISITION);
//    db.SaveChanges();
//    return RedirectToAction("Index");
//}
