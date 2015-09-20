using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using CardChequeModule.Models;
using CardChequeModule.OraDBCardInfo;
using PagedList;

namespace CardChequeModule.Areas.ChequeRequisition.Controllers
{
    [Authorize(Roles = "teller,admin")]
    public class HomeController : Controller
    {
        private OBLCARDCHEQUEEntities db = new OBLCARDCHEQUEEntities();
        OraDBCardInfo.OradbaccessSoapClient cardApi = new OradbaccessSoapClient();
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

                var List = db.CARDCHEREUISITION.Include(c => c.BRANCHINFO).Where(x => x.ISDELETE == false).Where(x => x.CREATEDBY == user.ID).OrderByDescending(x => x.ID).ToList();

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
                    List = List.Where(x => x.CARDNO.Contains(CARDNO)).ToList();
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


        public ActionResult GetCardDetails(string cardNo)
        {
            string name = "";
            string msg = "";
            DateTime dob = DateTime.Now;
         //   int flag = 0;
           // cardNo.Trim(' ')
            try
            {
                if (!(cardNo.All(c => c >= '0' && c <= '9')))
                {
                    msg = "Please Enter Digit only";
                    return Json(new { msg, flag = 0 }, JsonRequestBehavior.AllowGet);
                }
                if (cardNo.Length>18)
                {
                    msg = "Card Number should not be more than 18 digit";
                    return Json(new { msg, flag = 0 }, JsonRequestBehavior.AllowGet);
                }
                if (!String.IsNullOrEmpty(cardNo))
                {
                    DataTable dt = cardApi.GetCCardDetail(cardNo.Trim(' '));

                    if (dt.Rows.Count > 0)
                    {

                        foreach (DataRow dataRow in dt.Rows)
                        {
                            name = (string)dataRow[2];
                            dob = (DateTime)dataRow[3];
                        }
                        string userName = "User Name: " + name;
                        String userDob = "Date of Birth: " + dob.Date.ToShortDateString();
                        return Json(new { userName, userDob, flag = 1 }, JsonRequestBehavior.AllowGet);
                    }
                    msg = "Invalid Card Number. Please Recheck";
                    return Json(new { msg, flag = 0 }, JsonRequestBehavior.AllowGet);
                }
                msg = "Card Number Can Not Be empty";
                return Json(new { msg, flag = 0 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {

                return Json(new{msg=exception.Message,flag=2}, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult Form(long? id)
        {
            try
            {
                if (id == null)
                {
                    user = (OCCUSER)Session["User"];
                    ViewBag.BRANCHCODE = new SelectList(db.BRANCHINFO, "ID", "BRANCHNAME");
                    CARDCHEREUISITION ccreq = new CARDCHEREUISITION();
                    //if (user.BRANCH != null)
                    //{
                    //    ccreq.BRANCHCODE = (long)user.BRANCH;
                    //}
                    long status = db.OCCENUMERATION.Where(x => x.Name == "applied").Select(x => x.ID).FirstOrDefault();
                    ViewBag.STATUSID = new SelectList(db.OCCENUMERATION.Where(x => x.Type == "chequereq"), "ID", "Name", status);
                    ViewBag.STATUS = status;
                    ccreq.CREATEDON = DateTime.Now;
                    return View(ccreq);
                }
                CARDCHEREUISITION cardchereuisition = db.CARDCHEREUISITION.Find(id);
                if (cardchereuisition == null)
                {
                    return HttpNotFound();
                }
                ViewBag.BRANCHCODE = new SelectList(db.BRANCHINFO, "ID", "BRANCHNAME", cardchereuisition.BRANCHCODE);

                ViewBag.STATUSID = new SelectList(db.OCCENUMERATION.Where(x => x.Type == "chequereq"), "ID", "Name", cardchereuisition.STATUS);

                return View(cardchereuisition);
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home", new { Area = "" });
            }

        }


        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Form(CARDCHEREUISITION cardchereuisition)
        {


            try
            {
                user = (OCCUSER)Session["User"];


                int adminFlag = 0;
                if (cardchereuisition.ID == 0)
                {
                    cardchereuisition.LEAFNO = 10;
                    cardchereuisition.ISACTIVE = false;
                    cardchereuisition.ISDELETE = false;
                    cardchereuisition.CREATEDBY = user.ID;
                    if (ModelState.IsValid)
                    {
                        db.CARDCHEREUISITION.Add(cardchereuisition);
                        db.SaveChanges();
                        if (user.TYPE == 1)
                        {
                            adminFlag = 1;
                        }
                        string msg = "Successfully Saved";
                        return Json(new { msg, adminFlag }, JsonRequestBehavior.AllowGet);
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
                    var modifiedcardCheque = db.CARDCHEREUISITION.Find(cardchereuisition.ID);

                   
                        modifiedcardCheque.REMARKS = cardchereuisition.REMARKS;
                        modifiedcardCheque.REFERENCENO = cardchereuisition.REFERENCENO;
                        modifiedcardCheque.MODIFIEDBY = user.ID;
                        modifiedcardCheque.MODIFIEDON = DateTime.Now;
                        db.Entry(modifiedcardCheque).State = EntityState.Modified;
                        db.SaveChanges();
                        string msg = "Successfully Updated";
                        return Json(new { msg, adminFlag }, JsonRequestBehavior.AllowGet);

                    
                }

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













