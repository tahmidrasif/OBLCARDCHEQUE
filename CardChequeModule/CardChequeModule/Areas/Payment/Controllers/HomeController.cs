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
using Microsoft.Ajax.Utilities;
using PagedList;

namespace CardChequeModule.Areas.Payment.Controllers
{
    [Authorize(Roles = "teller,admin")]
    public class HomeController : Controller
    {
        OBLCARDCHEQUEEntities db = new OBLCARDCHEQUEEntities();

        //
        // GET: /Payment/List/
        public ActionResult Index(int? BRANCH, string CARDNO, DateTime? CREATEDON, int? page, int? serial)
        {
            int pageSize = ConstantConfig.PageSizes;
            int pageNumber = (page ?? 1);

            OCCUSER user = (OCCUSER)Session["User"];
            try
            {
                var list = db.DEPOSIT.Where(x => x.CREATEDBY == user.ID).Where(x => x.ISDELETE == false).OrderByDescending(x => x.ID).ToList();
                var branch = db.BRANCHINFO.Select(x => new { x.BRANCHNAME, x.ID }).OrderBy(x => x.BRANCHNAME);
                ViewBag.BRANCH = new SelectList(branch, "ID", "BRANCHNAME");

                if (serial != null)
                {
                    @ViewBag.Sln = (pageNumber * pageSize) - 9;
                }
                else
                {
                    @ViewBag.Sln = 1;
                }

                if (BRANCH != null)
                {
                    list = list.Where(x => x.BRANCH == BRANCH).ToList();
                    ViewBag.BRANCH = new SelectList(branch, "ID", "BRANCHNAME", BRANCH);
                    ViewBag.currentBranch = BRANCH;
                }
                if (!String.IsNullOrEmpty(CARDNO))
                {
                    CARDNO = CARDNO.Trim();
                    list = list.Where(x => x.CARDNUMBER.Contains(CARDNO)).ToList();
                    ViewBag.CARDNO = CARDNO;
                }
                if (CREATEDON != null)
                {
                    list = list.Where(x => x.CREATEDON == CREATEDON).ToList();
                    ViewBag.CREATEDON = CREATEDON;
                }

                return View(list.ToPagedList(pageNumber, pageSize));
                // return View(list);
            }
            catch (Exception exception)
            {
                return RedirectToAction("Error", "Home", new { Area = "" });
            }

        }

        public ActionResult Form(long? id)
        {
            if (id == null)
            {
                List<string> currencyList = new List<string>() { "USD", "BDT" };
                ViewBag.BRANCH = new SelectList(db.BRANCHINFO.ToList(), "ID", "BRANCHNAME");
                ViewBag.CURRENCY = new SelectList(currencyList);
                return View(new DEPOSIT());
            }
            else
            {
                DEPOSIT deposit = db.DEPOSIT.Find(id);
                if (deposit == null)
                {
                    return HttpNotFound();
                }
                List<string> currencyList = new List<string>() { "USD", "BDT" };

                ViewBag.BRANCH = new SelectList(db.BRANCHINFO.ToList(), "ID", "BRANCHNAME", deposit.BRANCH);
                ViewBag.CURRENCY = new SelectList(currencyList, null, null, deposit.CURRENCY);
                return View(deposit);
            }
        }

        [HttpPost]
        public ActionResult Form(DEPOSIT deposit)
        {


            try
            {
                OCCUSER user = (OCCUSER)Session["User"];
                int adminFlag = 0;
                if (deposit.ID == 0)
                {
                    deposit.CREATEDBY = user.ID;
                    deposit.ISACTIVE = true;
                    deposit.ISDELETE = false;
                    deposit.ISDOWNLOAD = false;
                    var refNo = DateTime.Now.ToString("ddMMyyHHmmssfff");
                    //refNo += deposit.CARDNUMBER.Substring(deposit.CARDNUMBER.Length - 4);
                    deposit.REFERENCENUMBER = refNo;
                    db.DEPOSIT.Add(deposit);
                    db.SaveChanges();
                    //  Session["DepositSaveMsg"] = "Data saved successfully.\nReferance Number is: "+deposit.REFERENCENUMBER;
                    var msg = @"Data saved successfully.<br/>Referance Number is: " + deposit.REFERENCENUMBER;
                    if (user.TYPE == 1)
                    {
                        adminFlag = 1;
                    }
                    // return RedirectToAction("Index", "List");
                    return Json(new { msg, adminFlag }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        deposit.CREATEDBY = user.ID;
                        deposit.ISACTIVE = true;
                        deposit.ISDELETE = false;
                        deposit.ISDOWNLOAD = false;

                        db.Entry(deposit).State = EntityState.Modified;
                        if (user.TYPE == 1)
                        {
                            adminFlag = 1;
                        }
                        //db.DEPOSIT.AddOrUpdate(x=>x.ID==deposit.ID);
                        db.SaveChanges();
                        var msg = "Successfully Updated";
                        return Json(new { msg, adminFlag }, JsonRequestBehavior.AllowGet);

                    }
                    List<string> currencyList = new List<string>() { "USD", "BDT" };

                    ViewBag.BRANCH = new SelectList(db.BRANCHINFO.ToList(), "ID", "BRANCHNAME", db.DEPOSIT.Select(x => x.BRANCH));
                    ViewBag.CURRENCY = new SelectList(currencyList, null, null, db.DEPOSIT.Select(x => x.CURRENCY));
                    return View(deposit);
                }

            }
            catch (Exception exception)
            {
                return RedirectToAction("Error", "Home", new { Area = "" });
            }


        }

        public ActionResult GetCardInfo(string cardno)
        {
            try
            {
                if (!String.IsNullOrEmpty(cardno))
                {

                    OraDBCardInfo.OradbaccessSoap client = new OradbaccessSoapClient();
                    DataTable details = client.GetCCardDetail(cardno);

                    string clientName = "";

                    if (details != null)
                    {
                        foreach (DataRow row in details.Rows)
                        {
                            clientName = (string)row[2];
                            return Json(clientName, JsonRequestBehavior.AllowGet);
                        }
                        Json(null, JsonRequestBehavior.AllowGet);
                    }
                    return Json(null, JsonRequestBehavior.AllowGet);
                    
                }
                return Json(null, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {

                return Json("error", JsonRequestBehavior.DenyGet);
            }


        }
	}
}