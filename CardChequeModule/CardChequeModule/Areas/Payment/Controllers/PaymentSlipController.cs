using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using CardChequeModule.Models;
using CardChequeModule.OraDBCardInfo;
//using CardChequeModule.WebReference;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CardChequeModule.Areas.Payment.Controllers
{
    [Authorize(Roles = "teller,admin")]
    public class PaymentSlipController : Controller
    {
        private JsonSerializer serializer = new JsonSerializer();
        OBLCARDCHEQUEEntities db = new OBLCARDCHEQUEEntities();
        //
        // GET: /Payment/Submit/
        public ActionResult Index()
        {

            List<string> currencyList = new List<string>() {"USD", "BDT" };
            ViewBag.BRANCH = new SelectList(db.BRANCHINFO.ToList(), "ID", "BRANCHNAME");
            ViewBag.CURRENCY = new SelectList(currencyList);
            return View(new DEPOSIT());
        }

        public void DestroySession()
        {
            Session["DepositSaveMsg"] = null;
        }

        [HttpPost]
        public ActionResult Index(DEPOSIT deposit)
        {
            
            OCCUSER user = (OCCUSER)Session["User"];
            int adminFlag = 0;
            if (ModelState.IsValid)
            {
                try
                {
                    
                    deposit.CREATEDBY = user.ID;
                    deposit.ISACTIVE = true;
                    deposit.ISDELETE = false;
                    //var userId = user.EMPLOYEEID;
                    //var length = userId.Length;
                    //char first = userId[0];
                    //char last = userId[userId.Length - 1];
                    //char middle = userId[(userId.Length - 1) / 2];
                    //var refNo = (first + middle + last).ToString(CultureInfo.InvariantCulture);
                    //refNo += length;
                    var refNo= DateTime.Now.ToString("ddMMyyHHmmssfff");
                    //refNo += deposit.CARDNUMBER.Substring(deposit.CARDNUMBER.Length - 4);
                    deposit.REFERENCENUMBER = refNo;
                    db.DEPOSIT.Add(deposit);
                    db.SaveChanges();
                  //  Session["DepositSaveMsg"] = "Data saved successfully.\nReferance Number is: "+deposit.REFERENCENUMBER;
                    var msg=@"Data saved successfully.<br/>Referance Number is: "+deposit.REFERENCENUMBER;
                    if (user.TYPE == 1)
                    {
                        adminFlag = 1;
                    }
                   // return RedirectToAction("Index", "List");
                    return Json(new{msg,adminFlag},JsonRequestBehavior.AllowGet);
                }
                catch (Exception exception)
                {
                    return Json("Error", JsonRequestBehavior.DenyGet);
                    //  return RedirectToAction("Error", "Home", new { Area = "" });
                }
               
            }
            List<string> currencyList = new List<string>() {"BDT" };
            ViewBag.BRANCH = new SelectList(db.BRANCHINFO.ToList(), "ID", "BRANCHNAME");
            ViewBag.CURRENCY = new SelectList(currencyList);
            Session["DepositSaveMsg"] = "Data not saved";
            return View(deposit);
        }

       
        public ActionResult GetCardInfo(string cardno)
        
        {
            try
            {
                if (!String.IsNullOrEmpty(cardno))
                {

                    OraDBCardInfo.OradbaccessSoap client=new OradbaccessSoapClient();
                    DataTable details = client.GetCCardDetail(cardno);

                    string clientName = "";

                    if (details != null)
                    {
                        foreach (DataRow row in details.Rows)
                        {
                            clientName = (string) row[2];
                            return Json(clientName, JsonRequestBehavior.AllowGet);
                        }
                        Json(null, JsonRequestBehavior.AllowGet);
                    }
                    //return Json(null, JsonRequestBehavior.AllowGet);
                }
                return Json(null, JsonRequestBehavior.AllowGet);
               
            }
            catch (Exception)
            {

                return Json("error",JsonRequestBehavior.DenyGet);
            }
           
           
        }
    }
}