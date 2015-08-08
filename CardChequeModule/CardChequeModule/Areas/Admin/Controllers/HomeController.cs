using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CardChequeModule.Models;

namespace CardChequeModule.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
          return View();
        }
        //private MyDbContext db = new MyDbContext();

        //// GET: /Admin/Home/

        //public ActionResult Index()
        //{
        //    var occuser = db.OCCUSERS.Include(o => o.OCCROLE);
        //    return View(occuser.ToList());
        //}

        //// GET: /Admin/Home/Details/5
        //public ActionResult Details(long? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    OCCUSER occuser = db.OCCUSERS.Find(id);
        //    if (occuser == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(occuser);
        //}

        //// GET: /Admin/Home/Create
        //public ActionResult Create()
        //{
        //    ViewBag.TYPE = new SelectList(db.ROLES, "ID", "ROLENAME");
        //    return View();
        //}

        //// POST: /Admin/Home/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include="ID,EMPLOYEEID,USERNAME,PASSWORD,DEPARTMENT,DESIGNATION,TYPE,BRANCHCODE,ISACTIVE,CREATEDBY,CREATEDON,MODIFIEDBY,MODIFIEDON")] OCCUSER occuser)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.OCCUSERS.Add(occuser);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.TYPE = new SelectList(db.ROLES, "ID", "ROLENAME", occuser.TYPE);
        //    return View(occuser);
        //}

        //// GET: /Admin/Home/Edit/5
        //public ActionResult Edit(long? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    OCCUSER occuser = db.OCCUSERS.Find(id);
        //    if (occuser == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.TYPE = new SelectList(db.ROLES, "ID", "ROLENAME", occuser.TYPE);
        //    return View(occuser);
        //}

        //// POST: /Admin/Home/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include="ID,EMPLOYEEID,USERNAME,PASSWORD,DEPARTMENT,DESIGNATION,TYPE,BRANCHCODE,ISACTIVE,CREATEDBY,CREATEDON,MODIFIEDBY,MODIFIEDON")] OCCUSER occuser)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(occuser).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.TYPE = new SelectList(db.ROLES, "ID", "ROLENAME", occuser.TYPE);
        //    return View(occuser);
        //}

        //// GET: /Admin/Home/Delete/5
        //public ActionResult Delete(long? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    OCCUSER occuser = db.OCCUSERS.Find(id);
        //    if (occuser == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(occuser);
        //}

        //// POST: /Admin/Home/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(long id)
        //{
        //    OCCUSER occuser = db.OCCUSERS.Find(id);
        //    db.OCCUSERS.Remove(occuser);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
