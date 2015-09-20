using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CardChequeModule.Models;
using CardChequeModule.OraDBCardInfo;
using PagedList;

namespace CardChequeModule.Areas.CallCenter.Controllers
{
    [Authorize(Roles = "call center,admin")]
    public class HomeController : Controller
    {
        private OBLCARDCHEQUEEntities db = new OBLCARDCHEQUEEntities();


        public ActionResult Index(int? STATUS, string CARDNO, DateTime? CREATEDON, int? page)
        {
            try
            {
                var checkSts =
                    db.OCCENUMERATION.Where(x => x.Name == "delivered" && x.Type == "chequereq")
                        .Select(x => x.ID)
                        .FirstOrDefault();
                OCCUSER user = (OCCUSER)Session["User"];
                var cardchereuisition = db.CARDCHEREUISITION.Include(c => c.BRANCHINFO).Include(c => c.OCCENUMERATION).Include(c => c.OCCUSER).Include(c => c.OCCUSER1).Where(x => x.STATUS == checkSts && x.ISACTIVE == false).Where(x=>x.ISDELETE==false).ToList();
                //var status = db.OCCENUMERATION.Where(x => x.Type == "chequereq").Where(x => x.IsActive == true).ToList();
                var status = new Dictionary<int, string>() { { 16, "delivered" },{0,"Active"} };

                ViewBag.STATUS = new SelectList(status, "Key", "Value");

                if (STATUS != null)
                {
                    if (STATUS == 0)
                    {
                        cardchereuisition = db.CARDCHEREUISITION.Include(c => c.BRANCHINFO).Include(c => c.OCCENUMERATION).Include(c => c.OCCUSER).Include(c => c.OCCUSER1).Where(x=> x.ISACTIVE).ToList();
                   
                    }
                    else
                    {
                        cardchereuisition = db.CARDCHEREUISITION.Include(c => c.BRANCHINFO).Include(c => c.OCCENUMERATION).Include(c => c.OCCUSER).Include(c => c.OCCUSER1).Where(x => x.STATUS == STATUS && x.ISACTIVE == false).ToList(); 
                    }
                    
                    ViewBag.STATUS = new SelectList(status, "Key", "Value",STATUS);
                    ViewBag.currsts = STATUS;
                    // ViewBag.STATUS = new SelectList(statusID, "Key", "Value", statusID.Where(x => x.Key == STATUS));
                }
                
                if (!String.IsNullOrEmpty(CARDNO))
                {
                    CARDNO = CARDNO.Trim();
                    cardchereuisition = cardchereuisition.Where(x=>x.CARDNO.Contains(CARDNO)).ToList();
                    ViewBag.CARDNO = CARDNO;
                }
                if (CREATEDON != null)
                {
                    cardchereuisition =cardchereuisition.Where(x => x.CREATEDON ==CREATEDON).ToList();
                    ViewBag.CREATEDON = CREATEDON;
                }

                int pageSize = ConstantConfig.PageSizes;
                int pageNumber = ((page ?? 1));
                
                return View(cardchereuisition.ToPagedList(pageNumber, pageSize));
               
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { Area = "" });
            }
            
            
        }



        [HttpPost]
        public ActionResult Post(IEnumerable<long> idList)
        {

            OCCUSER user = (OCCUSER)Session["User"];

            try
            {
                foreach (var id in idList)
                {
                    var cheqreq = db.CARDCHEREUISITION.FirstOrDefault(x => x.ID == id);
                    cheqreq.CALLCENTERBY = user.ID;
                    cheqreq.CALLCENTERON = DateTime.Now;
                    cheqreq.ISACTIVE = true;
                    db.Entry(cheqreq).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
                return RedirectToAction("Error", "Home", new { Area = "" });
            }
        }

        public ActionResult BlockeLeaf()
        {

            return View();
        }

        public ActionResult GetCardInfo(string cardno)
        {
            try
            {
                OradbaccessSoap service = new OradbaccessSoapClient();
                DataTable dt = service.GetCCardDetail(cardno);

                if (dt != null)
                {
                    string userPhoto = "";
                    string signature = "";
                    string clientname = "";
                    string bday = "";
                    DateTime dob = DateTime.Now;
                    foreach (DataRow dataRow in dt.Rows)
                    {
                        userPhoto = Convert.ToBase64String((byte[])dataRow[4]);
                        signature = Convert.ToBase64String((byte[])dataRow[5]);
                        clientname = (string)dataRow[2];
                        dob = (DateTime)dataRow[3];
                        bday = dob.ToString("dd-MMM-yyyy");
                    }
                    var model = new { Bday=bday, cardno, userPhoto, signature, name = clientname };
                    return Json(model,JsonRequestBehavior.AllowGet);
                }
                return Json(null);
            }
            catch (Exception)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
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
