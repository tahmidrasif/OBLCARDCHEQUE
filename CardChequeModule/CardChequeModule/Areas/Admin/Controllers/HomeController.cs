using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CardChequeModule.Models;
using NPOI.SS.Formula.Functions;
using PagedList;

namespace CardChequeModule.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class HomeController : Controller
    {
        OBLCARDCHEQUEEntities db=new OBLCARDCHEQUEEntities();
        public ActionResult Index()
        {
          return View();
        }

        public ActionResult UserCreation()
        {
            ViewBag.BRANCH = new SelectList(db.BRANCHINFO.ToList(), "ID", "BRANCHNAME");
            ViewBag.TYPE = new SelectList(db.OCCENUMERATION.Where(x => x.Type == "user").ToList(), "ID", "Name");
            return View();
        }


        public ActionResult UserInfoByEmpId(string empId)
        {
            string userName="";
            string branchCode="";
            string branchName = "";
            try
            {
                WebRef.OBLAPP oblApp = new WebRef.OBLAPP();
                DataTable dt=oblApp.GetByUserID(empId);

                foreach (DataRow dataRow in dt.Rows)
                {
                   userName= (string) dataRow[3];
                   branchCode=(string) dataRow[22];
                   branchName = (string) dataRow[21];
                }

                long branchId=db.BRANCHINFO.Where(x => x.BRANCHCODE == branchCode).Select(x=>x.ID).FirstOrDefault();

                return Json(new { userName,branchId,branchName}, JsonRequestBehavior.AllowGet);
              
            }
            catch (Exception)
            {

                return Json("null", JsonRequestBehavior.DenyGet);
            }
          
            
        }

        [HttpPost]
        public ActionResult UserCreation(OCCUSER occuser, int? BranchId)
        {
            try
            {
                OCCUSER user = (OCCUSER)Session["User"];
                occuser.CREATEDBY = user.ID;
                occuser.CREATEDON = DateTime.Now.Date;
                occuser.ISACTIVE = true;
                occuser.BRANCH = BranchId;
                if (ModelState.IsValid)
                {
                    db.OCCUSER.Add(occuser);
                    db.SaveChanges();
                    var msg = "Data saved successfully. ";

                    return Json(msg, JsonRequestBehavior.AllowGet);
                }


                ViewBag.BRANCH = new SelectList(db.BRANCHINFO.ToList(), "ID", "BRANCHNAME",BranchId);
                ViewBag.TYPE = new SelectList(db.OCCENUMERATION.Where(x => x.Type == "user").ToList(), "ID", "Name",occuser.TYPE);
                return View();
            }
            catch (Exception)
            {

                return RedirectToAction("Error", "Home", new {Area = ""});
            }

        }

        public ActionResult UserList(int? TYPE, string EMPLOYEEID, string USERNAME, int? page)
        {
            ViewBag.TYPE = new SelectList(db.OCCENUMERATION.Where(x => x.Type == "user").ToList(), "ID", "Name");
            var List = db.OCCUSER.Include(c => c.BRANCHINFO).Include(c => c.OCCENUMERATION).Where(x=>x.ISACTIVE==true).ToList();

            if (TYPE != null)
            {
                List = List.Where(x => x.TYPE == TYPE).ToList();
                ViewBag.TYPE = new SelectList(db.OCCENUMERATION.Where(x => x.Type == "user").ToList(), "ID", "Name",TYPE);
                ViewBag.currtype = TYPE;
                // ViewBag.STATUS = new SelectList(statusID, "Key", "Value", statusID.Where(x => x.Key == STATUS));
            }
            if (!String.IsNullOrEmpty(EMPLOYEEID))
            {
                EMPLOYEEID = EMPLOYEEID.Trim();
                List = List.Where(x => x.EMPLOYEEID == EMPLOYEEID).ToList();
                ViewBag.curEmpId = EMPLOYEEID;
            }
            if (!String.IsNullOrEmpty(USERNAME))
            {
                USERNAME = USERNAME.Trim();
                List = List.Where(x => x.USERNAME.Contains(USERNAME)).ToList();
                ViewBag.curUserName = USERNAME;
            }
            int pageSize = 3;
            int pageNumber = ((page ?? 1));
            return View(List.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult UserDetail(long? id)
        {
            try
            {
                var user = db.OCCUSER.FirstOrDefault(x => x.ID == id);
                if (user != null)
                {

                    ViewBag.BRANCH = new SelectList(db.BRANCHINFO.ToList(), "ID", "BRANCHNAME",user.BRANCH);
                    ViewBag.TYPE = new SelectList(db.OCCENUMERATION.Where(x => x.Type == "user").ToList(), "ID", "Name",user.TYPE);
                    return View(user);
                }
                return RedirectToAction("Error", "Home", new { Area = "" });
            }
            catch (Exception)
            {

                return RedirectToAction("Error", "Home", new { Area = "" });
            }
          
            
        }

        [HttpPost]
        public ActionResult UserDetail(OCCUSER aUser, string btnName)
        {
            try
            {
                OCCUSER user = (OCCUSER)Session["User"];
                if (String.Equals(btnName, "update"))
                {
                    
                    aUser.MODIFIEDBY = user.ID;
                    aUser.MODIFIEDON = DateTime.Now.Date;
                    db.Entry(aUser).State = EntityState.Modified;
                    db.SaveChanges();
                    var msg = "Successfull Updated";
                    return Json(msg, JsonRequestBehavior.AllowGet);
                }
                if (String.Equals(btnName, "delete"))
                {
                    aUser.MODIFIEDBY = user.ID;
                    aUser.MODIFIEDON = DateTime.Now.Date;
                    aUser.ISACTIVE = false;
                    db.Entry(aUser).State = EntityState.Modified;
                    db.SaveChanges();
                    var msg = "Successfully Removed";
                    return Json(msg, JsonRequestBehavior.DenyGet);
                }
               return RedirectToAction("Error", "Home", new { Area = "" });
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
