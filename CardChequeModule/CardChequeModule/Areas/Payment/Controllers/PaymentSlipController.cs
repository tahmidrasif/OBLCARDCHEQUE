using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using CardChequeModule.Models;
using CardChequeModule.WebReference;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CardChequeModule.Areas.Payment.Controllers
{
    public class PaymentSlipController : Controller
    {
        private JsonSerializer serializer = new JsonSerializer();
        OBLCARDCHEQUEEntities db = new OBLCARDCHEQUEEntities();
        //
        // GET: /Payment/Submit/
        public ActionResult Index()
        {

            List<string> currencyList = new List<string>() { "USD", "BDT" };
            ViewBag.BRANCH = new SelectList(db.BRANCHINFO.ToList(), "ID", "BRANCHNAME");
            ViewBag.CURRENCY = new SelectList(currencyList);
            return View(new DEPOSIT());
        }

        public void DestroySession()
        {
            Session["DepositSaveMsg"] = null;
        }

        [HttpPost]
        public ActionResult Index(DEPOSIT deposit, string ADI1, string ADI2)
        {
            if (deposit.PAYMENTTYPE == "ADI")
            {
                deposit.ADI = ADI1 + ADI2;
            }
            OCCUSER user = (OCCUSER)Session["User"];
            if (ModelState.IsValid)
            {
                try
                {
                    
                    deposit.CREATEDBY = user.ID;
                    deposit.ISAUTHORIZED = false;

                    var userId = user.EMPLOYEEID;
                    var length = userId.Length;
                    char first = userId[0];
                    char last = userId[userId.Length - 1];
                    char middle = userId[(userId.Length - 1) / 2];
                    var refNo = (first + middle + last).ToString(CultureInfo.InvariantCulture);
                    refNo += length;
                    refNo += DateTime.Now.ToString("ddMMyyHHmmssfff");
                    refNo += deposit.CARDNUMBER.Substring(deposit.CARDNUMBER.Length - 4);
                    deposit.REFERENCENUMBER = refNo;
                    db.DEPOSIT.Add(deposit);
                    db.SaveChanges();
                    Session["DepositSaveMsg"] = "Data saved successfully.\nReferance Number is: ";
                    return RedirectToAction("Index", "List");
                }
                catch (Exception exception)
                {

                    return RedirectToAction("Error", "Home", new { Area = "" });
                }
               
            }
            List<string> currencyList = new List<string>() { "USD", "BDT" };
            ViewBag.BRANCH = new SelectList(db.BRANCHINFO.ToList(), "ID", "BRANCHNAME");
            ViewBag.CURRENCY = new SelectList(currencyList);
            Session["DepositSaveMsg"] = "Data not saved";
            return View(deposit);
        }

        // public ActionResult Index(string branch, DateTime? depositDate, string name, string mobile, string cardNo, decimal? amount, string paymentType)

        public ActionResult GetCardInfo(string cardno)
        {
            try
            {
                WebReference.CCMService ccmService = new CCMService();
                string details = ccmService.GetClientDetails(cardno);

                if (details == "null")
                {

                    return Json(null, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    JObject json = JObject.Parse(details);
                    ClientDetails clientDetails = (ClientDetails)serializer.Deserialize(new JTokenReader(json), typeof(ClientDetails));
                    return Json(clientDetails, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {

                return RedirectToAction("Error", "Home", new { Area = "" });
            }
           
           // return RedirectToAction("Index");
        }
    }
}